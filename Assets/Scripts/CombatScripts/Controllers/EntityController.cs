using System.Collections.Generic;
using UnityEngine;

namespace NextOne.Controllers
{
    public abstract class EntityController : MonoBehaviour, IDamageable
    {
        public bool SkillInUse = false;
        public abstract void TakeDamage(int _damage);
        public abstract bool HasAnimatorPlaying(EAnimation _getRandomAnimationName, int _i);

        public abstract bool IsAnimationLastAtLeast(float _getRandomEffectiveTime, int _p1);

        public abstract void CanMove(bool _b);
        public abstract void ResetTriggersAnimator();
        public abstract void SetTriggerAnimator(EAnimation _getRandomAnimationName);
        //TODO: remove abstract -> implement the function
        public abstract List<Transform> GetCastPoint(ECastPoint _weapons);
    }
}