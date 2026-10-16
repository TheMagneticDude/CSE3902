
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using sprint2.Blocks;
using sprint2.Bosses;
using sprint2.Players;

namespace sprint2.Rooms;

public class RoomManager
{
    private readonly RoomFactory _roomFactory;
    private readonly List<IBlock> _blocks;
    private readonly List<IBoss> _bosses;
    private readonly IPlayer _player;
 
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
    }

    public void LoadCurrentRoom()
    {
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

    public void Update(PlayerInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

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