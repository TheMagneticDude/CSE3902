using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using sprint2.Combat;
using sprint2.StateMachines;
using sprint2.Weapons;
namespace sprint2.Players;

public class Player : CombatEntity, IPlayer
{

    public enum PlayerState
    {
        Idle,
        Walk,
        Jump
    }

    public enum PlayerItem
    {
        Empty,
        Melee,
        Bow,
        Staff
    }

    private const int HotbarSlotCount = 3;
    private readonly PlayerItem[] _hotbar = new PlayerItem[HotbarSlotCount];
    public int SelectedHotbarSlot { get; private set; }
    public PlayerItem SelectedHotbarItem =>
        _hotbar[SelectedHotbarSlot];
    public Vector2 Location { get; set; }
    public Vector2 Velocity { get; set; }

    internal AnimatedSprite Sprite { get; private set; }
    public bool IsGrounded { get; private set; }
    public PlayerInput Input { get; private set; }

    private TextureAtlas _atlas;

    private StateMachine<PlayerState> _stateMachine;

    //player physics values
    private const float MovementSpeed = 5f;
    private const float Gravity = 0.98f;//acceleration
    private const float JumpStrength = -25f;
    private const float GroundLevel = 450f;

    private bool _facingRight = true;

    //Weapon stuff
    private IWeapon _equippedWeapon;


    public Player(AnimatedSprite sprite, TextureAtlas atlas)
    {
        Sprite = sprite;
        _atlas = atlas;
        Location = new Vector2(400, 500);
        Velocity = new Vector2(0, 0);
        _hotbar[0] = PlayerItem.Melee;
        _hotbar[1] = PlayerItem.Bow;
        _hotbar[2] = PlayerItem.Staff;
        SelectedHotbarSlot = 0;
        //statemachine init
        _stateMachine = new StateMachine<PlayerState>();
        _stateMachine.AddState(PlayerState.Idle, new IdleState(this));
        _stateMachine.AddState(PlayerState.Walk, new WalkState(this));
        _stateMachine.AddState(PlayerState.Jump, new JumpState(this));
        _stateMachine.ChangeState(PlayerState.Idle);
    }
    private void HandleHotbarInput()
    {
        if (Input.IsNewPress(KeyAction.UseItem1))
        {
            SelectedHotbarSlot = 0;
        }
        else if (Input.IsNewPress(KeyAction.UseItem2))
        {
            SelectedHotbarSlot = 1;
        }
        else if (Input.IsNewPress(KeyAction.UseItem3))
        {
            SelectedHotbarSlot = 2;
        }
    }
    public void Update(GameTime gameTime, PlayerInput input)
    {
        Input = input;
        if(_equippedWeapon != null && Input.IsNewPress(KeyAction.Attack))
        {
            _equippedWeapon.Attack(Location, _facingRight);
        }
        if (_equippedWeapon != null)
        {
            _equippedWeapon.Update(gameTime, _facingRight);
        }

        if (Input.IsPressed(KeyAction.UseItem1)){EquipWeapon(WeaponFactory.Instance.CreateSword());}
        if (Input.IsPressed(KeyAction.UseItem2)){EquipWeapon(WeaponFactory.Instance.CreateDagger());}

        ApplyPhysics();
        HandleHotbarInput();

        _stateMachine.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

        Sprite.Update(gameTime);
    }

    private void ApplyPhysics()
    {
        Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity);
        Location += Velocity;

        if (Location.Y >= GroundLevel)
        {
            Location = new Vector2(Location.X, GroundLevel);
            Velocity = new Vector2(Velocity.X, 0f);
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }
    void HandleHorizontalMovement()
    {
        float componentX = (Input.IsPressed(KeyAction.MoveRight) ? 1f : 0f) - (Input.IsPressed(KeyAction.MoveLeft) ? 1f : 0f);

        Velocity = new Vector2(componentX * MovementSpeed, Velocity.Y);

        if (Velocity.X > 0) _facingRight = true;
        else if (Velocity.X < 0) _facingRight = false;
    }

    public void EquipWeapon(IWeapon weapon)
    {
        _equippedWeapon = weapon;
    }

    public void UnequipWeapon()
    {
        _equippedWeapon = null;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effect = _facingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        Sprite.Draw(spriteBatch, Location, effect);

        if (_equippedWeapon != null)
        {
            _equippedWeapon.Draw(spriteBatch, Location, _facingRight);
        }
    }













    //=====================================state implementations=====================================

    private class IdleState : IState
    {
        private Player _player;
        public IdleState(Player player) { _player = player; }

        public void Enter() { _player.Sprite = _player._atlas.CreateAnimatedSprite("Idle"); }
        public void Exit() { }

        public void Update(float deltaTime)
        {
            _player.HandleHorizontalMovement();

            if (!_player.IsGrounded)
            {
                _player._stateMachine.ChangeState(PlayerState.Jump);//falling
                return;
            }

            if (_player.Input.IsPressed(KeyAction.MoveUp))
            {
                _player._stateMachine.ChangeState(PlayerState.Jump);
            }
            else if (_player.Velocity.X != 0)
            {
                _player._stateMachine.ChangeState(PlayerState.Walk);
            }
        }
    }

    private class WalkState : IState
    {
        private Player _player;
        public WalkState(Player player) { _player = player; }

        public void Enter() { _player.Sprite = _player._atlas.CreateAnimatedSprite("Walk"); }
        public void Exit() { }

        public void Update(float deltaTime)
        {
            _player.HandleHorizontalMovement();

            if (!_player.IsGrounded)
            {
                _player._stateMachine.ChangeState(PlayerState.Jump);
                return;
            }

            if (_player.Input.IsPressed(KeyAction.MoveUp))
            {
                _player._stateMachine.ChangeState(PlayerState.Jump);
            }
            else if (_player.Velocity.X == 0)
            {
                _player._stateMachine.ChangeState(PlayerState.Idle);
            }
        }
    }


    private class JumpState : IState
    {
        private Player _player;
        public JumpState(Player player) { _player = player; }

        public void Enter() { _player.Sprite = _player._atlas.CreateAnimatedSprite("Jump"); _player.Velocity = new Vector2(_player.Velocity.X, JumpStrength); _player.IsGrounded = false; }
        public void Exit() { }

        public void Update(float deltaTime)
        {
            // allow horizontal control while in the air
            _player.HandleHorizontalMovement();

            if (_player.IsGrounded)
            {
                if (_player.Velocity.X != 0)
                    _player._stateMachine.ChangeState(PlayerState.Walk);
                else
                    _player._stateMachine.ChangeState(PlayerState.Idle);
            }
        }
    }
}
