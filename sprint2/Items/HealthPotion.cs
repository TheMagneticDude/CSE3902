using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Players;

namespace sprint2.Items;

public class HealthPotion : IItem
{
    private const float DisplayDuration = 0.5f;

    private readonly Texture2D _texture;
    private float _timer;

    public string Name
    {
        get { return "Health Potion"; }
    }
    public bool IsActive { get; private set; }

    public HealthPotion(Texture2D texture)
    {
        _texture = texture;
        IsActive = false;
    }

    public void Use(IPlayer player)
    {
        IsActive = true;
        _timer = DisplayDuration;
    }

    public void Update(GameTime gameTime, IPlayer player)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        ArgumentNullException.ThrowIfNull(player);
        if (!IsActive)
        {
            return;
        }

        _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer <= 0f)
        {
            IsActive = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch, IPlayer player)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        ArgumentNullException.ThrowIfNull(player);
        if (!IsActive)
        {
            return;
        }

        float horizontalOffset = 20f;

        if (!player.FacingRight)
        {
            horizontalOffset = -20f;
        }

        Vector2 position = player.Location + new Vector2(horizontalOffset, 0f);
        spriteBatch.Draw(_texture, position, Color.White);
    }
}
