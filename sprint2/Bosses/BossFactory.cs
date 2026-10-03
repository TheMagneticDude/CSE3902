using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace sprint2.Bosses;

public class BossFactory
{
    private TextureAtlas _queenBeeAtlas;

    private TextureAtlas _cthulhuAtlas;

    private TextureAtlas _slimeAtlas;

    private static BossFactory instance = new BossFactory();
    
    public List<IBoss> ActiveBosses { get; private set; }

     public static BossFactory Instance
    {
        get
        {
            return instance;
        }
    }

    private BossFactory()
    {
        ActiveBosses = new List<IBoss>();
    }

    public void SetList(List<IBoss> list)
    {
        ActiveBosses = list;
    }

    public void Add(IBoss b)
    {
        ActiveBosses.Add(b);
    }

    public void UpdateAll(GameTime gameTime, Vector2 playerPos)
    {
        for (int i = ActiveBosses.Count - 1; i >= 0; i--)
        {
            ActiveBosses[i].Update(gameTime, playerPos);
        }
    }

    public void DrawAll(SpriteBatch spriteBatch)
    {
        foreach (var boss in ActiveBosses)
        {
            boss.Draw(spriteBatch);
        }
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

    public Slime CreateColoredSlime(Color c)
    {
            AnimatedSprite sprite = _slimeAtlas.CreateAnimatedSprite("slime");

            sprite.CenterOrigin();

            return new Slime(sprite, _slimeAtlas, c);
    }

}