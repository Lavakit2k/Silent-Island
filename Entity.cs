using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Silent_Island_PC
{

    public class Entity : Objekt
    {
        public int health { get; set; }
        public int speed { get; set; }

        public static Dictionary<int, string> Entitys { get; } = new Dictionary<int, string>()
        {
            { 0, "Empty" },
            { 1, "Player" },
            { 2, "Blob" },
        };
        public Entity(Vector2 koordinaten, Texture2D textur) : base(koordinaten, textur)
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
            name = "Entity";
            health = 0;
            speed = 0;
        }

    }

}
