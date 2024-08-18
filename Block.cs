using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Silent_Island
{
    public class Block : Objekt
    {
        public bool placed { get; set; }
        public int amount { get; set; }
        public static Dictionary<int, string> Blocks { get; } = new Dictionary<int, string>()
        {
            { 0, "Void" },
            { 1, "Grass" },
            { 2, "Water"},
            { 3, "Gravel" },
            { 4, "GrasRoots" },
            { 5, "TreeLog" },
            { 6, "TreeLeaves" },

            { 7, "DekoMoss" },
            { 8, "DekoStone" },
            { 9, "DekoStoneMoss" },

        };

        public Block(Vector2 koordinaten, Texture2D textur) : base(koordinaten, textur)
        {
            texture = textur;
            coords = koordinaten;
            color = Color.White;
            rotation = MathHelper.ToRadians(0);
            axis = new Vector2(textur.Width / 2f, textur.Height / 2f);
            scale = new Vector2(1, 1);
            effekt = SpriteEffects.None;
            Hitbox = new Rectangle((int)koordinaten.X, (int)koordinaten.Y, textur.Width, textur.Height);
            placed = false;
            ID = 0;
            amount = 0;
            name = "Void";
        }
        //TODO für Layer statt Objekt
        /*
        public void Place(Objekt objekt, Objekt[,] item, Vector2 maus)
        {
            int i = (int)(maus.X / 64);
            int j = (int)(maus.Y / 64);

            // Überprüfen, ob die Indizes innerhalb der Grenzen des block-Arrays liegen
            if (i >= 0 && i < item.GetLength(0) && j >= 0 && j < item.GetLength(1))
            {
                objekt.coords = item[i, j].coords;
                objekt.hitbox = new Vector2(objekt.coords.X + objekt.texture.Width, objekt.coords.Y + objekt.texture.Height);
            }
        }
        */
        public void Aufnehmen(Block item, Item[] HotbarSlot)
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
        public void Abnehmen(Block item, Item[] HotbarSlot, Textures t)
        {
            for (int i = 0; i < 7; i++)
            {
                if (HotbarSlot[i].ID == item.ID && item.amount > 2)
                {
                    --HotbarSlot[i].amount;
                    break;
                }
                else if (HotbarSlot[i].ID == item.ID && item.amount == 1)
                {
                    --HotbarSlot[i].amount;
                    HotbarSlot[i].ID = 0;
                    HotbarSlot[i].texture = t.Empty;
                    break;
                }
            }
        }
    }
}
