using Chess.General;
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
    class BoardView : BaseView
    {
        private SpriteSheet Squares;
        private Texture2D selectedTexture;
        private Piece selected;

        public BoardView(ContentManager contentManager, SpriteBatch spriteBatch, Piece selected) 
            : base(contentManager, spriteBatch)
        {
            var board = ContentManager.Load<Texture2D>("board");
            selectedTexture = contentManager.Load<Texture2D>("selected");
            Squares = new SpriteSheet(16, 16, board, spriteBatch);

            this.selected = selected;
        }

        public override void Draw()
        {
            int counter = 0; // flip between black and white, start with white top left 

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Squares.Draw(new Vector2(x * 16, y * 16), counter, Color.White);
                    counter ^= 1;
                }
                counter ^= 1;
            }

            // if square is selected, draw outline
            if (selected != null)
            {
                int[] coords = ChessFunctions.CoordsToNums(selected.pos);
                SpriteBatch.Draw(selectedTexture, new Rectangle(coords[0] * 16, coords[1] * 16, 16, 16), Color.White);

            }
        }
    }
}
