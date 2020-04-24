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

        public BoardView(ContentManager contentManager, SpriteBatch spriteBatch) 
            : base(contentManager, spriteBatch)
        {
            var board = ContentManager.Load<Texture2D>("board");
            Squares = new SpriteSheet(16, 16, board, spriteBatch);
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
        }
    }
}
