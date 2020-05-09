using System;
using System.Collections.Generic;
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
        // easy ways to get squares up, down, left, right, diagonally (all from white's perspective)
        public static string getVerticalSquare(string original, int num) // original --> original square, num --> number of squares (can be negative)
        {
            // only deals with rank, so only change int part of string
            int newRank = int.Parse(original.Substring(1)) + num;
            if (newRank > 8 || newRank < 1)
                throw new FormatException("Cannot move that many squares!");
            else
                return original.Substring(0, 1) + newRank;
        }

        public static string getHorizontalSquare(string original, int num) // original --> original square, num --> number of squares (can be negative){
        {
            // only deals with file, so only change char part of string
            char newFile = (char)(Char.ConvertToUtf32(original, 0) + num);
            if (Char.IsLetter(newFile))
                throw new FormatException("Cannot move that many squares!");
            else
                return (char)newFile + original.Substring(1);
        }

        public static string getRDiagonalSquare(string original, int num) // original --> original square, num --> number of squares (can be negative){
        {
            // deal with both rank and file, combine two methods above
            int newRank = int.Parse(original.Substring(1)) + num;
            char newFile = (char)(Char.ConvertToUtf32(original, 0) + num);
            if (newRank > 8 || newRank < 1 || Char.IsLetter(newFile))
                throw new FormatException("Cannot move that many squares!");
            else
                return newFile + newRank.ToString();
        }

        public static string getLDiagonalSquare(string original, int num) // original --> original square, num --> number of squares (can be negative){
        {
            // deal with both rank and file, combine two methods above
            int newRank = int.Parse(original.Substring(1)) + num;
            char newFile = (char)(Char.ConvertToUtf32(original, 0) - num);
            if (newRank > 8 || newRank < 1 || Char.IsLetter(newFile))
                throw new FormatException("Cannot move that many squares!");
            else
                return newFile + newRank.ToString();
        }

        public static int[] CoordsToNums(string coords) // e.g. e4 --> [4, 3]
        {
            int row;
            int col;

            col = Char.ConvertToUtf32(coords, 0);
            col -= 97;

            if (col > 7)
            {
                throw new FormatException("Invalid Coordinates!");
            }

            if (!int.TryParse(coords.Substring(1), out row))
            {
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

        public static string[] getValidMoves(char piece, char[,] boardState)
        {
            return null;
        }

        // TO GET MOVES FROM A SQUARE
        public static List<String> getPawnMoves(string pos, Board board, int side)
        {
            List<String> results = new List<String>();
            // check square ahead
            string ahead = getVerticalSquare(pos, side == 0 ? 1 : -1);
            if (!board.isFilled(ahead))
                results.Add(ahead);

            // check if pawn is in original position, if so, can move two squares
            if (pos.Substring(1).Equals(side == 0 ? 2 : 7))
                results.Add(pos.Substring(0, 1) + (side == 0 ? 4 : 5));

            // check captures and en passant square
            string enPassant = board.enPassant;
            string diagR = getRDiagonalSquare(pos, side == 0 ? 1 : -1);
            string diagL = getLDiagonalSquare(pos, side == 0 ? 1 : -1);
            if (board.isFilled(diagR) || diagR.Equals(enPassant))
                results.Add(diagR);
            if (board.isFilled(diagL) || diagL.Equals(enPassant))
                results.Add(diagL);

            // implement promotion here?

            // if move makes king in check, it is illegal
            foreach (string move in results)
            {
                Board potential = board.makeCopy();
                potential.updateBoard('P', pos, move);
                if (isKingInCheck(side, potential))
                    results.Remove(move);
            }
            return results;
        }

        public static List<String> getKnightMoves(string pos, Board board, int side)
        {
            List<String> results = new List<String>();
            // hardcoded knight moves
            results.Add(getVerticalSquare(getHorizontalSquare(pos, 2), -1));
            results.Add(getVerticalSquare(getHorizontalSquare(pos, 2), 1));

            results.Add(getHorizontalSquare(getVerticalSquare(pos, 2), 1));
            results.Add(getHorizontalSquare(getVerticalSquare(pos, 2), -1));

            results.Add(getVerticalSquare(getHorizontalSquare(pos, -2), 1));
            results.Add(getVerticalSquare(getHorizontalSquare(pos, -2), -1));

            results.Add(getHorizontalSquare(getVerticalSquare(pos, -2), 1));
            results.Add(getHorizontalSquare(getVerticalSquare(pos, -2), 1));

            // if move makes king in check, it is illegal; also check for collisions
            foreach (string move in results)
            {
                Board potential = board.makeCopy();
                potential.updateBoard('P', pos, move);
                if (isKingInCheck(side, potential) || board.isFilled(move))
                    results.Remove(move);
            }
            return results;
        }

        public static List<String> getRookMoves(string pos, Board board, int side)
        {
            List<string> results = new List<String>();
            // check vertical moves
            int i = 1; // counter for moves
            // upwards
            while (true)
            {
                try
                {
                    string up = getVerticalSquare(pos, i);
                    if (board.isFilled(up))
                    {
                        i = -1;
                        break;
                    }
                    results.Add(up);
                }
                catch (FormatException e)
                {
                    i = -1;
                    break;
                }
                i++;
            }
            // downwards
            while (true)
            {
                try
                {
                    string down = getVerticalSquare(pos, i);
                    if (board.isFilled(down))
                    {
                        i = 1;
                        break;
                    }
                    results.Add(down);
                }
                catch (FormatException e)
                {
                    i = 1;
                    break;
                }
                i--;
            }
            // rightwards
            while (true)
            {
                try
                {
                    string down = getHorizontalSquare(pos, i);
                    if (board.isFilled(down))
                    {
                        i = -1;
                        break;
                    }
                    results.Add(down);
                }
                catch (FormatException e)
                {
                    i = -1;
                    break;
                }
                i++;
            }
            // leftwards
            while (true)
            {
                try
                {
                    string down = getHorizontalSquare(pos, i);
                    if (board.isFilled(down))
                    {
                        break;
                    }
                    results.Add(down);
                }
                catch (FormatException e)
                {
                    break;
                }
                i--;
            }

            // if move makes king in check, it is illegal
            foreach (string move in results)
            {
                Board potential = board.makeCopy();
                potential.updateBoard('p', pos, move);
                if (isKingInCheck(side, potential))
                    results.Remove(move);
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
                try
                {
                    string up = getRDiagonalSquare(pos, i);
                    if (board.isFilled(up))
                    {
                        i = -1;
                        break;
                    }
                    results.Add(up);
                }
                catch (FormatException e)
                {
                    i = -1;
                    break;
                }
                i++;
            }
            // left-downwards
            while (true)
            {
                try
                {
                    string down = getRDiagonalSquare(pos, i);
                    if (board.isFilled(down))
                    {
                        i = 1;
                        break;
                    }
                    results.Add(down);
                }
                catch (FormatException e)
                {
                    i = 1;
                    break;
                }
                i--;
            }
            // left-upwards
            while (true)
            {
                try
                {
                    string down = getLDiagonalSquare(pos, i);
                    if (board.isFilled(down))
                    {
                        i = -1;
                        break;
                    }
                    results.Add(down);
                }
                catch (FormatException e)
                {
                    i = -1;
                    break;
                }
                i++;
            }
            // right-downwards
            while (true)
            {
                try
                {
                    string down = getLDiagonalSquare(pos, i);
                    if (board.isFilled(down))
                    {
                        break;
                    }
                    results.Add(down);
                }
                catch (FormatException e)
                {
                    break;
                }
                i--;
            }

            // if move makes king in check, it is illegal
            foreach (string move in results)
            {
                Board potential = board.makeCopy();
                potential.updateBoard('p', pos, move);
                if (isKingInCheck(side, potential))
                    results.Remove(move);
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
            foreach (string move in results)
            {
                Board potential = board.makeCopy();
                potential.updateBoard('p', pos, move);
                if (isKingInCheck(side, potential))
                    results.Remove(move);
            }

            return results;

        }
    }
}
