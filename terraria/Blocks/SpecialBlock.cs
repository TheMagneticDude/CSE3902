
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using static sprint2.Constants;
using sprint2.Collision;

using System;


namespace sprint2.Blocks;

public class SpecialBlock: IBlock
{
    public Vector2 Location { get; set; }

    public Sprite TheSprite { get; set; }

    public RectangularCollider Hitbox {get;set;}
    public PlayerMovementEffects MovementEffect {get;set;}

    public void Update(GameTime gameTime)
    {
        Hitbox.Update(Location);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        TheSprite.Draw(spriteBatch, Location);
        Hitbox.DrawDebug(spriteBatch);
    }

    public SpecialBlock(Sprite sprite, Vector2 location, PlayerMovementEffects effectType)
    {
        ArgumentNullException.ThrowIfNull(sprite);

        this.TheSprite = sprite;
        this.Location = location;
        MovementEffect = effectType;

        Hitbox = new RectangularCollider(
        Location,
        sprite.Width,
        sprite.Height
        );
    }

}