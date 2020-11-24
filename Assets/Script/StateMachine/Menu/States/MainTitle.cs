using UnityEngine;

namespace NextOne
{
    // Example state using Data contained by Menu COntext
    class MainTitle : State<MenuContext>
    {
        // Constructor taking the state machine + the state id
        public MainTitle(StateMachine<MenuContext> stateMachine) : base(stateMachine, (int)MenuStates.MainTitle) { }

        public override void OnEnter()
        {
            Debug.Log("Title Entered");
            this.sm.ctx.logo.color = Color.blue;
        }
    }
}
