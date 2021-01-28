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
            this.sm.ctx.Play.onClick.AddListener(OnPlay);
            this.sm.ctx.Workbench.onClick.AddListener(OnWorkbench);
            this.sm.ctx.Character.onClick.AddListener(OnCharacter);
            this.sm.ctx.BlackMarket.onClick.AddListener(OnBlackMarket);
            this.sm.ctx.optionButton.onClick.AddListener(OnOption);
            this.sm.ctx.saveButton.onClick.AddListener(OnSave);

        }
        public override void OnExit()
        {
            this.sm.ctx.Play.onClick.RemoveListener(OnPlay);
            this.sm.ctx.Workbench.onClick.RemoveListener(OnWorkbench);
            this.sm.ctx.Character.onClick.RemoveListener(OnCharacter);
            this.sm.ctx.BlackMarket.onClick.RemoveListener(OnBlackMarket);
            this.sm.ctx.optionButton.onClick.RemoveListener(OnOption);
            this.sm.ctx.saveButton.onClick.RemoveListener(OnSave);
        }

        public void OnPlay()
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OnOption()
        {

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

    }
}