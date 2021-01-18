using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills
{
    public class SkillHolder : MonoBehaviour
    {
        public Skill[] Skills;

        public void Update()
        {
            //Example
            foreach (var skill in Skills)
            {
                var target = skill.SkillAim.GetTarget(transform);
                if (target != null)
                {
                    //Example access attribute
                    //skill.SkillName;

                    if (skill.SkillTrigger.IsTriggered())
                    {
                        skill.Use(transform, target);
                    }
                }
            }
        }
    }
}