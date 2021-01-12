using UnityEngine;

namespace NextOne
{
    // Example state using Data contained by Menu COntext
    class FadeInTitle : State<MenuContext>
    {
        // Constructor taking the state machine + the state id
        public FadeInTitle(StateMachine<MenuContext> stateMachine) : base(stateMachine, (int)MenuStates.FadeInTitle) { }

        public override void OnUpdate()
        {
            // If the starting animation hasn't ended keep going
            if (this.sm.ctx.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                this.sm.ctx.animator.SetTrigger("FadeInToMainTitle");
                this.sm.SwitchState((int)MenuStates.MainTitle);
            }   
        }
    }
}
