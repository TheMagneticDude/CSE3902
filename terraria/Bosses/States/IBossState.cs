using Microsoft.Xna.Framework;

namespace sprint2.Bosses.States;

public interface IBossState
{
    void Enter();
    void Update(GameTime gameTime);
}