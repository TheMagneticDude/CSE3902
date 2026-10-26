using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2
{
    public static class DEBUG 
    {
        // global toggle for hitboxes
        public static bool ShowHitboxes { get; set; } = false; 
        
        public static Texture2D BlankTexture { get; private set; }

        public static void LoadContent(GraphicsDevice graphicsDevice, Texture2D blankTexture)
        {
            ArgumentNullException.ThrowIfNull(blankTexture);

            BlankTexture = blankTexture;
            BlankTexture.SetData(new[] { Color.White });
        }

        internal static void DrawLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 p1, Vector2 p2, Color color, int thickness = 1)
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
    }
}