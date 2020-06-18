using Chess.Models;
using Chess.Views;
using Chess.General;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Chess.Controllers
{
    class GameController : BaseController
    {        
        /// <summary>
        /// To Keep track of whose turn it is. 
        /// 0 --> White
        /// 1 --> Black
        /// </summary>
        public int turn;
        public Piece selected = null;
        public char[,] boardState;

        private Board board;
        private ContentManager Content;
        private SpriteBatch spriteBatch;

        private Dictionary<string, BaseView> views = new Dictionary<string, BaseView>();
        private BaseView debug;
        private List<Piece> blackPieces = new List<Piece>();
        private List<Piece> whitePieces = new List<Piece>();

        private MouseState prevState;
        private int squareSize;

        public GameController(ContentManager contentManager, SpriteBatch spriteBatch, int scale) // scale to know size of square; important for piece selection
        {
            turn = 0; // white's turn first
            Content = contentManager;
            this.spriteBatch = spriteBatch;
            squareSize = scale * 16; // 16 is side length of square texture
        }
        
        public override void Initialize()
        {
            loadPieces();
            loadBoard();
            // Add all views, in correct order
            views["board"] = (new BoardView(Content, spriteBatch, selected));
            views["pieces"] = (new PiecesView(Content, spriteBatch, blackPieces, whitePieces));
            debug = (new DebugView(Content, spriteBatch, board));
            System.Diagnostics.Debug.WriteLine(board.ToString());
        }

        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();
            string square = getSquare(state);
            // check if square is a real square; if it is "" then not a square selected
            if (!square.Equals("") && state.LeftButton == ButtonState.Pressed && prevState.LeftButton != ButtonState.Pressed && prevState != null)
            {
                // if player clicking on their piece, they must be trying to select it
                // if white's turn
                if (turn == 0)
                {
                    foreach (Piece p in whitePieces)
                    {
                        if (clickedOnPiece(p, state))
                        {
                            selected = p;
                            board.selected = p;
                            board.currentMoves = ChessFunctions.getValidMoves(selected, board);
                        }
                    }
                }
                // black's turn
                else if (turn == 1)
                {
                    foreach (Piece p in blackPieces)
                    {
                        if (clickedOnPiece(p, state))
                        {
                            // System.Diagnostics.Debug.WriteLine(p.ToString());
                            selected = p;
                            board.selected = p;
                            board.currentMoves = ChessFunctions.getValidMoves(selected, board);
                        }
                    }
                }
                // piece already selected, selected square must be a square the player tries to move piece to
                if (selected != null)
                {                    
                    if (board.currentMoves.Contains(square)) //checkValidSquare(selected, square)
                    {
                        // move piece here
                        // System.Diagnostics.Debug.WriteLine(square)
                        // check if captures
                        if (board.isFilled(square) && board.getSquareColor(selected.pos) != board.getSquareColor(square))
                        {
                            System.Diagnostics.Debug.WriteLine("Piece that just made a capture: " + selected.ToString());
                            // delete piece that is already there
                            if (board.getSquareColor(selected.pos) == 0)
                            {
                                // current piece is white, so check black pieces to remove
                                System.Diagnostics.Debug.WriteLine("Piece that just made a capture: " + selected.ToString());
                                foreach (var piece in blackPieces)
                                {
                                    System.Diagnostics.Debug.WriteLine("Comparing: " + square + " and: " + piece.pos);
                                    if (piece.pos.Equals(square))
                                    {
                                        System.Diagnostics.Debug.WriteLine("captured: " + piece.ToString());
                                        blackPieces.Remove(piece);
                                        break;
                                    }
                                }
                            }
                            else if (board.getSquareColor(selected.pos) == 1)
                            {
                                // current piece is black, so checck white pieces to remove
                                System.Diagnostics.Debug.WriteLine("Piece that just made a capture: " + selected.ToString());
                                foreach (var piece in whitePieces)
                                {
                                    if (piece.pos.Equals(square))
                                    {
                                        whitePieces.Remove(piece);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                // error: no piece???
                            }
                        }
                        board.updateBoard(selected.pieceCode, selected.pos, square);
                        selected.move(square);
                        selected = null;
                        turn ^= 1;
                        System.Diagnostics.Debug.WriteLine(board.ToString());
                        System.Diagnostics.Debug.WriteLine(board.inCheck(turn));
                        board.selected = new Piece();
                        board.currentMoves = new List<String>();
                    }
                }
                // here if square is ""
                
            }

            prevState = state;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var view in views.Values)
            {
                view.Draw();
            }
        }

        public override void Debug(GameTime gameTime)
        {
            debug.Draw();
        }

        // get square that user is hovering on
        private string getSquare(MouseState mouseState)
        {
            int x = mouseState.X;
            int y = mouseState.Y;
            if (x / squareSize < 8 && y / squareSize < 8)
                return ChessFunctions.NumsToCoords(new int[] { x / squareSize, y / squareSize });
            else
                return "";
        }

        // check if user has clicked on a piece
        private bool clickedOnPiece(Piece p, MouseState mouseState)
        {
            int piecex = p.column * squareSize;
            int piecey = p.row * squareSize;
            // System.Diagnostics.Debug.WriteLine("MouseX: " + mouseState.X + " MouseY: " + mouseState.Y + "; PieceX: " + piecex + "PieceY: " + piecey);
            return (mouseState.X > piecex && mouseState.X < piecex + squareSize
                && mouseState.Y > piecey && mouseState.Y < piecey + squareSize);
        }

        // check if mouse position is a valid space for piece to go
        private bool checkValidSquare(Piece p, String square)
        {
            List<String> moves = ChessFunctions.getValidMoves(p, board);
            return moves.Contains(square);
        }

        private void loadBoard()
        {
            /*
            boardState = new char[8,8] { {'r', 'n', 'b', 'q', 'k', 'b', 'n', 'r'}, 
                                         {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'}, 
                                         {'-', '-', '-', '-', '-', '-', '-', '-'},
                                         {'-', '-', '-', '-', '-', '-', '-', '-'},
                                         {'-', '-', '-', '-', '-', '-', '-', '-'},
                                         {'-', '-', '-', '-', '-', '-', '-', '-'},
                                         {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'}, 
                                         {'R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R'} };
            */
            board = new Board(whitePieces,blackPieces);
        }

        private void loadPieces()
        {
            Dictionary<string, Texture2D> black = LoadListContent<Texture2D>(Content, "_black");
            Dictionary<string, Texture2D> white = LoadListContent<Texture2D>(Content, "_white");

            // set up board; positions hard-coded for each piece

            // black pieces
            Piece bRook1 = new Piece(black["blackRook"], "a8", 'r');
            blackPieces.Add(bRook1);
            Piece bKnight1 = new Piece(black["blackKnight"], "b8", 'n');
            blackPieces.Add(bKnight1);
            Piece bBishop1 = new Piece(black["blackBishop"], "c8", 'b');
            blackPieces.Add(bBishop1);
            Piece bQueen = new Piece(black["blackQueen"], "d8", 'q');
            blackPieces.Add(bQueen);
            Piece bKing = new Piece(black["blackKing"], "e8", 'k');
            blackPieces.Add(bKing);
            Piece bBishop2 = new Piece(black["blackBishop"], "f8", 'b');
            blackPieces.Add(bBishop2);
            Piece bKnight2 = new Piece(black["blackKnight"], "g8", 'n');
            blackPieces.Add(bKnight2);
            Piece bRook2 = new Piece(black["blackRook"], "h8", 'r');
            blackPieces.Add(bRook2);

            // white pieces
            Piece wRook1 = new Piece(white["whiteRook"], "a1", 'R');
            whitePieces.Add(wRook1);
            Piece wKnight1 = new Piece(white["whiteKnight"], "b1", 'N');
            whitePieces.Add(wKnight1);
            Piece wBishop1 = new Piece(white["whiteBishop"], "c1", 'B');
            whitePieces.Add(wBishop1);
            Piece wQueen = new Piece(white["whiteQueen"], "d1", 'Q');
            whitePieces.Add(wQueen);
            Piece wKing = new Piece(white["whiteKing"], "e1", 'K');
            whitePieces.Add(wKing);
            Piece wBishop2 = new Piece(white["whiteBishop"], "f1", 'B');
            whitePieces.Add(wBishop2);
            Piece wKnight2 = new Piece(white["whiteKnight"], "g1", 'N');
            whitePieces.Add(wKnight2);
            Piece wRook2 = new Piece(white["whiteRook"], "h1", 'R');
            whitePieces.Add(wRook2);

            // pawns
            for (int i = 97; i < 105; i++)
            {
                string coords = Char.ConvertFromUtf32(i);
                blackPieces.Add(new Piece(black["blackPawn"], coords + "7", 'p'));
                whitePieces.Add(new Piece(white["whitePawn"], coords + "2", 'P'));
            }
        }
        public static Dictionary<string, T> LoadListContent<T>(ContentManager contentManager, string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, T> result = new Dictionary<String, T>();

            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);


                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }
            return result;
        }
    }
}
