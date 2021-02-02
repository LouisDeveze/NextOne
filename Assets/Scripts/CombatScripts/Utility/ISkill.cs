namespace NextOne
{
    public interface ISkill
    {
        void Use(SkillUseParams _useParams);

        void Detach();
        bool CanCast();
    }
}