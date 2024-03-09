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
            Name = "Empty";
        }

    }
}
