using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic Projectile Move !!! VERY BASIC
public class ProjectileMove : MonoBehaviour
{
    //Speed of the projectile
    public float speed;

    //Accuracy of the projectile
    [Tooltip("From 0% to 100%")] public float accuracy;

    //Fire Rate (The time between two cast/fire)
    public float fireRate;

    //The prefab for the muzzle effect if applicable
    public GameObject muzzlePrefab;

    //The prefab for the hit effect if applicable
    public GameObject hitPrefab;

    //Audio if applicable
    public AudioClip castSFX;
    public AudioClip hitSFX;

    public List<GameObject> trails;

    private float speedRandomness;

    private Vector3 offset;

    //To keep track if the projectile collided or not
    private bool collided;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        //Get the rigid body
        rb = GetComponent<Rigidbody>();

        //used to create a radius for the accuracy and have a very unique randomness
        if (accuracy < 100f)
        {
            accuracy = 1 - (accuracy / 100);

            for (int i = 0; i < 2; i++)
            {
                var val = 1 * Random.Range(-accuracy, accuracy);
                var index = Random.Range(0, 2);
                if (i == 0)
                {
                    offset = index == 0 ? new Vector3(0, -val, 0) : new Vector3(0, val, 0);
                }
                else
                {
                    offset = index == 0 ? new Vector3(0, offset.y, -val) : new Vector3(0, offset.y, val);
                }
            }
        }

        //Instantiate muzzle effect if applicable
        if (muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward + offset;
            var ps = muzzleVFX.GetComponent<ParticleSystem>();
            if (ps != null)
                Destroy(muzzleVFX, ps.main.duration);
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }

        //Play Cast sfx
        if (castSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(castSFX);
        }
    }

    //...
    void FixedUpdate()
    {
        //If the projectile is moving
        if (speed != 0 && rb != null)
            //move the projectile forward with a given offset
            rb.position += (transform.forward + offset) * (speed * Time.deltaTime);
    }

    //If the projectile collide
    void OnCollisionEnter(Collision collision)
    {
        //If the game object is not an other cast & hasn't already collided
        if (collision.gameObject.tag != "cast" && !collided)
        {
            collided = true;

            //Play collision sfx
            if (castSFX != null && GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().PlayOneShot(hitSFX);
            }

            //If the projectile has a trail, destroy it
            if (trails.Count > 0)
            {
                foreach (var t in trails)
                {
                    t.transform.parent = null;
                    var ps = t.GetComponent<ParticleSystem>();
                    if (ps == null) continue;
                    ps.Stop();
                    Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                }
            }

            //Stop projectile movement
            speed = 0;
            GetComponent<Rigidbody>().isKinematic = true;

            //Get the contact point
            ContactPoint contactPoint = collision.GetContact(0);
            //Get the normal to the y axis
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
            //Get the position of the contact point
            Vector3 pos = contactPoint.point;

            //Instantiate hit effect if applicable !
            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, pos - new Vector3(-.5f, +1, 0), rotation) as GameObject;

                var ps = hitVFX.GetComponent<ParticleSystem>();
                if (ps == null)
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
                else
                    Destroy(hitVFX, ps.main.duration);
            }

            StartCoroutine(DestroyParticle(0f));
        }
    }

    //Destroy all attached particle
    public IEnumerator DestroyParticle(float waitTime)
    {
        if (transform.childCount > 0 && waitTime != 0)
        {
            List<Transform> tList = new List<Transform>();

            foreach (Transform t in transform.GetChild(0).transform)
            {
                tList.Add(t);
            }

            while (transform.GetChild(0).localScale.x > 0)
            {
                yield return new WaitForSeconds(0.01f);
                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                foreach (var t in tList)
                {
                    t.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }

        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}