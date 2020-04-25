using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
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

        public abstract void Draw();
    }
}
