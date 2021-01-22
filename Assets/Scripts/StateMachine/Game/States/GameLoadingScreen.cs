using UnityEngine;

namespace NextOne
{
    class GameLoadingScreen : State<GameContext>
    {
        public float interpolation;
        public float timer;
        public Material material;


        public GameObject vignette;
        public GameObject loadingScreenRoom;
        public GameObject LoadingScreenUI;

        // Constructor taking the state machine + the state id
        public GameLoadingScreen(StateMachine<GameContext> stateMachine) : base(stateMachine, (int)GameStates.GameLoadingScreen) { }


        public override void OnEnter()
        {
            interpolation = 0;
            material = this.sm.ctx.Electric.material;

            vignette = GameObject.Find("Vignette");
            loadingScreenRoom = GameObject.Instantiate(sm.ctx.LoadingScreenRoom,sm.ctx.levelManager.transform);
            LoadingScreenUI = GameObject.Find("LoadingScreen");
        }

        public override void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer < 1) { interpolation -= Mathf.Max(-1,Time.deltaTime); }
            else if (timer > 3 && interpolation < 0) { interpolation = 1; }
            else if (timer > 3 ) { interpolation -= Mathf.Max(0, Time.deltaTime); }
            material.SetFloat("_MaskOffset", interpolation);
            

            // If the starting animation hasn't ended keep going
            if (this.sm.ctx.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {

                this.sm.SwitchState((int)GameStates.GameLevelGeneration);
            }
        }

        public override void OnExit()
        {

            // Cleaning Up
            GameObject.Destroy(vignette);
            GameObject.Destroy(loadingScreenRoom);
            GameObject.DestroyImmediate(LoadingScreenUI);

            material.SetFloat("_MaskOffset", 0);
        }
    }
}
