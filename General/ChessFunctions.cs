using Chess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// TODO: isKingInCheck()
///     
/// </summary>

namespace Chess.General
{
    class ChessFunctions
    {
        /// <summary>
        /// Returns true if square is a valid square on the chessboard.
        /// </summary>
        /// <param name="square">Square to validate.</param>
        /// <returns></returns>
        public static bool validateSquare(string square)
        {
            System.Diagnostics.Debug.WriteLine("squre to validate: " + square);
            int col = Char.ConvertToUtf32(square.Substring(0, 1), 0);
            int row = Int32.Parse(square.Substring(1));
            return (col > 96 && col < 105 && row > 0 && row < 9);
        }

        // easy ways to get squares up, down, left, right, diagonally (all from white's perspective)
        public static string getVerticalSquare(string original, int num) // original --> original square, num --> number of squares (can be negative)
        {
            // only deals with rank, so only change int part of string
            int newRank = int.Parse(original.Substring(1)) + num;        
            return original.Substring(0, 1) + newRank;
        }

        public static string getHorizontalSquare(string original, int num) // original --> original square, num --> number of squares (can be negative){
        {
            // only deals with file, so only change char part of string
            char newFile = (char)(Char.ConvertToUtf32(original, 0) + num);           
            return (char)newFile + original.Substring(1);
        }

        public static string getRDiagonalSquare(string original, int num) // original --> original square, num --> number of squares (can be negative){
        {
            // deal with both rank and file, combine two methods above
            int newRank = int.Parse(original.Substring(1)) + num;
            char newFile = (char)(Char.ConvertToUtf32(original, 0) + num);
            return newFile + newRank.ToString();
        }

        public static string getLDiagonalSquare(string original, int num) // original --> original square, num --> number of squares (can be negative){
        {
            // deal with both rank and file, combine two methods above
            int newRank = int.Parse(original.Substring(1)) + num;
            char newFile = (char)(Char.ConvertToUtf32(original, 0) - num);
            return newFile + newRank.ToString();
        }

        public static int[] CoordsToNums(string coords) // e.g. e4 --> [4, 3]
        {
            int row;
            int col;

            col = Char.ConvertToUtf32(coords, 0);
            col -= 97;

            if (!int.TryParse(coords.Substring(1), out row))
            {
                System.Diagnostics.Debug.WriteLine("big bad coords:");
                System.Diagnostics.Debug.WriteLine(coords);
                throw new FormatException("Invalid Coordinates!");
            }
            row = 8 - row;

            return new int[] { row, col };
        }

        public static string NumsToCoords(int[] nums)
        {
            string col;
            string row;

            col = Char.ConvertFromUtf32(nums[0] + 97);
            row = (8 - nums[1]).ToString();

            return col + row;
        }

        // TODO: check if king is in check in given position, given a side
        public static bool isKingInCheck(int color, Board board)
        {
            return false;
        }

        public static List<String> getValidMoves(Piece p, Board board)
        {
            int side = Char.IsUpper(p.pieceCode) ? 0 : 1;
            switch (p.pieceCode.ToString().ToLower())
            {
                case "p":
                    return getPawnMoves(p.pos, board, side);
                case "n":
                    return getKnightMoves(p.pos, board, side);
                case "b":
                    return getBishopMoves(p.pos, board, side);
                case "r":
                    return getRookMoves(p.pos, board, side);
                case "q":
                    return getQueenMoves(p.pos, board, side);
                case "k":
                    return getKingMoves(p.pos, board, side);
                default:
                    throw new FormatException("Piece Code is incorrect");
            }
        }

