
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using sprint2.Blocks;
using sprint2.Bosses;
using sprint2.Players;
using sprint2.Collision;

using static sprint2.Constants;
using Microsoft.Xna.Framework.Graphics;



namespace sprint2.Rooms;

public class RoomManager
{
    private readonly RoomFactory _roomFactory;
    private readonly List<IBlock> _blocks;
    private readonly List<IBoss> _bosses;
    private readonly IPlayer _player;

    public List<CollisionSurface> WorldMesh {get; set;} //contains all merged collision surfaces (this is to prevent internal collisions behaving weirdly)

 
    private readonly string[] _roomFiles =
    {
        "SlimeRoom.csv",
        "BeeRoom.csv",
        "CthulhuRoom.csv"
    };

    private int _currRoom;

    public RoomManager(RoomFactory roomFactory, List<IBlock> blocks, List<IBoss> bosses, IPlayer player)
    {
        _roomFactory = roomFactory;
        _blocks = blocks;
        _bosses = bosses;
        _player = player;
        _currRoom = 0;
        WorldMesh = new List<CollisionSurface>();
    }

      public void LoadMesh(List<IBlock> blockList)
    {
        ArgumentNullException.ThrowIfNull(blockList);

        WorldMesh.Clear();//make sure previous room's mesh doesent persist
        
        //very efficient for lookups
        HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

        foreach(IBlock b in blockList)
        {
            occupiedPositions.Add(b.Location);
        }


        HashSet<Vector2> mergedHorizontally = new HashSet<Vector2>();//tracks blocks already merged horizontally so vertical pass doesent need to scan them
        

        //Horizontal pass
        for (int i = 0; i < blockList.Count; i++)
        {
            Vector2 startPos = blockList[i].Location;

            bool hasBlockOnTop = occupiedPositions.Contains(new Vector2(startPos.X, startPos.Y - BlockSize));
            if (mergedHorizontally.Contains(startPos) || hasBlockOnTop)
            {
                continue;
            }

            int runLength = 1;

            bool nextExists = true;
            bool nextHasBlockOnTop = false;
            while (nextExists && !nextHasBlockOnTop)
            {
                Vector2 nextPos = new Vector2(startPos.X + (runLength * BlockSize), startPos.Y);
                nextExists = occupiedPositions.Contains(nextPos);
                nextHasBlockOnTop = occupiedPositions.Contains(new Vector2(nextPos.X, nextPos.Y - BlockSize));

                if (nextExists && !nextHasBlockOnTop)
                {
                    runLength++;
                }
            }

            
            bool hasBlockBelow = occupiedPositions.Contains(new Vector2(startPos.X, startPos.Y + BlockSize));
            //means this is likely the top of a column
            if (runLength == 1 && hasBlockBelow)
            {
                continue;
            }

            // all blocks in this strip have been merged
            for (int k = 0; k < runLength; k++)
            {
                mergedHorizontally.Add(new Vector2(startPos.X + (k * BlockSize), startPos.Y));
            }

            WorldMesh.Add(new CollisionSurface(new Vector2((int)startPos.X,(int)startPos.Y), runLength * BlockSize, false));
        }



        //remaining vertical pass
        int j = 0;
        while (j < blockList.Count)
        {
            Vector2 startPos = blockList[j].Location;

            // sip if already scanned by the horizontal pass
            if (mergedHorizontally.Contains(startPos))
            {
                j++;
                continue;
            }

            bool hasExposedSide = !occupiedPositions.Contains(new Vector2(startPos.X - BlockSize, startPos.Y)) || !occupiedPositions.Contains(new Vector2(startPos.X + BlockSize, startPos.Y));//check both sides
            if (!hasExposedSide)
            {
                j++;
                continue;
            }

            int runHeight = 1;
            while (j + runHeight < blockList.Count)
            {
                Vector2 nextPos = blockList[j + runHeight].Location;
                Vector2 expectedPos = new Vector2(startPos.X, startPos.Y + (runHeight * BlockSize));

                bool nextHasExposedSide = !occupiedPositions.Contains(new Vector2(nextPos.X - BlockSize, nextPos.Y)) || !occupiedPositions.Contains(new Vector2(nextPos.X + BlockSize, nextPos.Y));

                if (nextPos == expectedPos && !mergedHorizontally.Contains(nextPos) && nextHasExposedSide)
                {
                    runHeight++;
                }
                else
                {
                    break;
                }
            }


            WorldMesh.Add(new CollisionSurface(new Vector2((int)startPos.X,(int)startPos.Y), runHeight * BlockSize, true));

            j += runHeight;
        }


    }

