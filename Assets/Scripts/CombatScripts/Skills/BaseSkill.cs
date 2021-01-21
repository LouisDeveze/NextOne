using Assets.Scripts.CombatScripts.Skills.Trigger;
using Assets.Scripts.Utility;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills
{
    public enum SkillAction
    {
        None,
        PrimaryAction,
        SecondaryAction,
        TertiaryAction
    }

    [CreateAssetMenu(fileName = "NewBaseSkill", menuName = "Next One/Skills/Base Skill")]
    public class BaseSkillD : SkillData
    {
        [SerializeField] private GameObject SkillEffect;
        [SerializeField] private SkillAction SkillAction;

        /*public override void Use(GameObject _origin, Target _target)
        {
            var instance =
                Instantiate(Effect, _origin.transform.position + _origin.transform.forward,
                    _origin.transform.rotation) as GameObject;
            //
        }*/
        
        public override ISkill AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            throw new System.NotImplementedException();
        }

        /*ublic override void Use(SkillUseParams _useParams)
        {
            throw new System.NotImplementedException();
        }*/

        public GameObject Effect => SkillEffect;
        public SkillAction Action => SkillAction;
    }
}