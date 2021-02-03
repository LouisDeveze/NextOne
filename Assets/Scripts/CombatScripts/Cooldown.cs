using UnityEngine;

namespace NextOne
{
    public class Cooldown
    {
        private float Timer;
        private float SkillCooldown;
        private bool CooldownTimerStarted;
        private bool SkillReadyToCast;

        public Cooldown(float _skillCooldown)
        {
            this.SkillCooldown = _skillCooldown;
          //  Debug.Log("Cooldown Constructor: " + SkillCooldown);
            this.Timer = _skillCooldown;
            this.SkillReadyToCast = true;
            this.CooldownTimerStarted = false;
        }


        public void UseCooldown()
        {
          //  Debug.Log("Use CD before check");
            if (!SkillReadyToCast) return;

           // Debug.Log("USE CD After Check");
            CooldownTimerStarted = true;
            SkillReadyToCast = false;
        }

        public void UpdateCooldown()
        {
            if (!CooldownTimerStarted) return;
            Timer -= Time.deltaTime;
          //  Debug.Log("Timer: " + Timer);

            if (!(Timer <= 0f)) return;
         //   Debug.Log("Timer Reached: Reset Timer");
            ResetTimer();
            SkillReadyToCast = true;
        }

        private void ResetTimer()
        {
            Timer = SkillCooldown;
            CooldownTimerStarted = false;
        }

        public bool ReadyToCast => SkillReadyToCast;

        public float CurrentCooldown => Timer;
    }
}