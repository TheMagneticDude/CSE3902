using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace sprint0;

//snake player
public class SnekPlayer : IPlayer
{

    private Sprite[] spritelist;

    private struct SnekSegment
    {
        public Vector2 Position;
        public MoveDir Direction;
    }

    private List<SnekSegment> _body;
    
    private Vector2 _position;
    private uint _score;
    private float _moveTimer = 0f; //moves the snek in game ticks on the grid
    private const float MOVEMENT_SPEED = 0.5f; 
    private const float BOOST_SPEED = 0.05f;

    public PlayerInput Input { get; private set; }
    public Vector2 Position => _position;
    public Circle Bounds => new Circle(
        (int)(_position.X + (spritelist[0].Width * 0.5f)),
        (int)(_position.Y + (spritelist[0].Height * 0.5f)),
        (int)(spritelist[0].Width * 0.5f)
    );

    public enum MoveDir 
    { 
        MoveUp, MoveDown, MoveLeft, MoveRight
    }

    private MoveDir currDir;


    public uint Score => _score;

    public SnekPlayer(TextureAtlas atlas, Vector2 startPosition)
    {
        //init spritelist 

        spritelist =
        [
            atlas.CreateSprite("snek_head"),
            atlas.CreateSprite("snek_mid"),
            atlas.CreateSprite("snek_tail"),
            atlas.CreateSprite("snek_down_right"),
            atlas.CreateSprite("snek_down_left"),
            atlas.CreateSprite("snek_up_right"),
            atlas.CreateSprite("snek_up_left"),
            atlas.CreateSprite("snek_tail"),
        ];

        _body = new List<SnekSegment>();
        _body.Add(new SnekSegment { Position = startPosition, Direction = currDir });

        //move right by default
        currDir = MoveDir.MoveRight;


        //set all sprite scales to 4x
        for (int i = 0; i < spritelist.Length; i++)
        {
            spritelist[i].Scale = new Vector2(4.0f, 4.0f);
        }

        _position = startPosition;
        _score = 0; //initialize score to 0

    }

