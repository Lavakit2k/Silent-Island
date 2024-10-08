﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Silent_Island
{

    public class Button
    {
        private Main main { get; set; }
        public Vector2 pos { get; set; }
        public Vector2 size { get; set; }
        public Texture2D texture { get; set; }
        public string text { get; set; }
        public SpriteFont font { get; set; }
        public SpriteBatch spriteBatch { get; set; }
        public Rectangle Hitbox { get; private set; }
        public bool activ { get; set; }
        public int ID { get; set; }

        private Vector2 textSize;
        private Vector2 textPosition;

        public static int buttonHit;

        public static Dictionary<int, Button> LoadedButtons = new Dictionary<int, Button>();

        public enum Sections
        {
            debug,
            menu
        }



        public Button(Vector2 position, Vector2 size, Texture2D texture, SpriteFont font, string text, int id)
        {
            this.pos = position;
            this.size = size;
            this.texture = texture;
            this.text = text;
            this.font = font;
            this.activ = false;
            this.ID = id;

            UpdateHitbox();
            UpdateTextPosition();
        }

        public Button(Main m)
        {
            this.main = m;
        }


        public void Zeichne(SpriteBatch spriteBatch)
        {
            if (this.activ)
            {
                spriteBatch.Draw(texture, Hitbox, Color.White);
                spriteBatch.DrawString(font, text, textPosition, Color.Black);
            }
        }
        public bool hit(Vector2 maus)
        {
            return Hitbox.Contains(maus);
        }
        public static void CheckButtonHit(Vector2 MousePos)
        {
            foreach (var KeyValuePair in LoadedButtons)
            {
                if (KeyValuePair.Value.hit(MousePos))
                {
                    buttonHit = KeyValuePair.Value.ID;
                    break;
                }
            }
        }
        private void UpdateTextPosition()
        {
            textSize = font.MeasureString(text);
            textPosition = pos + (size - textSize) / 2f;
        }
        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
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

        public Button DebugLevelDesign;
        public Button Test;
        public Button Save;
        //244
        public void LoadAllButton()
        {
            DebugLevelDesign = new Button(UI.DebugMenu.pos, new Vector2(UI.DebugMenu.texture.Width, 64), Textures.Slot, Main.font, "Level Design", 0);
            LoadedButtons.Add(0, DebugLevelDesign);

            Test = new Button(new Vector2(UI.DebugMenu.pos.X, UI.DebugMenu.pos.Y + 64), new Vector2(UI.DebugMenu.texture.Width, 64), Textures.Slot, Main.font, "Level Design", 1);
            LoadedButtons.Add(1, Test);
        }
        public void UpdateAll()
        {
            DebugLevelDesign.SetPosition(new Vector2(
                UI.DebugMenu.pos.X - UI.DebugMenu.texture.Width / 2,
                UI.DebugMenu.pos.Y - UI.DebugMenu.texture.Height / 2
            ));

            Test.SetPosition(new Vector2(
                UI.DebugMenu.pos.X - UI.DebugMenu.texture.Width / 2,
                UI.DebugMenu.pos.Y - UI.DebugMenu.texture.Height / 2 + 64
            ));
        }
        public void ZeichneAll()
        {
            foreach (var KeyValuePair in LoadedButtons)
            {
                KeyValuePair.Value.Zeichne(Main.spriteBatch);
            }
        }
    }
}

