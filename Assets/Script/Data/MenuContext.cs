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
        public Button optionButton;
        #endregion

        #region Elevator
        public float elevatorSpeed = 1;
        #endregion
    }
}
