using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.FSM
{
    abstract class State
    {
        /// <summary>
        /// Reference to the state machine.
        /// </summary>
        public StateMachine StateMachine { get; }

        /// <summary>
        /// Update the state.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// Draw the state.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Enter the state. Perform any spin-up in here.
        /// </summary>
        public abstract void Enter();

        /// <summary>
        /// Exit the state. Perform any cleanup in here.
        /// </summary>
        public abstract void Exit();


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stateMachine">State machine</param>
        public State(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
    }
}
