using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.General
{
    class ChessFunctions
    {
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

        public static string[] getValidMoves(char piece, char[,] boardState)
        {
            return null;
        }
    }
}
