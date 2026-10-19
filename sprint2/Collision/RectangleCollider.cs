
using System;
using System.Data;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using static sprint2.Constants;

namespace sprint2.Collision;

public class RotatedRectangleCollider : ICollider
{
    // Position is the rectangle's center.
    public Vector2 Position { get; set; }

    public float Width { get; set; }
    public float Height { get; set; }
    public float Rotation { get; set; }

    public void Update(Vector2 pos)
    {
        Position = pos;
    }

    public RotatedRectangleCollider(Vector2 position, float width, float height)
    {
        Position = position;
        Width = width;
        Height = height;
    }

    private Vector2 Rotate(Vector2 point)
    {
        float cos = MathF.Cos(Rotation);
        float sin = MathF.Sin(Rotation);

        return new Vector2(
            point.X * cos - point.Y * sin,
            point.X * sin + point.Y * cos);
    }

    private Vector2[] GetCorners()
    {
        float w = Width / 2f;
        float h = Height / 2f;

        return new[]
        {
            Position + Rotate(new Vector2(-w, -h)),
            Position + Rotate(new Vector2( w, -h)),
            Position + Rotate(new Vector2( w,  h)),
            Position + Rotate(new Vector2(-w,  h))
        };
    }

    public Rectangle BoundingBox
    {
        get
        {
            Vector2[] corners = GetCorners();

            float minX = corners[0].X;
            float maxX = corners[0].X;
            float minY = corners[0].Y;
            float maxY = corners[0].Y;

            foreach (Vector2 p in corners)
            {
                minX = MathF.Min(minX, p.X);
                maxX = MathF.Max(maxX, p.X);
                minY = MathF.Min(minY, p.Y);
                maxY = MathF.Max(maxY, p.Y);
            }

            int left = (int)MathF.Floor(minX);
            int top = (int)MathF.Floor(minY);
            int right = (int)MathF.Ceiling(maxX);
            int bottom = (int)MathF.Ceiling(maxY);

            return new Rectangle(left, top, right - left, bottom - top);
        }
    }

    private static Vector2[] GetRectangleCorners(Rectangle r)
    {
        return new[]
        {
            new Vector2(r.Left, r.Top),
            new Vector2(r.Right, r.Top),
            new Vector2(r.Right, r.Bottom),
            new Vector2(r.Left, r.Bottom)
        };
    }

    private static bool OverlapOnAxis(Vector2[] a, Vector2[] b, Vector2 axis)
    {
        float minA = float.MaxValue;
        float maxA = float.MinValue;
        float minB = float.MaxValue;
        float maxB = float.MinValue;

        foreach (Vector2 point in a)
        {
            float projection = Vector2.Dot(point, axis);
            minA = MathF.Min(minA, projection);
            maxA = MathF.Max(maxA, projection);
        }

        foreach (Vector2 point in b)
        {
            float projection = Vector2.Dot(point, axis);
            minB = MathF.Min(minB, projection);
            maxB = MathF.Max(maxB, projection);
        }

        return maxA >= minB && maxB >= minA;
    }

    private bool IntersectsPolygon(
        Vector2[] other,
        Vector2 otherAxisX,
        Vector2 otherAxisY)
    {
        Vector2[] own = GetCorners();

        Vector2[] axes =
        {
            Rotate(Vector2.UnitX),
            Rotate(Vector2.UnitY),
            otherAxisX,
            otherAxisY
        };

        foreach (Vector2 axis in axes)
        {
            if (!OverlapOnAxis(own, other, axis))
                return false;
        }

        return true;
    }

    public bool Intersects(Rectangle rectangle)
    {
        if (!BoundingBox.Intersects(rectangle))
            return false;

        return IntersectsPolygon(
            GetRectangleCorners(rectangle),
            Vector2.UnitX,
            Vector2.UnitY
        );
    }

    public bool Intersects(RotatedRectangleCollider other)
    {
        if (other == null ||
            !BoundingBox.Intersects(other.BoundingBox))
            return false;

        return IntersectsPolygon(
            other.GetCorners(),
            other.Rotate(Vector2.UnitX),
            other.Rotate(Vector2.UnitY)
        );
    }

    private bool IntersectsCircle(Circle circle)
    {
        // Check the circle in the rectangle's local coordinates.
        Vector2 relative = new Vector2(circle.X, circle.Y) - Position;

        float cos = MathF.Cos(Rotation);
        float sin = MathF.Sin(Rotation);

        Vector2 local = new Vector2(
            relative.X * cos + relative.Y * sin,
            -relative.X * sin + relative.Y * cos
        );

        float x = MathHelper.Clamp(local.X, -Width / 2f, Width / 2f);

        float y = MathHelper.Clamp(local.Y, -Height / 2f, Height / 2f);

        Vector2 delta = local - new Vector2(x, y);

        return delta.LengthSquared() <= circle.Radius * circle.Radius;
    }

    public bool Intersects(CapsuleCollider capsule)
    {
        if (capsule == null ||
            !BoundingBox.Intersects(capsule.BoundingBox))
            return false;

        return Intersects(capsule.MiddleRectangle)
            || IntersectsCircle(capsule.TopCircle)
            || IntersectsCircle(capsule.BottomCircle);
    }

    public bool Intersects(ICollider other)
    {
        if (other == null)
            return false;

        return other switch
        {
            RotatedRectangleCollider rotated =>
                Intersects(rotated),

            CapsuleCollider capsule =>
                Intersects(capsule),

            _ => Intersects(other.BoundingBox)
        };
    }

    public void DrawDebug(SpriteBatch spriteBatch)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        if (!DEBUG.ShowHitboxes)
            return;

        Vector2[] corners = GetCorners();
        Texture2D pixel = DEBUG.BlankTexture;

        for (int i = 0; i < 4; i++)
        {
            Vector2 start = corners[i];
            Vector2 end = corners[(i + 1) % 4];
            DEBUG.DrawLine(spriteBatch, pixel, start, end, Color.Red, thickness: 2);
        }
    }
}
