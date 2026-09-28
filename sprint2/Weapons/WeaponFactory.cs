using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace sprint2.Weapons;

public class WeaponFactory
{
    private Texture2D _swordTexture;

    private static readonly WeaponFactory instance = new WeaponFactory();

    public static WeaponFactory Instance
    {
        get
        {
            return instance;
        }
    }

    private WeaponFactory()
    {
        
    }
    
    public void LoadAllTextures(ContentManager content)
    {
        _swordTexture = content.Load<Texture2D>("Weapons/Sword");
    }

    public Sword CreateSword()
    {
        return new Sword(_swordTexture);
    }
}