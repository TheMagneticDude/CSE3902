using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Bosses;

public class BossFactory
{
    private TextureAtlas _queenBeeAtlas;

    private TextureAtlas _cthulhuAtlas;

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
    }

    public QueenBee CreateQueenBee()
    {
            AnimatedSprite sprite = _queenBeeAtlas.CreateAnimatedSprite("queen-bee-idle");

            sprite.CenterOrigin();

            return new QueenBee(sprite, _queenBeeAtlas);
    }

    public EyeOfCthulhu CreateCthulhu()
    {
        AnimatedSprite sprite = _cthulhuAtlas.CreateAnimatedSprite("cthulhu-phase-1");

        //sprite.Origin = new Vector2(75, 134);
        sprite.CenterOrigin();

        sprite.Rotation = 1.5708f;

        return new EyeOfCthulhu(sprite, _cthulhuAtlas);
    }

}