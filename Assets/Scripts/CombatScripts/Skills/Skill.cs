using System;
using Assets.Scripts.CombatScripts.Skills.Aim;
using Assets.Scripts.CombatScripts.Skills.Trigger;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills
{
    public abstract class Skill : ScriptableObject
    {
        public String SkillName;
        public String SkillDescription;
        public SkillAim SkillAim;
        public SkillTrigger SkillTrigger;

        public abstract void Use(Transform _origin, Target _target);
        
    }
}
