using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Silent_Island
{
    public class UI : Objekt
    {
        Dictionary<int, UI> LoadedUIs = new Dictionary<int, UI>();
        private int screenWidth;
        private int screenHeight;
        public UI(Vector2 koordinaten, Texture2D textur) : base(koordinaten, textur)
        {
            texture = textur;
            pos = koordinaten;
            color = Color.White;
            rotation = MathHelper.ToRadians(0);
            axis = new Vector2(textur.Width / 2f, textur.Height / 2f);
            scale = new Vector2(1, 1);
            effekt = SpriteEffects.None;
            Hitbox = new Rectangle((int)koordinaten.X, (int)koordinaten.Y, textur.Width, textur.Height);
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

        //TODO an Resolution anpassbar
        public void UpdateUI(Vector2 cam, float x, float y)
        {
            this.pos = new Vector2(cam.X + x, cam.Y + y);
        }
        public void HotbarSwitch(int hotbarSlot)
        {

        }

        public UI Hotbar;
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
            Hotbar = new UI(Vector2.Zero, textures.Hotbar);
            Hotbar.color = new Color(255, 255, 255, 0.5f);
            LoadedUIs.Add(0, Hotbar);

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
            LoadedUIs.Add(5, ExtraHotbar);
            ExtraHotbar.activ = false;

            ExtraHotbarMarker = new UI(Vector2.Zero, textures.HotbarMarker);
            LoadedUIs.Add(6, ExtraHotbarMarker);
            ExtraHotbarMarker.activ = false;

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
        public void ZeichneAll(SpriteBatch s)
        {
            foreach(var KeyValuePair in LoadedUIs)
            {
                KeyValuePair.Value.Zeichne(s);
            }
        }
        public void UpdateAll(Vector2 cam)
        {
            //TODO screenWidth hier zwischenspeichern
            HandObjekt.UpdateUI(main.entity.Player.pos, 30, 0);
            HandObjekt.texture = main.SlotObjekt[main.HotbarSlotNum].texture;
            Hotbar.UpdateUI(cam, screenWidth / 2, screenHeight - 50);
            HotbarMarker.UpdateUI(cam, screenWidth / 2 - 216 + main.HotbarSlotNum * 72, screenHeight - 50);
            FishingBar.UpdateUI(cam,screenWidth / 2 - textures.FishingBar.Width / 2 + 146, screenHeight - 120);
            if (FishingBarPointer.activ)
                FishingBarPointer.UpdateUI(cam,screenWidth / 2 + 20 + 128f * (float)Math.Sin((main.timeCounter + main.fishingPointerOffset) / 1000 * 2f), screenHeight - 132);
            //                                 start                   amplitude               t             offset                         frequenz
            DebugMenu.UpdateUI(new Vector2(cam.X, cam.Y), 122, 149);
        }
    }
}

