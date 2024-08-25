using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silent_Island
{
    public class Objekt
    {
        public Main main { get; set; }
        public Textures textures { get; set; }
        public Vector2 pos { get; set; }
        public Texture2D texture { get; set; }
        public Color color { get; set; }
        public float rotation { get; set; }
        public Vector2 axis { get; set; }
        public Vector2 scale { get; set; }
        public SpriteEffects effekt { get; set; }
        public Rectangle Hitbox { get; set; }
        public int ID { get; set; }
        public bool activ { get; set; }
        public string name { get; set; }
       

        public Objekt(Vector2 koordinaten, Texture2D textur)
        {
            this.texture = textur;
            this.pos = koordinaten;
            this.color = Color.White;
            this.rotation = MathHelper.ToRadians(0);
            this.axis = Vector2.Zero;
            this.scale = Vector2.One;
            this.effekt = SpriteEffects.None;
            this.Hitbox = new Rectangle((int)koordinaten.X, (int)koordinaten.Y, textur.Width, textur.Height);
            this.ID = 0;
            this.activ = true;
            this.name = "Empty";
        }
        public Objekt(Textures texture, Main main)
        {
            this.textures = texture;
            this.main = main;

        }

        public void Zeichne(SpriteBatch sprite)
        {
            if (activ)
                sprite.Draw(this.texture, this.pos, null, this.color, this.rotation, this.axis, this.scale, this.effekt, 0);
        }
        public bool hit(Vector2 maus)
        {
            return this.Hitbox.Contains(maus);
        }


        public bool colideObjekt(Objekt objekt1, Objekt objekt2)
        {
            if (objekt1.pos.X > objekt2.pos.X
            && objekt1.Hitbox.X < objekt2.Hitbox.X
            && objekt1.pos.Y > objekt2.pos.Y
                && objekt1.Hitbox.Y < objekt2.Hitbox.Y)
            {
                return true;
            }
            return false;
        }

    }
}