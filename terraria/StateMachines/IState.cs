using Microsoft.Xna.Framework;
namespace terraria.StateMachines;
public interface IState
{
    void Enter();
    void Update(float time);
    void Exit();
}