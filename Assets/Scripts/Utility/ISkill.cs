using Assets.Scripts.CombatScripts.Skills;

namespace Assets.Scripts.Utility
{
    public interface ISkill
    {
        void Use(SkillUseParams _useParams);
    }
}