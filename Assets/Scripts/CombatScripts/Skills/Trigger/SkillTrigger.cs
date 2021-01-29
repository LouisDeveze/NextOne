using UnityEngine;

namespace NextOne
{
    public abstract class SkillTrigger : ScriptableObject
    {
        public abstract bool IsTriggered(SkillUseParams _useParams);
    }
}
