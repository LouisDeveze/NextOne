using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills
{
    public class SkillUseParams
    {
        private GameObject SUPOrigin;
        private Target SUPTarget;
        

        public GameObject Origin
        {
            get => SUPOrigin;
            set => SUPOrigin = value;
        }

        public Target Target
        {
            get => SUPTarget;
            set => SUPTarget = value;
        }
    }
}