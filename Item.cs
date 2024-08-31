using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Silent_Island
{
    public class Item : Objekt
    {
        public int amount { get; set; }
        public string slotAmount { get; set; }
        public Dictionary<int, Item> LoadedItems = new Dictionary<int, Item>();

        public Item(Vector2 koordinaten, Texture2D texture, int id, string name) : base(koordinaten, texture, id, name)
        {
            this.texture = texture;
            pos = koordinaten;
            ID = id;
            this.name = name;
            amount = 1;
            slotAmount = new string("" + amount);

        }
        public Item(Textures t, Main m) : base(t, m)
        {
            this.textures = t;
            this.main = m;
        }

        public void ToolAufnehmen(Item item)
        {
            for (int i = 0; i < 4; i++)
            {
                if (ToolHotbarItem[i].ID == item.ID && item.amount < 100)
                {
                    ++ToolHotbarItem[i].amount;
                    break;
                }
                else if (ToolHotbarItem[i].ID != item.ID && ToolHotbarItem[i].ID == 0)
                {
                    ++ToolHotbarItem[i].amount;
                    ToolHotbarItem[i].ID = item.ID;
                    ToolHotbarItem[i].texture = item.texture;
                    break;
                }
            }
        }

        public void UpdateAll(Vector2 cam, UI ui)
        {
            for (int i = 0; i < ToolHotbarItem.Length; i++)
            {
                ToolHotbarItem[i].Update(ui.ToolHotbar.pos, 8 + (i % 2) * 72, 8 + (i / 2) * 72);
            }
            //ExtraHotbarMarker.UpdateUI(ExtraHotbar.pos, 8 + main.ExtraHotbarSlotNum * 72, 8);
        }
        public void DrawItems(SpriteBatch spriteBatch, SpriteFont font)
        {
            for (int i = 0; i < 4; i++)
            {
                ToolHotbarItem[i].Zeichne(spriteBatch);
                spriteBatch.DrawString(font, "" + ToolHotbarItem[i].amount, new Vector2(ToolHotbarItem[i].pos.X + 58, ToolHotbarItem[i].pos.Y + 50), new Color(0, 0, 0));
            }
        }

        public override Item Clone()
        {
            return new Item(this.pos, this.texture, this.ID, this.name);
        }

        public Item Empty;
        public Item FishingRod;
        public Item Fish;
        public Item Shark;
        public Item FishingLine;
        public Item Shovel;
        public Item SeaShell1;
        public Item SeaShell2;
        public Item SeaShell3;
        public Item SeaShell4;
        public Item Pistol;
        public Item Rock;
        public Item IronIngot;
        public Item Wood;

        public Item[] ToolHotbarItem;

        public void LoadAllItems()
        {

            Empty = new Item(Vector2.Zero, textures.Empty, 0, "Empty");
            LoadedItems.Add(0, Empty);

            FishingRod = new Item(Vector2.Zero, textures.FishingRod, 1, "FishingRod");
            LoadedItems.Add(1, FishingRod);

            Fish = new Item(Vector2.Zero, textures.Fish, 2, "Fish");
            LoadedItems.Add(2, Fish);

            Shark = new Item(Vector2.Zero, textures.Shark, 3, "Shark");
            LoadedItems.Add(3, Shark);

            FishingLine = new Item(Vector2.Zero, textures.FishingLine, 4, "FishingLine");
            FishingLine.activ = false;
            LoadedItems.Add(4, FishingLine);

            Shovel = new Item(Vector2.Zero, textures.Shovel, 5, "Shovel");
            LoadedItems.Add(5, Shovel);

            SeaShell1 = new Item(Vector2.Zero, textures.SeaShell1, 6, "SeaShell1");
            LoadedItems.Add(6, SeaShell1);

            SeaShell2 = new Item(Vector2.Zero, textures.SeaShell2, 7, "SeaShell2");
            LoadedItems.Add(7, SeaShell2);

            SeaShell3 = new Item(Vector2.Zero, textures.SeaShell3, 8, "SeaShell3");
            LoadedItems.Add(8, SeaShell3);

            SeaShell4 = new Item(Vector2.Zero, textures.SeaShell4, 9, "SeaShell4");
            LoadedItems.Add(9, SeaShell4);

            Pistol = new Item(Vector2.Zero, textures.Pistol, 10, "Pistol");
            LoadedItems.Add(10, Pistol);

            Rock = new Item(Vector2.Zero, textures.Rock, 11, "Rock");
            LoadedItems.Add(11, Rock);

            IronIngot = new Item(Vector2.Zero, textures.IronIngot, 12, "IronIngot");
            LoadedItems.Add(12, IronIngot);

            Wood = new Item(Vector2.Zero, textures.Wood, 13, "Wood");
            LoadedItems.Add(13, Wood);

            ToolHotbarItem = new Item[4];
            for (int i = 0; i < ToolHotbarItem.Length; i++)
            {
                if (i == 0 || i == 1)
                {
                    ToolHotbarItem[0] = FishingRod.Clone();
                    ToolHotbarItem[1] = Shovel.Clone();
                }
                else
                {
                    ToolHotbarItem[i] = new Item(Vector2.Zero, textures.Empty, 0, "Empty");
                    LoadedItems.Add(14 + i, ToolHotbarItem[i]);
                }
            }
        }
    }
}
