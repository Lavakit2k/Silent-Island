using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Island;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;


namespace Silent_Island
{

    public class Entity : Objekt
    {
        public int health { get; set; }
        public int speed { get; set; }

        public Entity(Vector2 koordinaten, Texture2D textur) : base(koordinaten, textur)
        {
            texture = textur;
            pos = koordinaten;
            color = Color.White;
            rotation = MathHelper.ToRadians(0);
            axis = new Vector2(textur.Width / 2f, textur.Height / 2f);
            scale = new Vector2(1, 1);
            effekt = SpriteEffects.None;
            // Die Hitbox als Rechteck initialisieren
            Hitbox = new Rectangle((int)koordinaten.X, (int)koordinaten.Y, textur.Width, textur.Height);
            ID = 0;
            name = "Entity";
            health = 0;
            speed = 5;
        }

            public void MovePlayer(bool moving, Texture2D newTexture, int x, int y)
        {
            moving = true;
            this.texture = newTexture;
            this.pos = new Vector2(this.pos.X + x, this.pos.Y + y);
            this.Hitbox = new Rectangle((int)this.pos.X, (int)this.pos.Y + 32, this.texture.Width, this.texture.Height - 32);
        }

        public bool ColideLayer(Layer layer, Vector2 moveVector)
        {
            // Berechne die zukünftige Position der Spielfigur nach der Bewegung
            Rectangle futureHitbox = new Rectangle(
                (int)(this.pos.X + moveVector.X),
                (int)(this.pos.Y + moveVector.Y),
                this.Hitbox.Width,
                this.Hitbox.Height
            );

            // Prüfe auf Kollision mit jeder relevanten Kachel
            int minX = futureHitbox.Left / 64;
            int maxX = futureHitbox.Right / 64;
            int minY = futureHitbox.Top / 64;
            int maxY = futureHitbox.Bottom / 64;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (x >= 0 && x < layer.objekt.GetLength(0) && y >= 0 && y < layer.objekt.GetLength(1))
                    {
                        if (layer.IDLayer[x, y] == 2 && futureHitbox.Intersects(layer.objekt[x, y].Hitbox))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

    }


}
