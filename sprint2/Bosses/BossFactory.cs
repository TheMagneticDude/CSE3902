using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace sprint2.Bosses;

public class BossFactory
{
    private TextureAtlas _queenBeeAtlas;

    private TextureAtlas _cthulhuAtlas;

    private TextureAtlas _slimeAtlas;

    private static BossFactory instance = new BossFactory();

     public static BossFactory Instance
    {
        get
        {
            return instance;
        }
    }

    private BossFactory() 
    {
        
    }

    public void LoadAllTextures(ContentManager content)
    {
        _queenBeeAtlas = TextureAtlas.FromFile(content,"Bosses/QueenBee.xml");
        _cthulhuAtlas = TextureAtlas.FromFile(content, "Bosses/Cthulhu.xml");
        _slimeAtlas = TextureAtlas.FromFile(content, "Bosses/Slime.xml");
    }

    public QueenBee CreateQueenBee()
    {
            AnimatedSprite sprite = _queenBeeAtlas.CreateAnimatedSprite("queen-bee-idle");

            sprite.Origin = new Vector2(sprite.Width/2f, sprite.Height);

            return new QueenBee(sprite, _queenBeeAtlas);
    }

    public EyeOfCthulhu CreateCthulhu()
    {
        AnimatedSprite sprite = _cthulhuAtlas.CreateAnimatedSprite("cthulhu-phase-1");

       // sprite.Origin = new Vector2(sprite.Width/2f, sprite.Height);
        sprite.CenterOrigin();

        sprite.Rotation = 1.5708f;

        return new EyeOfCthulhu(sprite, _cthulhuAtlas);
    }

    public Slime CreateSlime()
    {
            AnimatedSprite sprite = _slimeAtlas.CreateAnimatedSprite("slime");

            sprite.CenterOrigin();

            return new Slime(sprite, _slimeAtlas);
    }

}