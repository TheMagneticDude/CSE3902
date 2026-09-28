using Microsoft.Xna.Framework;
namespace sprint2.StateMachines;
public interface IState
{
    void Enter();
    void Update(float time);
    void Exit();
}