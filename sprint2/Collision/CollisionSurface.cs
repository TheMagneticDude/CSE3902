using System;
using System.Data;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using static sprint2.Constants;



namespace sprint2.Collision;

public class CollisionSurface: ICollider
{

    public Vector2 Position { get; set;}

    public bool VerticalOrientation {get; set;}
    public float Width { get; set; }

    
    public Rectangle BoundingBox 
    {
        get 
        {
            int topLeftX, topLeftY, totalWidth, totalHeight;
            if (VerticalOrientation)
            {
                topLeftX = (int)Position.X;
                topLeftY = (int)Position.Y;
                totalWidth = (int) BlockSize;
                totalHeight = (int)Width;
            }
            else
            {
                topLeftX = (int)Position.X;
            topLeftY = (int)Position.Y;
            totalWidth = (int)Width;
            totalHeight = (int) BlockSize;
            }

            return new Rectangle(topLeftX, topLeftY, totalWidth, totalHeight);
        }
    }





    public void Update(Vector2 pos)
    {
        Position = pos;
    }

    public CollisionSurface(Vector2 pos, float w, bool vertical)
    {
        Position = pos;
        Width = w;
        VerticalOrientation = vertical;
    }

    
    public bool Intersects(ICollider other)
    {
        if(other == null || !this.BoundingBox.Intersects(other.BoundingBox)){return false;}

        return other.Intersects(this.BoundingBox);
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
        if(BoundingBox.Intersects(r)){return true;}
   
        return false;
    }

    public bool Intersects(Circle c)
    {
        return CircleIntersectsRectangle(c,BoundingBox);
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

            DrawLine(spriteBatch, pixel, p1, p2, color);
        }
    }
    private void DrawLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 p1, Vector2 p2, Color color, int thickness = 1)
    {
        float distance = Vector2.Distance(p1, p2);
        float angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);

        spriteBatch.Draw(
            texture: pixel,
            position: p1,
            sourceRectangle: null,
            color: color,
            rotation: angle,
            origin: Vector2.Zero,
            scale: new Vector2(distance, thickness), // Stretch the 1x1 pixel to the line's length
            effects: SpriteEffects.None,
            layerDepth: 0f
        );
    }
    public void DrawDebug(SpriteBatch spriteBatch)
    {
        if (!DEBUG.ShowHitboxes) return;

   
        Texture2D pixel = DEBUG.BlankTexture;

        DrawRectangle(spriteBatch, pixel, BoundingBox, Color.Blue);

    }
}