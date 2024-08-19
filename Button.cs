using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silent_Island
{
    public class Button
    {
        private Main main { get; set; }
        private Textures textures { get; set; }
        public Vector2 pos { get; set; }
        public Vector2 size { get; set; }
        public Texture2D texture { get; set; }
        public string text { get; set; }
        public SpriteFont font { get; set; }
        public SpriteBatch spriteBatch { get; set; }
        public Rectangle Hitbox { get; private set; }

        private Vector2 textSize;
        private Vector2 textPosition;

        public Button(Vector2 position, Vector2 size, Texture2D texture, SpriteFont font, string text)
        {
            this.pos = position;
            this.size = size;
            this.texture = texture;
            this.text = text;
            this.font = font;

            UpdateHitbox();
            UpdateTextPosition();
        }

        public Button(Textures t, Main m)
        {
            this.textures = t;
            this.main = m;
        }

        // Hitbox aktualisieren basierend auf Größe und Position
        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

        // Textposition aktualisieren basierend auf Größe und Position
        private void UpdateTextPosition()
        {
            textSize = font.MeasureString(text);
            textPosition = pos + (size - textSize) / 2f;
        }

        public void Zeichne(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Hitbox, Color.White);
            spriteBatch.DrawString(font, text, textPosition, Color.Black);
        }

        public Button myButton;

        public void LoadAllButton()
        {
            myButton = new Button(new Vector2(100, 100), new Vector2(200, 50), textures.Slot, main.font, "Click Me!");
        }

        public bool hit(Vector2 maus)
        {
            return Hitbox.Contains(maus);
        }

        public void SetPosition(Vector2 newPos)
        {
            pos = newPos;
            UpdateHitbox();
            UpdateTextPosition();
        }

        public void SetSize(Vector2 newSize)
        {
            size = newSize;
            UpdateHitbox();
            UpdateTextPosition();
        }

        public void SetText(string newText)
        {
            text = newText;
            UpdateTextPosition();
        }
    }
}

