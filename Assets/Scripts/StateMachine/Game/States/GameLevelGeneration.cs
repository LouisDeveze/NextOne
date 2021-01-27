namespace NextOne
{
    class GameLevelGeneration : State<GameContext>
    {
        // Constructor taking the state machine + the state id
        public GameLevelGeneration(StateMachine<GameContext> stateMachine) : base(stateMachine, (int)GameStates.GameLevelGeneration) { }

        public override void OnEnter()
        {
            this.sm.ctx.playerController.gameObject.SetActive(true);
            this.sm.ctx.maze.SetActive(true);
            this.sm.ctx.playerController.gameObject.SetActive(true);
            this.sm.ctx.sun.gameObject.SetActive(true);
            this.sm.ctx.globalVolume.sharedProfile = this.sm.ctx.postProcessProfile;
            
        }

        public override void OnUpdate()
        {
            this.sm.ctx.animator.SetTrigger("GameFadeIn");
            this.sm.SwitchState((int)GameStates.GameFadeIn);
        }
    }
}
