using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

using System.Diagnostics;//debug output

namespace sprint0;

public class Game1 : Core
{

    private InputManager _inputManager;
    private IPlayer _player1;
    private PlayerInput _p1Input;

    SpriteFont font1;
    Vector2 fontPos;

    //bat Vars
    private Sprite _apple;
    private Vector2 _applePosition;
    private Vector2 _appleVelocity;
    private const float MOVEMENT_SPEED = 5.0f;



    public Game1() : base("Sprint0", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();

        _inputManager = new InputManager();
        _p1Input = new PlayerInput();

        // Assign the initial random velocity to the bat.
        AssignRandomBatVelocity();
    }

    protected override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");


       
        _player1 = new SnekPlayer(atlas, Vector2.Zero);
        




        //apple init
        _apple = atlas.CreateSprite("apple");
        _apple.Scale = new Vector2(4.0f, 4.0f);
        //init pos
        _applePosition = new Vector2(16, 0);



        //font init
        // Create a new SpriteBatch, which can be used to draw textures.
        font1 = Content.Load<SpriteFont>("font/DefaultFont");

        fontPos = new Vector2(20, 20);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _inputManager.Update(gameTime);
        _p1Input.Update(gameTime, _inputManager);

        var keyboard = Input.Keyboard;
        var gamePad1 = Input.GamePads[(int)PlayerIndex.One];

        //global exit keys
        if (_p1Input.KeyBinds[KeyAction.Exit].IsNewPress)
        {
            Exit();
        }
       
        // Create a bounding rectangle for the screen.
        Rectangle screenBounds = new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        //tick entities
        _player1.Update(gameTime, screenBounds, _p1Input);
        

        //bat pos + bounding box
        Vector2 newBatPosition = _applePosition + _appleVelocity;
        Circle batBounds = new Circle(
            (int)(newBatPosition.X + (_apple.Width * 0.5f)),
            (int)(newBatPosition.Y + (_apple.Height * 0.5f)),
            (int)(_apple.Width * 0.5f)
        );

        Vector2 normal = Vector2.Zero;

        // Use distance based checks to determine if the bat is within the
        // bounds of the game screen, and if it is outside that screen edge,
        // reflect it about the screen edge normal.
        if (batBounds.Left < screenBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newBatPosition.X = screenBounds.Left;
        }
        else if (batBounds.Right > screenBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newBatPosition.X = screenBounds.Right - _apple.Width;
        }

        if (batBounds.Top < screenBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newBatPosition.Y = screenBounds.Top;
        }
        else if (batBounds.Bottom > screenBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newBatPosition.Y = screenBounds.Bottom - _apple.Height;
        }

        // If the normal is anything but Vector2.Zero, this means the bat had
        // moved outside the screen edge so we should reflect it about the
        // normal.
        if (normal != Vector2.Zero)
        {
            normal.Normalize();
            _appleVelocity = Vector2.Reflect(_appleVelocity, normal);
        }

        _applePosition = newBatPosition;


        if (_player1.Bounds.Intersects(batBounds))
        {
            // Divide the width  and height of the screen into equal columns and
            // rows based on the width and height of the bat.
            int totalColumns = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_apple.Width;
            int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_apple.Height;

            // Choose a random row and column based on the total number of each
            int column = Random.Shared.Next(0, totalColumns);
            int row = Random.Shared.Next(0, totalRows);

            // Change the bat position by setting the x and y values equal to
            // the column and row multiplied by the width and height.
            _applePosition = new Vector2(column * _apple.Width, row * _apple.Height);

            // Assign a new random velocity to the bat
            AssignRandomBatVelocity();
        }
    }

    private void AssignRandomBatVelocity()
    {
        if (_player1 != null) 
        {
            _player1.incScore(); // increment player 
            Debug.WriteLine("Player1: " + _player1.getScore());
        }

        // Generate a random angle.
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

        // Convert angle to a direction vector.
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        // Multiply the direction vector by the movement speed.
        _appleVelocity = direction * MOVEMENT_SPEED;
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        GraphicsDevice.Clear(Color.DarkSeaGreen);

        // Begin the sprite batch to prepare for rendering.
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the slime sprite.
        _player1.Draw(SpriteBatch);

        // Draw the bat sprite.
        _apple.Draw(SpriteBatch, _applePosition);

        string scoreText = "Score: " + _player1.getScore();
        SpriteBatch.DrawString(font1, scoreText, fontPos, Color.Black);

        // Always end the sprite batch when finished.
        SpriteBatch.End();

        base.Draw(gameTime);
    }

}
