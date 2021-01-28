﻿using UnityEngine;

namespace NextOne
{
    public class MeleeAttackBehavior : BaseSkillBehavior
    {
        private SkillUseParams UseParams;

        public override void Use(SkillUseParams _useParams)
        {
            
            UseParams = _useParams;
            Randomize();
            OnEffectStart();
        }

        void Update()
        {
            if (!SkillInUse) return;

            if (!Player.hasAnimatorPlaying(GetRandomAnimationName(), 0)) return;

            if (!Player.IsAnimationLastAtLeast(GetRandomAnimationTime(), 0)) return;
            Debug.Log("Melee Skill Ended in: " + SkillData.Name + " - " + this.GetInstanceID());
            OnEffectEnd();
        }

        protected override void OnEffectStart()
        {
            Player.ResetTriggersAnimator();
            Player.ActiveWeaponTrigger(true);
            Player.SetTriggerAnimator(GetRandomAnimationName());
            Debug.Log("Melee Basic Attack in: " + SkillData.Name + " - " + this.GetInstanceID());
            Player.SkillInUse = true;
            SkillInUse = true;
            Player.CanMove(false);
        }

        protected override void OnEffectEnd()
        {
            Player.ActiveWeaponTrigger(false);
            Player.ResetTriggersAnimator();
            Player.CanMove(true);
            Player.SkillInUse = false;
            SkillInUse = false;
        }

        protected override void OnInitialization()
        {
            Player = GetComponent<PlayerController>();
        }

        protected override void OnEffectiveUse()
        {
            //NOTHING
        }
    }
}