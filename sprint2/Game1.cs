using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using sprint2.Blocks;
using sprint2.Bosses;
using sprint2.Hotbar;
using sprint2.Items;
using sprint2.Music;
using sprint2.Players;
using sprint2.Projectiles;
using sprint2.Weapons;
using sprint2.World;
using sprint2.Rooms;
using static sprint2.Constants;

namespace sprint2;

public class Game1 : Core
{
    //================Boss init===========================
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

    private HotbarUI _hotbarUI;

    private RoomManager _roomManager;
    private List<IBlock> _blocks;   

    //================Projectiles===========================
    private readonly ProjectileManager _projectileManager = new ProjectileManager();


    //================Debug===========================
    public static Texture2D blankTexture;

    public Game1() : base(AppName, WindowWidth, WindowHeight, false)
    {
    }

    protected override void Initialize()
    {
        _inputManager = new InputManager();
        _p1Input = new PlayerInput(_inputManager);
        _worldHandler = new WorldHandler(WindowWidth, WindowHeight);
        _screenRectangle = new Rectangle(0, 0, WindowWidth, WindowHeight);

        // Core.Initialize triggers LoadContent in this project, so all fields
        // needed by LoadContent must be initialized before this call.
        base.Initialize();
        blankTexture = new Texture2D(GraphicsDevice, 1, 1);
        DEBUG.LoadContent(GraphicsDevice, blankTexture);
    }

    protected override void LoadContent()
    {
        BlockFactory.Instance.LoadAllTextures(Content);
        BossFactory.Instance.LoadAllTextures(Content);

        _blocks = new List<IBlock>();
        _bosses = new List<IBoss>();

        ItemFactory.Instance.LoadAllTextures(Content);
        ProjectileFactory.Instance.LoadAllTextures(Content);
        WeaponFactory.Instance.LoadAllTextures(Content);


        // Will create a class that automatically handles player creation later.
        TextureAtlas playerAtlas = TextureAtlas.FromFile(Content, "Player/Player.xml");

        IHotbarEntry[] hotbarEntries =
        [
            new WeaponHotbarEntry("Sword", WeaponFactory.Instance.CreateSword(), false),
            new WeaponHotbarEntry("Dagger", WeaponFactory.Instance.CreateDagger(_projectileManager), false),
            new EmptyHotbarEntry("Empty"),
            ItemFactory.Instance.CreateHealthPot(),
            ItemFactory.Instance.CreateRecallPot()
        ];

        PlayerHotbar hotbar = new PlayerHotbar(hotbarEntries);
        _player1 = new Player(playerAtlas.CreateAnimatedSprite("Idle"), playerAtlas, hotbar);

        RoomFactory roomFactory = new RoomFactory();
        _roomManager = new RoomManager(roomFactory,_blocks,_bosses,_player1);

        _playList =
        [
            Content.Load<Song>("Music/Overworld/Music-Overworld_Day"),
            Content.Load<Song>("Music/Overworld/Music-Overworld_Night"),
            Content.Load<Song>("Music/Overworld/Music-Underground"),
        ];

        _music = new MusicPlayer(_playList, false, 0.5f);
        _music.Start();

        _backgroundTexture = Content.Load<Texture2D>("Background/Forestbackground");

        _hotbarUI = new HotbarUI();

        _roomManager.LoadCurrentRoom();
    }

    protected override void Update(GameTime gameTime)
    {




        base.Update(gameTime);

        _music.Update();

        // Input should update before anything reads it.
        _inputManager.Update(gameTime);
        _p1Input.Update(gameTime);
        _roomManager.Update(_p1Input);

        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].Update(gameTime);
        }

        BossFactory.Instance.UpdateAll(gameTime, _player1.Location);

        // Player may spawn projectiles during this update.
        _player1.Update(gameTime, _p1Input);
        _projectileManager.Update(gameTime);

        _worldHandler.Update(_player1.Location);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            transformMatrix: _worldHandler.GetTransformMatrix()
        );

        SpriteBatch.Draw(_backgroundTexture, _screenRectangle, Color.White);

        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].Draw(SpriteBatch);
        }
        BossFactory.Instance.DrawAll(SpriteBatch);

        _player1.Draw(SpriteBatch);
        _projectileManager.Draw(SpriteBatch);

        SpriteBatch.End();

        SpriteBatch.Begin(
        samplerState: SamplerState.PointClamp
        );

        _hotbarUI.Draw(SpriteBatch, _player1.Hotbar);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
