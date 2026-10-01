using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using sprint2.Weapons;

namespace sprint2.Projectiles;

public class ProjectileFactory
{
    private Texture2D _daggerTexture;

    private static readonly ProjectileFactory instance = new ProjectileFactory();

    public static ProjectileFactory Instance
    {
        get
        {
            return instance;
        }
    }

    private ProjectileFactory()
    {
        
    }
    
    public void LoadAllTextures(ContentManager content)
    {
        _daggerTexture = TextureAtlas.FromFile(content, "Weapons/Magic_Dagger.xml");
    }

    public Dagger CreateDagger(Vector2 position, Vector2 velocity)
    {
        return new Dagger(_daggerTexture, position, velocity);
    }
}