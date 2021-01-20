using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public class PlayerController : MonoBehaviour
    {
        // The model and his hands
        public GameObject model;
        public Transform RightHand;
        public Transform LeftHand;

        // The weapon to use
        public Weapon weapon;
        
        // The Rigidbody use for physics
        Rigidbody rigidbd;

        // The animator of the model
        Animator animator;

        // Movement Speed;
        public float speed = 6;
        private float animationSpeed;


        public float angularSpeed = 360;
        private float angularTreshold = 5;

        // Utils for rotation
        private float distanceFromCamera = 0;
        private Plane plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
        // Utils for movements
        private float mH;
        private float mV;

        void Start()
        {
            this.rigidbd = model.GetComponent<Rigidbody>();
            this.animator = model.GetComponent<Animator>();
            // Weapon is created and sets the runtime animator controller to use
            weapon.Create(this.animator, this.RightHand, this.LeftHand);
        }

        // Update RigidBody inside Fixed Update for physics
        void FixedUpdate()
        {
            // Give directional smoothed velocity
            this.rigidbd.velocity = new Vector3(mH * speed, rigidbd.velocity.y, mV * speed);
        }

        private void Update()
        {
            #region Movement
            // Retrieve Movement Input
            mH = Input.GetAxisRaw("Horizontal");
            mV = Input.GetAxisRaw("Vertical");
            float sumOfMovement = Mathf.Abs(mH) + Mathf.Abs(mV);
            if (mH != 0 && mV != 0)
            {
                mH *= Mathf.Sqrt(2) / 2;
                mV *= Mathf.Sqrt(2) / 2;
            }
            #endregion

            #region Rotation with mouse
            //Get the Screen positions of the object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distanceFromCamera))
            {
                Vector3 worldPosition = ray.GetPoint(distanceFromCamera);
                Vector3 toMouse = worldPosition - this.model.transform.position;
                float angle = Vector3.SignedAngle(toMouse, this.model.transform.forward, Vector3.up);

                // Calculate Variation Angle
                float variationAngle = angle > 0 ? Time.deltaTime * angularSpeed : -Time.deltaTime * angularSpeed;
                variationAngle = Mathf.Abs(variationAngle) > Mathf.Abs(angle) ? angle : variationAngle;
                variationAngle = Mathf.Abs(angle) > angularTreshold ? variationAngle : 0;

                // Finally Rotate the model
                this.model.transform.Rotate(Vector3.up, -variationAngle);

            }
            #endregion

        }


    }

}
