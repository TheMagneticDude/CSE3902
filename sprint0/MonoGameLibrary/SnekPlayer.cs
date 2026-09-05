using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace sprint0;

//snake player
public class SnekPlayer : IPlayer
{

    private Sprite[] spritelist;
    
    private Vector2 _position;
    private uint _score;
    private const float MOVEMENT_SPEED = 5.0f;
    private const float BOOST_MULTIPLIER = 5.5f;

    public PlayerInput Input { get; private set; }
    public Vector2 Position => _position;
    public Circle Bounds => new Circle(
        (int)(_position.X + (spritelist[0].Width * 0.5f)),
        (int)(_position.Y + (spritelist[0].Height * 0.5f)),
        (int)(spritelist[0].Width * 0.5f)
    );

    public enum MoveDir 
    { 
        MoveUp, MoveDown, MoveLeft, MoveRight
    }

    private MoveDir currDir;


    public uint Score => _score;

    public SnekPlayer(TextureAtlas atlas, Vector2 startPosition)
    {
        //init spritelist 

        spritelist =
        [
            atlas.CreateSprite("snek_head"),
            atlas.CreateSprite("snek_mid"),
            atlas.CreateSprite("snek_tail"),
            atlas.CreateSprite("snek_down_right"),
            atlas.CreateSprite("snek_down_left"),
            atlas.CreateSprite("snek_up_right"),
            atlas.CreateSprite("snek_up_left"),
            atlas.CreateSprite("snek_tail"),
        ];

        //move right by default
        currDir = MoveDir.MoveRight;


        //set all sprite scales to 4x
        for (int i = 0; i < spritelist.Length; i++)
        {
            spritelist[i].Scale = new Vector2(4.0f, 4.0f);
        }

        _position = startPosition;
        _score = 0; //initialize score to 0
    }

    public void Update(GameTime gameTime, Rectangle screenBounds, PlayerInput input)
    {
        Input = input;
        HandleInput();
        KeepInBounds(screenBounds);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //moving left is 0
        //up is 90
        //right is 180
        //down is 270
        switch (currDir)
        {
            case MoveDir.MoveLeft:
                spritelist[0].Draw(spriteBatch, _position, 0);
            break;
            case MoveDir.MoveUp:
                spritelist[0].Draw(spriteBatch, _position, 90);
            break;
            case MoveDir.MoveRight:
                spritelist[0].Draw(spriteBatch, _position, 180);
            break;
            case MoveDir.MoveDown:
                spritelist[0].Draw(spriteBatch, _position, 270);
            break;
        }
        
        
    }

    private void HandleInput()
    {
        float speed = MOVEMENT_SPEED;
        
        //handles all keybinds
        
        if (Input.KeyBinds[KeyAction.Zoom].IsPressed) 
        {
            speed *= BOOST_MULTIPLIER;
        }

        //get keybind states
        if (Input.KeyBinds[KeyAction.MoveUp].IsPressed) {currDir = MoveDir.MoveUp;}
        if (Input.KeyBinds[KeyAction.MoveDown].IsPressed) {currDir = MoveDir.MoveDown;}
        if (Input.KeyBinds[KeyAction.MoveLeft].IsPressed) {currDir = MoveDir.MoveLeft;}
        if (Input.KeyBinds[KeyAction.MoveRight].IsPressed) {currDir = MoveDir.MoveRight;}

        //apply movement
        switch (currDir)
        {
            case MoveDir.MoveUp:
                _position.Y -= speed;
                break;
            case MoveDir.MoveDown:
                _position.Y += speed;
                break;
            case MoveDir.MoveLeft:
                _position.X -= speed;
                break;
            case MoveDir.MoveRight:
                _position.X += speed;
                break;
        }
    }

    private void KeepInBounds(Rectangle screenBounds)
        {
            // Just use the properties directly! They already know they are scaled 4x.
            float actualWidth = spritelist[0].Width;
            float actualHeight = spritelist[0].Height;

            // Left wall
            if (_position.X < screenBounds.Left) 
                _position.X = screenBounds.Left;
            // Right wall
            else if (_position.X + actualWidth > screenBounds.Right) 
                _position.X = screenBounds.Right - actualWidth;

            // Top wall
            if (_position.Y < screenBounds.Top) 
                _position.Y = screenBounds.Top;
            // Bottom wall
            else if (_position.Y + actualHeight > screenBounds.Bottom) 
                _position.Y = screenBounds.Bottom - actualHeight;
        }


    public uint getScore()
    {
        return _score;
    }

    public void setScore(uint s)
    {
        _score = s;
    }

    public void incScore()//increments score
    {
        _score++;
    }
}