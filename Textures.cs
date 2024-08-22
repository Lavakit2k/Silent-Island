using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Silent_Island
{
    public class Textures
    {
        private ContentManager content;
        public SpriteBatch spriteBatch { get; private set; }

        #region Blocks
        public Texture2D Grass { get; private set; }
        public Texture2D Water { get; private set; }
        public Texture2D Gravel { get; private set; }
        public Texture2D Stone { get; private set; }
        public Texture2D GrassRoot { get; private set; }

        public Texture2D GrassUp { get; private set; }
        public Texture2D GrassUpCorner { get; private set; }

        public Texture2D IronOre { get; private set; }

        public Texture2D TreeLog { get; private set; }
        public Texture2D TreeLeave { get; private set; }
        #endregion

        #region Decoration
        public Texture2D GrassEdgeI { get; private set; }
        public Texture2D GrassEdgeH { get; private set; }
        public Texture2D GrassEdgeL { get; private set; }
        public Texture2D GrassEdgeU { get; private set; }
        public Texture2D GrassEdgeO { get; private set; }

        public Texture2D DekoMoss { get; private set; }
        public Texture2D DekoStone { get; private set; }
        public Texture2D DekoMossStone { get; private set; }
        #endregion

        #region Items
        public Texture2D FishingRod { get; private set; }
        public Texture2D FishingRodOut { get; private set; }
        public Texture2D FishingLine { get; private set; }
        public Texture2D Fish { get; private set; }
        public Texture2D Shark { get; private set; }

        public Texture2D Chair { get; private set; }
        public Texture2D Barrel { get; private set; }
        public Texture2D Oven { get; private set; }

        public Texture2D Pistol { get; private set; }
        public Texture2D PistolProjectile { get; private set; }

        public Texture2D Wood { get; private set; }
        public Texture2D Rock { get; private set; }
        public Texture2D IronIngot { get; private set; }

        public Texture2D SeaShell1 { get; private set; }
        public Texture2D SeaShell2 { get; private set; }
        public Texture2D SeaShell3 { get; private set; }
        public Texture2D SeaShell4 { get; private set; }

        public Texture2D GoldCoin { get; private set; }
        public Texture2D FishingShop { get; private set; }

        public Texture2D Shovel { get; private set; }

        #endregion

        #region Entities
        public Texture2D PlayerUp { get; private set; }
        public Texture2D PlayerDown { get; private set; }
        public Texture2D PlayerLeft { get; private set; }
        public Texture2D PlayerRight { get; private set; }
        #endregion

        #region UI
        public Texture2D Inventory { get; private set; }
        public Texture2D Slot { get; private set; }
        public Texture2D Hotbar { get; private set; }
        public Texture2D ExtraHotbar { get; private set; }
        public Texture2D Map { get; private set; }
        public Texture2D TopUI { get; private set; }
        public Texture2D HotbarMarker { get; private set; }
        public Texture2D FishingBar { get; private set; }
        public Texture2D FishingBarPointer { get; private set; }
        public Texture2D Heart { get; private set; }
        public Texture2D EmptyHeart { get; private set; }
        public Texture2D EmptySeeShell { get; private set; }
        public Texture2D DebugMenu { get; private set; }

        #endregion

        #region Rest 
        public Texture2D Empty { get; private set; }
        public Texture2D TestBlock { get; private set; }
        public Texture2D TestEdge { get; private set; }
        #endregion


        public Textures(ContentManager content)
        {
            this.content = content;
        }
        public void Initialize(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
        }
        public Texture2D GetTextureByName(string name)
        {
            return name switch
            {
                // Blocks
                "Grass" => Grass,
                "Water" => Water,
                "Gravel" => Gravel,
                "Stone" => Stone,
                "GrassRoot" => GrassRoot,
                "GrassUp" => GrassUp,
                "GrassUpCorner" => GrassUpCorner,
                "IronOre" => IronOre,
                "TreeLog" => TreeLog,
                "TreeLeave" => TreeLeave,

                // Decoration
                "GrassEdgeI" => GrassEdgeI,
                "GrassEdgeH" => GrassEdgeH,
                "GrassEdgeL" => GrassEdgeL,
                "GrassEdgeU" => GrassEdgeU,
                "GrassEdgeO" => GrassEdgeO,
                "DekoMoss" => DekoMoss,
                "DekoStone" => DekoStone,
                "DekoMossStone" => DekoMossStone,

                // Items
                "FishingRod" => FishingRod,
                "FishingRodOut" => FishingRodOut,
                "FishingLine" => FishingLine,
                "Fish" => Fish,
                "Shark" => Shark,
                "Chair" => Chair,
                "Barrel" => Barrel,
                "Oven" => Oven,
                "Pistol" => Pistol,
                "PistolProjectile" => PistolProjectile,
                "Wood" => Wood,
                "Rock" => Rock,
                "IronIngot" => IronIngot,
                "SeaShell1" => SeaShell1,
                "SeaShell2" => SeaShell2,
                "SeaShell3" => SeaShell3,
                "SeaShell4" => SeaShell4,
                "GoldCoin" => GoldCoin,
                "FishingShop" => FishingShop,
                "Shovel" => Shovel,

                // Entities
                "PlayerUp" => PlayerUp,
                "PlayerDown" => PlayerDown,
                "PlayerLeft" => PlayerLeft,
                "PlayerRight" => PlayerRight,

                // UI
                "Inventory" => Inventory,
                "Slot" => Slot,
                "Hotbar" => Hotbar,
                "ExtraHotbar" => ExtraHotbar,
                "Map" => Map,
                "TopUI" => TopUI,
                "HotbarMarker" => HotbarMarker,
                "FishingBar" => FishingBar,
                "FishingBarPointer" => FishingBarPointer,
                "Heart" => Heart,
                "EmptyHeart" => EmptyHeart,
                "EmptySeeShell" => EmptySeeShell,
                "DebugMenu" => DebugMenu,

                // Rest
                "Empty" => Empty,
                "TestBlock" => TestBlock,
                "TestEdge" => TestEdge,

                // Default
                _ => TestBlock,
            };
        }

        public void LoadAllTextures()
        {
            #region Blocks
            Grass = content.Load<Texture2D>("Texturen/Grass");
            Water = content.Load<Texture2D>("Texturen/Water");
            Gravel = content.Load<Texture2D>("Texturen/Gravel");
            Stone = content.Load<Texture2D>("Texturen/Stone");
            GrassRoot = content.Load<Texture2D>("Texturen/Grass_Root");
            GrassUp = content.Load<Texture2D>("Texturen/Grass_Up");
            GrassUpCorner = content.Load<Texture2D>("Texturen/Grass_Up_Corner");
            IronOre = content.Load<Texture2D>("Texturen/Iron_Ore");
            TreeLog = content.Load<Texture2D>("Texturen/Tree_Log");
            TreeLeave = content.Load<Texture2D>("Texturen/Tree_Leave");
            #endregion

            #region Decoration
            GrassEdgeI = content.Load<Texture2D>("Texturen/Grass_Edge_I");
            GrassEdgeH = content.Load<Texture2D>("Texturen/Grass_Edge_H");
            GrassEdgeL = content.Load<Texture2D>("Texturen/Grass_Edge_L");
            GrassEdgeU = content.Load<Texture2D>("Texturen/Grass_Edge_U");
            GrassEdgeO = content.Load<Texture2D>("Texturen/Grass_Edge_O");
            DekoMoss = content.Load<Texture2D>("Texturen/Deko_Moss");
            DekoStone = content.Load<Texture2D>("Texturen/Deko_Stone");
            DekoMossStone = content.Load<Texture2D>("Texturen/Deko_Moss_Stone");
            #endregion

            #region Items
            FishingRod = content.Load<Texture2D>("Texturen/Fishing_Rod");
            FishingRodOut = content.Load<Texture2D>("Texturen/Fishing_Rod_out");
            FishingLine = content.Load<Texture2D>("Texturen/Fishing_Line");
            Fish = content.Load<Texture2D>("Texturen/Fish");
            Shark = content.Load<Texture2D>("Texturen/Shark");
            Chair = content.Load<Texture2D>("Texturen/Chair");
            Barrel = content.Load<Texture2D>("Texturen/Barrel");
            Oven = content.Load<Texture2D>("Texturen/Oven");
            Pistol = content.Load<Texture2D>("Texturen/Pistol");
            PistolProjectile = content.Load<Texture2D>("Texturen/Pistol_Projectile");
            Wood = content.Load<Texture2D>("Texturen/Item_Wood");
            Rock = content.Load<Texture2D>("Texturen/Item_Stone");
            IronIngot = content.Load<Texture2D>("Texturen/Iron_Ingot");
            SeaShell1 = content.Load<Texture2D>("Texturen/Seashell_1");
            SeaShell2 = content.Load<Texture2D>("Texturen/Seashell_2");
            SeaShell3 = content.Load<Texture2D>("Texturen/Seashell_3");
            SeaShell4 = content.Load<Texture2D>("Texturen/Seashell_4");
            GoldCoin = content.Load<Texture2D>("Texturen/Gold_Coin");
            FishingShop = content.Load<Texture2D>("Texturen/Fishing_Shop");
            Shovel = content.Load<Texture2D>("Texturen/Shovel");
            #endregion

            #region Entities
            PlayerUp = content.Load<Texture2D>("Texturen/Player_Up");
            PlayerDown = content.Load<Texture2D>("Texturen/Player_Down");
            PlayerLeft = content.Load<Texture2D>("Texturen/Player_Left");
            PlayerRight = content.Load<Texture2D>("Texturen/Player_Right");
            #endregion

            #region UI
            Inventory = content.Load<Texture2D>("Texturen/UI_Inventory");
            Slot = content.Load<Texture2D>("Texturen/UI_Single_Slot");
            Hotbar = content.Load<Texture2D>("Texturen/UI_Hotbar");
            ExtraHotbar = content.Load<Texture2D>("Texturen/UI_Extra_Hotbar");
            Map = content.Load<Texture2D>("Texturen/UI_Map");
            TopUI = content.Load<Texture2D>("Texturen/UI_Top");
            HotbarMarker = content.Load<Texture2D>("Texturen/UI_Hotbar_Marker");
            FishingBar = content.Load<Texture2D>("Texturen/Fishing_Bar");
            FishingBarPointer = content.Load<Texture2D>("Texturen/Fishing_Bar_Pointer");
            Heart = content.Load<Texture2D>("Texturen/Heart");
            EmptyHeart = content.Load<Texture2D>("Texturen/EmptyHeart");
            EmptySeeShell = content.Load<Texture2D>("Texturen/Empty_Seashell");
            DebugMenu = content.Load<Texture2D>("Texturen/Debug_Menu");

            #endregion

            #region Rest 
            Empty = content.Load<Texture2D>("Texturen/Empty");
            TestBlock = content.Load<Texture2D>("Texturen/Test_Block");
            TestEdge = content.Load<Texture2D>("Texturen/Test_Edge");
            #endregion
        }
    }

}
