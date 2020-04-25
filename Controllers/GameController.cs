using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Controllers
{
    class GameController : BaseController
    {
        /// <summary>
        /// To Keep track of whose turn it is. 
        /// 0 --> White
        /// 1 --> Black
        /// </summary>
        public int turn;

        public GameController()
        {
            turn = 0; // white's turn first
        }
        public override void Update(float deltaTime)
        {
            
        }
    }
}
