using Microsoft.Xna.Framework;

public interface IState
{
    void Enter();
    void Update(float time);
    void Exit();
}