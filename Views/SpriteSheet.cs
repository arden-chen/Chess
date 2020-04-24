using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Views
{
    class SpriteSheet
    {
        private int Width;
        private int Height;

        private int columns;
        private int rows;

        private Texture2D Sheet;
        private SpriteBatch SpriteBatch;

        public SpriteSheet(int width, int height, Texture2D sheet, SpriteBatch spriteBatch)
        {
            Width = width;
            Height = height;
            Sheet = sheet;
            SpriteBatch = spriteBatch;

            columns = Sheet.Width / Width;
            rows = Sheet.Height / Height;
        }

        public void Draw(Vector2 position, int frame, Color color)
        {
            if (frame < 0 || frame >= columns * rows)
                throw new ArgumentOutOfRangeException($"{frame} is out of range!");

            var column = frame % columns;
            var row = frame / columns;
            var x = column * Width;
            var y = row * Height;

            SpriteBatch.Draw(Sheet,
                             position,
                             new Rectangle(x, y, Width, Height),
                             color);
        }

    }
}
