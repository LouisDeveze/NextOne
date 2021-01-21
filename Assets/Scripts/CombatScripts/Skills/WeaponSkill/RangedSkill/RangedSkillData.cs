﻿using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.RangedSkill
{
    [CreateAssetMenu(fileName = "RangedSkill", menuName = "Next One/Skills/Ranged Skill")]
    public class RangedSkillData : SkillData
    {
        [SerializeField] private GameObject SkillProjectilePrefab;
        [SerializeField] private GameObject SkillMuzzlePrefab;
        [SerializeField] private GameObject SkillHitPrefab;

        [SerializeField] private float SkillVelocity = 10f;
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

        public float Velocity => SkillVelocity;

        public int Projectiles => NumberOfProjectile;

        public int Damage => ProjectileDamage;

        public float Delay => ProjectileDelay;
    }
}