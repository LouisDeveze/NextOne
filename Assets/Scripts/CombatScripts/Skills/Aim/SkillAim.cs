using UnityEngine;

namespace NextOne
{
    public abstract class SkillAim : ScriptableObject
    {
        public abstract void GetTarget(SkillUseParams _params);
    }
}
