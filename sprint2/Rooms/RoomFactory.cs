
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using sprint2.Blocks;
using sprint2.Bosses;

namespace sprint2.Rooms;

public class RoomFactory
{

    public List<RoomObjectData> LoadRoom(string fileName)
    {
        List<RoomObjectData> objects = new List<RoomObjectData>();

        string[] lines = File.ReadAllLines(fileName);

        for (int ii = 1; ii < lines.Length; ii++)
        {
            string[] values = lines[ii].Split(',');

            RoomObjectData data = new RoomObjectData();

            data.ObjectType = values[0];
            data.ObjectName = values[1];
            data.Location = values[2];

            objects.Add(data);
        }

        return objects;
    }
    public object CreateObject(RoomObjectData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        Vector2 location = GetLocation(data.Location);
        if (data.ObjectType == "Block")
        {
            return CreateBlock(data.ObjectName, location);
        }

        if (data.ObjectType == "Boss")
        {
            return CreateBoss(data.ObjectName, location);
        }
        throw new Exception("Unknown object type: " + data.ObjectType);
    }

    private IBlock CreateBlock(string objectName, Vector2 location)
    {
        Sprite sprite;

        if (objectName == "Carved-Brick")
        {
            sprite = BlockFactory.Instance.CreateCarvedBrickBlockSprite();
        }
        else if (objectName == "Pearlstone")
        {
            sprite = BlockFactory.Instance.CreatePearlstoneBlockSprite();
        }
        else if (objectName == "Copper-Brick")
        {
            sprite = BlockFactory.Instance.CreateCopperBrickBlockSprite();
        }
        else if (objectName == "Green-Brick")
        {
            sprite = BlockFactory.Instance.CreateGreenBrickBlockSprite();
        }
        else if (objectName == "Red-Brick")
        {
            sprite = BlockFactory.Instance.CreateRedBrickBlockSprite();
        }
        else if (objectName == "Blue-Brick")
        {
            sprite = BlockFactory.Instance.CreateBlueBrickBlockSprite();
        }
        else
        {
            throw new Exception("Unknown block: " + objectName);
        }

        return new Block(sprite, location);
    }

     private IBoss CreateBoss(string objectName, Vector2 location)
    {
        IBoss boss;

        if (objectName == "QueenBee")
        {
            boss = BossFactory.Instance.CreateQueenBee();
        }
        else if (objectName == "Cthulhu")
        {
            boss = BossFactory.Instance.CreateCthulhu();
        }
        else if (objectName == "Slime")
        {
            boss = BossFactory.Instance.CreateSlime();
        }
        else
        {
            throw new Exception("Unknown boss: " + objectName);
        }

        boss.Position = location;

        return boss;
    }

    private Vector2 GetLocation(string location)
    {
        string[] values = location.Split(' ');

        float x = float.Parse(values[0]);
        float y = float.Parse(values[1]);

        return new Vector2(x, y);
    }

}