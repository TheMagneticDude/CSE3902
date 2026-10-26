
using System;
using System.Collections.Generic;

namespace terraria.Rooms;

public class Room
{
    public String Name { get; }
    public List<RoomObjectData> Objects { get; }

    public Room(String name)
    {
        Name = name;
        Objects = new List<RoomObjectData>();
    }
}