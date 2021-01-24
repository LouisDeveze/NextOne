﻿using UnityEngine;
using UnityEngine.PlayerLoop;

namespace NextOne
{
    public class MorphingSkillBehavior : BaseSkillBehavior
    {
        void Update()
        {
            if (SkillInUse)
            {
                Framecount++;
                Debug.Log(Framecount);
                Debug.Log(this.GetInstanceID());
            }

            if (!Player.IsAnimationEnded(SkillData.AnimationTime) || !SkillInUse || !(Framecount > 1)) return;
            OnEffectEnd();
            Debug.Log("Update 2");
        }

        public override void Use(SkillUseParams _useParams)
        {
            Debug.Log("Update 0");
            Debug.Log("Instance ID:" + this.GetInstanceID());
            Debug.Log("Framecount: " + Framecount);
            SkillInUse = Player.AttemptWeaponChange();

            if (!SkillInUse) return;

            Framecount = 0;
            Player.CanMove(false);
            OnEffectStart();
        }


        protected override void OnEffectStart()
        {
            //Animation Related
            Player.ResetTriggersAnimator();
            Player.SetTriggerAnimator(SkillData.AnimationName);
        }

        protected override void OnEffectEnd()
        {
            Player.CanMove(true);
            Player.ResetTriggersAnimator();
            Player.ChangeWeapon();
            SkillInUse = false;
            Player.SkillInUse = false;
        }
    }
}