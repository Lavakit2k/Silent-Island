using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent_Island
{
    public class InventorySystem
    {
        public List<Item> Inventory = new List<Item>();
        public int MaxStacks = 20;                      // Maximale Anzahl von Stacks im Inventar
        public int MaxItemsPerStack = 100;              // Maximale Anzahl von Items pro Stack

        private Texture2D backgroundTexture;
        private Vector2 inventoryPosition;

        private int inventoryWidth = 300; // Width of the inventory box
        private int inventoryHeight; // Height based on the number of items

        private int screenWidth;
        private int screenHeight;

        public int scrollOffset;
        private int maxScrollOffset;
        private int itemsPerPage = 5;  // Anzahl der Elemente, die gleichzeitig angezeigt werden können

        public InventorySystem(GraphicsDevice graphicsDevice)
        {
            backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            backgroundTexture.SetData(new[] { Color.Gray });
            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;
        }


        public bool AddItem(Item item, int amount = 1)
        {
            //default 1
            item.amount = amount;

            foreach (var inventoryItem in Inventory)
            {
                if (inventoryItem.ID == item.ID)
                {
                    // into stack
                    if (inventoryItem.amount + item.amount <= MaxItemsPerStack)
                    {
                        inventoryItem.amount += item.amount;
                        return true;
                    }
                    // into new stack
                    else
                    {
                        int overflow = inventoryItem.amount + item.amount - MaxItemsPerStack;
                        inventoryItem.amount = MaxItemsPerStack;
                        item.amount = overflow;
                        break;
                    }
                }
            }

            // new stack for new item
            if (Inventory.Count < MaxStacks)
            {
                Inventory.Add(item.Clone());
                return true;
            }

            // Inventory full
            return false;
        }

        public void UpdateInventoryInterface(Vector2 cam, int scrollDelta)
        {
            // Berechne die maximale Scroll - Position
            maxScrollOffset = Math.Max(0, Inventory.Count - itemsPerPage);

            inventoryHeight = Math.Min(itemsPerPage * 36, Inventory.Count * 36);

            inventoryPosition = new Vector2(cam.X + (screenWidth - inventoryWidth) / 2, cam.Y + (screenHeight - inventoryHeight) / 2);

            

            // Aktualisiere den Scroll-Offset basierend auf dem Scroll-Eingang
            scrollOffset = Math.Clamp(scrollOffset + scrollDelta, 0, maxScrollOffset);
        }

        public void DrawInventoryInterface(SpriteBatch spriteBatch, SpriteFont font)
        {
            // background
            spriteBatch.Draw(backgroundTexture, new Rectangle((int)inventoryPosition.X, (int)inventoryPosition.Y, inventoryWidth, inventoryHeight), new Color(255, 255, 255, 0.7f));

            // only draw Page
            for (int i = 0; i < itemsPerPage && (i + scrollOffset) < Inventory.Count; i++)
            {
                Item item = Inventory.ElementAt(i + scrollOffset);
                Vector2 itemPosition = new Vector2(inventoryPosition.X + 10, inventoryPosition.Y + i * 36 + 2);
                float iconY = itemPosition.Y + (32 - item.texture.Height) / 2;
                Rectangle iconRectangle = new Rectangle((int)itemPosition.X, (int)iconY, item.texture.Width, item.texture.Height);

                spriteBatch.Draw(item.texture, iconRectangle, Color.White);

                Vector2 textPosition = new Vector2(itemPosition.X + 40, itemPosition.Y + 8);
                string itemText = $"{item.name} x{item.amount}";
                spriteBatch.DrawString(font, itemText, textPosition, Color.White);
            }
        }
    }
}