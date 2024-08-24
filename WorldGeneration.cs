using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent_Island
{
    public class WorldGeneration
    {
        private Random random = new Random();
        private Textures texture;
        private Main main;
        private Structure structure;
        private Block block;
        public int PWater;
        public int PGrass;
        public int PGravel;
        public int smoothness = 10;
        public int heavy;
        public int worldSizeX = 64;
        public int worldSizeY = 64;

        //works with Bits
        [Flags]
        public enum Directions
        {
            None = 0,
            Up = 1,     //0001
            Down = 2,   //0010
            Left = 4,   //0100
            Right = 8   //1000
        }


        public WorldGeneration(Main main, Textures textures, Structure structure, Block block)
        {
            this.texture = textures;
            this.structure = structure;
            this.main = main;
            this.block = block;
            for (int i = 0; i < worldSizeX; ++i)
            {
                for (int j = 0; j < worldSizeY; ++j)
                {
                    block.BaseLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.Empty, random.Next(1, 3), "");
                    block.StructureLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.Empty, 0, "");
                    block.DekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.Empty, random.Next(1, 3), "");
                    block.ItemLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.Empty, 0, "");
                }
            }
            
            GenerateBase();
            GenerateDeko();
            GenerateStructers();
            
        }
        
        #region Base
        public void GenerateBase()
        {
            GenerateTerrain();
            AddExtraTerrain();
        }
        private void GenerateTerrain()
        {
            for (int i = 0; i < worldSizeX; ++i)
            {
                for (int j = 0; j < worldSizeY; ++j)
                {
                    for (int t = 0; t < smoothness; ++t)
                    {
                        #region Grass/Water
                        int PGrass = GetSurroundingBlockIDType(block.BaseLayer, i, j, 1);
                        int PWater = GetSurroundingBlockIDType(block.BaseLayer, i, j, 2);

                        if (block.BaseLayer[i, j].ID == 2 && PGrass > 4)
                        {
                            //Grass
                            SetBlock(block.BaseLayer, i, j, 1);
                        }
                        else if (block.BaseLayer[i, j].ID == 1 && PWater > 5)
                        {
                            //Water
                            SetBlock(block.BaseLayer, i, j, 2);
                        }
                        else
                        {
                            //No Change
                            switch (block.BaseLayer[i, j].ID)
                            {
                                case 1:
                                    SetBlock(block.BaseLayer, i, j, 1);
                                    break;
                                case 2:
                                    SetBlock(block.BaseLayer, i, j, 2);
                                    break;
                            }
                        }
                        #endregion
                    }
                }
            }
        }
        private void AddExtraTerrain()
        {
            for (int i = 0; i < worldSizeX; ++i)
            {
                for (int j = 0; j < worldSizeY; ++j)
                {
                    for (int t = 0; t < 3; ++t)
                    {
                        #region Gravel//Roots

                        if (block.BaseLayer[i, j].ID == 2) break;

                        int PGravel = GetSurroundingBlockIDType(block.BaseLayer, i, j, 3);

                        if (PGravel > 4 && random.Next(1, PGravel - PGravel / 2) == 1)
                        {
                            SetBlock(block.BaseLayer, i, j, 3);
                        }
                        else if (random.Next(1, 30) == 1)
                        {
                            SetBlock(block.BaseLayer, i, j, 3);
                        }

                        if (random.Next(1, 80) == 3)
                        {
                            SetBlock(block.BaseLayer, i, j, 4);
                        }

                        #endregion
                    }
                }
            }
        }

        #endregion

        #region Structures
        public void GenerateStructers()
        {
            for (int i = 0; i < worldSizeX; ++i)
            {
                for (int j = 0; j < worldSizeY; ++j)
                {
                    if (block.BaseLayer[i, j].ID == 4)
                    {
                        GenerateTree(block.StructureLayer, i, j);
                    }
                }
            }
        }

        // starts in upper corner
        public void GenerateStructure(Block[,] structureLayer, int startX, int startY, int[,] pattern)
        {
            int patternWidth = pattern.GetLength(0);
            int patternHeight = pattern.GetLength(1);

            for (int i = 0; i < patternWidth; ++i)
            {
                for (int j = 0; j < patternHeight; ++j)
                {
                    int blockType = pattern[j, i];
                    int x = startX + i;
                    int y = startY + j;

                    if (x >= 0 && x < structureLayer.GetLength(0) && y >= 0 && y < structureLayer.GetLength(1))
                    {
                        if (structureLayer[x, y].ID == 0 || structureLayer[x, y].ID == 5) // Only set if no block exists
                        {
                            if (blockType != 0)
                            {
                                SetBlock(block.StructureLayer, x, y, pattern[j, i]);
                            }
                        }
                    }
                }
            }
        }

        public void GenerateTree(Block[,] structureLayer, int i, int j)
        {
            GenerateStructure(structureLayer, i - 2, j - 4, structure.treePattern);
        }

        #endregion

        #region Deko
        public void GenerateDeko()
        {
            for (int i = 0; i < worldSizeX; ++i)
            {
                for (int j = 0; j < worldSizeY; ++j)
                {
                    GenerateEdges(i, j);
                    GenerateExtraDeko(i, j);
                }
            }
        }

        //Edges
        private void GenerateEdges(int i, int j)
        {
            //currentBlockID DER BLOCK WELCHEM DEM RAND ANGEPASST IST z.B. Gras
            //TODO für später mehrere blockIDs machbar z.B. Kies
            int currentBlockID = 1;

            Directions touchingDirections = GetTouchingBlockDirections(block.BaseLayer, i, j, currentBlockID);

            //Nur wenn Wasser/ Gravel
            if (block.BaseLayer[i, j].ID == 2 || block.BaseLayer[i, j].ID == 3)
                switch (touchingDirections)
                {
                    //all
                    // Up, Down, Left, Right
                    case Directions.Up | Directions.Down | Directions.Left | Directions.Right:
                        SetBlockDeko(block.DekoLayer, i, j, 10);

                        break;

                    //U 
                    case Directions.Up | Directions.Down | Directions.Left:
                        SetBlockDeko(block.DekoLayer, i, j, 11);
                        block.DekoLayer[i, j].rotation = MathHelper.ToRadians(90);
                        break;
                    case Directions.Up | Directions.Down | Directions.Right:
                        SetBlockDeko(block.DekoLayer, i, j, 11);
                        block.DekoLayer[i, j].rotation = MathHelper.ToRadians(-90);
                        break;
                    case Directions.Up | Directions.Left | Directions.Right:
                        SetBlockDeko(block.DekoLayer, i, j, 11);
                        block.DekoLayer[i, j].rotation = MathHelper.ToRadians(180);
                        break;
                    case Directions.Down | Directions.Left | Directions.Right:
                        SetBlockDeko(block.DekoLayer, i, j, 11);
                        break;

                    //H
                    case Directions.Up | Directions.Down:
                        SetBlockDeko(block.DekoLayer, i, j, 12);
                        block.DekoLayer[i, j].rotation = MathHelper.ToRadians(90);
                        break;
                    case Directions.Left | Directions.Right:
                        SetBlockDeko(block.DekoLayer, i, j, 12);
                        break;

                    //L
                    case Directions.Up | Directions.Left:
                        SetBlockDeko(block.DekoLayer, i, j, 13);
                        block.DekoLayer[i, j].rotation = MathHelper.ToRadians(-180);
                        break;
                    case Directions.Up | Directions.Right:
                        SetBlockDeko(block.DekoLayer, i, j, 13);
                        block.DekoLayer[i, j].rotation = MathHelper.ToRadians(-90);
                        break;
                    case Directions.Down | Directions.Left:
                        SetBlockDeko(block.DekoLayer, i, j, 13);
                        block.DekoLayer[i, j].rotation = MathHelper.ToRadians(90);
                        break;
                    case Directions.Down | Directions.Right:
                        SetBlockDeko(block.DekoLayer, i, j, 13);
                        break;

                    //I
                    case Directions.Up:
                        SetBlockDeko(block.DekoLayer, i, j, 14);
                        block.DekoLayer[i, j].rotation = MathHelper.ToRadians(-90);
                        break;
                    case Directions.Down:
                        SetBlockDeko(block.DekoLayer, i, j, 14);
                        block.DekoLayer[i, j].rotation = MathHelper.ToRadians(90);
                        break;
                    case Directions.Left:
                        SetBlockDeko(block.DekoLayer, i, j, 14);
                        block.DekoLayer[i, j].rotation = MathHelper.ToRadians(180);
                        break;
                    case Directions.Right:
                        SetBlockDeko(block.DekoLayer, i, j, 14);
                        break;

                    default:
                        SetBlockDeko(block.DekoLayer, i, j, 0);
                        break;

                }

        }
        public Directions GetTouchingBlockDirections(Block[,] objektLayer, int i, int j, int valueToCheck)
        {
            Directions directions = Directions.None;

            if (GetNeighborBlockID(objektLayer, i, j, 0, -1) == valueToCheck)
            {
                directions |= Directions.Up;
            }
            if (GetNeighborBlockID(objektLayer, i, j, 0, 1) == valueToCheck)
            {
                directions |= Directions.Down;
            }
            if (GetNeighborBlockID(objektLayer, i, j, -1, 0) == valueToCheck)
            {
                directions |= Directions.Left;
            }
            if (GetNeighborBlockID(objektLayer, i, j, 1, 0) == valueToCheck)
            {
                directions |= Directions.Right;
            }

            return directions;
        }
        public void GenerateExtraDeko(int i, int j)
        {
            int PMoss = 40;
            int PStone = 110;
            int PSM = 200;

            //Grass
            if (block.BaseLayer[i, j].ID == 1)
            {
                if (random.Next(1, PMoss) < 5)
                {
                    SetBlock(block.DekoLayer, i, j, 7);
                }
                else if (random.Next(1, PStone) < 5)
                {
                    SetBlock(block.DekoLayer, i, j, 8);
                }
                else if (random.Next(1, PSM) < 5)
                {
                    SetBlock(block.DekoLayer, i, j, 9);
                }
            }
        }

        #endregion

        #region Item

        #endregion

        #region Methoden
        private void SetBlock(Block[,] objektLayer, int i, int j, int id)
        {
            objektLayer[i, j] = block.Blocks[id].Clone();
            objektLayer[i, j].pos = new Vector2(i * 64, j * 64);
            //TODO wichtig?
            objektLayer[i, j].Hitbox = new Rectangle((int)objektLayer[i, j].pos.X, (int)objektLayer[i, j].pos.Y, objektLayer[i, j].texture.Width, objektLayer[i, j].texture.Height);
            

        }
        private void SetBlockDeko(Block[,] objektLayer, int i, int j, int id)
        {
            objektLayer[i, j] = block.Blocks[id].Clone();
            block.DekoLayer[i, j].axis = new Vector2(32, 32);
            objektLayer[i, j].pos = new Vector2(i * 64 + 32, j * 64 + 32);
            //TODO wichtig?
            objektLayer[i, j].Hitbox = new Rectangle((int)objektLayer[i, j].pos.X, (int)objektLayer[i, j].pos.Y, objektLayer[i, j].texture.Width, objektLayer[i, j].texture.Height);
            
        }

        public int[] GetSurroundingBlockIDs(Block[,] objektLayer, int i, int j)
        {
            return new int[]
            {
                GetNeighborBlockID(objektLayer, i, j, 0, -1),   // up
                GetNeighborBlockID(objektLayer, i, j, 0, 1),    // down
                GetNeighborBlockID(objektLayer, i, j, -1, 0),   // left
                GetNeighborBlockID(objektLayer, i, j, 1, 0),    // right
                GetNeighborBlockID(objektLayer, i, j, -1, -1),  // UL
                GetNeighborBlockID(objektLayer, i, j, 1, -1),   // UR
                GetNeighborBlockID(objektLayer, i, j, -1, 1),   // DL
                GetNeighborBlockID(objektLayer, i, j, 1, 1)     // DR
            };
        }
        public int[] GetTouchingBlockIDs(Block[,] objektLayer, int i, int j)
        {
            return new int[]
            {
                GetNeighborBlockID(objektLayer, i, j, 0, -1),   // up
                GetNeighborBlockID(objektLayer, i, j, 0, 1),    // down
                GetNeighborBlockID(objektLayer, i, j, -1, 0),   // left
                GetNeighborBlockID(objektLayer, i, j, 1, 0)     // right
            };
        }
        public int GetNeighborBlockID(Block[,] objektLayer, int i, int j, int xOffset, int yOffset)
        {
            int newRow = i + xOffset;
            int newCol = j + yOffset;

            if (newRow >= 0 && newRow < objektLayer.GetLength(0) && newCol >= 0 && newCol < objektLayer.GetLength(1))
            {
                return objektLayer[newRow, newCol].ID;
            }
            else
            {
                return 0;
            }
        }

        public int GetMostCommonSurroundingNeighbor(Block[,] objektLayer, int i, int j)
        {
            int[] neighbors = GetSurroundingBlockIDs(objektLayer, i, j);
            Dictionary<int, int> frequency = new Dictionary<int, int>();


            foreach (int id in neighbors)
            {
                if (frequency.ContainsKey(id))
                {
                    frequency[id]++;
                }
                else
                {
                    frequency[id] = 1;
                }
            }

            //Sort by frequency
            return frequency.OrderByDescending(pair => pair.Value).FirstOrDefault().Key;
        }
        public int GetSumOfNeighborsIDs(Block[,] objektLayer, int i, int j)
        {
            int[] neighbors = GetTouchingBlockIDs(objektLayer, i, j);
            int sum = 0;
            foreach (int value in neighbors)
            {
                sum += value;
            }
            return sum;
        }
        private int GetSurroundingBlockIDType(Block[,] objektLayer, int i, int j, int id)
        {
            int count = 0;
            int[] surroundingBlockIDs = GetSurroundingBlockIDs(objektLayer, i, j);
            for (int k = 0; k < surroundingBlockIDs.Length; ++k)
            {
                if (surroundingBlockIDs[k] == id)
                {
                    ++count;
                }
            }
            return count;
        }

        #endregion
        
    }
}
