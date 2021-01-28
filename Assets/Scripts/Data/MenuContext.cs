using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NextOne
{
    public class MenuContext : MonoBehaviour
    {
        #region Managers
        public GameObject uiManager;
        public GameObject cameraManager;
        #endregion

        #region Utils
        public Animator animator;
        public RawImage logo;
        #endregion

        #region Buttons
        
        public Button newgameButton;
        public Button loadgameButton;

        public Button saveButton;
        public Button optionButton;
        public Button quitButton;
        public Button Play;

       

        public Button BlackMarket;
        public Button Character;
        public Button Workbench;

       
        #endregion

        #region Elevator
        public float elevatorSpeed = 1;
        #endregion

        #region Screens
        public GameObject GameSelection;
        public GameObject CharacterSelection;
        public GameObject SaveSelection;
        public GameObject WeaponSelection;
        public GameObject MenuTeaser;
        public GameObject MenuEpisode;
        #endregion


        public List<PlayerData> AvailablePlayer;


        #region Characterselect
        public Text Character_1_name;
        public Image Character_1_sprite;
        public Text Character_1_description;
        public Text Character_2_name;
        public Image Character_2_sprite;
        public Text Character_2_description;
        public Text Character_3_name;
        public Image Character_3_sprite;
        public Text Character_3_description;
        public Button Character1;
        public Button Character2;
        public Button Character3;
        #endregion

        #region Weaponselect
        public Text Weapon_1_name;
        public Image Weapon_1_sprite;
        public Text Weapon_1_description;
        public Text Weapon_2_name;
        public Image Weapon_2_sprite;
        public Text Weapon_2_description;
        public Text Weapon_3_name;
        public Image Weapon_3_sprite;
        public Text Weapon_3_description;
        public Button Weapon1;
        public Button Weapon2;
        public Button Weapon3;
        #endregion
    }
}
