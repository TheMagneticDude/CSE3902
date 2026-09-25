using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Input;

namespace sprint0;

//keybind input scanning formnat converted from past project 

public enum KeyAction 
{ 
    MoveUp, MoveDown, MoveLeft, MoveRight, Zoom, Exit 
}



public class PlayerInput
{
    // Replaces std::unordered_map
    public Dictionary<KeyAction, TriggerKey> KeyBinds { get; private set; }

    public float DAS_delay = 0.3f;

    public PlayerInput()
    {
        // initialize the map with default binds
        KeyBinds = new Dictionary<KeyAction, TriggerKey>
        {
            { KeyAction.MoveUp, new TriggerKey(Keys.W) },
            { KeyAction.MoveDown, new TriggerKey(Keys.S) },
            { KeyAction.MoveLeft, new TriggerKey(Keys.A, useDAS: true) },
            { KeyAction.MoveRight, new TriggerKey(Keys.D, useDAS: true) },
            { KeyAction.Zoom, new TriggerKey(MouseButton.Left) },
            { KeyAction.Exit, new TriggerKey(Keys.Escape) }
        };
    }

    public void SetKey(KeyAction action, Keys keyCode)
    {
        if (KeyBinds.ContainsKey(action))
        {
            KeyBinds[action].SetKeyCode(keyCode);
        }
    }

    public void Update(GameTime gameTime, InputManager inputs)
    {
        
        foreach (var key in KeyBinds.Values)
        {
            key.Update(gameTime, inputs);
        }
        
        
        HandleDAS(KeyBinds[KeyAction.MoveLeft]);
        HandleDAS(KeyBinds[KeyAction.MoveRight]);
    }

    private void HandleDAS(TriggerKey key)
    {
        if (key.UseDAS && key.HoldTime > DAS_delay) // 0.3s default delay before auto-shift
        {
            key.ResetHold(); // triggers IsNewPress to be true again
        }
    }

    // scan keybvoard states
    public Keys[] ScanKey(KeyboardInfo keyboard)
    {
        return keyboard.CurrentState.GetPressedKeys();
    }
}