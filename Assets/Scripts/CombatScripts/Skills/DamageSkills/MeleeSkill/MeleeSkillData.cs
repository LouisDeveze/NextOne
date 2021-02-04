using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "MeleeSkill", menuName = "Next One/Skills/Melee Skill")]
    public class MeleeSkillData : SkillData
    {
        [SerializeField] private int SkillDamage;
        [SerializeField] private GameObject SkillCastVfx;
        [SerializeField] private AudioClip SkillCastSfx;
        [SerializeField] private GameObject SkillHitVfx;
        [SerializeField] private AudioClip SkillHitSfx;
        [SerializeField] private float SkillRadius;

        public override ISkill AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            var behaviorComponent = _gameObjectToAttachTo.AddComponent<MeleeBasicAttackBehavior>();
            behaviorComponent.SetData(this);
            Behavior = behaviorComponent;

            return Behavior;
        }

        public int Damage => SkillDamage;

        public GameObject CastVfx => SkillCastVfx;
        public GameObject HitVfx => SkillHitVfx;
        public AudioClip CastSfx => SkillCastSfx;
        public AudioClip HitSfx => SkillHitSfx;

        public float Radius => SkillRadius;
    }
}