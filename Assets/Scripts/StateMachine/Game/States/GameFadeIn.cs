using UnityEngine;

namespace NextOne
{
    class GameFadeIn : State<GameContext>
    {
        // Constructor taking the state machine + the state id
        public GameFadeIn(StateMachine<GameContext> stateMachine) : base(stateMachine, (int)GameStates.GameFadeIn) { }


        public override void OnEnter()
        {
            
            // Changing Camera to Trace the player
            CameraTracer tracer = this.sm.ctx.cameraManager.GetComponentInChildren<CameraTracer>();
            tracer.enabled = true;
            //tracer.target = this.sm.ctx
            tracer.transform.localRotation = Quaternion.identity;
            //
            tracer.target = this.sm.ctx.playerController.Model.transform;

        }

        public override void OnUpdate()
        {
            if (this.sm.ctx.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                this.sm.ctx.animator.SetTrigger("GamePlaying");
                this.sm.SwitchState((int)GameStates.GamePlaying);
            }
        }
    }
}