﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Silent_Island_PC
{
    public class Block : Objekt
    {
        public bool placed { get; set; }
        public static Dictionary<int, string> Blocks { get; } = new Dictionary<int, string>()
        {
            { 0, "Void" },
            { 1, "Grass" },
            { 2, "Water"},
            { 3, "Gravel" },

        };
        public Block(Vector2 koordinaten, Texture2D textur) : base(koordinaten, textur)
        {
            Textur = textur;
            Koordinaten = koordinaten;
            Färbung = Color.White;
            Rotation = MathHelper.ToRadians(0);
            Drehpunkt = new Vector2(0, 0);
            Skalierung = new Vector2(1, 1);
            Effekt = SpriteEffects.None;
            Stufe = 0;
            Hitbox = new Vector2(koordinaten.X + textur.Width, koordinaten.Y + textur.Height);
            aktiv = true;
            placed = false;
            ID = 0;
            Name = "Void";
        }
        public void Place(Objekt objekt, Objekt[,] iteem, Vector2 maus)
        {
            int i = (int)(maus.X / 64);
            int j = (int)(maus.Y / 64);

            // Überprüfen, ob die Indizes innerhalb der Grenzen des block-Arrays liegen
            if (i >= 0 && i < iteem.GetLength(0) && j >= 0 && j < iteem.GetLength(1))
            {
                objekt.Koordinaten = iteem[i, j].Koordinaten;
                objekt.Hitbox = new Vector2(objekt.Koordinaten.X + objekt.Textur.Width, objekt.Koordinaten.Y + objekt.Textur.Height);
            }
        }

    }
}
