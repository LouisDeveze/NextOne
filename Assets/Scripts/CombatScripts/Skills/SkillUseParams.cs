using UnityEngine;

namespace NextOne
{
    public class SkillUseParams
    {
        private GameObject SUPOrigin;
        private Target SUPTarget;
        private float SUPRadius;
        private float SUPDistanceToPlayer;
        public float SUPDetectRange;


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

        public float DistanceToPlayer
        {
            get => SUPDistanceToPlayer;
            set => SUPDistanceToPlayer = value;
        }

        public float DetectRange
        {
            get => SUPDetectRange;
            set => SUPDetectRange = value;
        }
    }
}