using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

using static sprint2.Constants;

namespace sprint2.Collision;


public interface ICollider
{
    Vector2 Position { get; set; }
    Rectangle BoundingBox { get; }
    bool Intersects(ICollider other);
    bool Intersects(Rectangle otherRectangle);
    void DrawDebug(SpriteBatch spriteBatch);

    void Update(Vector2 pos);
}
