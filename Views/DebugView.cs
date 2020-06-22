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
    class DebugView : BaseView
    {
        private int _WIDTH = 256;
        private int _HEIGHT = 128;
        private int scale = 8; // change based on the one in chessgame

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
            spriteBatch.DrawString(font, "Turn: " + (board.turn == 0 ? "white" : "black"), new Vector2(_WIDTH/2 * scale, 0), Color.White);
            spriteBatch.DrawString(font, "Last Move: " + board.lastMove, new Vector2(_WIDTH / 2 * scale, _HEIGHT / 10 * scale), Color.White);
            spriteBatch.DrawString(font, "Selected Piece: " + board.selected.ToString(), new Vector2(_WIDTH / 2 * scale, _HEIGHT / 10 * scale * 2), Color.White);
            spriteBatch.DrawString(font, "Current Valid Moves: " + formatMoves(board.currentMoves), new Vector2(_WIDTH / 2 * scale, _HEIGHT / 10 * scale * 3), Color.White);
            if (board.win == 1)
                spriteBatch.DrawString(font, "White wins!", new Vector2(_WIDTH / 2 * scale, _HEIGHT / 2 * scale), Color.White);
            else if (board.win == 2)
                spriteBatch.DrawString(font, "Black wins!", new Vector2(_WIDTH / 2 * scale, _HEIGHT / 2 * scale), Color.White);
        }

        // format moves list to print out
        private string formatMoves(List<String> moves)
        {
            if (moves.Count == 0)
                return "None";
            else
            {
                string result = "";
                foreach (var move in moves)
                {
                    result += move + " ";
                }
                return result;
            }
        }
    }
}
