
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using static sprint2.Constants;

namespace sprint2.World;

//interface layer to convert between pixel space (absolute position on screen) and world space (actual coordinates in game)
public class WorldHandler
{
    public Vector2 ScreenCenter { get; private set; }
    public Vector2 CameraPosition { get; set; }


    public WorldHandler(int screenWidth, int screenHeight)
    {
        ScreenCenter = new Vector2(screenWidth / 2f, screenHeight / 2f);
        CameraPosition = Vector2.Zero;
    }

    public void Update(Vector2 Pos)
    {
        CameraPosition = Vector2.Lerp(CameraPosition, Pos, FollowSpeed);
    }

    public Vector2 WorldToPixelSpace(Vector2 worldPosition)
    {
        return worldPosition - CameraPosition + ScreenCenter;
    }

    public Vector2 PixelToWorldSpace(Vector2 screenPosition)
    {
        return screenPosition + CameraPosition - ScreenCenter;
    }


    //apparently this works on every sprite in the game
    public Matrix GetTransformMatrix()
    {
        return Matrix.CreateTranslation(new Vector3(-CameraPosition.X, -CameraPosition.Y, 0)) *
               Matrix.CreateTranslation(new Vector3(ScreenCenter.X, ScreenCenter.Y, 0));
    }

   
}