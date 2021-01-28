using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public class SharedData : MonoBehaviour
    {

        // Shared Data Instance Singleton
        private static SharedData instance = null;


        #region Database
        
        public List<PlayerData> availablePlayers;
        public List<BaseWeaponData> availableWeapons;
        #endregion

        #region Selections

        public PlayerData playerSelected;
        public BaseWeaponData weaponSelected;
        #endregion

        #region Awake instance
        void Awake()
        {

            // Create a Don't destroy instance of the Shared data to share it among scenes
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        #endregion

        /// <summary>
        /// Returns the instance of the shared data structure
        /// </summary>
        public static SharedData Instance { get => instance; }
    }

}
