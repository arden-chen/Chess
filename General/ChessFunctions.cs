using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.General
{
    class ChessFunctions
    {
        public static int[] CoordsToNums(string coords) // e.g. e4
        {
            int row;
            int col;

            row = Char.ConvertToUtf32(coords, 0);
            row -= 97; 

            if (row > 7)
            {
                throw new FormatException("Invalid Coordinates!");
            }

            if (!int.TryParse(coords.Substring(1), out col))
            {
                throw new FormatException("Invalid Coordinates!");
            }
            col = 8 - col;            

            return new int[] { row, col };
        }

        public static string NumsToCoords(int[] nums)
        {
            string row;
            string col;

            col = Char.ConvertFromUtf32(nums[1] + 97);
            row = (8 - nums[0]).ToString();

            return col + row;
        }

        public static string[] getValidMoves(int piece, string pos, )
    }
}
