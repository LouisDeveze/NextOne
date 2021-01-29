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
        public Animator animator;
        public RawImage Electric;
        public GameObject LoadingScreenRoom;
        public GameObject PlayingScreen;
        public SkillUI SkillAutoAttackUI;
        public SkillUI SkillPrimaryUI;
        public SkillUI SkillSecondaryUI;
        public SkillUI SkillTertiaryUI;
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
