using AntiPacman.Engine.States;
using System.Collections.Generic;

namespace AntiPacman.Engine
{
    public class StateManager
    {
        private List<State> stack;

        public StateManager(Game game)
        {
            stack = new List<State>
            {
                new MenuState(game, this)
            };
        }

        public void Push(State newState)
        {
            stack.Add(newState);
            newState.Initialize();
            newState.Load();
            newState.PostLoad();
        }
        public void Pop()
        {
            stack.RemoveAt(stack.Count - 1);
        }
        public void Set(State newState)
        {
            Pop();
            Push(newState);
        }
        public State CurrentState()
        {
            return stack[stack.Count - 1];
        }

        public void Dispose()
        {
            foreach (State state in stack)
            {
                state.Dispose();
            }
        }
    }
}