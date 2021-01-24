using UnityEngine;

namespace NextOne
{
    public abstract class BaseSkillBehavior : MonoBehaviour, ISkill
    {
        protected SkillData SkillData = null;
        protected PlayerController Player = null;
        protected bool SkillInUse = false;

        public void SetData(SkillData _skillData)
        {
            this.SkillData = _skillData;
        }

        void Start()
        {
            Player = GetComponent<PlayerController>();
        }

        public abstract void Use(SkillUseParams _useParams);

        protected abstract void OnEffectStart();

        protected abstract void OnEffectEnd();

        public void Detach()
        {
            Destroy(this);
        }
    }
}