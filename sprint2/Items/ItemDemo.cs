using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using sprint2.Players;


namespace sprint2.Items;

public class ItemDemo
{
    private Sprite[] _items;
    private int _currItem = 0;

    public ItemDemo(ContentManager Content)
    {
        TextureAtlas healthAtlas = TextureAtlas.FromFile(Content, "Items/healthPotion.xml");

        TextureAtlas recallAtlas = TextureAtlas.FromFile(Content, "Items/recallPotion.xml");

        _items = new Sprite[]
        {
            healthAtlas.CreateSprite("healthPotion"),
            recallAtlas.CreateSprite("recallPotion"),
        };

        // scale items for demo
        foreach (Sprite item in _items)
        {
            item.Scale = new Vector2(1, 1);
        }

    }

    public void Update(GameTime gameTime, PlayerInput input)
    {

        if(input.IsNewPress(KeyAction.ItemLeft)) {_currItem = (_currItem - 1) % _items.Length;}
        if(_currItem < 0) {_currItem = _items.Length - 1;}
        if(input.IsNewPress(KeyAction.ItemRight)) {_currItem = (_currItem + 1) % _items.Length;}
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _items[_currItem].Draw(spriteBatch, new Vector2(1000, 150));
    }

}

