using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NextOne
{
    // Example state using Data contained by Menu COntext
    class OptionSelection : State<MenuContext>
    {
        // Constructor taking the state machine + the state id
        public OptionSelection(StateMachine<MenuContext> stateMachine) : base(stateMachine, (int)MenuStates.OptionSelection) { }

        public override void OnEnter()
        {
            Debug.Log("Option Entered");
        }
    }
}
