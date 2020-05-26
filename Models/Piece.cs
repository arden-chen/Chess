using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.General;
using System.Security.Cryptography;

namespace Chess.Models
{
    class Piece
    {
        public int row; // 0-7
        public int column; // 0-7

        public string pos; // chess notation e.g. e4
        
        /// Code to know what kind of piece this is: Upper case for white, lower for black
        /// Pp - Pawn
        /// Nn - Knight
        /// Rr - Rook
        /// Bb - Bishop
        /// Qq - Queen
        /// Kk - King
        /// 0 if empty; used for debugging
        public char pieceCode;

        public Texture2D texture;

        public Piece()
        {
            pos = "";
            pieceCode = '0';
        }

        public Piece(Texture2D tx, string coords, char code)
        {
            pos = coords;
            int[] c = ChessFunctions.CoordsToNums(coords);
            row = c[0];
            column = c[1];
            pieceCode = code;
            texture = tx;
        }

        public override string ToString()
        {
            if (pos.Equals("") || pieceCode.Equals('0'))
                return "No piece";
            else
                return pieceCode.ToString() + ", at: " + pos + "; row: " + row + ", col: " + column;
        }

        public void move(string destination)
        {
            pos = destination;
            int[] position = ChessFunctions.CoordsToNums(destination);
            row = position[0];
            column = position[1];
        }
    }
}
