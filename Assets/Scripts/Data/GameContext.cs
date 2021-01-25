using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace NextOne
{
    public class GameContext : MonoBehaviour
    {
        #region Managers
        public GameObject uiManager;
        public GameObject cameraManager;
        public GameObject environmentManager;
        public GameObject levelManager;
        #endregion

        #region UI
        public Animator animator;
        public RawImage Electric;
        public GameObject LoadingScreenRoom;
        #endregion

        #region Environment
        public Light sun;
        public VolumeProfile postProcessProfile;
        public Volume globalVolume;
        #endregion

        #region Objects
        public PlayerController playerController;
        public GameObject maze;
        #endregion

        #region Materials
        public Material semiTransparent;
        #endregion

        #region Logic
        public bool playerOccluded;
        #endregion
    }
}
