using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace NextOne
{
    class CharacterSelection : State<MenuContext>
    {
        PlayerData char1;
        PlayerData char2;
        PlayerData char3;
        // Constructor taking the state machine + the state id
        public CharacterSelection(StateMachine<MenuContext> stateMachine) : base(stateMachine, (int)MenuStates.CharacterSelection) { }

        public override void OnEnter()
        {

            this.sm.ctx.Character1.onClick.AddListener(OnCharacter1Selected);
            this.sm.ctx.Character2.onClick.AddListener(OnCharacter2Selected);
            this.sm.ctx.Character3.onClick.AddListener(OnCharacter3Selected);
            
            Debug.Log("CharacterSelect");
            List<PlayerData> availables = SharedData.Instance.availablePlayers;
            System.Random rand = new System.Random();

            char1 = availables[rand.Next()%availables.Count];
            char2 = availables[rand.Next()%availables.Count];
            char3 = availables[rand.Next()%availables.Count];

            //assigning character to UI
            this.sm.ctx.Character_1_name.text = char1.Name;
            this.sm.ctx.Character_2_name.text = char2.Name;
            this.sm.ctx.Character_3_name.text = char3.Name;

            this.sm.ctx.Character_1_sprite.sprite = char1.Pose;
            this.sm.ctx.Character_2_sprite.sprite = char2.Pose;
            this.sm.ctx.Character_3_sprite.sprite = char3.Pose;

            this.sm.ctx.Character_1_description.text = char1.Description;
            this.sm.ctx.Character_2_description.text = char2.Description;
            this.sm.ctx.Character_3_description.text = char3.Description;
        }
        public override void OnExit()
        {
            this.sm.ctx.Character1.onClick.RemoveListener(OnCharacter1Selected);
            this.sm.ctx.Character2.onClick.RemoveListener(OnCharacter2Selected);
            this.sm.ctx.Character3.onClick.RemoveListener(OnCharacter3Selected);
        }
        public void OnCharacter1Selected()
        {
            SharedData.Instance.playerSelected = char1;
            this.sm.ctx.CharacterSelection.SetActive(false);
            this.sm.ctx.WeaponSelection.SetActive(true);
            this.sm.SwitchState((int)MenuStates.WeaponSelection);
        }
        public void OnCharacter2Selected()
        {
            SharedData.Instance.playerSelected = char2;
            this.sm.ctx.CharacterSelection.SetActive(false);
            this.sm.ctx.WeaponSelection.SetActive(true);
            this.sm.SwitchState((int)MenuStates.WeaponSelection);
        }
        public void OnCharacter3Selected()
        {
            SharedData.Instance.playerSelected = char3;
            this.sm.ctx.CharacterSelection.SetActive(false);
            this.sm.ctx.WeaponSelection.SetActive(true);
            this.sm.SwitchState((int)MenuStates.WeaponSelection);
        }
    }
        
}