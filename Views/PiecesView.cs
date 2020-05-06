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
    // Piece Storage
    class PiecesView : BaseView
    {
        private List<Piece> blackPieces = new List<Piece>();
        private List<Piece> whitePieces = new List<Piece>();

        public PiecesView(ContentManager contentManager, SpriteBatch spriteBatch, List<Piece> black, List<Piece> white)
            : base(contentManager, spriteBatch)
        {
            blackPieces = black;
            whitePieces = white;
        }

        public override void Draw()
        {
            foreach (Piece p in whitePieces)
            {
                SpriteBatch.Draw(p.texture, new Rectangle(p.column * 16, p.row * 16, 16, 16), Color.White);
            }

            foreach (Piece p in blackPieces)
            {
                SpriteBatch.Draw(p.texture, new Rectangle(p.column * 16, p.row * 16, 16, 16), Color.White);
            }
        }
    }
}
