using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.MeleeSkill
{
    public class MeleeSkillBehavior : MonoBehaviour, ISkill
    {
        private MeleeSkillData MeleeSkillData = null;

        void Start()
        {
            
        }

        public void SetData(MeleeSkillData _meleeSkillData)
        {
            this.MeleeSkillData = _meleeSkillData;
        }
        
        public void Use(SkillUseParams _useParams)
        {
            //Reduce Enemy Health HERE TARGETED because Auto Attack
            throw new System.NotImplementedException();
        }

        private void PlayEffect()
        {
            //TODO: Implement VFX
        }
    }
}