    public event Action RoomLoaded;

    public void LoadCurrentRoom()
    {
        RoomLoaded?.Invoke();
        _blocks.Clear();
        _bosses.Clear();

        string roomFile = Path.Combine(
            AppContext.BaseDirectory,
            "Content",
            "Rooms",
            _roomFiles[_currRoom]
        );

        List<RoomObjectData> roomData = _roomFactory.LoadRoom(roomFile);

        for (int i = 0; i < roomData.Count; i++)
        {
            object gameObject = _roomFactory.CreateObject(roomData[i]);

            if (gameObject is IBlock block)
            {
                _blocks.Add(block);
            }
            else if (gameObject is IBoss boss)
            {
                _bosses.Add(boss);
            }
        }

        LoadMesh(_blocks);//load collision mesh

        _player.Location = new Vector2(200, 500);

        BossFactory.Instance.SetList(_bosses);
    }

    public void NextRoom()
    {
        if (_currRoom < _roomFiles.Length - 1)
        {
            _currRoom++;
            LoadCurrentRoom();
        }
    }

    public void PreviosRoom()
    {
        if (_currRoom > 0)
        {
            _currRoom--;
            LoadCurrentRoom();
        }
    }

    public void ProcessWorldCollisions()
    {
        //process x collision first
        Vector2 loc = new Vector2(_player.Location.X, _player.Location.Y);
        loc.X += _player.Velocity.X; // project one tick into the future
        _player.Location = loc;
        _player.Hitbox.Update(_player.Location);

        for (int i = 0; i < WorldMesh.Count; i++)
        {
            CollisionSurface surface = WorldMesh[i];
            if (!surface.VerticalOrientation) continue;

            Rectangle surfaceRect = surface.BoundingBox;
            if (_player.Hitbox.BoundingBox.Intersects(surfaceRect) && _player.Hitbox.Intersects(surfaceRect))
            {
                ResolveXAxisCollision(surface, surfaceRect);
                _player.Hitbox.Update(_player.Location);
            }
        }

        // process y collision second
        loc = _player.Location;
        loc.Y += _player.Velocity.Y; // project one tick into the future
        _player.Location = loc;
        _player.Hitbox.Update(_player.Location);

        for (int i = 0; i < WorldMesh.Count; i++)
        {
            CollisionSurface surface = WorldMesh[i];
            if (surface.VerticalOrientation) continue;

            Rectangle surfaceRect = surface.BoundingBox;
            if (_player.Hitbox.BoundingBox.Intersects(surfaceRect) && _player.Hitbox.Intersects(surfaceRect))
            {
                ResolveYAxisCollision(surface, surfaceRect);
                _player.Hitbox.Update(_player.Location);
            }
        }
    }

    private void ResolveXAxisCollision(CollisionSurface surface, Rectangle surfaceRect)
    {
        Rectangle playerBounds = _player.Hitbox.BoundingBox;
        Vector2 newLocation = _player.Location;

        if (_player.Velocity.X > 0)
        {
            //moving right, push left
            newLocation.X = surfaceRect.Left - playerBounds.Width;
        }
        else if (_player.Velocity.X < 0)
        {
            //moving left, push right
            newLocation.X = surfaceRect.Right;
        }

        _player.Location = newLocation;
        _player.Velocity = new Vector2(0, _player.Velocity.Y);
    }

    private void ResolveYAxisCollision(CollisionSurface surface, Rectangle surfaceRect)
    {
        Rectangle playerBounds = _player.Hitbox.BoundingBox;
        Vector2 newLocation = _player.Location;

        if (_player.Velocity.Y > 0)
        {
            //falling, push poiu
            newLocation.Y = surfaceRect.Top - playerBounds.Height;
            _player.IsGrounded = true;
        }
        else if (_player.Velocity.Y < 0)
        {
            //moving up, puish down
            newLocation.Y = surfaceRect.Bottom;
        }

        _player.Location = newLocation;
        _player.Velocity = new Vector2(_player.Velocity.X, 0);
    }

    private void DEBUGHitbox(SpriteBatch sb)
    {
        foreach(CollisionSurface s in WorldMesh)
        {
            s.DrawDebug(sb);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (DEBUG.ShowHitboxes) {DEBUGHitbox(spriteBatch);}
    }

    public void Update(PlayerInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        
        

        ProcessWorldCollisions();

        if (input.IsNewPress(KeyAction.NextRoom))
        {
        NextRoom();
        }
        else if (input.IsNewPress(KeyAction.PreviousRoom))
        {
            PreviosRoom();
        }
    }
}