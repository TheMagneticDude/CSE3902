using Microsoft.Xna.Framework;

namespace sprint2.Bosses.States;

public class CthulhuPhase2State: IBossState
{
    private EyeOfCthulhu _eye;
    private float _timer;

    public CthulhuPhase2State(EyeOfCthulhu eye)
    {
        _eye = eye;
    }

    public void Enter()
    {
        _timer = 0;
        _eye.SetAnimation("cthulhu-phase-2");
    }

    public void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _timer += elapsed;

        //timer to test states
        _eye.Location += new Vector2(-100 * elapsed, 0);

        if (_timer >= 1)
        {
            _eye.ChangeState(new CthulhuPhase1State(_eye));
        }
    }

}