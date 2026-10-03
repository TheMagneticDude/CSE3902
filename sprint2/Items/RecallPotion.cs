using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using sprint2.Players;

namespace sprint2.Items;

public class RecallPotion: IItem
{
    private Vector2 _recallLocation = new Vector2(640,450);

    private Vector2 _potPosition;

    private Sprite _sprite;

    private IPlayer _player;

    private float _timer;

    public bool IsActive { get; private set; }
    //public bool IsConsumed { get; private set; }

    public RecallPotion(TextureAtlas atlas)
    {
        _sprite = atlas.CreateSprite("recallPotion");
        //IsConsumed = false;
        IsActive = false;
    }

    public void Use(IPlayer player)
    {
        //if (!IsConsumed)
        //{
            _player = player;
            IsActive = true;
            _timer = 0.5f;
       // }
    }
    public void Update(GameTime gameTime)
    {
        if (IsActive)
        {
            _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer <= 0)
            {
                _player.Location = _recallLocation;
                _player.Velocity = Vector2.Zero;

                IsActive = false;
                //IsConsumed = true;
            }

        }
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 playerLocation, bool facingRight)
    {
        //if(IsActive && !IsConsumed)
        if(IsActive)
        {
            if (facingRight)
            {
                _potPosition = playerLocation + new Vector2(17, 8);
            }
            else
            {
                _potPosition = playerLocation + new Vector2(-15, 8);
            }
            _sprite.Draw(spriteBatch, _potPosition);
        }
    }
}

