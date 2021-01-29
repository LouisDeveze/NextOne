using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace NextOne
{
    class MenuEpisode : State<MenuContext>
    {
        // Constructor taking the state machine + the state id
        public MenuEpisode(StateMachine<MenuContext> stateMachine) : base(stateMachine, (int)MenuStates.MenuEpisode) { }

        public override void OnEnter()
        {
            this.sm.ctx.Menu_Play.onClick.AddListener(OnPlay);
            this.sm.ctx.Workbench.onClick.AddListener(OnWorkbench);
            this.sm.ctx.Character.onClick.AddListener(OnCharacter);
            this.sm.ctx.BlackMarket.onClick.AddListener(OnBlackMarket);
            this.sm.ctx.Menu_optionButton.onClick.AddListener(OnOption);
            this.sm.ctx.Menu_saveButton.onClick.AddListener(OnSave);
            this.sm.ctx.Menu_quitButton.onClick.AddListener(OnQuit);

        }
        public override void OnExit()
        {
            this.sm.ctx.Menu_Play.onClick.RemoveListener(OnPlay);
            this.sm.ctx.Workbench.onClick.RemoveListener(OnWorkbench);
            this.sm.ctx.Character.onClick.RemoveListener(OnCharacter);
            this.sm.ctx.BlackMarket.onClick.RemoveListener(OnBlackMarket);
            this.sm.ctx.Menu_optionButton.onClick.RemoveListener(OnOption);
            this.sm.ctx.Menu_saveButton.onClick.RemoveListener(OnSave);
            this.sm.ctx.Menu_quitButton.onClick.RemoveListener(OnQuit);
        }

        public void OnPlay()
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OnOption()
        {
            this.sm.ctx.OptionSelection.SetActive(true);
        }

        public void OnWorkbench()
        {

        }
        public void OnCharacter()
        {

        }
        public void OnBlackMarket()
        {

        }
        public void OnSave()
        {

        }
        public void OnQuit()
        {
            Application.Quit();
        }

    }
}