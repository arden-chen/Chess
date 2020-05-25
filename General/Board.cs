using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.General
{
    // used to store all chess information
    class Board
    {
        public char[,] board;      

        public string lastMove;
        public string enPassant;

        public bool blackCastleKingside = true;
        public bool blackCastleQueenside = true;
        public bool whiteCastleKingside = true;
        public bool whiteCastleQueenside = true;

        // keep track of turn and who made the last move
        // 0 = white
        // 1 = black
        // starts 1 because updateBoard will change this to 0 immediately after white makes a move
        private int turn = 1;

        // total moves
        // increase by 1 every time white makes a move
        private int turnCount = 0;

        public Board(char[,] board)
        {
            this.board = board;
            lastMove = "";
            enPassant = "";
        }

        // used to make a deep copy
        public Board(char[,] board, string lastMove, string enPassant, bool blackCastleKingside, bool blackCastleQueenside, bool whiteCastleKingside, bool whiteCastleQueenside, int turn, int turnCount)
        {
            this.board = board;
            this.lastMove = lastMove;
            this.enPassant = enPassant;
            this.blackCastleKingside = blackCastleKingside;
            this.blackCastleQueenside = blackCastleQueenside;
            this.whiteCastleKingside = whiteCastleKingside;
            this.whiteCastleQueenside = whiteCastleQueenside;
            this.turn = turn;
            this.turnCount = turnCount;
        }

        // called when a move is made
        public void updateBoard(char piece, string original, string final)
        {
            turn ^= 1;
            if (turn == 0)
                turnCount++;

            int[] initPos = ChessFunctions.CoordsToNums(original);
            int[] finalPos = ChessFunctions.CoordsToNums(final);

            board[initPos[0],initPos[1]] = '-';
            board[finalPos[0],finalPos[1]] = piece;
            lastMove = Char.ToLower(piece) + original + " to " + final; // not exactly correct notation; used for data purposes
            // TODO make correct notation
            System.Diagnostics.Debug.WriteLine(lastMove);

            // print new board
            updateData();
        }

        public bool isFilled(string square)
        {            
            int[] coords = ChessFunctions.CoordsToNums(square);
            bool filled = !board[coords[0], coords[1]].Equals('-');
            // System.Diagnostics.Debug.WriteLine(square + " has: " + board[coords[0], coords[1]]);
            /*
            if (filled)
                System.Diagnostics.Debug.WriteLine(square + " is filled");
            else
                System.Diagnostics.Debug.WriteLine(square + " is empty");
                */
            return filled;
        }

        public Board makeCopy()
        {
            return new Board(
                board,
                lastMove,
                enPassant,
                blackCastleKingside,
                blackCastleQueenside,
                whiteCastleKingside,
                whiteCastleQueenside,
                turn,
                turnCount);
        }
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    result += board[i, j];
                }
                result += "\n";
            }
            return result;
        }

        private void updateData()
        {
            // get last move details
            string lastPiece = lastMove.Substring(0, 1);

            switch (lastPiece)
            {
                case "p": // check en passant
                    if (lastMove.Substring(2, 1).Equals("2") && lastMove.Substring(8, 1).Equals("4"))
                    {
                        // white's last move was double pawn move; viable en passsant square behind it
                        enPassant = lastMove.Substring(1, 1) + "3";
                    }
                    else if (lastMove.Substring(2, 1).Equals("7") && lastMove.Substring(8, 1).Equals("5"))
                    {
                        // black's last move was double pawn move; viable en passsant square behind it
                        enPassant = lastMove.Substring(1, 1) + "6";
                    }
                    else
                    {
                        enPassant = "";
                    }
                    // check promotion
                    if (lastMove.Substring(8, 1).Equals("8"))
                    {
                        // white is promoting a pawn
                    } else if (lastMove.Substring(8, 1).Equals("1"))
                    {
                        // black is promoting a pawn
                    }
                    break;
                case "k": // check castle rights
                    if (turn == 0)
                    {
                        whiteCastleKingside = false;
                        whiteCastleQueenside = false;
                    } else
                    {
                        blackCastleKingside = false;
                        blackCastleQueenside = false;
                    }
                    break;
                default:
                    break;
            }
            
            // TODO: check if king is in check
            // use board visualization to check

        }
        
        public char getSquare(string square)
        {
            int[] coords = ChessFunctions.CoordsToNums(square);
            return board[coords[0], coords[1]];
        }
    }
}
