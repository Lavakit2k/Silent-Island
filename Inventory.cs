using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent_Island
{
    public class Inventory
    {
        public List<Item> InventoryList = new List<Item>();
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

        public Inventory(GraphicsDevice graphicsDevice)
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

            foreach (var inventoryItem in InventoryList)
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
            if (InventoryList.Count < MaxStacks)
            {
                InventoryList.Add(item.Clone());
                return true;
            }

            // InventoryList full
            return false;
        }

        public void UpdateInventoryInterface(int scrollDelta)
        {
            // Berechne die maximale Scroll - Position
            maxScrollOffset = Math.Max(0, InventoryList.Count - itemsPerPage);

            inventoryHeight = Math.Min(itemsPerPage * 36, InventoryList.Count * 36);

            inventoryPosition = new Vector2(Main.cameraPosition.X + (screenWidth - inventoryWidth) / 2, Main.cameraPosition.Y + (screenHeight - inventoryHeight) / 2);

            

            // Aktualisiere den Scroll-Offset basierend auf dem Scroll-Eingang
            scrollOffset = Math.Clamp(scrollOffset + scrollDelta, 0, maxScrollOffset);
        }

        public void DrawInventoryInterface()
        {
            // background
            Main.spriteBatch.Draw(backgroundTexture, new Rectangle((int)inventoryPosition.X, (int)inventoryPosition.Y, inventoryWidth, inventoryHeight), new Color(255, 255, 255, 0.7f));

            // only draw Page
            for (int i = 0; i < itemsPerPage && (i + scrollOffset) < InventoryList.Count; i++)
            {
                Item item = InventoryList.ElementAt(i + scrollOffset);
                Vector2 itemPosition = new Vector2(inventoryPosition.X + 10, inventoryPosition.Y + i * 36 + 2);
                float iconY = itemPosition.Y + (32 - item.texture.Height) / 2;
                Rectangle iconRectangle = new Rectangle((int)itemPosition.X, (int)iconY, item.texture.Width, item.texture.Height);

                Main.spriteBatch.Draw(item.texture, iconRectangle, Color.White);

                Vector2 textPosition = new Vector2(itemPosition.X + 40, itemPosition.Y + 8);
                string itemText = $"{item.name} x{item.amount}";
                Main.spriteBatch.DrawString(Main.font, itemText, textPosition, Color.White);
            }
        }

        
    }
}