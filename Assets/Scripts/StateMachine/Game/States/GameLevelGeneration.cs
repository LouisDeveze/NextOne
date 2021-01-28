using UnityEngine;

namespace NextOne
{
    class GameLevelGeneration : State<GameContext>
    {
        // Constructor taking the state machine + the state id
        public GameLevelGeneration(StateMachine<GameContext> stateMachine) : base(stateMachine, (int)GameStates.GameLevelGeneration) { }

        public override void OnEnter()
        {
            // Generate the Map
            LevelGenerator level = this.sm.ctx.levelManager.GetComponent<LevelGenerator>();
            level.Generate();
            OnPostGeneration();          
            
        }

        public override void OnUpdate()
        {
            this.sm.ctx.animator.SetTrigger("GameFadeIn");
            this.sm.SwitchState((int)GameStates.GameFadeIn);
        }

        private void OnPostGeneration()
        {
            // Post Generation
            GameObject player = GameObject.Find("Player");
            if (player == null) Debug.LogError("No PLayer Gameobject found :(");
            this.sm.ctx.playerController = player.GetComponent<PlayerController>();
            player.transform.parent = null;

            this.sm.ctx.playerController.gameObject.SetActive(true);
            this.sm.ctx.sun.gameObject.SetActive(true);
            this.sm.ctx.globalVolume.sharedProfile = this.sm.ctx.postProcessProfile;
        }
    }
}
