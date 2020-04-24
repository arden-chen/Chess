using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Views
{
    abstract class BaseView
    {
        public ContentManager ContentManager;

        public SpriteBatch SpriteBatch;

        public BaseView(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            ContentManager = contentManager;
            SpriteBatch = spriteBatch;
        }

        public abstract void Draw();
    }
}
