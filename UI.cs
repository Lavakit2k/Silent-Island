using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Island;
using System.Collections.Generic;


namespace Silent_Island
{

    public class UI : Objekt
    {
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
        }

        //TODO an Resolution anpassbar
        public void UpdateUI(Main main, float x, float y)
        {
            this.pos = new Vector2(main.cameraPosition.X + x, main.cameraPosition.Y + y);
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

        public void LoadAllUIs()
        {
            Hotbar = new UI(Vector2.Zero, textures.Hotbar);
            Hotbar.color = new Color(255, 255, 255, 0.5f);
            HotbarMarker = new UI(Vector2.Zero, textures.HotbarMarker);
            Inventory = new UI(Vector2.Zero, textures.Inventory);
            TopUI = new UI(Vector2.Zero, textures.TopUI);
            MapFrame = new UI(Vector2.Zero, textures.Map);
            ExtraHotbar = new UI(Vector2.Zero, textures.ExtraHotbar);
            ExtraHotbarMarker = new UI(Vector2.Zero, textures.HotbarMarker);
            FishingBar = new UI(Vector2.Zero, textures.FishingBar);
            FishingBarPointer = new UI(Vector2.Zero, textures.FishingBarPointer);
            FishingBarPointer.activ = false;
            FishingBar.activ = false;
        }
    }
} 

