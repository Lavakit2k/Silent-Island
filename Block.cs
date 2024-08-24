using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Silent_Island
{
    public class Block : Objekt
    {
        public bool placed { get; set; }
        public int amount { get; set; }
        public Dictionary<int, Block> Blocks { get; } = new Dictionary<int, Block>();

        public Block(Textures t, Main m) : base(t, m)
        {
            this.textures = t;
            this.main = m;
        }

        public Block(Vector2 koordinaten, Texture2D textur, int id, string name) : base(koordinaten, textur)
        {
            this.texture = textur;
            this.pos = koordinaten;
            this.color = Color.White;
            this.rotation = MathHelper.ToRadians(0);
            this.axis = Vector2.Zero;
            this.scale = new Vector2(1, 1);
            this.effekt = SpriteEffects.None;
            this.Hitbox = new Rectangle((int)koordinaten.X, (int)koordinaten.Y, textur.Width, textur.Height);
            this.placed = false;
            this.activ = true;
            this.ID = id;
            this.amount = 0;
            this.name = name;

        }

        public void ZeichneLayer(SpriteBatch spriteBatch, Block[,] layer)
        {
            for (int i = 0; i < main.worldSizeX; i++)
            {
                for (int j = 0; j < main.worldSizeY; j++)
                {
                    if (layer[i, j].pos.X > main.cameraPosition.X - main.screenWidth - 64 &&
                        layer[i, j].pos.X < main.cameraPosition.X + main.screenWidth + 64 &&
                        layer[i, j].pos.Y > main.cameraPosition.Y - main.screenHeight - 64 &&
                        layer[i, j].pos.Y < main.cameraPosition.Y + main.screenHeight + 64)
                    {
                        layer[i, j].Zeichne(main.spriteBatch);
                    }
                }
            }
        }
        public void ZeichneAll(SpriteBatch spriteBatch)
        {
            this.ZeichneLayer(spriteBatch, BaseLayer);
            this.ZeichneLayer(spriteBatch, DekoLayer);
            this.ZeichneLayer(spriteBatch, StructureLayer);
            this.ZeichneLayer(spriteBatch, ItemLayer);
        }

        private void UpdateHitbox()
        {
            Hitbox = new Rectangle(
                (int)pos.X,
                (int)pos.Y,
                (int)(texture.Width * scale.X),
                (int)(texture.Height * scale.Y)
            );
        }
        public void DrawHitboxOutline(SpriteBatch spriteBatch, Texture2D p)
        {
            // Hitbox-Ränder (Positionen der Linien)
            int x = Hitbox.X;
            int y = Hitbox.Y;
            int width = Hitbox.Width;
            int height = Hitbox.Height;

            // Linienbreite (z.B. 2 Pixel)
            int lineWidth = 1;

            // Obere Linie
            spriteBatch.Draw(p, new Rectangle(x, y, width, lineWidth), Color.White);

            // Untere Linie
            spriteBatch.Draw(p, new Rectangle(x, y + height - lineWidth, width, lineWidth), Color.White);

            // Linke Linie
            spriteBatch.Draw(p, new Rectangle(x, y, lineWidth, height), Color.White);

            // Rechte Linie
            spriteBatch.Draw(p, new Rectangle(x + width - lineWidth, y, lineWidth, height), Color.White);
        }
        public void HitboxAllDraw(SpriteBatch spriteBatch, Texture2D p)
        {
            for (int i = 0; i < main.worldSizeX; i++)
            {
                for (int j = 0; j < main.worldSizeY; j++)
                {
                    BaseLayer[i,j].DrawHitboxOutline(spriteBatch, p);
                }
            }
        }

        public void EditorModeUpdate(Vector2 cam)
        {
            for (int i = 0; i < takeBlock.Length; i++)
            {
                takeBlock[i].pos = new Vector2(cam.X + main.screenWidth - 128  + (i % 2) * 64, cam.Y + (i / 2) * 64);
                takeBlock[i].UpdateHitbox();
            }
        }
        public void EditorModeGetBlock(Vector2 mouse)
        {
            for (int i = 0; i < takeBlock.Length; i++)
            {
                if (takeBlock[i].hit(mouse))
                {
                    tokenBlock = takeBlock[i].Clone();
                }
            }
        }
        public void EditorModeSetBlock(Vector2 mouse)
        {
            // Berechne die Blockkoordinaten basierend auf der Mausposition
            int x = (int)(mouse.X / 64);
            int y = (int)(mouse.Y / 64);

            // Stelle sicher, dass die berechneten Indizes innerhalb der Array-Grenzen liegen
            if (x >= 0 && x < BaseLayer.GetLength(0) && y >= 0 && y < BaseLayer.GetLength(1))
            {
                if (tokenBlock != null)
                {
                    Block clonedBlock = tokenBlock.Clone();
                    clonedBlock.axis = new Vector2(0, 0);
                    clonedBlock.pos = new Vector2(x * 64, y * 64);
                    clonedBlock.UpdateHitbox();

                    BaseLayer[x, y] = clonedBlock;
                   
                }
            }
        }
        public void EditorModeDraw(SpriteBatch sprite)
        {
            foreach (var KeyValuePair in Blocks)
            {
                takeBlock[KeyValuePair.Key].Zeichne(sprite);
            }
        }

        public Block Clone()
        {
            return new Block(this.pos, this.texture, this.ID, this.name);
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

        public Block[,] BaseLayer;
        public Block[,] DekoLayer;
        public Block[,] StructureLayer;
        public Block[,] ItemLayer;

        public Block[] takeBlock;
        public Block tokenBlock;

        //TODO recalc Hitbox 
        public void LoadAllBlocks()
        {
            Void = new Block(Vector2.Zero, textures.Empty, 0, "Void");
            Blocks.Add(0, Void);

            Grass = new Block(Vector2.Zero, textures.Grass, 1, "Grass");
            Blocks.Add(1, Grass);

            Water = new Block(Vector2.Zero, textures.Water, 2, "Water");
            Blocks.Add(2, Water);

            Gravel = new Block(Vector2.Zero, textures.Gravel, 3, "Gravel");
            Blocks.Add(3, Gravel);

            GrassRoot = new Block(Vector2.Zero, textures.GrassRoot, 4, "GrassRoot");
            Blocks.Add(4, GrassRoot);

            TreeLog = new Block(Vector2.Zero, textures.TreeLog, 5, "TreeLog");
            Blocks.Add(5, TreeLog);

            TreeLeave = new Block(Vector2.Zero, textures.TreeLeave, 6, "TreeLeave");
            Blocks.Add(6, TreeLeave);

            DekoMoss = new Block(Vector2.Zero, textures.DekoMoss, 7, "DekoMoss");
            Blocks.Add(7, DekoMoss);

            DekoStone = new Block(Vector2.Zero, textures.DekoStone, 8, "DekoStone");
            Blocks.Add(8, DekoStone);

            DekoMossStone = new Block(Vector2.Zero, textures.DekoMossStone, 9, "DekoMossStone");
            Blocks.Add(9, DekoMossStone);

            GrassEdgeO = new Block(Vector2.Zero, textures.GrassEdgeO, 10, "GrassEdgeO");
            Blocks.Add(10, GrassEdgeO);

            GrassEdgeU = new Block(Vector2.Zero, textures.GrassEdgeU, 11, "GrassEdgeU");
            Blocks.Add(11, GrassEdgeU);

            GrassEdgeH = new Block(Vector2.Zero, textures.GrassEdgeH, 12, "GrassEdgeH");
            Blocks.Add(12, GrassEdgeH);

            GrassEdgeL = new Block(Vector2.Zero, textures.GrassEdgeL, 13, "GrassEdgeL");
            Blocks.Add(13, GrassEdgeL);

            GrassEdgeI = new Block(Vector2.Zero, textures.GrassEdgeI, 14, "GrassEdgeI");
            Blocks.Add(14, GrassEdgeI);

            Chair = new Block(Vector2.Zero, textures.Chair, 15, "Chair");
            Blocks.Add(15, Chair);


            // 1 Layer = 2 MB + 1 MB Draw
            BaseLayer = new Block[main.worldSizeX, main.worldSizeY];
            DekoLayer = new Block[main.worldSizeX, main.worldSizeY];
            StructureLayer = new Block[main.worldSizeX, main.worldSizeY];
            ItemLayer = new Block[main.worldSizeX, main.worldSizeY];

            takeBlock = new Block[Blocks.Count];
            foreach (var KeyValuePair in Blocks)
            {
                takeBlock[KeyValuePair.Key] = KeyValuePair.Value.Clone();
            }
        }


    }
}
