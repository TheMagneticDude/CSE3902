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
    private float _timer = 0;

    private const float TIME = 2f;

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
            block.Scale = new Vector2(3, 3);
        }

    }

    public void Update(GameTime gameTime, PlayerInput input)
    {
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if(input.IsNewPress(KeyAction.ScrollLeft)) {_currBlock = (_currBlock - 1) % _blocks.Length;}
        if(_currBlock < 0) {_currBlock = _blocks.Length - 1;}
        if(input.IsNewPress(KeyAction.ScrollRight)) {_currBlock = (_currBlock + 1) % _blocks.Length;}
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _blocks[_currBlock].Draw(spriteBatch, new Vector2(1000, 150));
    }

}

