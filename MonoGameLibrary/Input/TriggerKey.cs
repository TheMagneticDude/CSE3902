//format from old tetrio++ keybinds

using System.Collections.Concurrent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Input; 

namespace sprint0;

//select from three input types
public enum InputDeviceType
{
    Keyboard,
    Mouse,
    GamePadButton
}

public class TriggerKey
{
    public InputDeviceType DeviceType { get; private set; }
    
    //storage for each type of bind
    public Keys KeyCode { get; private set; }
    public MouseButton MouseButtonCode { get; private set; }
    public Buttons GamePadButtonCode { get; private set; }    



    public bool IsPressed { get; private set; }
    public bool IsNewPress { get; private set; }
    public bool UseDAS { get; set; }
    public float HoldTime { get; private set; }
    private bool _wasPressedLastFrame;

    //keyboard constructor
    public TriggerKey(Keys keyCode, bool useDAS = false)
    {
        DeviceType = InputDeviceType.Keyboard;
        KeyCode = keyCode;
        UseDAS = useDAS;
    }
    //nmouse constructor
    public TriggerKey(MouseButton mouseButton, bool useDAS = false)
    {
        DeviceType = InputDeviceType.Mouse;
        MouseButtonCode = mouseButton;
        UseDAS = useDAS;
    }

    //gamepad constructor
    public TriggerKey(Buttons gamePadButton, bool useDAS = false)
    {
        DeviceType = InputDeviceType.GamePadButton;
        GamePadButtonCode = gamePadButton;
        UseDAS = useDAS;
    }


    //set keycodes for each input typoe
    public void SetKeyCode(Keys k)
    {
        KeyCode = k;
    }

    public void SetKeyCode(MouseButton m)
    {
        MouseButtonCode = m;
    }

    public void SetKeyCode(Buttons b)
    {
        GamePadButtonCode = b;
    }

    
    public void Update(GameTime gameTime, InputManager inputs)
    {
        bool currentlyPressed = false;

        switch (DeviceType)
        {
            case InputDeviceType.Keyboard:
                currentlyPressed = inputs.Keyboard.IsKeyDown(KeyCode);
                break;
            case InputDeviceType.Mouse:
                currentlyPressed = inputs.Mouse.IsButtonDown(MouseButtonCode);
                break;
            case InputDeviceType.GamePadButton:
                //Only supports one controller for now...
                //will scan through all available controllers for if one is open
                int i = 0;
                while (!currentlyPressed && (i < inputs.GamePads.Length))
                {
                    currentlyPressed = inputs.GamePads[i].IsButtonDown(GamePadButtonCode);
                    i++;
                }
                break;
        }

        IsNewPress = currentlyPressed && !_wasPressedLastFrame;
        IsPressed = currentlyPressed;
        _wasPressedLastFrame = currentlyPressed;

        if (IsNewPress)
        {
            HoldTime = 0f;
        }
        else if (IsPressed)
        {
            HoldTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            HoldTime = 0f;
        }
    }

    public void ResetHold()
    {
        HoldTime = 0f;
        IsNewPress = true; //DAS repeats
    }
}