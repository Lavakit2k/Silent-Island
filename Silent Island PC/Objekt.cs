using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silent_Island_PC
{
    public abstract class Objekt
    {
        #pragma warning disable CS0649 // Dem Feld "Objekt.emptyTexture" wird nie etwas zugewiesen, und es hat immer seinen Standardwert von "null".
        Texture2D emptyTexture;
        #pragma warning restore CS0649 // Dem Feld "Objekt.emptyTexture" wird nie etwas zugewiesen, und es hat immer seinen Standardwert von "null".
        public Vector2 Koordinaten { get; set; }
        public Texture2D Textur { get; set; }
        public Color Färbung { get; set; }
        public float Rotation { get; set; }
        public Vector2 Drehpunkt { get; set; }
        public Vector2 Skalierung { get; set; }
        public SpriteEffects Effekt { get; set; }
        public float Stufe { get; set; }
        public Vector2 Hitbox { get; set; }
        public bool aktiv { get; set; }
        public int ID { get; set; }
        public Objekt(Vector2 koordinaten, Texture2D textur)
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
        }

        public void Zeichne(SpriteBatch spriteBatch, Objekt objekt)
        {
            if (objekt.aktiv == true)
            {
                spriteBatch.Draw(objekt.Textur, objekt.Koordinaten, null, objekt.Färbung, objekt.Rotation, objekt.Drehpunkt, objekt.Skalierung, objekt.Effekt, objekt.Stufe);
            }
            else if (objekt.aktiv == false)
            {
                spriteBatch.Draw(emptyTexture, objekt.Koordinaten, null, objekt.Färbung, objekt.Rotation, objekt.Drehpunkt, objekt.Skalierung, objekt.Effekt, objekt.Stufe);
            }
        }
        public void RZeichne(SpriteBatch spriteBatch, Objekt objekt, float rotation, Vector2 drehpunkt)
        {
            spriteBatch.Draw(objekt.Textur, objekt.Koordinaten, null, objekt.Färbung, MathHelper.ToRadians(rotation), drehpunkt, objekt.Skalierung, objekt.Effekt, objekt.Stufe);
        }
        public void LZeichne(SpriteBatch spriteBatch, Objekt objekt, float rotation, Vector2 skalierung)
        {
            spriteBatch.Draw(objekt.Textur, objekt.Koordinaten, null, objekt.Färbung, rotation, objekt.Drehpunkt, skalierung, objekt.Effekt, objekt.Stufe);
        }
        public bool hit(Vector2 maus, Objekt objekt)
        {
            if (maus.X > objekt.Koordinaten.X
                && maus.X < objekt.Hitbox.X
                && maus.Y > objekt.Koordinaten.Y
                && maus.Y < objekt.Hitbox.Y)
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