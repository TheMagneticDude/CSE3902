using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;


namespace sprint2.Weapons;

public class WeaponFactory
{
    private TextureAtlas swordAtlas;
    private TextureAtlas daggerAtlas;
    

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
        swordAtlas = TextureAtlas.FromFile(content, "Weapons/sword.xml");
        daggerAtlas = TextureAtlas.FromFile(content, "Weapons/Magic_Dagger.xml");

    }

    public Sword CreateSword()
    {
        return new Sword(swordAtlas.Texture);
    }

    public Dagger CreateDagger()
    {
        return new Dagger(daggerAtlas.Texture);
    }
}