        // TO GET MOVES FROM A SQUARE
        // side: 0 is white, 1 is black
        public static List<String> getPawnMoves(string pos, Board board, int side)
        {
            List<String> results = new List<String>();
            // check square ahead
            string ahead = getVerticalSquare(pos, side == 0 ? 1 : -1);
            if (!board.isFilled(ahead))
            {
                results.Add(ahead);

                // check if pawn is in original position, if so, can move two squares (only if moving one square is also possible)
                if (pos.Substring(1).Equals(side == 0 ? "2" : "7"))
                {
                    string twoahead = getVerticalSquare(pos, side == 0 ? 2 : -2);
                    if (!board.isFilled(twoahead))
                        results.Add(twoahead);
                }
            }

            // check captures and en passant square
            string enPassant = board.enPassant;
            string diagR = getRDiagonalSquare(pos, side == 0 ? 1 : -1);
            string diagL = getLDiagonalSquare(pos, side == 0 ? 1 : -1);
            if (!pos.Substring(0, 1).Equals("h") && (Char.IsLower(board.getSquare(diagR)) || diagR.Equals(enPassant)))
                results.Add(diagR);
            if (!pos.Substring(0, 1).Equals("a") && (Char.IsLower(board.getSquare(diagL)) || diagL.Equals(enPassant)))
                results.Add(diagL);

            // implement promotion here?

            // if move makes king in check, it is illegal
            foreach (string move in new List<String>(results))
            {
                if (!validateSquare(move))
                {
                    results.Remove(move);
                    continue;
                }
                if (isKingInCheck(side, board))
                {
                    results.Remove(move);
                    continue;
                }
            }
            return results;
        }

        public static List<String> getKnightMoves(string pos, Board board, int side)
        {
            List<String> results = new List<String>();
            // hardcoded knight moves

            // to the right by 2
            string right_two = getHorizontalSquare(pos, 2);
            results.Add(getVerticalSquare(right_two, -1));
            results.Add(getVerticalSquare(right_two, 1));
            
            // up by 2
            string up_two = getVerticalSquare(pos, 2);
            results.Add(getHorizontalSquare(up_two, 1));
            results.Add(getHorizontalSquare(up_two, -1));
           
            // to the left by 2
            string left_two = getHorizontalSquare(pos, -2);
            results.Add(getVerticalSquare(left_two, 1));
            results.Add(getVerticalSquare(left_two, -1));
            
            // down by 2
            string down_two = getVerticalSquare(pos, -2);
            results.Add(getHorizontalSquare(down_two, -1));
            results.Add(getHorizontalSquare(down_two, 1));
            
            // if move makes king in check, it is illegal; also check for collisions
            foreach (string move in new List<String>(results))
            {
                if (!validateSquare(move))
                {
                    results.Remove(move);
                    continue;
                }
                if (isKingInCheck(side, board) || board.isFilled(move))
                {
                    results.Remove(move);
                    continue;
                }
            }
            return results;
        }

        public static List<String> getRookMoves(string pos, Board board, int side)
        {
            List<string> results = new List<String>();
            // check vertical moves
            int i = 1; // counter for moves
            // right
            while (true)
            {
                string up = getHorizontalSquare(pos, i);
                if (!validateSquare(up))
                {
                    System.Diagnostics.Debug.WriteLine(up);
                    i = -1;
                    break;
                }

                if (board.isFilled(up))
                {
                    if (board.getSquareColor(up) != side)
                    {
                        results.Add(up);
                    }
                    i = -1;
                    break;
                }
                results.Add(up);
                i++;
            }
            // left
            while (true)
            {
                string down = getHorizontalSquare(pos, i);
                if (!validateSquare(down))
                {
                    i = 1;
                    break;
                }

                if (board.isFilled(down))
                {
                    if (board.getSquareColor(down) != side)
                    {
                        results.Add(down);
                    }
                    i = -1;
                    break;
                }
                results.Add(down);
                i--;
            }
            // up
            while (true)
            {
                string down = getVerticalSquare(pos, i);
                if (!validateSquare(down))
                {
                    i = -1;
                    break;
                }

                if (board.isFilled(down))
                {
                    if (board.getSquareColor(down) != side)
                    {
                        results.Add(down);
                    }
                    i = -1;
                    break;
                }
                results.Add(down);                
                i++;
            }
            // right-downwards
            while (true)
            {
                string down = getVerticalSquare(pos, i);
                if (!validateSquare(down))
                {
                    break;
                }

                if (board.isFilled(down))
                {
                    if (board.getSquareColor(down) != side)
                    {
                        results.Add(down);
                    }
                    i = -1;
                    break;
                }
                results.Add(down);                
                i--;
            }

            // if move makes king in check, it is illegal
            foreach (string move in new List<String>(results))
            {
                if (isKingInCheck(side, board))
                {
                    System.Diagnostics.Debug.WriteLine("move is filled: " + move);
                    results.Remove(move);
                    continue;
                }
            }

            return results;
        }

