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

            if (this.sm.ctx.playerController.Health <= 0)
            {
                this.sm.ctx.animator.SetTrigger("DeathScreen");
                this.sm.SwitchState((int)GameStates.GameDeath);
            }
        }
    }
}