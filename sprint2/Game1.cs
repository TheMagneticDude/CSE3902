using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using sprint2.Blocks;
using sprint2.Bosses;

namespace sprint2;

public class Game1 : Core
{

    private BlocksDemo _blocksDemo;

    private QueenBee _queenBee;

    private EyeOfCthulhu _eyeOfCthulhu;

    public Game1() : base ("game", 1280, 720, false)
    {
        
    }
    protected override void Initialize()
    {

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _blocksDemo = new BlocksDemo(Content);
        BossFactory.Instance.LoadAllTextures(Content);
        _queenBee = BossFactory.Instance.CreateQueenBee();
        _eyeOfCthulhu = BossFactory.Instance.CreateCthulhu();


    }

    protected override void Update(GameTime gameTime)
    {

        base.Update(gameTime);

        _blocksDemo.Update(gameTime);
        _queenBee.Update(gameTime);
        _eyeOfCthulhu.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _blocksDemo.Draw(SpriteBatch);
        _queenBee.Draw(SpriteBatch);
        _eyeOfCthulhu.Draw(SpriteBatch);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
