using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace sprint2.Items;

public class HealthPotion
{
    private Vector2 _location;

    private Texture2D _texture;

    public bool IsActive { get; private set; }
    public bool IsConsumed { get; private set; }

    public HealthPotion(Texture2D texture)
    {
        _texture = texture;
        IsConsumed = false;
        IsActive = false;
    }

    public void Use(Vector2 playerLocation)
    {
        IsActive = true;
    }
    public void Update(GameTime gameTime)
    {
        //will update player health
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 playerLocation)
    {
        if(IsActive && !IsConsumed)
        {
            Vector2 position = playerLocation + new Vector2(20, 0);
            spriteBatch.Draw(_texture, playerLocation, Color.White);
        }
    }
}