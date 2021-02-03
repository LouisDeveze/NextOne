using UnityEngine;
using UnityEngine.AI;
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
        public Animator animator = null;
        public RawImage Electric = null;
        public GameObject LoadingScreenRoom = null;
        public GameObject PlayingScreen = null;
        public SkillUI SkillAutoAttackUI = null;
        public SkillUI SkillPrimaryUI = null;
        public SkillUI SkillSecondaryUI = null;
        public SkillUI SkillTertiaryUI = null;
        public LifeBarUI LifeBarUI = null;

        public GameObject PauseScreen = null;
        public Button Resume = null;
        public Button GiveUp = null;
        public Button QuitGame = null;
        public Button NextSeason = null;
        public Button QuitOnDeath = null;
        #endregion

        #region Environment
        public Light sun;
        public VolumeProfile postProcessProfile;
        public Volume globalVolume;
        #endregion

        #region Objects
        public PlayerController playerController = null;
        #endregion

        #region Materials
        public Material semiTransparent;
        #endregion

        #region Logic
        public bool playerOccluded;
        #endregion

        #region Navigation
        internal NavMeshDataInstance navMeshDataInstance;
        internal NavMeshData navMeshData;
        public NavMeshSurface navMeshSurface;
        #endregion
    }
}
