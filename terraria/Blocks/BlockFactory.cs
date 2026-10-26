
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace terraria.Blocks;

public class BlockFactory
{
    private TextureAtlas blockAtlas;
    private TextureAtlas specialBlockAtlas;

    private static BlockFactory instance = new BlockFactory();

    public static BlockFactory Instance
    {
        get
        {
            return instance;
        }
    }

    private BlockFactory() 
    {
        
    }

    public void LoadAllTextures(ContentManager content)
    {
        blockAtlas = TextureAtlas.FromFile(content, "Blocks/blocks.xml");
        specialBlockAtlas = TextureAtlas.FromFile(content, "Blocks/SpecialBlocks.xml");

    }

    public Sprite CreateCarvedBrickBlockSprite()
    {
        return blockAtlas.CreateSprite("Carved-Brick");
    }

    public Sprite CreatePearlstoneBlockSprite()
    {
        return blockAtlas.CreateSprite("Pearlstone");
    }

    public Sprite CreateCopperBrickBlockSprite()
    {
        return blockAtlas.CreateSprite("Copper-Brick");
    }

    public Sprite CreateGreenBrickBlockSprite()
    {
        return blockAtlas.CreateSprite("Green-Brick");
    }

    public Sprite CreateRedBrickBlockSprite()
    {
        return blockAtlas.CreateSprite("Red-Brick");
    }

    public Sprite CreateBlueBrickBlockSprite()
    {
        return blockAtlas.CreateSprite("Blue-Brick");
    }

    public Sprite CreateHellstoneBlockSprite()
    {
        Sprite sprite = specialBlockAtlas.CreateSprite("Hellstone");
        sprite.Scale = new Vector2(0.5f, 0.5f);
        return sprite;
    }

    public Sprite CreateHellstoneBrickBlockSprite()
    {
        Sprite sprite = specialBlockAtlas.CreateSprite("Hellstone-Brick");
        sprite.Scale = new Vector2(0.5f, 0.5f);
        return sprite;
    }

    public Sprite CreateHoneyBlockSprite()
    {
        Sprite sprite =  specialBlockAtlas.CreateSprite("Honey");
        sprite.Scale = new Vector2(0.5f, 0.5f);
        return sprite;
    }


}