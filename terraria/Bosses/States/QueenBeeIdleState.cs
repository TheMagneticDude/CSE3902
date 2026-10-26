using System;

using Microsoft.Xna.Framework;

namespace sprint2.Bosses.States;

public class QueenBeeIdleState: IBossState
{
    private QueenBee _queenBee;
    private float _timer;

    public QueenBeeIdleState(QueenBee queenBee)
    {
        _queenBee = queenBee;
    }

    public void Enter()
    {
        _timer = 0;
        _queenBee.SetAnimation("queen-bee-idle");
    }

    public void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        _queenBee.Velocity = Vector2.Zero;

        //timer to test states
        if (_timer >= 3)
        {
            _queenBee.ChangeState(new QueenBeeFlyingState(_queenBee));
        }
    }
}