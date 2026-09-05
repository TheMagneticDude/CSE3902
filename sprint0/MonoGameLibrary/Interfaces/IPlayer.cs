using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;


namespace sprint0;


public interface IPlayer
{
    Vector2 Position { get; } //object position
    Circle Bounds {get;} //bounding (circle) box

    uint Score {get;}
    
    
    void Update(GameTime gameTime, Rectangle screenBounds, PlayerInput input);
    void Draw(SpriteBatch spriteBatch);

    public uint getScore();
    public void setScore(uint s);
    public void incScore();//increments score
}