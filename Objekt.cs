using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Silent_Island
{
    public class Objekt
    {
        public Main main { get; set; }
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
       

        public Objekt(Vector2 koordinaten, Texture2D texture, int id, string name)
        {
            this.texture = texture;
            this.pos = koordinaten;
            this.color = Color.White;
            this.rotation = MathHelper.ToRadians(0);
            this.axis = Vector2.Zero;
            this.scale = Vector2.One;
            this.effekt = SpriteEffects.None;
            this.Hitbox = new Rectangle((int)koordinaten.X, (int)koordinaten.Y, texture.Width, texture.Height);
            this.ID = id;
            this.activ = true;
            this.name = name;
        }
        public Objekt(Main main)
        {
            this.main = main;
        }
        //virtual -> override in sub class to "override" the methode 
        public virtual Objekt Clone()
        {
            return new Objekt(this.pos, this.texture, this.ID, this.name);
        }
        public void Zeichne()
        {
            if (activ)
                Main.spriteBatch.Draw(this.texture, this.pos, null, this.color, this.rotation, this.axis, this.scale, this.effekt, 0);
        }
        public bool hit()
        {
            return this.Hitbox.Contains(InputManager.MousePos);
        }
        public static bool colideObjekt(Objekt objekt1, Objekt objekt2)
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
        public void DrawHitboxOutline(Color c)
        {
            // Hitbox-Ränder (Positionen der Linien)
            int x = Hitbox.X;
            int y = Hitbox.Y;
            int width = Hitbox.Width;
            int height = Hitbox.Height;

            int lineWidth = 1;

            // Obere Linie
            Main.spriteBatch.Draw(Textures.Pixel, new Rectangle(x, y, width, lineWidth), c);

            // Untere Linie
            Main.spriteBatch.Draw(Textures.Pixel, new Rectangle(x, y + height - lineWidth, width, lineWidth), c);

            // Linke Linie
            Main.spriteBatch.Draw(Textures.Pixel, new Rectangle(x, y, lineWidth, height), c);

            // Rechte Linie
            Main.spriteBatch.Draw(Textures.Pixel, new Rectangle(x + width - lineWidth, y, lineWidth, height), c);
        }
        protected void UpdateHitbox()
        {
            Hitbox = new Rectangle(
                (int)pos.X,
                (int)pos.Y,
                (int)(texture.Width * scale.X),
                (int)(texture.Height * scale.Y)
            );
        }
        public void Update(Vector2 baseVector, float x, float y)
        {
            this.pos = new Vector2(baseVector.X + x, baseVector.Y + y);
            this.UpdateHitbox();
        }
    }
}