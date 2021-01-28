using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace NextOne
{
    public abstract class SkillData : ScriptableObject
    {
        [SerializeField] private int SkillId = 0;
        [SerializeField] private String SkillName = "default";
        [SerializeField] private String SkillDescription = "default description";
        [SerializeField] private SkillAim SkillAim;
        [SerializeField] private SkillTrigger SkillTrigger;

        //[SerializeField] private float SkillAnimationTime;
        [SerializeField] private List<float> SkillAnimationTime;
        //[SerializeField] private float AnimationEffectiveUseTime;
        [SerializeField] private List<float> AnimationEffectiveUseTime;
        //[SerializeField] private EAnimation SkillAnimationName;
        [SerializeField] private List<EAnimation> SkillAnimationName;

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

        public List<float> AnimationTime => SkillAnimationTime;

        //public EAnimation AnimationName => SkillAnimationName;
        public List<EAnimation> AnimationName => SkillAnimationName;

        public List<float> EffectiveUseTime => AnimationEffectiveUseTime;
    }
}