using System;
using System.Collections.Generic;
using System.Data;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using static sprint2.Constants;



namespace sprint2.Collision;

public class CapsuleCollider: ICollider
{

    public Vector2 Position { get; set;}
    public float Radius { get; set; }
    public float Height { get; set; } // distance between the top and bottom circle centers

    

  public Circle TopCircle
    {
        get
        {
            Point center = new Point((int)(Position.X + Radius), (int)(Position.Y + Radius));
            return new Circle(center, (int)Radius);
        }
    }

    public Rectangle MiddleRectangle 
    {
        get 
        {
            int width = (int)(Radius * 2);
            int height = (int)Height;
            
            return new Rectangle((int)Position.X, (int)(Position.Y + Radius), width, height);
        }
    }
    

  
    public Circle BottomCircle
    {
        get
        {
            Point center = new Point((int)(Position.X + Radius), (int)(Position.Y + Radius + Height));
            return new Circle(center, (int)Radius);
        }
    }
    public Rectangle BoundingBox 
    {
        get 
        {
            int totalWidth = (int)(Radius * 2);
            int totalHeight = (int)(Height + (Radius * 2));
            
            return new Rectangle((int)Position.X, (int)Position.Y, totalWidth, totalHeight);
        }
    }





    public void Update(Vector2 pos)
    {
        Position = pos;
    }

    public CapsuleCollider(Vector2 pos, float rad, float h)
    {
        Position = pos;
        Radius = rad;
        Height = h;
    }

    
    public bool Intersects(ICollider other)
    {
        if(other == null || !this.BoundingBox.Intersects(other.BoundingBox)){return false;}

        if (other is RotatedRectangleCollider rotated)
        {
            return rotated.Intersects(this);
        }
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
    


    //helper function
    private bool CircleIntersectsRectangle(Circle c, Rectangle r)
    {
        float closestX = MathHelper.Clamp(c.X, r.Left, r.Right);
        float closestY = MathHelper.Clamp(c.Y, r.Top, r.Bottom);

        float distanceX = c.X - closestX;
        float distanceY = c.Y - closestY;

        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
        
        //if within radius distance then is an intersection
        return distanceSquared <= (c.Radius * c.Radius);
        
    }
    public bool Intersects(Rectangle r)
    {

        if(MiddleRectangle.Intersects(r) || 
        CircleIntersectsRectangle(TopCircle, r) || 
        CircleIntersectsRectangle(BottomCircle,r)
        ){return true;}
   
        return false;
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
    private void DrawCircle(SpriteBatch spriteBatch, Texture2D pixel, Circle circle, Color color, int segments = 16)
    {
        float increment = MathHelper.TwoPi / segments;
        float angle = 0f;

        for (int i = 0; i < segments; i++)
        {
            // Calculate the starting point of the line segment
            Vector2 p1 = new Vector2(
                circle.X + (float)Math.Cos(angle) * circle.Radius,
                circle.Y + (float)Math.Sin(angle) * circle.Radius);

            angle += increment;

            // Calculate the ending point of the line segment
            Vector2 p2 = new Vector2(
                circle.X + (float)Math.Cos(angle) * circle.Radius,
                circle.Y + (float)Math.Sin(angle) * circle.Radius);

            DEBUG.DrawLine(spriteBatch, pixel, p1, p2, color);
        }
    }
    public void DrawDebug(SpriteBatch spriteBatch)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);

        if (!DEBUG.ShowHitboxes) return;

        Texture2D pixel = DEBUG.BlankTexture;

        DrawRectangle(spriteBatch, pixel, BoundingBox, Color.Red);
        DrawRectangle(spriteBatch, pixel, MiddleRectangle, Color.Green);
        DrawCircle(spriteBatch, pixel, TopCircle, Color.Green);
        DrawCircle(spriteBatch, pixel, BottomCircle, Color.Green);
    }
}