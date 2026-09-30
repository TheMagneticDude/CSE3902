using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Input;

namespace sprint2.Players;

public enum KeyAction 
{ 
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    Attack,
    ScrollLeft,
    ScrollRight,
    UseItem1,
    UseItem2,
    UseItem3,
    TakeDamage,
    Exit,
    Reset,
}

public class PlayerInput
{
    public Dictionary<KeyAction, MultiBind> KeyBinds { get; private set; }

    public float DAS_delay = 0.3f;

    public PlayerInput()
    {
        KeyBinds = new Dictionary<KeyAction, MultiBind>
        {
            { KeyAction.MoveUp,    new MultiBind(new TriggerKey(Keys.W), new TriggerKey(Keys.Up), new TriggerKey(Keys.Space)) },
            { KeyAction.MoveDown,  new MultiBind(new TriggerKey(Keys.S), new TriggerKey(Keys.Down)) },
            { KeyAction.MoveLeft,  new MultiBind(new TriggerKey(Keys.A), new TriggerKey(Keys.Left)) },
            { KeyAction.MoveRight, new MultiBind(new TriggerKey(Keys.D), new TriggerKey(Keys.Right)) },
            { KeyAction.Attack,    new MultiBind(new TriggerKey(Keys.Z), new TriggerKey(Keys.N), new TriggerKey(MouseButton.Left,useDAS: true, dasTimeMs: 0.5)) },
            { KeyAction.ScrollLeft,new MultiBind(new TriggerKey(Keys.T,useDAS: true, dasTimeMs: 0.5)) },
            { KeyAction.ScrollRight,new MultiBind(new TriggerKey(Keys.Y,useDAS: true, dasTimeMs: 0.5)) },
            { KeyAction.UseItem1,  new MultiBind(new TriggerKey(Keys.D1), new TriggerKey(Keys.NumPad1), new TriggerKey(MouseButton.Right)) },
            { KeyAction.UseItem2,  new MultiBind(new TriggerKey(Keys.D2), new TriggerKey(Keys.NumPad2)) },
            { KeyAction.UseItem3,  new MultiBind(new TriggerKey(Keys.D3), new TriggerKey(Keys.NumPad3)) },
            { KeyAction.TakeDamage, new MultiBind(new TriggerKey(Keys.E)) },
            { KeyAction.Exit,      new MultiBind(new TriggerKey(Keys.Q), new TriggerKey(Keys.Escape)) },
            { KeyAction.Reset,     new MultiBind(new TriggerKey(Keys.R)) }
        };
    }

    public bool IsPressed(KeyAction action)
    {
        return KeyBinds.TryGetValue(action, out var group) && group.IsPressed;
    }

    public bool IsNewPress(KeyAction action)
    {
        return KeyBinds.TryGetValue(action, out var group) && group.IsNewPress;
    }

    public void AddKey(KeyAction action, Keys keyCode)
    {
        if (KeyBinds.TryGetValue(action, out var group))
        {
            group.AddKey(new TriggerKey(keyCode));
        }
        else
        {
            KeyBinds[action] = new MultiBind(new TriggerKey(keyCode));
        }
    }

    public void Update(GameTime gameTime, InputManager inputs)
    {
        foreach (var group in KeyBinds.Values)
        {
            group.Update(gameTime, inputs, DAS_delay);
        }
    }

    public Keys[] ScanKey(KeyboardInfo keyboard)
    {
        return keyboard.CurrentState.GetPressedKeys();
    }
}
