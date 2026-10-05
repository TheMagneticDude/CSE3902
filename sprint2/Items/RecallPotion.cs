using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using sprint2.Players;

namespace sprint2.Items;

public class RecallPotion: IItem
{
    private readonly Vector2 _recallLocation = new Vector2(640,450);

    private Vector2 _potPosition;

    private readonly Sprite _sprite;

    private float _timer;

    public string Name
    {
        get { return "Recall Potion"; }
    }
    public bool IsActive { get; private set; }

    public RecallPotion(TextureAtlas atlas)
    {
        ArgumentNullException.ThrowIfNull(atlas);
        _sprite = atlas.CreateSprite("recallPotion");
        IsActive = false;
    }

    public void Use(IPlayer player)
    {
        IsActive = true;
        _timer = 0.5f;
    }

    public void Update(GameTime gameTime, IPlayer player)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        ArgumentNullException.ThrowIfNull(player);
        if (IsActive)
        {
            _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer <= 0)
            {
                player.Location = _recallLocation;
                player.Velocity = Vector2.Zero;

                IsActive = false;
            }

        }
    }
    public void Draw(SpriteBatch spriteBatch, IPlayer player)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        ArgumentNullException.ThrowIfNull(player);
        if(IsActive)
        {
            if (player.FacingRight)
            {
                _potPosition = player.Location + new Vector2(17, 8);
            }
            else
            {
                _potPosition = player.Location + new Vector2(-15, 8);
            }
            _sprite.Draw(spriteBatch, _potPosition);
        }
    }
}

