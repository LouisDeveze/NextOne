using UnityEngine;

namespace NextOne
{
    class GamePlaying : State<GameContext>
    {
        // Constructor taking the state machine + the state id
        public GamePlaying(StateMachine<GameContext> stateMachine) : base(stateMachine, (int)GameStates.GamePlaying) { }


        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this.sm.SwitchState((int)GameStates.GamePause);
            }

            if (/*Died*/Input.GetKeyDown(KeyCode.N))
            {
                this.sm.ctx.animator.SetTrigger("DeathScreen");
                this.sm.SwitchState((int)GameStates.GameDeath);
                
            }
        }
    }
}