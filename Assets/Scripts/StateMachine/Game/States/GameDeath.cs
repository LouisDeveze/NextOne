using UnityEngine;
using UnityEngine.SceneManagement;

namespace NextOne
{
    class GameDeath : State<GameContext>
    {
        // Constructor taking the state machine + the state id
        public GameDeath(StateMachine<GameContext> stateMachine) : base(stateMachine, (int)GameStates.GameDeath) { }

        public override void OnEnter()
        {
            BindButtons();
            this.sm.ctx.playerController.enabled = false;
            this.sm.ctx.playerController.ResetTriggersAnimator();
            this.sm.ctx.playerController.SetTriggerAnimator(EAnimation.Stunned);
        }

        public void BindButtons()
        {
            this.sm.ctx.NextSeason.onClick.AddListener(OnNextSeason);
            this.sm.ctx.QuitOnDeath.onClick.AddListener(OnQuitOnDeath);
        }

        public void UnbindButtons()
        {
            this.sm.ctx.NextSeason.onClick.RemoveListener(OnNextSeason);
            this.sm.ctx.QuitOnDeath.onClick.RemoveListener(OnQuitOnDeath);
        }

        public void OnNextSeason()
        {
            UnbindButtons();
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        public void OnQuitOnDeath()
        {
            UnbindButtons();
            Application.Quit();
        }

    }
}
