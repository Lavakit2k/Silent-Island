using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Island;

namespace Silent_Island_PC
{
    public abstract class Objekt
    {
        public Vector2 coords { get; set; }
        public Texture2D texture { get; set; }
        public Color color { get; set; }
        public float rotation { get; set; }
        public Vector2 axis { get; set; }
        public Vector2 scale { get; set; }
        public SpriteEffects effekt { get; set; }
        public float layer { get; set; }
        public Vector2 hitbox { get; set; }
        public bool activ { get; set; }
        public int ID { get; set; }
        public string name { get; set; }

        
        public Objekt(Vector2 koordinaten, Texture2D textur)
        {
            texture = textur;
            coords = koordinaten;
            color = Color.White;
            rotation = MathHelper.ToRadians(0);
            axis = new Vector2(textur.Width / 2f, textur.Height / 2f);
            scale = Vector2.One;
            effekt = SpriteEffects.None;
            layer = 0;
            activ = true;
            ID = 0;
            name = "Empty";
        }

        public void Zeichne(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.coords, null, this.color, this.rotation, this.axis, this.scale, this.effekt, this.layer);
        }
        public void RZeichne(float rotation, Vector2 drehpunkt, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.coords, null, this.color, MathHelper.ToRadians(rotation), drehpunkt, this.scale, this.effekt, this.layer);
        }
        public void LZeichne(float rotation, Vector2 skalierung, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.coords, null, this.color, rotation, this.axis, skalierung, this.effekt, this.layer);
        }
        public bool hit(Vector2 maus)
        {
            if (maus.X > this.coords.X
                && maus.X < this.hitbox.X
                && maus.Y > this.coords.Y
                && maus.Y < this.hitbox.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool colideObjekt(Objekt objekt1, Objekt objekt2)
        {
            if (objekt1.coords.X > objekt2.coords.X
            && objekt1.hitbox.X < objekt2.hitbox.X
            && objekt1.coords.Y > objekt2.coords.Y
                && objekt1.hitbox.Y < objekt2.hitbox.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}