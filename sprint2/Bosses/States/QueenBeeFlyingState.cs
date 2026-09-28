using Microsoft.Xna.Framework;

namespace sprint2.Bosses.States;

public class QueenBeeFlyingState: IBossState
{
    private QueenBee _queenBee;
    private float _timer;

    public QueenBeeFlyingState(QueenBee queenBee)
    {
        _queenBee = queenBee;
    }

    public void Enter()
    {
        _timer = 0;
        _queenBee.SetAnimation("queen-bee-attack");
    }

    public void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _timer += elapsed;

        //timer to test states
        _queenBee.Location += new Vector2(-100 * elapsed, 0);

        if (_timer >= 1)
        {
            _queenBee.ChangeState(new QueenBeeIdleState(_queenBee));
        }
    }

}