
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Content;

namespace sprint2.Blocks;

public class BlockFactory
{
    private TextureAtlas blockAtlas;

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


}