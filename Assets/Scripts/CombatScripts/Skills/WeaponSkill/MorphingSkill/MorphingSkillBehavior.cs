using UnityEngine;

namespace NextOne
{
    public class MorphingSkillBehavior : MonoBehaviour, ISkill
    {
        private MorphingSkillData MorphingSkillData = null;
        PlayerController Player = null;

        void Start()
        {
            Player = GetComponent<PlayerController>();
        }

        public void SetData(MorphingSkillData _morphingSkillDataToSet)
        {
            this.MorphingSkillData = _morphingSkillDataToSet;
        }

        public void Use(SkillUseParams _useParams)
        {
            Player.ChangeWeapon();
            PlayEffect();
        }

        public void Detach()
        {
            Destroy(this);
        }

        private void PlayEffect()
        {
            //TODO: Implement VFX
        }
    }
}