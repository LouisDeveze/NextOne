using System;
using Assets.Scripts.CombatScripts.Skills.Aim;
using Assets.Scripts.CombatScripts.Skills.Trigger;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills
{
    public abstract class SkillData : ScriptableObject
    {
        [SerializeField] private int SkillId = 0;
        [SerializeField] private String SkillName = "default";
        [SerializeField] private String SkillDescription = "default description";
        [SerializeField] private SkillAim SkillAim;
        [SerializeField] private SkillTrigger SkillTrigger;

        protected ISkill Behavior;

        public abstract ISkill AttachComponentTo(GameObject _gameObjectToAttachTo);

        public void Detach()
        {
            Behavior.Detach();
        }

        public void Use(SkillUseParams _useParams)
        {
            Behavior.Use(_useParams);
        }

        public string Name => SkillName;

        public string Description => SkillDescription;

        public int Id => SkillId;

        public SkillAim Aim => SkillAim;

        public SkillTrigger Trigger => SkillTrigger;
    }
}