using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public class PlayerController : MonoBehaviour
    {

        public Weapon weapon;
        
        public Transform RightHand;
        public Transform LeftHand;

        Rigidbody rb;
        // Movement Speed;
        public float speed = 6;
        private float animationSpeed;


        public float angularSpeed = 360;
        public float angularTreshold = 2;

        // Utils for rotation
        private float distanceFromCamera = 0;
        private Plane plane = new Plane(Vector3.up, new Vector3(0, 0, 0));

        private float mH;
        private float mV;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            weapon.Create(this.RightHand, this.LeftHand);
        }


        void FixedUpdate()
        {
            // Give directional smoothed velocity
            rb.velocity = new Vector3(mH * speed, rb.velocity.y, mV * speed);
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

            #region Rotation
            //Get the Screen positions of the object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distanceFromCamera))
            {
                Vector3 worldPosition = ray.GetPoint(distanceFromCamera);
                Vector3 toMouse = worldPosition - this.transform.position;
                float angle = Vector3.SignedAngle(toMouse, this.transform.forward, Vector3.up);

                // Calculate Variation Angle
                float variationAngle = angle > 0 ? Time.deltaTime * angularSpeed : -Time.deltaTime * angularSpeed;
                variationAngle = Mathf.Abs(variationAngle) > Mathf.Abs(angle) ? angle : variationAngle;
                variationAngle = Mathf.Abs(angle) > angularTreshold ? variationAngle : 0;

                // Finally Rotate the model
                this.transform.Rotate(Vector3.up, -variationAngle);

            }
            #endregion

        }


    }

}
