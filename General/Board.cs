using Chess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.General
{
    // used to store all chess information
    // also used to print out debug information
    class Board
    {
        public char[,] board;
        public char[,] testboard; // test moves, updated to be the same as board after every move

        public List<Piece> blackPieces;
        public List<Piece> whitePieces;

        public string lastMove;
        public string enPassant;

        public bool blackCastleKingside = true;
        public bool blackCastleQueenside = true;
        public bool whiteCastleKingside = true;
        public bool whiteCastleQueenside = true;

        // keep track of turn and who made the last move
        // 0 = white
        // 1 = black
        public int turn = 0;

        // total moves
        // increase by 1 every time white makes a move
        public int turnCount = 0;

        // keep track of current selected piece; useful for debugging.
        public Piece selected;

        // keep track of selected pieces moves; useful for debugging.
        public List<String> currentMoves;

        public Board(List<Piece> whitePieces, List<Piece> blackPieces)
        {
            this.board = new char[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = '-';
                }
            }

            this.blackPieces = blackPieces;
            this.whitePieces = whitePieces;
            foreach (Piece p in blackPieces)
            {
                int[] pos = ChessFunctions.CoordsToNums(p.pos);
                this.board[pos[0], pos[1]] = p.pieceCode;
            }
            foreach (Piece p in whitePieces)
            {
                int[] pos = ChessFunctions.CoordsToNums(p.pos);
                this.board[pos[0], pos[1]] = p.pieceCode;
            }
            lastMove = "";
            enPassant = "";
            selected = new Piece();
            currentMoves = new List<String>();
        }

        public Board(char[,] board)
        {
            this.board = board;
            lastMove = "";
            enPassant = "";
            selected = new Piece();
            currentMoves = new List<String>();
        }

        /// used to make a deep copy
        public Board(char[,] board, string lastMove, string enPassant, bool blackCastleKingside, bool blackCastleQueenside, bool whiteCastleKingside, bool whiteCastleQueenside, int turn, int turnCount, Piece selected, List<String> currentMoves)
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
            this.selected = selected;
            this.currentMoves = currentMoves;
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
            // System.Diagnostics.Debug.WriteLine(lastMove);

            // print new board
            updateData();
            // update test board
            testboard = board;
        }

        public bool isFilled(string square)
        {            
            int[] coords = ChessFunctions.CoordsToNums(square);
            // System.Diagnostics.Debug.WriteLine(square + ": " + coords[0] + "," + coords[1]);
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

        /// <summary>
        /// Make a deep copy of this board
        /// </summary>
        /// <returns></returns>
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
                turnCount,
                selected,
                currentMoves);
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
        
        /// <summary>
        /// Get the code of the piece at the square.
        /// </summary>
        /// <param name="square">Square that is desired.</param>
        /// <returns></returns>
        public char getSquare(string square)
        {
            int[] coords = ChessFunctions.CoordsToNums(square);
            return board[coords[0], coords[1]];
        }

        /// <summary>
        /// Get the color of the piece at this square, if there is one.
        /// </summary>
        /// <param name="square">Square that is desired.</param>
        /// <returns>Returns 0 if white, 1 if black, -1 if not applicable (no piece).</returns>
        public int getSquareColor(string square)
        {
            char piece = getSquare(square);
            if (Char.IsLower(piece))
                return 1;
            else if (Char.IsUpper(piece))
                return 0;
            else
                return -1;
        }

        /// <summary>
        /// Returns the square that the king is on (given a side)
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public string getKingSquare(int side)
        {
            System.Diagnostics.Debug.WriteLine("side: " + side);
            if (side == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i,j].Equals('K'))
                            return ChessFunctions.NumsToCoords(new int[] { j, i });
                    }
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i, j].Equals('k'))
                            return ChessFunctions.NumsToCoords(new int[] { j, i });
                    }
                }
            }
            // something REALLY BAD happened
            return null;
        }

        /// <summary>
        /// Test if side is in check.
        /// 0 --> white
        /// 1 --> black 
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public bool inCheck(int side)
        {
            // list of all squares that opposing side's pieces attack
            List<String> moves = new List<string>();
            // get king's square
            string king = getKingSquare(side);
            if (side == 0)
            {
                // white
                foreach (Piece p in blackPieces)
                {
                    moves.AddRange(ChessFunctions.getValidMoves(p, this));
                }
            }
            else
            {
                // black
                foreach (Piece p in whitePieces)
                {
                    moves.AddRange(ChessFunctions.getValidMoves(p, this));
                }
            }
            System.Diagnostics.Debug.Write("moves: ");
            foreach (string move in moves)
                System.Diagnostics.Debug.Write(move + " ");
            System.Diagnostics.Debug.Write("\n king: " + king);
            return moves.Contains(king);
        }

        /// <summary>
        /// Given a move, see if the move puts its own king in check.
        /// </summary>
        /// <param name="piece">Code for piece in question.</param>
        /// <param name="original">Position of the piece originally.</param>
        /// <param name="final">Position where the piece seeks to go.</param>
        /// <returns>If move is invalid, returns false. If valid, true.</returns>
        public bool checkValidate(char piece, string original, string final)
        {
            // make a copy of the board, to test this move
            Board newBoard = new Board(this.board);
            newBoard.updateBoard(piece, original, final);
            System.Diagnostics.Debug.WriteLine("this board:");
            System.Diagnostics.Debug.WriteLine(this.ToString());
            System.Diagnostics.Debug.WriteLine("test board:");
            System.Diagnostics.Debug.WriteLine(newBoard.ToString());
            return true;
        }
    }
}
