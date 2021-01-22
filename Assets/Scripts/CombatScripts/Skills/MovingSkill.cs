using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "MovingSkill", menuName = "Next One/Skills/MovingSkill")]
    public class MovingSkill : SkillData
    {
        [SerializeField] private float SkillVelocity = 10f;
        [SerializeField] private float SkillRange = 10f;

        [SerializeField] private GameObject MovingCastEffect;
        [SerializeField] private GameObject MovingHitEffect;


        /*public override void Use(GameObject _origin, Target _target)
        {
            var instance =
                Instantiate(Effect, _origin.transform.position + _origin.transform.forward,
                    _origin.transform.rotation) as GameObject;
            var rb = instance.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(_origin.transform.forward * Velocity);
        }*/

        public float Velocity => SkillVelocity;

        public float Range => SkillRange;

        public GameObject CastEffect => MovingCastEffect;

        public GameObject HitEffect => MovingHitEffect;
        public override ISkill AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            throw new System.NotImplementedException();
        }
    }
}