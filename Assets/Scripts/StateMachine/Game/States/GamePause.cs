using UnityEngine;
using UnityEngine.SceneManagement;

namespace NextOne
{
    class GamePause : State<GameContext>
    {
        // Constructor taking the state machine + the state id
        public GamePause(StateMachine<GameContext> stateMachine) : base(stateMachine, (int)GameStates.GamePause) { }

        
        public override void OnEnter()
        {
            Time.timeScale = 0;
            this.sm.ctx.PauseScreen.SetActive(true);

            // Bind Buttons
            BindButtons();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnResume();
            }
        }

        public void OnResume()
        {
            Time.timeScale = 1;
            UnbindButtons();
            this.sm.ctx.PauseScreen.SetActive(false);
            this.sm.SwitchState((int)GameStates.GamePlaying);
        }

        public void OnGiveUp()
        {
            Time.timeScale = 1;
            UnbindButtons();
            // SAVE TO IMPLEMENT
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        public void OnQuitGame()
        {
            Time.timeScale = 1;
            UnbindButtons();
            // SAVE TO IMPLEMENT
            Application.Quit();

        }

        public void BindButtons()
        {
            this.sm.ctx.Resume.onClick.AddListener(OnResume);
            this.sm.ctx.GiveUp.onClick.AddListener(OnGiveUp);
            this.sm.ctx.QuitGame.onClick.AddListener(OnQuitGame);
        }

        public void UnbindButtons()
        {
            this.sm.ctx.Resume.onClick.RemoveListener(OnResume);
            this.sm.ctx.GiveUp.onClick.RemoveListener(OnGiveUp);
            this.sm.ctx.QuitGame.onClick.RemoveListener(OnQuitGame);
        }
    }
}
