using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using sprint2.Players;


namespace sprint2.Blocks;

public class BlocksDemo
{
    private Sprite[] _blocks;
    private int _currBlock = 0;

    public BlocksDemo(ContentManager Content)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "Blocks/blocks.xml");
        _blocks = new Sprite[]
        {
            atlas.CreateSprite("Carved-Brick"),
            atlas.CreateSprite("Pearlstone"),
            atlas.CreateSprite("Copper-Brick"),
            atlas.CreateSprite("Green-Brick"),
            atlas.CreateSprite("Red-Brick"),
            atlas.CreateSprite("Blue-Brick"),
        };

        // scale blocks for demo
        foreach (Sprite block in _blocks)
        {
            block.Scale = new Vector2(2, 2);
        }

    }

    public void Update(GameTime gameTime, PlayerInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        if(input.IsNewPress(KeyAction.BlockLeft)) {_currBlock = (_currBlock - 1) % _blocks.Length;}
        if(_currBlock < 0) {_currBlock = _blocks.Length - 1;}
        if(input.IsNewPress(KeyAction.BlockRight)) {_currBlock = (_currBlock + 1) % _blocks.Length;}
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _blocks[_currBlock].Draw(spriteBatch, new Vector2(800, 150));
    }

}

