using Microsoft.Xna.Framework;

namespace sprint2.Bosses.States;

public class CthulhuPhase1State: IBossState
{
    private EyeOfCthulhu _eye;
    private float _timer;

    public CthulhuPhase1State(EyeOfCthulhu eye)
    {
        _eye = eye;
    }

    public void Enter()
    {
        _timer = 0;
        _eye.SetAnimation("cthulhu-phase-1");
    }

    public void Update(GameTime gameTime)
    {
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        //timer to test states
        if (_timer >= 3)
        {
            _eye.HandleAttack(gameTime);
            _eye.ChangeState(new CthulhuPhase2State(_eye));
        }
    }
}