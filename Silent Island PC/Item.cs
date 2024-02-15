using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace Silent_Island_PC
{
    
    public class Item : Objekt
    {
        public int amount { get; set; }
        public string SlotAnzahl { get; set; }
        public static Dictionary<int, string> Items { get; } = new Dictionary<int, string>()
        {
            { 0, "Empty" },
            { 1, "Fishing_Rod" },
            { 2, "Chair" },
        };

        public Item(Vector2 koordinaten, Texture2D textur) : base(koordinaten, textur)
        {
            Textur = textur;
            Koordinaten = koordinaten;
            Färbung = Color.White;
            Rotation = MathHelper.ToRadians(0);
            Drehpunkt = new Vector2(0, 0);
            Skalierung = new Vector2(1, 1);
            Effekt = SpriteEffects.None;
            Stufe = 0;
            Hitbox = new Vector2(koordinaten.X + textur.Width, koordinaten.Y + textur.Height);
            aktiv = true;
            ID = 0;
            amount = 0;
            SlotAnzahl = new string("" + amount);
        }
        
    }
}
