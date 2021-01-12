using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NextOne
{
    // Example state using Data contained by Menu COntext
    class OptionState : State<MenuContext>
    {
        // Constructor taking the state machine + the state id
        public OptionState(StateMachine<MenuContext> stateMachine) : base(stateMachine, (int)MenuStates.OptionState) { }

        public override void OnEnter()
        {
            Debug.Log("Option Entered");
        }
    }
}
