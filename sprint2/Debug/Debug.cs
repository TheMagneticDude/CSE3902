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
            BlankTexture = blankTexture;
            BlankTexture.SetData(new[] { Color.White });
        }
    }
}