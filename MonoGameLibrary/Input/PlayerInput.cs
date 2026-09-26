using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Input;

namespace sprint0;

//keybind input scanning formnat converted from past project 

public enum KeyAction 
{ 
    MoveUp, MoveDown, MoveLeft, MoveRight, Attack, Use, Exit,
}





public class PlayerInput
{
    // Replaces std::unordered_map
    public Dictionary<KeyAction, TriggerKey> KeyBinds { get; private set; }

    public float DAS_delay = 0.3f;
    //Delayed auto shift is functionality ported over from our tetrio game
    //effectivly it just means the delay before the key starts repeting 
    //dasTimeMs is the how many miliseconds between each repeted action 

    public PlayerInput()
    {
        // initialize the map with default binds
        KeyBinds = new Dictionary<KeyAction, TriggerKey>
        {
            { KeyAction.MoveUp, new TriggerKey(Keys.W) },
            { KeyAction.MoveDown, new TriggerKey(Keys.S) },
            { KeyAction.MoveLeft, new TriggerKey(Keys.A) },
            { KeyAction.MoveRight, new TriggerKey(Keys.D) },
            { KeyAction.Attack, new TriggerKey(MouseButton.Left, useDAS: true, dasTimeMs: 1000) },
            { KeyAction.Use, new TriggerKey(MouseButton.Right, useDAS: true, dasTimeMs: 1000) },
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
            if (key.UseDAS && (key.LastDasTime>= key.DasTimeMs))
            {
                key.LastDasTime = 0;
                HandleDAS(key);
            }
        }
        
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