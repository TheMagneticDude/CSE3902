using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace sprint2.UI;

public static class HitFlash
{
    private static readonly Color Tint = new(255, 64, 64);

    public static void Draw(SpriteBatch spriteBatch, Sprite sprite, Vector2 position,
        bool isFlashing, Color baseColor, SpriteEffects effects = SpriteEffects.None,
        float rotation = 0f)
    {
        System.ArgumentNullException.ThrowIfNull(spriteBatch);
        System.ArgumentNullException.ThrowIfNull(sprite);
        Color originalColor = sprite.Color;
        sprite.Color = isFlashing ? Tint : baseColor;
        sprite.Draw(spriteBatch, position, effects, rotation);
        sprite.Color = originalColor;
    }
}
