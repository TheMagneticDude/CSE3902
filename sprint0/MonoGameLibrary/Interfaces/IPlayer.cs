using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;


namespace sprint0;


public interface IPlayer
{
    Keybinds Binds {get;} //get keybinds

    Vector2 Position { get; } //object position
    Circle Bounds {get;} //bounding (circle) box
    
    
    void Update(GameTime gameTime, Rectangle screenBounds, Keybinds binds);
    void Draw(SpriteBatch spriteBatch);

}