using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silent_Island_PC
{
    internal class Database
    {
        public Block BlockArt { get; set; }
        public Deko DekoArt { get; set; }
        public Objekt ObjektID { get; set; }

        public enum Block
        {
            Gras = 1,
            Wasser = 2,
            Kies = 3,
            Stein = 4
        }
        public enum Deko
        {
            Empty,
            Moos,
            MoosStein,
            Stein,
            GrasRand_1, 
            GrasRand_2L, 
            GrasRand_2H, 
            GrasRand_3, 
            GrasRand_4, 
        }
        public enum Objekt
        {
            Angel,
            Stuhl
        }
        //Deko heute = Deko.Moos;
    }
}