    public void Update(GameTime gameTime, Rectangle screenBounds, PlayerInput input)
    {
        Input = input;
        HandleInput();
       
        //set movement multiplier 
        float tickSpeed = Input.KeyBinds[KeyAction.Zoom].IsPressed ? BOOST_SPEED : MOVEMENT_SPEED;

        //accumulate time
        _moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_moveTimer >= tickSpeed)
        {
            _moveTimer = 0f;
            SnekStep(screenBounds);
        }

    }

    private void SnekStep(Rectangle screenBounds)
    {
        Vector2 newPos = _body[0].Position;
        float stepSize = spritelist[1].Width; //move by width of middle body segement length 

        switch (currDir)
        {
            case MoveDir.MoveUp: 
                newPos.Y -= stepSize; 
            break;
            case MoveDir.MoveDown: 
                newPos.Y += stepSize; 
            break;
            case MoveDir.MoveLeft: 
                newPos.X -= stepSize; 
            break;
            case MoveDir.MoveRight: 
                newPos.X += stepSize; 
            break;
        }

        newPos = KeepInBounds(newPos, screenBounds);

        //add new segmnent
        _body.Insert(0, new SnekSegment { Position = newPos, Direction = currDir });

        _position = newPos;
        

        //cull trailing body segnments
        while (_body.Count > _score + 1)
        {
            _body.RemoveAt(_body.Count - 1);
        } 
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //moving left is 0
        //up is 90
        //right is 180
        //down is 270

        //custom head rotation pivot so body connects
        float gapHeadOffset = 24f;

        float gapHeadSecondaryOffset = 12f;

        float gapTailOffset = 6f;

        float gapCornerOffset = 11f;

        Vector2 headOffset = Vector2.Zero;
        Vector2 tailOffset = Vector2.Zero;
        Vector2 cornerOffset = Vector2.Zero;
        cornerOffset.Y = gapCornerOffset;

        //offset body/tail in direction of movement to not have gap
        switch (currDir)
        {
            case MoveDir.MoveLeft:  
                headOffset.X = -gapHeadOffset;  
                break; //body is to the right
            //case MoveDir.MoveRight: 
            // visualOffset.X = -gapHeadOffset; break; //body is to the left no offset needed
            case MoveDir.MoveUp:
                headOffset.Y = 0; headOffset.X = -gapHeadSecondaryOffset; 
                tailOffset.X = - gapTailOffset;
                
                break; //body is below
            case MoveDir.MoveDown:  
                headOffset.Y = 0; headOffset.X = -gapHeadSecondaryOffset; 
                tailOffset.X = - gapTailOffset;
                
            break; //body is above
        }

        for (int i = 1; i < _body.Count; i++)
        {
            SnekSegment current = _body[i];
            if (i == _body.Count - 1)
            {
                //draw tail
                spritelist[2].Draw(spriteBatch, _body[i].Position  + tailOffset, GetRotation(_body[i].Direction));
            }
            else
            {
                SnekSegment ahead = _body[i-1];//look ahead
                
                if (current.Direction == ahead.Direction)
                {
                // draw body straight
                spritelist[1].Draw(spriteBatch, current.Position, GetRotation(current.Direction));


                }
                else//if the ahead piece is not aligned, the snek turned
                {
                    Sprite corner = GetCornerSprite(current.Direction, ahead.Direction);//select correct sprite depending on orientation
                    corner.Draw(spriteBatch, current.Position + cornerOffset);
                }
            }
            
        }

        //draw head sprite last so it appears on top of the body

        spritelist[0].Draw(spriteBatch, _position + headOffset, GetRotation(currDir));
    }

    private float GetRotation(MoveDir dir)
    {
        switch (dir)
        {
            case MoveDir.MoveLeft:
                return MathHelper.ToRadians(0);
            case MoveDir.MoveUp:
                return MathHelper.ToRadians(90);
            case MoveDir.MoveRight:
                return MathHelper.ToRadians(180);
            case MoveDir.MoveDown:
                return MathHelper.ToRadians(270);
            default:
            return 0;

        };
    }

    //literally look at every sprite ahead and behind to determine what direction it goes
    private Sprite GetCornerSprite(MoveDir currentDir, MoveDir aheadDir)
    {
        //Right to Up
        if (currentDir == MoveDir.MoveRight && aheadDir == MoveDir.MoveUp) return spritelist[6];
        
        // Right to Down
        if (currentDir == MoveDir.MoveRight && aheadDir == MoveDir.MoveDown) return spritelist[4];

        // Left to Up
        if (currentDir == MoveDir.MoveLeft && aheadDir == MoveDir.MoveUp) return spritelist[5];
        
        // Left to Down
        if (currentDir == MoveDir.MoveLeft && aheadDir == MoveDir.MoveDown) return spritelist[3];

        // Up to Right
        if (currentDir == MoveDir.MoveUp && aheadDir == MoveDir.MoveRight) return spritelist[3];
        
        // Up to Left
        if (currentDir == MoveDir.MoveUp && aheadDir == MoveDir.MoveLeft) return spritelist[4];

        // Down to Right
        if (currentDir == MoveDir.MoveDown && aheadDir == MoveDir.MoveRight) return spritelist[5];
        
        // Down to Left
        if (currentDir == MoveDir.MoveDown && aheadDir == MoveDir.MoveLeft) return spritelist[6];

    return spritelist[1]; 
}

    private void HandleInput()
    {
        float speed = MOVEMENT_SPEED;
        
        //handles all keybinds
        
        //get keybind states
        if (Input.KeyBinds[KeyAction.MoveUp].IsPressed) {currDir = MoveDir.MoveUp;}
        if (Input.KeyBinds[KeyAction.MoveDown].IsPressed) {currDir = MoveDir.MoveDown;}
        if (Input.KeyBinds[KeyAction.MoveLeft].IsPressed) {currDir = MoveDir.MoveLeft;}
        if (Input.KeyBinds[KeyAction.MoveRight].IsPressed) {currDir = MoveDir.MoveRight;}
    }

    private Vector2 KeepInBounds(Vector2 pos, Rectangle screenBounds)
        {
           
            float actualWidth = spritelist[0].Width;
            float actualHeight = spritelist[0].Height;

            Vector2 outPos = pos;

            //loop walls
            // Left wall
            if (pos.X < screenBounds.Left) 
                //outPos.X = screenBounds.Left;
                outPos.X = screenBounds.Right - actualWidth;
            // Right wall
            else if (pos.X + actualWidth > screenBounds.Right) 
                //outPos.X = screenBounds.Right - actualWidth;
                outPos.X = screenBounds.Left;

            // Top wall
            if (pos.Y < screenBounds.Top) 
                //outPos.Y = screenBounds.Top;
                outPos.Y = screenBounds.Bottom - actualHeight;
            // Bottom wall
            else if (pos.Y + actualHeight > screenBounds.Bottom) 
                //outPos.Y = screenBounds.Bottom - actualHeight;
                outPos.Y = screenBounds.Top;

            return outPos;
        }


    public uint getScore()
    {
        return _score;
    }

    public void setScore(uint s)
    {
        _score = s;
    }

    public void incScore()//increments score
    {
        _score++;
    }
}