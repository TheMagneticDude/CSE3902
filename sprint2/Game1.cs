using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using sprint2.Blocks;
using sprint2.Bosses;
using sprint2.Players;
using sprint2.Weapons;
using sprint2.Music;
using sprint2.World;
using static sprint2.Constants;
namespace sprint2;

public class Game1 : Core
{

//================Boss init===========================
    private BlocksDemo _blocksDemo;

    private List<IBoss> _bosses;

//================Player init===========================
    private InputManager _inputManager;
    private IPlayer _player1;
    private PlayerInput _p1Input;

//================Music init===========================    
    private MusicPlayer _music;
    private List<Song> _playList;

//================World Handling===========================    
    private WorldHandler _worldHandler;
    private Texture2D _backgroundTexture;
    private Rectangle _screenRectangle;
    public Game1() : base (AppName, WindowWidth, WindowHeight, false)
    {
        
    }
    protected override void Initialize()
    {
        base.Initialize();

        _inputManager = new InputManager();
        _p1Input = new PlayerInput();
        _worldHandler = new WorldHandler(WindowWidth, WindowHeight);
        _screenRectangle = new Rectangle(0, 0, WindowWidth, WindowHeight);
        
    }

    protected override void LoadContent()
    {
        _blocksDemo = new BlocksDemo(Content);
        BossFactory.Instance.LoadAllTextures(Content);
        _bosses =
        [
            BossFactory.Instance.CreateQueenBee(),
            BossFactory.Instance.CreateCthulhu(),
            BossFactory.Instance.CreateSlime(),
            BossFactory.Instance.CreateColoredSlime(Color.Blue),
            BossFactory.Instance.CreateColoredSlime(Color.Red),
            BossFactory.Instance.CreateColoredSlime(Color.SeaGreen),
        ];
        BossFactory.Instance.SetList(_bosses);


        
        //will create class that automatically handles creation of players later
        TextureAtlas _playerAtlas = TextureAtlas.FromFile(Content,"Player/Player.xml");
        _player1 = new Player(_playerAtlas.CreateAnimatedSprite("Idle"), _playerAtlas);

        //Weapon stuff
        WeaponFactory.Instance.LoadAllTextures(Content);
        _player1.EquipWeapon(WeaponFactory.Instance.CreateSword());
        _playList = [
            Content.Load<Song>("Music/Overworld/Music-Overworld_Day"),
            Content.Load<Song>("Music/Overworld/Music-Overworld_Night"),
            Content.Load<Song>("Music/Overworld/Music-Underground"), 
        ];
        _music = new MusicPlayer(_playList, false, 0.5f);
        _music.Start();



        _backgroundTexture = Content.Load<Texture2D>("Background/Forestbackground");
    }

    protected override void Update(GameTime gameTime)
    {

        base.Update(gameTime);
        _music.Update();

        _blocksDemo.Update(gameTime, _p1Input);

        BossFactory.Instance.UpdateAll(gameTime, _player1.Location);

        

        _inputManager.Update(gameTime);
        _p1Input.Update(gameTime, _inputManager);

        //tick entities
        _player1.Update(gameTime, _p1Input);
        _worldHandler.Update(_player1.Location);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _worldHandler.GetTransformMatrix());
        
        SpriteBatch.Draw(_backgroundTexture, _screenRectangle, Color.White);

        _blocksDemo.Draw(SpriteBatch);

        BossFactory.Instance.DrawAll(SpriteBatch);
        
        _player1.Draw(SpriteBatch);

        SpriteBatch.End();


        base.Draw(gameTime);
    }
}
