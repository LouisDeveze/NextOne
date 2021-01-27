using UnityEngine;
using UnityEditor;

namespace NextOne
{
     class GameSelection : State<MenuContext>     
    {
        // Constructor taking the state machine + the state id
        public GameSelection(StateMachine<MenuContext> stateMachine) : base(stateMachine, (int)MenuStates.GameSelection) { }

    }
}