        public static List<String> getBishopMoves(string pos, Board board, int side)
        {
            List<string> results = new List<String>();
            // check vertical moves
            int i = 1; // counter for moves
            // right-upwards
            while (true)
            {
                string up = getRDiagonalSquare(pos, i);
                if (!validateSquare(up))
                {
                    System.Diagnostics.Debug.WriteLine(up);
                    i = -1;
                    break;
                }

                if (board.isFilled(up))
                {
                    if (board.getSquareColor(up) != side)
                    {
                        results.Add(up);
                    }
                    i = -1;
                    break;
                }
                results.Add(up);
                i++;
            }
            // left-downwards
            while (true)
            {
                string down = getRDiagonalSquare(pos, i);
                if (!validateSquare(down))
                {
                    i = 1;
                    break;
                }

                if (board.isFilled(down))
                {
                    if (board.getSquareColor(down) != side)
                    {
                        results.Add(down);
                    }
                    i = -1;
                    break;
                }
                results.Add(down);
                i--;
            }
            // left-upwards
            while (true)
            {
                string down = getLDiagonalSquare(pos, i);
                if (!validateSquare(down))
                {
                    i = -1;
                    break;
                }

                if (board.isFilled(down))
                {
                    if (board.getSquareColor(down) != side)
                    {
                        results.Add(down);
                    }
                    i = -1;
                    break;
                }
                results.Add(down);                
                i++;
            }
            // right-downwards
            while (true)
            {
                string down = getLDiagonalSquare(pos, i);
                if (!validateSquare(down))
                {
                    break;
                }

                if (board.isFilled(down))
                {
                    if (board.getSquareColor(down) != side)
                    {
                        results.Add(down);
                    }
                    i = -1;
                    break;
                }
                results.Add(down);                
                i--;
            }

            // if move makes king in check, it is illegal
            foreach (string move in new List<String>(results))
            {
                if (isKingInCheck(side, board))
                {
                    System.Diagnostics.Debug.WriteLine("move is filled: " + move);
                    results.Remove(move);
                    continue;
                }
            }

            return results;
        }
        public static List<String> getQueenMoves(string pos, Board board, int side)
        {
            List<String> results = new List<String>();
            results.AddRange(getBishopMoves(pos, board, side));
            results.AddRange(getRookMoves(pos, board, side));
            return results;
        }

        public static List<String> getKingMoves(string pos, Board board, int side)
        {
            List<String> results = new List<String>();
            // hardcoded single moves
            results.Add(getVerticalSquare(pos, 1));
            results.Add(getVerticalSquare(pos, -1));

            results.Add(getHorizontalSquare(pos, 1));
            results.Add(getHorizontalSquare(pos, -1));

            results.Add(getRDiagonalSquare(pos, 1));
            results.Add(getRDiagonalSquare(pos, -1));

            results.Add(getLDiagonalSquare(pos, 1));
            results.Add(getLDiagonalSquare(pos, -1));

            // if move makes king in check, it is illegal
            foreach (string move in new List<String>(results))
            {
                if (!validateSquare(move))
                {
                    results.Remove(move);
                    continue;
                }
                if (isKingInCheck(side, board) || board.isFilled(move))
                {
                    results.Remove(move);
                    continue;
                }
            }

            return results;

        }
    }
}
