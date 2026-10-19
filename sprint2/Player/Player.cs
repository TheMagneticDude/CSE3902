using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using sprint2.Hotbar;
using sprint2.Combat;
using sprint2.StateMachines;
using sprint2.Weapons;
using sprint2.Collision;
using static sprint2.Constants;


namespace sprint2.Players;

public class Player : IPlayer
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Jump,
        Fall,
        UseItem
    }

    private readonly PlayerHotbar _hotbar;
    public PlayerHotbar Hotbar
    {
        get { return _hotbar; }
    }

    public int SelectedHotbarSlot
    {
        get { return _hotbar.SelectedSlot; }
    }

    public IHotbarEntry SelectedHotbarItem
    {
        get { return _hotbar.SelectedEntry; }
    }

    

    public Vector2 Location { get; set; }
    public Vector2 Velocity { get; set; }

    public CapsuleCollider Hitbox {get; set;}

    internal AnimatedSprite Sprite { get; private set; }
    public bool IsGrounded { get; set; }
    public PlayerInput Input { get; private set; }

    private TextureAtlas _atlas;
    private StateMachine<PlayerState> _stateMachine;

    // Player physics values.
    private const float _movementSpeed = 5f;
    private const float _jumpStrength = -20f;

    private bool _facingRight = true;
    private bool _voidFloor = true;

    public bool FacingRight
    {
        get { return _facingRight; }
    }

    // Weapon stuff.
    private IWeapon _equippedWeapon;
    private readonly HealthComponent _health = new HealthComponent(100, slowDuration: 0.75f);

    public int CurrentHealth => _health.CurrentHealth;
    public int MaxHealth => _health.MaxHealth;
    public bool IsDead => _health.IsDead;
    public Player(AnimatedSprite sprite, TextureAtlas atlas, PlayerHotbar hotbar)
    {
        Sprite = sprite;
        _atlas = atlas;
        _hotbar = hotbar;

        Location = new Vector2(400, 500);
        Velocity = new Vector2(0, 0);

        // State machine init.
        _stateMachine = new StateMachine<PlayerState>();
        _stateMachine.AddState(PlayerState.Idle, new IdleState(this));
        _stateMachine.AddState(PlayerState.Walk, new WalkState(this));
        _stateMachine.AddState(PlayerState.Jump, new JumpState(this));
        _stateMachine.AddState(PlayerState.Fall, new FallState(this));
        _stateMachine.AddState(PlayerState.UseItem, new UseItemState(this));
        _stateMachine.ChangeState(PlayerState.Idle);

        _hotbar.SelectSlot(0, this);

        Hitbox = new CapsuleCollider(
        Location,
        Sprite.Width/2f,
        Sprite.Height * 0.5f
        );
    }

    private void HandleToggleDebugInput()
    {
        //show hitboxes (same keybind as minecraft lol)
        if (Input.IsPressed(KeyAction.TriggerDebug) && Input.IsNewPress(KeyAction.ShowHitboxes))
        {
            DEBUG.ShowHitboxes = !DEBUG.ShowHitboxes;
        }

        if (Input.IsPressed(KeyAction.TriggerDebug) && Input.IsNewPress(KeyAction.VoidFloor))
        {
            _voidFloor = !_voidFloor;
        }
        
    }
    public void TakeDamage(int damage)
    {
        if (_health.TakeDamage(damage))
        {
            System.Diagnostics.Debug.WriteLine(
                $"Health: {CurrentHealth}/{MaxHealth}");
        }
    }

    public void Heal(int amount)
    {
        _health.Heal(amount);
    }
    private void HandleHotbarInput()
    {
        if (Input.IsNewPress(KeyAction.SelectSlot1))
        {
            _hotbar.SelectSlot(0, this);
        }
        else if (Input.IsNewPress(KeyAction.SelectSlot2))
        {
            _hotbar.SelectSlot(1, this);
        }
        else if (Input.IsNewPress(KeyAction.SelectSlot3))
        {
            _hotbar.SelectSlot(2, this);
        }
        else if (Input.IsNewPress(KeyAction.SelectSlot4))
        {
            _hotbar.SelectSlot(3, this);
        }
        else if (Input.IsNewPress(KeyAction.SelectSlot5))
        {
            _hotbar.SelectSlot(4, this);
        }

        if (Input.IsNewPress(KeyAction.UseItem))
        {
            _hotbar.UseSelected(this);
        }
    }

    public void Update(GameTime gameTime, PlayerInput input)
    {
        


        ArgumentNullException.ThrowIfNull(gameTime);
        ArgumentNullException.ThrowIfNull(input);
        Input = input;
        // Input.Update(gameTime);
        _health.Update(gameTime);

        if (Input.IsNewPress(KeyAction.Reset))
        {
            _health.Reset();
        }

        if (Input.IsNewPress(KeyAction.TakeDamage))
        {
            TakeDamage(10);
        }

        // Temporary death behavior to stop movement
        if (IsDead)
        {
            Velocity = Vector2.Zero;
            return;
        }
        HandleHotbarInput();
        HandleToggleDebugInput();

        Hitbox.Update(Location);

        if (_equippedWeapon != null && Input.IsNewPress(KeyAction.Attack))
        {
            _equippedWeapon.Attack(Location, _facingRight, Input.GetCursorPos());
            _stateMachine.ChangeState(PlayerState.UseItem);
        }

        if (_equippedWeapon != null)
        {
            _equippedWeapon.Update(gameTime, _facingRight);
        }

        _hotbar.Update(gameTime, this);

        ApplyPhysics();

        _stateMachine.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        Sprite.Update(gameTime);
    }

    private void ApplyPhysics()
    {
        Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity);
        Location += Velocity;
        if (_voidFloor)
        {
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
        
    }


    private void HandleHorizontalMovement()
    {
        float componentX =
            (Input.IsPressed(KeyAction.MoveRight) ? 1f : 0f) -
            (Input.IsPressed(KeyAction.MoveLeft) ? 1f : 0f);

        Velocity = new Vector2(componentX * _movementSpeed, Velocity.Y);

        if (Velocity.X > 0)
        {
            _facingRight = true;
        }
        else if (Velocity.X < 0)
        {
            _facingRight = false;
        }
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

        if (DEBUG.ShowHitboxes) {Hitbox.DrawDebug(spriteBatch);}

        // Vector2 origin = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
        // Sprite.Origin = origin;

        SpriteEffects effect =
            _facingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        Sprite.Draw(spriteBatch, Location, effect);

        if (_equippedWeapon != null)
        {
            _equippedWeapon.Draw(spriteBatch, Location, _facingRight);
        }

        _hotbar.Draw(spriteBatch, this);
    }

    //=====================================state implementations=====================================

    private class IdleState : IState
    {
        private Player _player;

        public IdleState(Player player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.Sprite = _player._atlas.CreateAnimatedSprite("Idle");
        }

        public void Exit()
        {
        }

        public void Update(float deltaTime)
        {
            _player.HandleHorizontalMovement();

            if (!_player.IsGrounded)
            {
                _player._stateMachine.ChangeState(PlayerState.Fall);
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

        public WalkState(Player player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.Sprite = _player._atlas.CreateAnimatedSprite("Walk");
        }

        public void Exit()
        {
        }

        public void Update(float deltaTime)
        {
            _player.HandleHorizontalMovement();

            if (!_player.IsGrounded)
            {
                _player._stateMachine.ChangeState(PlayerState.Fall);
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

        public JumpState(Player player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.Sprite = _player._atlas.CreateAnimatedSprite("Jump");
            _player.Velocity = new Vector2(_player.Velocity.X, _jumpStrength);
            _player.IsGrounded = false;
        }

        public void Exit()
        {
        }

        public void Update(float deltaTime)
        {
            // Allow horizontal control while in the air.
            _player.HandleHorizontalMovement();

            if (_player.Velocity.Y > 0)
            {
                _player._stateMachine.ChangeState(PlayerState.Fall);
                return;
            }
            if(_player.IsGrounded)
            {
                if (_player.Velocity.X != 0)
                {
                    _player._stateMachine.ChangeState(PlayerState.Walk);
                }
                else
                {
                    _player._stateMachine.ChangeState(PlayerState.Idle);
                }
            }
        }
    }

    private class FallState : IState
    {
        private Player _player;

        public FallState(Player player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.Sprite = _player._atlas.CreateAnimatedSprite("Fall");
        }

        public void Exit()
        {
        }

        public void Update(float deltaTime)
        {
            // Allow horizontal control while in the air.
            _player.HandleHorizontalMovement();

            if (_player.IsGrounded)
            {
                if (_player.Velocity.X != 0)
                {
                    _player._stateMachine.ChangeState(PlayerState.Walk);
                }
                else
                {
                    _player._stateMachine.ChangeState(PlayerState.Idle);
                }
            }
        }
    }

    private class UseItemState : IState
    {
        private Player _player;

        public UseItemState(Player player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.Sprite = _player._atlas.CreateAnimatedSprite("UseItem");
        }

        public void Exit()
        {
        }

        public void Update(float deltaTime)
        {
            //Move slower while attacking or using item
            float attackSpeedModifier = 0.9f;
            // Allow horizontal control
            _player.HandleHorizontalMovement();
            _player.Velocity = new Vector2(_player.Velocity.X * attackSpeedModifier, _player.Velocity.Y);
            
            {
                if (_player.Velocity.X != 0)
                {
                    _player._stateMachine.ChangeState(PlayerState.Walk);
                }
                else
                {
                    _player._stateMachine.ChangeState(PlayerState.Idle);
                }
            }
        }
    }
}
