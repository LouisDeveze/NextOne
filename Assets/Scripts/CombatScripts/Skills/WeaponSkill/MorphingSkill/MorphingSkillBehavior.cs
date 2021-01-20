using Assets.Scripts.CombatScripts.Player;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.MorphingSkill
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

        private void PlayEffect()
        {
            
            //TODO: Implement VFX
            
        }
    }
}