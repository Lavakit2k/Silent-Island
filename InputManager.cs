using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Silent_Island
{
    //TODO static?
    internal class InputManager
    {
        public static int hitLayerBlock()
        {
            int x = (int)(Main.MousePos.X / 64);
            int y = (int)(Main.MousePos.Y / 64);
            //TODO für mehrere Layer?

            if (Main.MousePos.X >= 0 && Main.MousePos.X < Main.worldSizeX * 64 && Main.MousePos.Y >= 0 && Main.MousePos.Y < Main.worldSizeY * 64)
                return Block.BaseLayer[x, y].ID;


            return 0;
        }
    }
}
