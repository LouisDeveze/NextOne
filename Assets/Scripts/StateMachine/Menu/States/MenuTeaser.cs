using UnityEngine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NextOne
{
    class MenuTeaser : State<MenuContext>
    {
        // Constructor taking the state machine + the state id
        public MenuTeaser(StateMachine<MenuContext> stateMachine) : base(stateMachine, (int)MenuStates.MenuTeaser) { }

        public override void OnEnter()
        {
            this.sm.ctx.Play.onClick.AddListener(OnPlay);
            this.sm.ctx.optionButton.onClick.AddListener(OnOption);
            this.sm.ctx.saveButton.onClick.AddListener(OnSave);

        }
        public override void OnExit()
        {
            this.sm.ctx.Play.onClick.RemoveListener(OnPlay);
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

        public void OnSave()
        {

        }


    }
}