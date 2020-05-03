using Chess.Models;
using Chess.Views;
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

// TODO: MAKE BOARD STATE, USING DOUBLE ARRAYS

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

        private ContentManager Content;
        private SpriteBatch spriteBatch;

        private List<BaseView> views = new List<BaseView>();
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

            // Add all views, in correct order
            views.Add(new BoardView(Content, spriteBatch));
            views.Add(new PiecesView(Content, spriteBatch, blackPieces, whitePieces));
        }

        public override void Update(float deltaTime)
        {
            MouseState state = Mouse.GetState();
            if (state.LeftButton == ButtonState.Pressed && prevState.LeftButton != ButtonState.Pressed && prevState != null)
            {               
                if (selected == null)
                {
                    if (turn == 0)
                    {
                        foreach (Piece p in whitePieces)
                        {
                            if (clickedOnPiece(p, state))
                            {
                                selected = p;
                            }
                        }
                    }
                    else if (turn == 1)
                    {
                        foreach (Piece p in blackPieces)
                        {
                            if (clickedOnPiece(p, state))
                            {
                                selected = p;
                            }
                        }
                    }
                }
                else
                {
                    if (checkValidSquare(selected, state))
                    {
                        // move piece here
                    }
                }
                
            }


            prevState = state;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var view in views)
            {
                view.Draw();
            }
        }

        // check if user has clicked on a piece
        private bool clickedOnPiece(Piece p, MouseState mouseState)
        {
            int piecex = p.column * squareSize;
            int piecey = p.row * squareSize;
            return (mouseState.X > piecex && mouseState.X < piecex + squareSize
                && mouseState.Y > piecey && mouseState.Y < piecey + squareSize);
        }

        // check if mouse position is a valid space for piece to go
        private bool checkValidSquare(Piece p, MouseState mouseState)
        {

        }

        private void loadPieces()
        {
            Dictionary<string, Texture2D> black = LoadListContent<Texture2D>(Content, "_black");
            Dictionary<string, Texture2D> white = LoadListContent<Texture2D>(Content, "_white");

            // set up board; positions hard-coded for each piece

            // black pieces
            Piece bRook1 = new Piece(black["blackRook"], "a8");
            blackPieces.Add(bRook1);
            Piece bKnight1 = new Piece(black["blackKnight"], "b8");
            blackPieces.Add(bKnight1);
            Piece bBishop1 = new Piece(black["blackBishop"], "c8");
            blackPieces.Add(bBishop1);
            Piece bQueen = new Piece(black["blackQueen"], "d8");
            blackPieces.Add(bQueen);
            Piece bKing = new Piece(black["blackKing"], "e8");
            blackPieces.Add(bKing);
            Piece bBishop2 = new Piece(black["blackBishop"], "f8");
            blackPieces.Add(bBishop2);
            Piece bKnight2 = new Piece(black["blackKnight"], "g8");
            blackPieces.Add(bKnight2);
            Piece bRook2 = new Piece(black["blackRook"], "h8");
            blackPieces.Add(bRook2);

            // white pieces
            Piece wRook1 = new Piece(white["whiteRook"], "a1");
            whitePieces.Add(wRook1);
            Piece wKnight1 = new Piece(white["whiteKnight"], "b1");
            whitePieces.Add(wKnight1);
            Piece wBishop1 = new Piece(white["whiteBishop"], "c1");
            whitePieces.Add(wBishop1);
            Piece wQueen = new Piece(white["whiteQueen"], "d1");
            whitePieces.Add(wQueen);
            Piece wKing = new Piece(white["whiteKing"], "e1");
            whitePieces.Add(wKing);
            Piece wBishop2 = new Piece(white["whiteBishop"], "f1");
            whitePieces.Add(wBishop2);
            Piece wKnight2 = new Piece(white["whiteKnight"], "g1");
            whitePieces.Add(wKnight2);
            Piece wRook2 = new Piece(white["whiteRook"], "h1");
            whitePieces.Add(wRook2);

            // pawns
            for (int i = 97; i < 105; i++)
            {
                string coords = Char.ConvertFromUtf32(i);
                blackPieces.Add(new Piece(black["blackPawn"], coords + "7"));
                whitePieces.Add(new Piece(white["whitePawn"], coords + "2"));
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
