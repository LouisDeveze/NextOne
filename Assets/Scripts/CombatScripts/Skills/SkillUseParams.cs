using UnityEngine;

namespace NextOne
{
    public class SkillUseParams
    {
        private GameObject SUPOrigin;
        private Target SUPTarget;
        private float SUPRadius;


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

        public float Radius
        {
            get => SUPRadius;
            set => SUPRadius = value;
        }
    }
}