﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Silent_Island
{

    public class Entity : Objekt
    {
        public int health { get; set; }
        public int speed { get; set; }

        public Dictionary<int, Entity> LoadedEntitys = new Dictionary<int, Entity>();

        public Entity(Vector2 koordinaten, Texture2D texture, int id, string name) : base(koordinaten, texture, id, name)
        {
            this.texture = texture;
            pos = koordinaten;
            Hitbox = new Rectangle((int)koordinaten.X + 16, (int)koordinaten.Y - 16, texture.Width -32, texture.Height - 32);
            ID = id;
            this.name = name;
            health = 0;
            speed = 5;
        }
        public Entity(Textures t, Main m) : base(t, m)
        {
            this.textures = t;
            this.main = m;
        }
        public void MovePlayer(bool moving, Texture2D newTexture, int x, int y)
        {
            moving = true;
            this.texture = newTexture;
            this.pos = new Vector2(this.pos.X + x, this.pos.Y + y);
            this.Hitbox = new Rectangle((int)this.pos.X, (int)this.pos.Y + 32, this.texture.Width, this.texture.Height - 32);
        }

        public bool ColideLayer(Block[,] layer, Vector2 moveVector)
        {
            // Berechne die zukünftige Position der Spielfigur nach der Bewegung
            Rectangle futureHitbox = new Rectangle(
                (int)(this.pos.X + moveVector.X),
                (int)(this.pos.Y + moveVector.Y),
                this.Hitbox.Width,
                this.Hitbox.Height
            );

            // Prüfe auf Kollision mit jeder relevanten Kachel
            int minX = futureHitbox.Left / 64;
            int maxX = futureHitbox.Right / 64;
            int minY = futureHitbox.Top / 64;
            int maxY = futureHitbox.Bottom / 64;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (x >= 0 && x < layer.GetLength(0) && y >= 0 && y < layer.GetLength(1))
                    {
                        if (layer[x, y].ID == 2 && futureHitbox.Intersects(layer[x, y].Hitbox))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public Entity Player;

        public void LoadAllEnitys(Main main)
        {
            //Player
            for (int i = 0; i < main.worldSizeX; i++)
            {
                for (int j = 0; j < main.worldSizeY; j++)
                {
                    if (main.block.BaseLayer[i, j].ID == 1 &&
                        i > 0 && main.block.BaseLayer[i - 1, j].ID == 1 && // links
                        i < main.worldSizeX - 1 && main.block.BaseLayer[i + 1, j].ID == 1 && // rechts
                        j > 0 && main.block.BaseLayer[i, j - 1].ID == 1 && // oben
                        j < main.worldSizeY - 1 && main.block.BaseLayer[i, j + 1].ID == 1) // unten
                    {
                        Player = new Entity(new Vector2(i * 64, j * 64), textures.PlayerUp, 1, "Player");
                        Player.speed = 8;
                        return;
                    }
                }
            }
            
        }
    }
}
