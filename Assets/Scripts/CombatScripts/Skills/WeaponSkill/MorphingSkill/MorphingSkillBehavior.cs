using UnityEngine;
using UnityEngine.PlayerLoop;

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

        void Update()
        {
            if (Player.IsAnimationEnded(MorphingSkillData.AnimationTime))
            {
                Player.CanMove(true);
                Player.ResetTriggersAnimator();
            }
        }

        public void SetData(MorphingSkillData _morphingSkillDataToSet)
        {
            this.MorphingSkillData = _morphingSkillDataToSet;
        }

        public void Use(SkillUseParams _useParams)
        {
            Player.CanMove(false);
            Player.ResetTriggersAnimator();
            Player.SetTriggerAnimator(MorphingSkillData.AnimationName);
            PlayEffect();
            Player.ChangeWeapon();
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