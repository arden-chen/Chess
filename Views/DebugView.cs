using Chess.General;
using Chess.Models;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Views
{
    class DebugView : BaseView
    {
        private SpriteFont font;
        private Board board;
        private SpriteBatch spriteBatch;

        public DebugView(ContentManager contentManager, SpriteBatch spriteBatch, Board board)
            : base(contentManager, spriteBatch)
        {
            this.board = board;
            this.spriteBatch = spriteBatch;
            font = contentManager.Load<SpriteFont>("font");
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }
    }
}
