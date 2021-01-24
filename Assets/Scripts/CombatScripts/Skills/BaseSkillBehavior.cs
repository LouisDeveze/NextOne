﻿using UnityEngine;

namespace NextOne
{
    public abstract class BaseSkillBehavior : MonoBehaviour, ISkill
    {
        protected SkillData SkillData = null;
        protected PlayerController Player = null;
        protected bool SkillInUse = false;
        protected int Framecount = 0;

        public void SetData(SkillData _skillData)
        {
            this.SkillData = _skillData;
        }

        void Start()
        {
            Debug.Log("ON START: " + this.GetInstanceID());
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