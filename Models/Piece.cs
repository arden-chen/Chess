using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.General;

namespace Chess.Models
{
    class Piece
    {
        public int row;
        public int column;

        public string pos;

        public Texture2D texture;

        public Piece(Texture2D tx, string coords)
        {
            pos = coords;
            int[] c = ChessFunctions.CoordsToNums(coords);
            row = c[0];
            column = c[1];

            texture = tx;
        }
    }
}
