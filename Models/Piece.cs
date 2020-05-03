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
        public int row; // 0-7
        public int column; // 0-7

        public string pos; // chess notation e.g. e4
        
        /// Code to know what kind of piece this is:
        /// 0 - Pawn
        /// 1 - Knight
        /// 2 - Rook
        /// 3 - Bishop
        /// 4 - Queen
        /// 5 - King
        public int pieceCode;

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
