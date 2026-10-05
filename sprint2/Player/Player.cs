using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using sprint2.Hotbar;
using sprint2.StateMachines;
using sprint2.Weapons;
using static sprint2.Constants;

namespace sprint2.Players;

public class Player : IPlayer
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Jump
    }

    private readonly PlayerHotbar _hotbar;

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

    internal AnimatedSprite Sprite { get; private set; }
    public bool IsGrounded { get; private set; }
    public PlayerInput Input { get; private set; }

    private TextureAtlas _atlas;
    private StateMachine<PlayerState> _stateMachine;

    // Player physics values.
    private const float _movementSpeed = 5f;
    private const float _jumpStrength = -25f;

    private bool _facingRight = true;

    public bool FacingRight
    {
        get { return _facingRight; }
    }

    // Weapon stuff.
    private IWeapon _equippedWeapon;

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
        _stateMachine.ChangeState(PlayerState.Idle);

        _hotbar.UseSelected(this);
    }

    private void HandleHotbarInput()
    {
        if (Input.IsNewPress(KeyAction.UseItem1))
        {
            _hotbar.UseSlot(0, this);
        }
        else if (Input.IsNewPress(KeyAction.UseItem2))
        {
            _hotbar.UseSlot(1, this);
        }
        else if (Input.IsNewPress(KeyAction.UseItem3))
        {
            _hotbar.UseSlot(2, this);
        }
        else if (Input.IsNewPress(KeyAction.UseItem4))
        {
            _hotbar.UseSlot(3, this);
        }
        else if (Input.IsNewPress(KeyAction.UseItem5))
        {
            _hotbar.UseSlot(4, this);
        }
    }

    public void Update(GameTime gameTime, PlayerInput input)
    {
        Input = input;
        HandleHotbarInput();

        if (_equippedWeapon != null && Input.IsNewPress(KeyAction.Attack))
        {
            _equippedWeapon.Attack(Location, _facingRight);
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
                _player._stateMachine.ChangeState(PlayerState.Jump);
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
}
