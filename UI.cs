using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Silent_Island_PC
{

    public class UI : Objekt
    {
        public static Dictionary<int, string> UIs { get; } = new Dictionary<int, string>()
        {
            { 0, "Empty" },
            { 1, "Inventory" },
            { 2, "Hotbar" },
            { 3, "HotbarSlot" },
        };
        public UI(Vector2 koordinaten, Texture2D textur) : base(koordinaten, textur)
        {
            texture = textur;
            coords = koordinaten;
            color = Color.White;
            rotation = MathHelper.ToRadians(0);
            axis = new Vector2(textur.Width / 2f, textur.Height / 2f);
            scale = new Vector2(1, 1);
            effekt = SpriteEffects.None;
            layer = 0;
            hitbox = new Vector2(koordinaten.X + textur.Width, koordinaten.Y + textur.Height);
            activ = true;
            ID = 0;
            name = "Empty";
        }
        //TODO Inventar und alles
        public void HotbarSwitch(int hotbarSlot)
        {

        }


    }
} 

