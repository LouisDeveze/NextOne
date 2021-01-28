using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace NextOne
{
    class WeaponSelection : State<MenuContext>
    {
        // Constructor taking the state machine + the state id
        public WeaponSelection(StateMachine<MenuContext> stateMachine) : base(stateMachine, (int)MenuStates.WeaponSelection) { }
        BaseWeaponData Weapon1;
        BaseWeaponData Weapon2;
        BaseWeaponData Weapon3;
        public override void OnEnter()
        {

            this.sm.ctx.Weapon1.onClick.AddListener(OnWeapon1Selected);
            this.sm.ctx.Weapon2.onClick.AddListener(OnWeapon2Selected);
            this.sm.ctx.Weapon3.onClick.AddListener(OnWeapon3Selected);

            Debug.Log("CharacterSelect");
            List<BaseWeaponData> availables = SharedData.Instance.availableWeapons;
            System.Random rand = new System.Random();

            Weapon1 = availables[rand.Next() % availables.Count];
            Weapon2 = availables[rand.Next() % availables.Count];
            Weapon3 = availables[rand.Next() % availables.Count];

            //assigning character to UI
            this.sm.ctx.Weapon_1_name.text = Weapon1.Name;
            this.sm.ctx.Weapon_2_name.text = Weapon2.Name;
            this.sm.ctx.Weapon_3_name.text = Weapon3.Name;

            this.sm.ctx.Weapon_1_sprite.sprite = Weapon1.Icon;
            this.sm.ctx.Weapon_2_sprite.sprite = Weapon2.Icon;
            this.sm.ctx.Weapon_3_sprite.sprite = Weapon3.Icon;

            this.sm.ctx.Weapon_1_description.text = Weapon1.Description;
            this.sm.ctx.Weapon_2_description.text = Weapon2.Description;
            this.sm.ctx.Weapon_3_description.text = Weapon3.Description;
        }
        public override void OnExit()
        {
            this.sm.ctx.Weapon1.onClick.RemoveListener(OnWeapon1Selected);
            this.sm.ctx.Weapon2.onClick.RemoveListener(OnWeapon2Selected);
            this.sm.ctx.Weapon3.onClick.RemoveListener(OnWeapon3Selected);
        }
        public void OnWeapon1Selected()
        {
            SharedData.Instance.weaponSelected = Weapon1;
            this.sm.ctx.MenuTeaser.SetActive(true);
            this.sm.ctx.WeaponSelection.SetActive(false);
            this.sm.SwitchState((int)MenuStates.MenuTeaser);
        }
        public void OnWeapon2Selected()
        {
            SharedData.Instance.weaponSelected = Weapon2;
            this.sm.ctx.MenuTeaser.SetActive(true);
            this.sm.ctx.WeaponSelection.SetActive(false);
            this.sm.SwitchState((int)MenuStates.MenuTeaser);
        }
        public void OnWeapon3Selected()
        {
            SharedData.Instance.weaponSelected = Weapon3;
            this.sm.ctx.MenuTeaser.SetActive(true);
            this.sm.ctx.WeaponSelection.SetActive(false);
            this.sm.SwitchState((int)MenuStates.MenuTeaser);
        }
    }

}
