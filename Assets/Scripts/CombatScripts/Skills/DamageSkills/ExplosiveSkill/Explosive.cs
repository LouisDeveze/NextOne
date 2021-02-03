using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.VFX;

namespace NextOne
{
    public class Explosive : MonoBehaviour
    {
        [SerializeField] private GameObject PSource;
        [SerializeField] private int ExplosionDamage;

        [SerializeField] private GameObject ExplosionEffect;
        [SerializeField] private AudioClip ExplosionSfx;

        [SerializeField] private SkillAim SkillExplosionAim;
        [SerializeField] private float SkillRadius;

        [SerializeField] public float SourceID;


        [SerializeField] private float ExplosionDelay;
        private bool HasExploded = false;

        void Start()
        {
        }

        void Update()
        {
            //Reduce by 1 each second
            if (ExplosionDelay > 0f)
            {
                ExplosionDelay -= Time.deltaTime;
            }

            if (ExplosionDelay <= 0f && !HasExploded)
            {
                Debug.Log("Explosion delay reached " + ExplosionDelay + " in: " + this.GetInstanceID() + " ParentID: " +
                          SourceID);
                Explode();
            }
        }

        private void Explode()
        {
            Debug.Log("Explosion in: " + this.GetInstanceID() + " ParentID:" + SourceID);
            HasExploded = true;
            //INSTANTIATE VFX GRAPH EFFECT
            GameObject go = Instantiate(ExplosionEffect, transform.position, transform.rotation);
            Destroy(this.gameObject, .1f);
            VisualEffect explosionEffect = go.GetComponent<VisualEffect>();
            explosionEffect.Play();

            //TODO: CHECK VFX EVENT BLA BLA BLA
            SkillUseParams skillUseParams = new SkillUseParams {Origin = this.gameObject, Radius = SkillRadius};
            //Get Enemies within range of explosion
            SkillExplosionAim.GetTarget(skillUseParams);

            //Iterate through all enemies caught
            Target target = skillUseParams.Target;
            if (target is EnemyTarget targets)
            {
                foreach (EnemyController enemy in targets.TargetEnemies)
                {
                    enemy.TakeDamage(ExplosionDamage);
                }
            }

            Destroy(go, 2);
        }

        public GameObject Source
        {
            get => PSource;
            set => PSource = value;
        }

        public int Damage
        {
            get => ExplosionDamage;
            set => ExplosionDamage = value;
        }

        public float Delay
        {
            get => ExplosionDelay;
            set => ExplosionDelay = value;
        }

        public GameObject EffectPrefab
        {
            get => ExplosionEffect;
            set => ExplosionEffect = value;
        }

        public AudioClip Sfx
        {
            get => ExplosionSfx;
            set => ExplosionSfx = value;
        }

        public SkillAim ExplosionAim
        {
            get => SkillExplosionAim;
            set => SkillExplosionAim = value;
        }

        public float Radius
        {
            get => SkillRadius;
            set => SkillRadius = value;
        }
    }
}