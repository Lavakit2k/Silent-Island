using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Island;
using System;

namespace Silent_Island_PC
{
    public class Layer
    {
        private Main main;
        public int x { get; set; }
        public int y { get; set; }
        public int[,] IDLayer { get; set; }
        public Objekt[,] objekt { get; set; }



        public Layer(Main main, Block[,] layer, int[,] layerID, Textures textures)
        {
            this.x = main.worldSizeX;
            this.y = main.worldSizeY;
            this.objekt = layer;
            this.IDLayer = layerID;
            this.main = main;
        }
        public void Zeichne(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.x; i++)
            {
                for (int j = 0; j < this.y; j++)
                {
                    if (this.objekt[i, j].coords.X > main.cameraPosition.X - main.screenWidth - 64 &&
                        this.objekt[i, j].coords.X < main.cameraPosition.X + main.screenWidth + 64 &&
                        this.objekt[i, j].coords.Y > main.cameraPosition.Y - main.screenHeight - 64 &&
                        this.objekt[i, j].coords.Y < main.cameraPosition.Y + main.screenHeight + 64)
                    {
                        this.objekt[i, j].Zeichne(spriteBatch);
                    }
                }
            }
        }

    }
}
