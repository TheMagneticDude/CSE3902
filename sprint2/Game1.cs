using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using sprint2.Blocks;
using sprint2.Bosses;
using sprint2.Players;
using sprint2.Weapons;
namespace sprint2;

public class Game1 : Core
{

//================Boss init===========================
    private BlocksDemo _blocksDemo;

    private QueenBee _queenBee;

    private EyeOfCthulhu _eyeOfCthulhu;

//================Player init===========================
    private InputManager _inputManager;
    private IPlayer _player1;
    private PlayerInput _p1Input;

    public Game1() : base ("game", 1280, 720, false)
    {
        
    }
    protected override void Initialize()
    {
        base.Initialize();

        _inputManager = new InputManager();
        _p1Input = new PlayerInput();
    }

    protected override void LoadContent()
    {
        _blocksDemo = new BlocksDemo(Content);
        BossFactory.Instance.LoadAllTextures(Content);
        _queenBee = BossFactory.Instance.CreateQueenBee();
        _eyeOfCthulhu = BossFactory.Instance.CreateCthulhu();
        
        //will create class that automatically handles creation of players later
        TextureAtlas _playerAtlas = TextureAtlas.FromFile(Content,"Player/Player.xml");
        _player1 = new Player(_playerAtlas.CreateAnimatedSprite("Idle"), _playerAtlas);

        //Weapon stuff
        WeaponFactory.Instance.LoadAllTextures(Content);
        _player1.EquipWeapon(WeaponFactory.Instance.CreateDagger());


    }

    protected override void Update(GameTime gameTime)
    {

        base.Update(gameTime);

        _blocksDemo.Update(gameTime, _p1Input);
        _queenBee.Update(gameTime);
        _eyeOfCthulhu.Update(gameTime);

        _inputManager.Update(gameTime);
        _p1Input.Update(gameTime, _inputManager);

        //tick entities
        _player1.Update(gameTime, _p1Input);

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _blocksDemo.Draw(SpriteBatch);
        _queenBee.Draw(SpriteBatch);
        _eyeOfCthulhu.Draw(SpriteBatch);
        _player1.Draw(SpriteBatch);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
