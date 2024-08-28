using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Silent_Island
{
    public class UI : Objekt
    {
        public Dictionary<int, UI> LoadedUIs = new Dictionary<int, UI>();

        private int screenWidth;
        private int screenHeight;
        public UI(Vector2 koordinaten, Texture2D textur) : base(koordinaten, textur)
        {
            texture = textur;
            pos = koordinaten;
            ID = 0;
            name = "Empty";
        }
        public UI(Textures t, Main m) : base(t, m)
        {
            this.textures = t;
            this.main = m;
            screenWidth = main.screenWidth; 
            screenHeight = main.screenHeight;
        }
        public void HitboxAllDraw(SpriteBatch s, Texture2D p)
        {
            foreach (var KeyValuePair in LoadedUIs)
            {
                if(KeyValuePair.Value.activ)
                KeyValuePair.Value.DrawHitboxOutline(s, p, Color.Blue);
            }
        }
        public void HotbarSwitch(int hotbarSlot)
        {

        }

        public void ZeichneAll(SpriteBatch s)
        {
            foreach (var KeyValuePair in LoadedUIs)
            {
                KeyValuePair.Value.Zeichne(s);
            }
        }
        public void UpdateAll(Vector2 cam, Item item)
        {
            //TODO screenWidth hier zwischenspeichern
            HandObjekt.Update(main.entity.Player.pos, 38, 16);
            HandObjekt.texture = item.ToolHotbarItem[main.ToolHotbarSlotNum].texture;
            HandObjekt.ID = item.ToolHotbarItem[main.ToolHotbarSlotNum].ID;
            ToolHotbar.Update(cam, screenWidth  - ToolHotbar.texture.Width, screenHeight - ToolHotbar.texture.Height);
            HotbarMarker.Update(ToolHotbar.pos, 8 + ((main.ToolHotbarSlotNum % 4) % 2 ) * 72, 8 + ((main.ToolHotbarSlotNum % 4) / 2) * 72);

            ExtraHotbar.Update(ToolHotbar.pos, - ToolHotbar.texture.Width - 100, ExtraHotbar.texture.Height - 8);
            ExtraHotbarMarker.Update(ExtraHotbar.pos, 8 + main.ExtraHotbarSlotNum * 72, 8);

            if (FishingBar.activ)
                FishingBar.Update(cam, screenWidth / 2 - textures.FishingBar.Width / 2 , screenHeight - 132);
            if (FishingBarPointer.activ)
                FishingBarPointer.Update(cam, screenWidth / 2 - 8 + 128f * (float)Math.Sin((main.timeCounter + main.fishingPointerOffset) / 1000 * 2f), screenHeight - 132);
            //                                  start                 amplitude               t                  offset                       frequenz

            DebugMenu.Update(new Vector2(cam.X, cam.Y), 0, 0);
        }

        public UI ToolHotbar;
        public UI HotbarMarker;
        public UI Inventory;
        public UI TopUI;
        public UI MapFrame;
        public UI ExtraHotbar;
        public UI ExtraHotbarMarker;
        public UI FishingBar;
        public UI FishingBarPointer;
        public UI Heart;
        public UI EmptyHeart;
        public UI DebugMenu;
        public UI HandObjekt;

        public void LoadAllUIs()
        {
            ToolHotbar = new UI(Vector2.Zero, textures.ToolHotbar);
            ToolHotbar.color = new Color(255, 255, 255, 0.5f);
            LoadedUIs.Add(0, ToolHotbar);

            HotbarMarker = new UI(Vector2.Zero, textures.HotbarMarker);
            LoadedUIs.Add(1, HotbarMarker);

            Inventory = new UI(Vector2.Zero, textures.Inventory);
            LoadedUIs.Add(2, Inventory);
            Inventory.activ = false;

            TopUI = new UI(Vector2.Zero, textures.TopUI);
            LoadedUIs.Add(3, TopUI);
            TopUI.activ = false;

            MapFrame = new UI(Vector2.Zero, textures.Map);
            LoadedUIs.Add(4, MapFrame);
            MapFrame.activ = false;

            ExtraHotbar = new UI(Vector2.Zero, textures.ExtraHotbar);
            ExtraHotbar.color = new Color(255, 255, 255, 0.5f);
            LoadedUIs.Add(5, ExtraHotbar);
            
            ExtraHotbarMarker = new UI(Vector2.Zero, textures.HotbarMarker);
            LoadedUIs.Add(6, ExtraHotbarMarker);

            FishingBar = new UI(Vector2.Zero, textures.FishingBar);
            FishingBar.activ = false;
            LoadedUIs.Add(7, FishingBar);

            FishingBarPointer = new UI(Vector2.Zero, textures.FishingBarPointer);
            FishingBarPointer.activ = false;
            LoadedUIs.Add(8, FishingBarPointer);

            DebugMenu = new UI(Vector2.Zero, textures.DebugMenu);
            LoadedUIs.Add(9, DebugMenu);
            DebugMenu.activ = false;

            HandObjekt = new UI(Vector2.Zero, textures.Empty);
            LoadedUIs.Add(10, HandObjekt);
            
        }
        
    }
}

