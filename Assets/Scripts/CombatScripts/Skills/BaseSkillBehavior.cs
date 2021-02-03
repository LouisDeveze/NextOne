using System;
using NextOne.Controllers;
using UnityEngine;

namespace NextOne
{
    public abstract class BaseSkillBehavior : MonoBehaviour, ISkill
    {
        protected SkillData SkillData = null;
        protected EntityController SourceController = null;
        protected bool SkillInUse = false;
        private int RandomAnimation;
        private Cooldown SkillCooldown;

        public void SetData(SkillData _skillData)
        {
            this.SkillData = _skillData;
            this.SkillCooldown = new Cooldown(this.SkillData.Cooldown);
        }

        protected virtual void Update()
        {
            SkillCooldown.UpdateCooldown();
            if(SourceController is PlayerController playerController)
                playerController.SetProgress(SkillData.AnimationName[0],SkillCooldown.CurrentCooldown,SkillData.Cooldown);
        }

        public virtual void Use(SkillUseParams _useParams)
        {
            Debug.Log("Base Use");
            SkillCooldown.UseCooldown();
        }

        void Start()
        {
            OnInitialization();
        }

        public bool CanCast()
        {
            Debug.Log("In Can Cast");
            return SkillCooldown.ReadyToCast;
        }

        protected void Randomize()
        {
            System.Random rdn = new System.Random();
            Debug.Log("In Randomize - rdn: " +
                      rdn + "List count: " + SkillData.AnimationName.Count);
            RandomAnimation = rdn.Next(0, SkillData.AnimationName.Count);
            Debug.Log("In Randomize - rdn animation: " +
                      RandomAnimation);
        }

        protected EAnimation GetRandomAnimationName()
        {
            return SkillData.AnimationName[RandomAnimation];
        }

        protected float GetRandomAnimationTime()
        {
            return SkillData.AnimationTime[RandomAnimation];
        }

        protected float GetRandomEffectiveTime()
        {
            return SkillData.EffectiveUseTime[RandomAnimation];
        }

        protected abstract void OnEffectStart();

        protected abstract void OnEffectEnd();

        protected abstract void OnInitialization();

        protected abstract void OnEffectiveUse();

        public void Detach()
        {
            Destroy(this);
        }
    }
}