using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Island;
using System;

namespace Silent_Island
{
    public class Layer
    {
        private Main main;
        Textures textures;
        public int x { get; set; }
        public int y { get; set; }
        public int[,] IDLayer { get; set; }
        public Objekt[,] objekt { get; set; }

        public Layer(Main main, Block[,] layer, int[,] layerID)
        {
            this.x = main.worldSizeX;
            this.y = main.worldSizeY;
            this.objekt = layer;
            this.IDLayer = layerID;
            this.main = main;
            this.textures = this.main.texture;
        }

        public void Zeichne()
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
                        this.objekt[i, j].Zeichne(main.spriteBatch);
                    }
                }
            }
        }
        public void UpdateLayer()
        {
            for (int i = 0; i < this.IDLayer.GetLength(0); i++)
            {
                for (int j = 0; j < this.IDLayer.GetLength(1); j++)
                {
                    this.objekt[i, j].texture = textures.GetTextureByBlockID(this.IDLayer[i, j]);
                    this.objekt[i, j].Hitbox = new Rectangle((int)this.objekt[i, j].coords.X, (int)this.objekt[i, j].coords.Y, this.objekt[i, j].texture.Width, this.objekt[i, j].texture.Height);
                }
            }
        }
    }

}
