using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Players;

namespace sprint2.Items;

public interface IItem
{

    //bool IsConsumed {get;}
    
    bool IsActive { get; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch, Vector2 playerLocation, bool facingRight);

    void Use(IPlayer player);
}