using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Island;
using System.Collections.Generic;


namespace Silent_Island_PC
{

    public class UI : Objekt
    {
        private Textures textures;
        public static Dictionary<int, string> UIs { get; } = new Dictionary<int, string>()
        {
            { 0, "Empty" },
            { 1, "Inventory" },
            { 2, "Hotbar" },
            { 3, "HotbarSlot" },
        };

        public UI(Textures textures, Vector2 koordinaten, Texture2D textur) : base(koordinaten, textur)
        {
            this.textures = textures;
        }

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
            Hitbox = new Rectangle((int)koordinaten.X, (int)koordinaten.Y, textur.Width, textur.Height);
            activ = true;
            ID = 0;
            name = "Empty";
        }

        //TODO an Resolution anpassbar
        public void UpdateUI(Main main, float x, float y)
        {
            this.coords = new Vector2(main.cameraPosition.X + x, main.cameraPosition.Y + y);
        }


        public void HotbarSwitch(int hotbarSlot)
        {

        }


    }
} 

