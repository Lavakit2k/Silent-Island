using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silent_Island
{
    public class Item : Objekt
    {
        public int amount { get; set; }
        public string slotAmount { get; set; }

        public Item(Vector2 koordinaten, Texture2D textur, int id) : base(koordinaten, textur)
        {
            texture = textur;
            pos = koordinaten;
            color = Color.White;
            rotation = MathHelper.ToRadians(0);
            axis = Vector2.Zero;
            scale = new Vector2(1, 1);
            effekt = SpriteEffects.None;
            Hitbox = new Rectangle((int)koordinaten.X, (int)koordinaten.Y, textur.Width, textur.Height);
            ID = id;
            name = "Empty";
            amount = 0;
            slotAmount = new string("" + amount);

        }
        public Item(Textures t, Main m) : base(t, m)
        {
            this.textures = t;
            this.main = m;
        }

        public void Aufnehmen(Item[] HotbarSlot)
        {
            for (int i = 0; i < 7; i++)
            {
                if (HotbarSlot[i].ID == this.ID && this.amount < 100)
                {
                    ++HotbarSlot[i].amount;
                    break;
                }
                else if (HotbarSlot[i].ID != this.ID && HotbarSlot[i].ID == 0)
                {
                    ++HotbarSlot[i].amount;
                    HotbarSlot[i].ID = this.ID;
                    HotbarSlot[i].texture = this.texture;
                    break;
                }
            }
            /* oben UI InventorySlot hinzufügen
             * 
             * for(int i = 0; i < 63; i++)
            {
                if (InventorySlot.ID == item.ID && item.amount < 100)
                {
                    ++item.amount;
                }
                else if (InventorySlot.ID != item.ID || InventorySlot.ID == 0)
                {
                    HotbarSlot.ID = item.ID;
                }
            }*/
        }
        public void MultiAufnehmen(Item[] HotbarSlot, int amount)
        {
            for (int i = 0; i < 7; i++)
            {
                if (HotbarSlot[i].ID == this.ID && this.amount < 100)
                {
                    HotbarSlot[i].amount += amount;
                    break;
                }
                else if (HotbarSlot[i].ID != this.ID && HotbarSlot[i].ID == 0)
                {
                    HotbarSlot[i].amount += amount;
                    HotbarSlot[i].ID = this.ID;
                    HotbarSlot[i].texture = this.texture;
                    break;
                }
            }
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

        public void LoadAllItems()
        {
            Empty = new Item(Vector2.Zero, textures.Empty, 0);
            FishingRod = new Item(Vector2.Zero, textures.FishingRod, 1);
            Fish = new Item(Vector2.Zero, textures.Fish, 2);
            Shark = new Item(Vector2.Zero, textures.Shark, 3);
            FishingLine = new Item(Vector2.Zero, textures.FishingLine, 4);
            FishingLine.activ = false;
            Shovel = new Item(Vector2.Zero, textures.Shovel, 5);
            SeaShell1 = new Item(Vector2.Zero, textures.SeaShell1, 6);
            SeaShell2 = new Item(Vector2.Zero, textures.SeaShell2, 7);
            SeaShell3 = new Item(Vector2.Zero, textures.SeaShell3, 8);
            SeaShell4 = new Item(Vector2.Zero, textures.SeaShell4, 9);
            Pistol = new Item(Vector2.Zero, textures.Pistol, 10);
            Rock = new Item(Vector2.Zero, textures.Rock, 11);
            IronIngot = new Item(Vector2.Zero, textures.IronIngot, 12);
            Wood = new Item(Vector2.Zero, textures.Wood, 13);
        }


    }

}
