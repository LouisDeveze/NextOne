using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "ExplosiveSkill", menuName = "Next One/Skills/Explosive Skill")]
    public class ExplosiveSkillData : SkillData
    {
        [SerializeField] private SkillAim ExplosionTargetAim;
        [SerializeField] private GameObject SkillProjectilePrefab;
        [SerializeField] private GameObject SkillExplosionEffect;
        [SerializeField] private AudioClip SkillExplosionSfx;
        [SerializeField] private int SkillExplosionDamage = 1;
        [SerializeField] private float SkillExplosionDelay = 1f;
        [SerializeField] private float SkillThrowForce = 10f;


        public override ISkill AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            var behaviorComponent = _gameObjectToAttachTo.AddComponent<ExplosiveSkillBehavior>();
            behaviorComponent.SetData(this);
            Behavior = behaviorComponent;

            return Behavior;
        }

        public SkillAim ExplosionTarget => ExplosionTargetAim;

        public GameObject ProjectilePrefab => SkillProjectilePrefab;

        public int Damage => SkillExplosionDamage;

        public GameObject ExplosionEffect => SkillExplosionEffect;
        public float ExplosiveDelay => SkillExplosionDelay;
        public float ThrowForce => SkillThrowForce;

        public AudioClip ExplosionSfx => SkillExplosionSfx;
    }
}