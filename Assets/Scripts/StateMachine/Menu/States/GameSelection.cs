﻿using UnityEngine;
using UnityEditor;

namespace NextOne
{
     class GameSelection : State<MenuContext>     
    {
        // Constructor taking the state machine + the state id
        public GameSelection(StateMachine<MenuContext> stateMachine) : base(stateMachine, (int)MenuStates.GameSelection) { }

        public override void OnEnter()
        {
            this.sm.ctx.newgameButton.onClick.AddListener(OnNewGame);
            this.sm.ctx.loadgameButton.onClick.AddListener(OnLoadGame);
        }
        public override void OnExit()
        {
            this.sm.ctx.newgameButton.onClick.RemoveListener(OnNewGame);
            this.sm.ctx.loadgameButton.onClick.RemoveListener(OnLoadGame);
        }

        public void OnNewGame()
        {
            this.sm.ctx.GameSelection.SetActive(false);
            this.sm.ctx.CharacterSelection.SetActive(true);
            this.sm.SwitchState((int)MenuStates.CharacterSelection);
        }
        public void OnLoadGame()
        {
            this.sm.ctx.GameSelection.SetActive(false);
            this.sm.ctx.SaveSelection.SetActive(true);
            this.sm.SwitchState((int)MenuStates.SaveSelection);
        }

    }
}