using Chess.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Views
{
    class PiecesView : BaseView
    {
        private List<Piece> blackPieces = new List<Piece>();
        private List<Piece> whitePieces = new List<Piece>();

        private SpriteBatch SpriteBatch;

        public PiecesView(ContentManager contentManager, SpriteBatch spriteBatch)
            : base(contentManager, spriteBatch)
        {
            Dictionary<string, Texture2D> black = LoadListContent<Texture2D>(contentManager, "_black");
            Dictionary<string, Texture2D> white = LoadListContent<Texture2D>(contentManager, "_white");

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


            SpriteBatch = spriteBatch;
        }

        public override void Draw()
        {
            foreach (Piece p in whitePieces)
            {
                SpriteBatch.Draw(p.texture, new Rectangle(p.row * 16, p.column * 16, 16, 16), Color.White);
            }

            foreach (Piece p in blackPieces)
            {
                SpriteBatch.Draw(p.texture, new Rectangle(p.row * 16, p.column * 16, 16, 16), Color.White);
            }
        }
    }
}
