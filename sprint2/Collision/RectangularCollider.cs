using System;
using System.Collections.Generic;
using System.Data;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using static sprint2.Constants;



namespace sprint2.Collision;

public class RectangularCollider: ICollider
{

    public Vector2 Position { get; set;}
    public float Width { get; set; }
    public float Height { get; set; } // distance between the top and bottom circle centers

    
    public Rectangle BoundingBox 
    {
        get 
        {
            
            return new Rectangle((int)Position.X, (int)Position.Y, (int) Width, (int) Height);
        }
    }





    public void Update(Vector2 pos)
    {
        Position = pos;
    }

    public RectangularCollider(Rectangle r)
    {
        Position = new Vector2(r.X,r.Y);
        Width = r.Width;
        Height = r.Height;
    }
    public RectangularCollider(Vector2 pos, float w, float h)
    {
        Position = pos;
        Width = w;
        Height = h;
    }

    
    public bool Intersects(ICollider other)
    {
        if(other == null || !this.BoundingBox.Intersects(other.BoundingBox)){return false;}

        return other.Intersects(this.BoundingBox);
    }

    public List<CollisionSurface> ScanIntersectingSurfaces(List<CollisionSurface> worldMesh)
    {
        ArgumentNullException.ThrowIfNull(worldMesh);
        
        List<CollisionSurface> hitSurfaces = new List<CollisionSurface>();
        

        // grab the outer bounding box once per scan
        Rectangle playerBroadBox = this.BoundingBox;

        for (int i = 0; i < worldMesh.Count; i++)
        {
            Rectangle surfaceRect = worldMesh[i].BoundingBox;

            if (!playerBroadBox.Intersects(surfaceRect))
            {
                continue;
            }

            //check if its close enough to trigger outer bounding box
            //then check capsule bounds
            if (this.Intersects(surfaceRect))
            {
                hitSurfaces.Add(worldMesh[i]);
            }
        }

        return hitSurfaces;
    }

    public bool Intersects(Rectangle r)
    {
   
        return BoundingBox.Intersects(r);
    }







    private void DrawRectangle(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color color, int thickness = 1)
    {
        // Top border
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        
        // Bottom border
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
        
        // Left border
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        
        // Right border
        spriteBatch.Draw(pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
    }
    public void DrawDebug(SpriteBatch spriteBatch)
    {
        if (!DEBUG.ShowHitboxes) return;

        Texture2D pixel = DEBUG.BlankTexture;

        DrawRectangle(spriteBatch, pixel, BoundingBox, Color.Red);

    }
}