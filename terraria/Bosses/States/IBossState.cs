using Microsoft.Xna.Framework;

namespace terraria.Bosses.States;

public interface IBossState
{
    void Enter();
    void Update(GameTime gameTime);
}