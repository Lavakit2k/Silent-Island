using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Silent_Island_PC
{

    public class Item : Objekt
    {
        public int amount { get; set; }
        public string slotAmount { get; set; }
        public static Dictionary<int, string> Items { get; } = new Dictionary<int, string>()
        {
            { 0, "Empty" },
            { 1, "Fishing_Rod" },
            { 2, "Chair" },
            { 3, "Barrel" },
            { 4, "Pistol" },

        };
        public Item(Vector2 koordinaten, Texture2D textur) : base(koordinaten, textur)
        {
            texture = textur;
            coords = koordinaten;
            color = Color.White;
            rotation = MathHelper.ToRadians(0);
            axis = new Vector2(textur.Width / 2f, textur.Height / 2f);
            scale = new Vector2(1, 1);
            effekt = SpriteEffects.None;
            layer = 0;
            hitbox = new Vector2(koordinaten.X + textur.Width, koordinaten.Y + textur.Height);
            activ = true;
            ID = 0;
            name = "Empty";
            amount = 0;
            slotAmount = new string("" + amount);
        }
        public void Aufnehmen(Item item, Item[] HotbarSlot)
        {
            for (int i = 0; i < 7; i++)
            {
                if (HotbarSlot[i].ID == item.ID && item.amount < 100)
                {
                    ++HotbarSlot[i].amount;
                    break;
                }
                else if (HotbarSlot[i].ID != item.ID && HotbarSlot[i].ID == 0)
                {
                    ++HotbarSlot[i].amount;
                    HotbarSlot[i].ID = item.ID;
                    HotbarSlot[i].texture = item.texture;
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
        public void MultiAufnehmen(Item item, Item[] HotbarSlot, int amount)
        {
            for (int i = 0; i < 7; i++)
            {
                if (HotbarSlot[i].ID == item.ID && item.amount < 100)
                {
                    HotbarSlot[i].amount += amount;
                    break;
                }
                else if (HotbarSlot[i].ID != item.ID && HotbarSlot[i].ID == 0)
                {
                    HotbarSlot[i].amount += amount;
                    HotbarSlot[i].ID = item.ID;
                    HotbarSlot[i].texture = item.texture;
                    break;
                }
            }
        }
    }

}
