using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Controllers
{
    abstract class BaseController
    {
        public abstract void Update(float deltaTime);
    }
}
