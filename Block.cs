using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Silent_Island
{
    public class Block : Objekt
    {
        public bool placed;
        public int amount;
        public Typ typ;
        public static Dictionary<int, Block> Blocks { get; } = new Dictionary<int, Block>();

        private int screenWidth;
        private int screenHeight;
        private int worldSizeX;
        private int worldSizeY;

        public enum Typ
        {
            ground,
            furniture,
            plant,
            building
        }
        //Main Init
        public Block(Main m) : base(m)
        {
            this.main = m;
            screenWidth = Main.screenWidth;
            screenHeight = Main.screenHeight;
            worldSizeX = Main.worldSizeX;
            worldSizeY = Main.worldSizeY;
        }

        //Constructure
        public Block(Vector2 koordinaten, Texture2D texture, int id, string name, Typ typ) : base(koordinaten, texture, id, name)
        {
            this.texture = texture;
            this.pos = koordinaten;
            this.Hitbox = new Rectangle((int)koordinaten.X, (int)koordinaten.Y, texture.Width, texture.Height);
            this.placed = false;
            this.ID = id;
            this.amount = 0;
            this.name = name;
            this.typ = typ;
            if (this.texture.Width == 32)
                this.scale = new Vector2(2, 2);
        }

        //Draw Method
        public void ZeichneLayer(Block[,] layer)
        {
            for (int i = 0; i < worldSizeX; i++)
            {
                for (int j = 0; j < worldSizeY; j++)
                {
                    if (layer[i, j].pos.X > Main.cameraPosition.X - screenWidth - 64 &&
                        layer[i, j].pos.X < Main.cameraPosition.X + screenWidth + 64 &&
                        layer[i, j].pos.Y > Main.cameraPosition.Y - screenHeight - 64 &&
                        layer[i, j].pos.Y < Main.cameraPosition.Y + screenHeight + 64)
                    {
                        layer[i, j].Zeichne();
                    }
                }
            }
        }
        public void ZeichneAll()
        {
            this.ZeichneLayer(BaseLayer);
            this.ZeichneLayer(DekoLayer);
            this.ZeichneLayer(StructureLayer);
            this.ZeichneLayer(ItemLayer);
        }
        public void HitboxAllDraw(SpriteBatch spriteBatch, Texture2D p)
        {
            for (int i = 0; i < worldSizeX; i++)
            {
                for (int j = 0; j < worldSizeY; j++)
                {
                    if (BaseLayer[i, j].pos.X > Main.cameraPosition.X - screenWidth - 64 &&
                        BaseLayer[i, j].pos.X < Main.cameraPosition.X + screenWidth + 64 &&
                        BaseLayer[i, j].pos.Y > Main.cameraPosition.Y - screenHeight - 64 &&
                        BaseLayer[i, j].pos.Y < Main.cameraPosition.Y + screenHeight + 64)

                        BaseLayer[i, j].DrawHitboxOutline(Color.Red);
                }
            }
        }

        //Editor Mode
        public void EditorModeUpdate(Vector2 cam)
        {
            for (int i = 0; i < takeBlock.Length; i++)
            {
                takeBlock[i].pos = new Vector2(cam.X + screenWidth - 128 + (i % 2) * 64, cam.Y + (i / 2) * 64);
                takeBlock[i].UpdateHitbox();
            }
        }
        public void EditorModeGetBlock(Vector2 mouse)
        {
            for (int i = 0; i < takeBlock.Length; i++)
            {
                if (takeBlock[i].hit())
                {
                    tokenBlock = takeBlock[i].Clone();
                }
            }
        }
        public void EditorModeSetBlock(Vector2 mouse)
        {
            int x = (int)(mouse.X / 64);
            int y = (int)(mouse.Y / 64);

            if (x >= 0 && x < BaseLayer.GetLength(0) && y >= 0 && y < BaseLayer.GetLength(1))
            {
                if (tokenBlock != null)
                {
                    Block clonedBlock = tokenBlock.Clone();
                    clonedBlock.pos = new Vector2(x * 64, y * 64);
                    clonedBlock.UpdateHitbox();

                    switch (clonedBlock.typ)
                    {
                        case Typ.furniture:
                            StructureLayer[x, y] = clonedBlock;
                            DekoLayer[x, y] = Void;
                            break;
                        case Typ.plant:
                            DekoLayer[x, y] = clonedBlock;
                            break;
                        default:
                            BaseLayer[x, y] = clonedBlock;
                            DekoLayer[x, y] = Void;
                            break;
                    }


                }
            }
        }
        public void EditorModeDraw(SpriteBatch sprite)
        {
            foreach (var KeyValuePair in Blocks)
            {
                takeBlock[KeyValuePair.Key].Zeichne();
            }
        }

        public override Block Clone()
        {
            return new Block(this.pos, this.texture, this.ID, this.name, this.typ);
        }
        public static Block CloneFromList(int id)
        {
            return new Block(Blocks[id].pos, Blocks[id].texture, Blocks[id].ID, Blocks[id].name, Blocks[id].typ);
        }

        public Block Void;
        public Block Grass;
        public Block Water;
        public Block Gravel;
        public Block GrassRoot;
        public Block TreeLog;
        public Block TreeLeave;
        public Block DekoMoss;
        public Block DekoStone;
        public Block DekoMossStone;

        public Block GrassEdgeO;
        public Block GrassEdgeU;
        public Block GrassEdgeH;
        public Block GrassEdgeL;
        public Block GrassEdgeI;

        public Block Chair;
        public Block Barrel;
        public Block WoodFloor;
        public Block GravelPath;

        public static Block[,] BaseLayer;
        public static Block[,] DekoLayer;
        public static Block[,] StructureLayer;
        public static Block[,] ItemLayer;

        public Block[] takeBlock;
        public Block tokenBlock;

        public void LoadAllBlocks()
        {
            Void = new Block(Vector2.Zero, Textures.Empty, 0, "Void", Typ.ground);
            Blocks.Add(0, Void);

            Grass = new Block(Vector2.Zero, Textures.Grass, 1, "Grass", Typ.ground);
            Blocks.Add(1, Grass);

            Water = new Block(Vector2.Zero, Textures.Water, 2, "Water", Typ.ground);
            Blocks.Add(2, Water);

            Gravel = new Block(Vector2.Zero, Textures.Gravel, 3, "Gravel", Typ.ground);
            Blocks.Add(3, Gravel);

            GrassRoot = new Block(Vector2.Zero, Textures.GrassRoot, 4, "GrassRoot", Typ.ground);
            Blocks.Add(4, GrassRoot);

            TreeLog = new Block(Vector2.Zero, Textures.TreeLog, 5, "TreeLog", Typ.plant);
            Blocks.Add(5, TreeLog);

            TreeLeave = new Block(Vector2.Zero, Textures.TreeLeave, 6, "TreeLeave", Typ.plant);
            Blocks.Add(6, TreeLeave);

            DekoMoss = new Block(Vector2.Zero, Textures.DekoMoss, 7, "DekoMoss", Typ.plant);
            Blocks.Add(7, DekoMoss);

            DekoStone = new Block(Vector2.Zero, Textures.DekoStone, 8, "DekoStone", Typ.plant);
            Blocks.Add(8, DekoStone);

            DekoMossStone = new Block(Vector2.Zero, Textures.DekoMossStone, 9, "DekoMossStone", Typ.plant);
            Blocks.Add(9, DekoMossStone);

            GrassEdgeO = new Block(Vector2.Zero, Textures.GrassEdgeO, 10, "GrassEdgeO", Typ.plant);
            Blocks.Add(10, GrassEdgeO);

            GrassEdgeU = new Block(Vector2.Zero, Textures.GrassEdgeU, 11, "GrassEdgeU", Typ.plant);
            Blocks.Add(11, GrassEdgeU);

            GrassEdgeH = new Block(Vector2.Zero, Textures.GrassEdgeH, 12, "GrassEdgeH", Typ.plant);
            Blocks.Add(12, GrassEdgeH);

            GrassEdgeL = new Block(Vector2.Zero, Textures.GrassEdgeL, 13, "GrassEdgeL", Typ.plant);
            Blocks.Add(13, GrassEdgeL);

            GrassEdgeI = new Block(Vector2.Zero, Textures.GrassEdgeI, 14, "GrassEdgeI", Typ.plant);
            Blocks.Add(14, GrassEdgeI);

            Chair = new Block(Vector2.Zero, Textures.Chair, 15, "Chair", Typ.furniture);
            Blocks.Add(15, Chair);

            Barrel = new Block(Vector2.Zero, Textures.Barrel, 16, "Barrel", Typ.furniture);
            Blocks.Add(16, Barrel);

            WoodFloor = new Block(Vector2.Zero, Textures.WoodFloor, 17, "WoodFloor", Typ.ground);
            Blocks.Add(17, WoodFloor);

            GravelPath = new Block(Vector2.Zero, Textures.GravelPath, 18, "GravelPath", Typ.ground);
            Blocks.Add(18, GravelPath);

            BaseLayer = new Block[worldSizeX, worldSizeY];
            DekoLayer = new Block[worldSizeX, worldSizeY];
            StructureLayer = new Block[worldSizeX, worldSizeY];
            ItemLayer = new Block[worldSizeX, worldSizeY];

            takeBlock = new Block[Blocks.Count];
            foreach (var KeyValuePair in Blocks)
            {
                takeBlock[KeyValuePair.Key] = KeyValuePair.Value.Clone();
            }
        }


    }
}
