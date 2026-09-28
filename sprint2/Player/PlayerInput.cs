using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Input;

namespace sprint2.Players;

public enum KeyAction 
{ 
    MoveUp, MoveDown, MoveLeft, MoveRight, Attack, Use, Exit,
}

public class PlayerInput
{
    public Dictionary<KeyAction, MultiBind> KeyBinds { get; private set; }

    public float DAS_delay = 0.3f;

    public PlayerInput()
    {
        KeyBinds = new Dictionary<KeyAction, MultiBind>
        {
            //double bind W and spacebar
            { KeyAction.MoveUp,    new MultiBind(new TriggerKey(Keys.W), new TriggerKey(Keys.Space)) },
            { KeyAction.MoveDown,  new MultiBind(new TriggerKey(Keys.S)) },
            { KeyAction.MoveLeft,  new MultiBind(new TriggerKey(Keys.A)) },
            { KeyAction.MoveRight, new MultiBind(new TriggerKey(Keys.D)) },
            { KeyAction.Attack,    new MultiBind(new TriggerKey(MouseButton.Left, useDAS: true, dasTimeMs: 1000)) },
            { KeyAction.Use,       new MultiBind(new TriggerKey(MouseButton.Right, useDAS: true, dasTimeMs: 1000)) },
            { KeyAction.Exit,      new MultiBind(new TriggerKey(Keys.Escape)) }
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