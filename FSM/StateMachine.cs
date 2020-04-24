using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.FSM
{
    class StateMachine
    {
        private Dictionary<string, State> States = new Dictionary<string, State>();
        private State CurrentState;

        public Game Game { get; }

        public StateMachine(Game game)
        {
            Game = game;
        }

        public void Add(State state, string key)
        {
            States[key] = state;
        }

        public void Change(String key)
        {
            if (!States.ContainsKey(key))
                throw new KeyNotFoundException($"{key} is not a valid state!");

            if (CurrentState != null)
                CurrentState.Exit();

            CurrentState = States[key];
            CurrentState.Enter();
        }

        /// <summary>
        /// Draw the current state.
        /// </summary>
        public void Draw()
        {
            if (CurrentState != null)
                CurrentState.Draw();
        }

        /// <summary>
        /// Update the current state.
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public void Update(float deltaTime)
        {
            if (CurrentState != null)
                CurrentState.Update(deltaTime);
        }
    }
}
