using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Island_PC;
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
        public int PWater;
        public int PGrass;
        public int PGravel;
        public int smoothness = 10;
        public int heavy;

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


        public WorldGeneration(Main main, Textures texture, Structure structure)
        {
            this.main = main;
            this.texture = texture;
            this.structure = structure;
            for (int i = 0; i < main.worldSizeX; ++i)
            {
                for (int j = 0; j < main.worldSizeY; ++j)
                {
                    main.BlockID[i, j] = random.Next(1, 3);
                    main.StructerID[i, j] = 0;
                    main.StructurLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.Empty);
                    main.DekoID[i, j] = random.Next(1, 3);
                    main.DekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.Empty);
                    main.ItemID[i, j] = 0;
                }
            }
        }

        #region Base
        public Layer GenerateBase(Block[,] blockLayer, int[,] blockID)
        {
            GenerateTerrain(blockLayer, blockID);
            AddExtraTerrain(blockLayer, blockID);

            return new Layer(main, blockLayer, blockID, texture);
        }

        private void GenerateTerrain(Block[,] blockLayer, int[,] blockID)
        {
            for (int i = 0; i < main.worldSizeX; ++i)
            {
                for (int j = 0; j < main.worldSizeY; ++j)
                {
                    for (int t = 0; t < smoothness; ++t)
                    {
                        #region Grass/Water
                        int PGrass = GetSurroundingBlockIDType(blockID, i, j, 1);
                        int PWater = GetSurroundingBlockIDType(blockID, i, j, 2);

                        if (blockID[i, j] == 2 && PGrass > 4)
                        {
                            SetBlock(blockLayer, blockID, i, j, 1, texture.Grass);
                        }
                        else if (blockID[i, j] == 1 && PWater > 5)
                        {
                            SetBlock(blockLayer, blockID, i, j, 2, texture.Water);
                        }
                        else
                        {
                            switch (blockID[i, j])
                            {
                                case 1:
                                    blockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.Grass);
                                    break;
                                case 2:
                                    blockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.Water);
                                    break;
                            }
                        }
                        #endregion
                    }
                }
            }
        }
        private void AddExtraTerrain(Block[,] blockLayer, int[,] blockID)
        {
            for (int i = 0; i < main.worldSizeX; ++i)
            {
                for (int j = 0; j < main.worldSizeY; ++j)
                {
                    for (int t = 0; t < 3; ++t)
                    {
                        #region Gravel//Roots

                        if (blockID[i, j] == 2) break;

                        int PGravel = GetSurroundingBlockIDType(blockID, i, j, 3);

                        if (PGravel > 4 && random.Next(1, PGravel - PGravel / 2) == 1)
                        {
                            SetBlock(blockLayer, blockID, i, j, 3, texture.Gravel);
                        }
                        else if (random.Next(1, 30) == 1)
                        {
                            SetBlock(blockLayer, blockID, i, j, 3, texture.Gravel);
                        }

                        if (random.Next(1, 80) == 3)
                        {
                            SetBlock(blockLayer, blockID, i, j, 4, texture.GrassRoot);
                        }

                        #endregion
                    }
                }
            }
        }

        #endregion

        #region Structures
        public Layer GenerateStructers(Block[,] structureLayer, int[,] structureID)
        {
            for (int i = 0; i < main.worldSizeX; ++i)
            {
                for (int j = 0; j < main.worldSizeY; ++j)
                {
                    if (main.BlockID[i, j] == 4)
                    {
                        GenerateTree(structureLayer, structureID, i, j);
                    }
                }
            }
            return new Layer(main, structureLayer, structureID, texture);
        }
        //starts in upper corner
        public void GenerateStructure(Block[,] structureLayer, int[,] structureID, int startX, int startY, int[,] pattern)
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


                    if (x >= 0 && x < structureID.GetLength(0) && y >= 0 && y < structureID.GetLength(1))
                    {
                        if (structureID[x, y] == 0 || structureID[x, y] == 5) // Nur setzen, wenn noch kein Block existiert
                        {
                            if (blockType != 0)
                            {
                                //Textur bestimmen
                                Texture2D blockTexture = null;
                                switch (blockType)
                                {
                                    case 5:
                                        blockTexture = texture.TreeLog;
                                        break;
                                    case 6:
                                        blockTexture = texture.TreeLeave;
                                        break;
                                }

                                if (blockTexture != null)
                                {
                                    structureLayer[x, y] = new Block(new Vector2(x * 64, y * 64), blockTexture);
                                    structureID[x, y] = blockType;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GenerateTree(Block[,] structureLayer, int[,] structureID, int i, int j)
        {
            GenerateStructure(structureLayer, structureID, i - 2, j - 4, structure.treePattern);
        }
        #endregion

        #region Deko
        public Layer GenerateDeko(Block[,] dekoLayer, int[,] dekoID)
        {
            for (int i = 0; i < main.worldSizeX; ++i)
            {
                for (int j = 0; j < main.worldSizeY; ++j)
                {
                    GenerateEdges(dekoLayer, dekoID, i, j);
                    GenerateExtraDeko(dekoLayer, dekoID, i, j);
                }
            }
            return new Layer(main, dekoLayer, dekoID, texture);
        }

        //Edges
        private void GenerateEdges(Block[,] dekoLayer, int[,] dekoID, int i, int j)
        {
            //currentBlockID DER BLOCK WELCHEM DEM RAND ANGEPASST IST z.B. Gras
            //TODO für später mehrere blockIDs machbar z.B. Kies
            int currentBlockID = 1;
            
            Directions touchingDirections = GetTouchingBlockDirections(main.BlockID, i, j, currentBlockID);

            //Nur wenn Wasser/ Gravel
            if (main.BlockID[i,j] == 2 || main.BlockID[i, j] == 3)
            switch (touchingDirections)
            {
                    //all
                case Directions.Up | Directions.Down | Directions.Left | Directions.Right:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeO); 
                    break;

                    //U 
                case Directions.Up | Directions.Down | Directions.Left:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeU); 
                    dekoLayer[i, j].rotation = MathHelper.ToRadians(90);
                    break;                    
                case Directions.Up | Directions.Down | Directions.Right:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeU);
                    dekoLayer[i, j].rotation = MathHelper.ToRadians(-90);
                    break;
                case Directions.Up | Directions.Left | Directions.Right:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeU);
                    dekoLayer[i, j].rotation = MathHelper.ToRadians(180);
                    break;
                case Directions.Down | Directions.Left | Directions.Right:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeU);                     
                    break;

                    //H
                case Directions.Up | Directions.Down:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeH);
                        dekoLayer[i, j].rotation = MathHelper.ToRadians(90);
                    break;
                case Directions.Left | Directions.Right:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeH);
                    break;

                    //L
                case Directions.Up | Directions.Left:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeL);
                    dekoLayer[i, j].rotation = MathHelper.ToRadians(-180);
                    break;
                case Directions.Up | Directions.Right:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeL);
                    dekoLayer[i, j].rotation = MathHelper.ToRadians(-90);
                    break;
                case Directions.Down | Directions.Left:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeL);
                    dekoLayer[i, j].rotation = MathHelper.ToRadians(90);
                    break;
                case Directions.Down | Directions.Right:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeL);                     
                    break;

                    //I
                case Directions.Up:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeI);
                    dekoLayer[i, j].rotation = MathHelper.ToRadians(-90);
                    break;
                case Directions.Down:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeI);
                    dekoLayer[i, j].rotation = MathHelper.ToRadians(90);
                    break;
                case Directions.Left:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeI);
                    dekoLayer[i, j].rotation = MathHelper.ToRadians(180);
                    break;
                case Directions.Right:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.GrassEdgeI);                     
                    break;
                default:
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.Empty);                     
                    break;
                
            }

        }
        public Directions GetTouchingBlockDirections(int[,] blockID, int i, int j, int valueToCheck)
        {
            Directions directions = Directions.None;

            if (GetNeighborBlockID(blockID, i, j, 0, -1) == valueToCheck)
            {
                directions |= Directions.Up;
            }
            if (GetNeighborBlockID(blockID, i, j, 0, 1) == valueToCheck)
            {
                directions |= Directions.Down;
            }
            if (GetNeighborBlockID(blockID, i, j, -1, 0) == valueToCheck)
            {
                directions |= Directions.Left;
            }
            if (GetNeighborBlockID(blockID, i, j, 1, 0) == valueToCheck)
            {
                directions |= Directions.Right;
            }

            return directions;
        }
        public void  GenerateExtraDeko(Block[,] dekoLayer, int[,] dekoID, int i, int j)
        {
            int PMoss = 40;
            int PStone = 110;
            int PSM = 200;

            //Grass
            if (main.BlockID[i,j] == 1)
            {
                if(random.Next(1, PMoss) < 5)
                {
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.DekoMoss);
                    dekoID[i, j] = 7;
                }
                else if(random.Next(1, PStone) < 5)
                {
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.DekoStone);
                    dekoID[i, j] = 8;
                }
                else if(random.Next(1, PSM) < 5)
                {
                    dekoLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture.DekoMossStone);
                    dekoID[i, j] = 9;
                }
            }
        }

        #endregion

        #region Item
        public Layer GenerateItemLayer(Block[,] structureLayer, int[,] structureID)
        {

            return new Layer(main, structureLayer, structureID, texture);
        }

        #endregion

        #region Methoden
        private void SetBlock(Block[,] objektLayer, int[,] objektID, int i, int j, int objektType, Texture2D texture)
        {
            objektID[i, j] = objektType;
            objektLayer[i, j] = new Block(new Vector2(i * 64, j * 64), texture);
        }

        public int[] GetSurroundingBlockIDs(int[,] blockID, int i, int j)
        {
            return new int[]
            {
                GetNeighborBlockID(blockID, i, j, 0, -1),   // up
                GetNeighborBlockID(blockID, i, j, 0, 1),    // down
                GetNeighborBlockID(blockID, i, j, -1, 0),   // left
                GetNeighborBlockID(blockID, i, j, 1, 0),    // right
                GetNeighborBlockID(blockID, i, j, -1, -1),  // UL
                GetNeighborBlockID(blockID, i, j, 1, -1),   // UR
                GetNeighborBlockID(blockID, i, j, -1, 1),   // DL
                GetNeighborBlockID(blockID, i, j, 1, 1)     // DR
            };
        }
        public int[] GetTouchingBlockIDs(int[,] blockID, int i, int j)
        {
            return new int[]
            {
                GetNeighborBlockID(blockID, i, j, 0, -1),   // up
                GetNeighborBlockID(blockID, i, j, 0, 1),    // down
                GetNeighborBlockID(blockID, i, j, -1, 0),   // left
                GetNeighborBlockID(blockID, i, j, 1, 0)     // right
            };
        }
        
        public int GetNeighborBlockID(int[,] blockID, int i, int j, int xOffset, int yOffset)
        {
            int newRow = i + xOffset;
            int newCol = j + yOffset;

            if (newRow >= 0 && newRow < blockID.GetLength(0) && newCol >= 0 && newCol < blockID.GetLength(1))
            {
                return blockID[newRow, newCol];
            }
            else
            {
                return 0;
            }
        }
        public int GetMostCommonSurroundingNeighbor(int[,] blockID, int i, int j)
        {
            int[] neighbors = GetSurroundingBlockIDs(blockID, i, j);
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
        public int GetSumOfNeighborsIDs(int[,] blockID, int i, int j)
        {
            int[] neighbors = GetTouchingBlockIDs(blockID, i, j);
            int sum = 0;
            foreach (int value in neighbors)
            {
                sum += value;
            }
            return sum;
        }
        private int GetSurroundingBlockIDType(int[,] blockID, int i, int j, int blockType)
        {
            int count = 0;
            int[] surroundingBlockIDs = GetSurroundingBlockIDs(blockID, i, j);
            for (int k = 0; k < surroundingBlockIDs.Length; ++k)
            {
                if (surroundingBlockIDs[k] == blockType)
                {
                    ++count;
                }
            }
            return count;
        }

        #endregion
    }
}
