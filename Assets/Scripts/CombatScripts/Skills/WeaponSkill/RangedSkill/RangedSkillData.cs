using System.Collections.Generic;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.RangedSkill
{
    [CreateAssetMenu(fileName = "RangedSkill", menuName = "Next One/Skills/Ranged Skill")]
    public class RangedSkillData : SkillData
    {
        [SerializeField] private GameObject SkillProjectilePrefab;
        [SerializeField] private GameObject SkillMuzzlePrefab;
        [SerializeField] private GameObject SkillHitPrefab;
        [SerializeField] private List<GameObject> SkillTrailsPrefab;

        [SerializeField] private AudioClip SkillCastSFX;
        [SerializeField] private AudioClip SkillHitSFX;


        [SerializeField] private float SkillVelocity = 10f;
        [SerializeField] private float SkillAccuracy = 100f;
        [SerializeField] private int SkillMaxCollision = 1;
        [SerializeField] private int NumberOfProjectile = 1;
        [SerializeField] private int ProjectileDamage = 1;
        [SerializeField] private float ProjectileDelay = 0.01f;

        public override ISkill AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            var behaviorComponent = _gameObjectToAttachTo.AddComponent<RangedSkillBehavior>();
            behaviorComponent.SetData(this);
            Behavior = behaviorComponent;

            return Behavior;
        }

        public GameObject ProjectilePrefab => SkillProjectilePrefab;

        public GameObject MuzzlePrefab => SkillMuzzlePrefab;

        public GameObject HitPrefab => SkillHitPrefab;

        public List<GameObject> TrailsPrefab => SkillTrailsPrefab;
        public float Velocity => SkillVelocity;

        public int MaxCollision => SkillMaxCollision;

        public int Projectiles => NumberOfProjectile;

        public int Damage => ProjectileDamage;

        public float Delay => ProjectileDelay;

        public AudioClip CastSfx => SkillCastSFX;

        public AudioClip HitSfx => SkillHitSFX;

        public float Accuracy => SkillAccuracy;
    }
}