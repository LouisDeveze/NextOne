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

        #region Rotation / Movement fields
        // Hyper Parameters
        public float speed = 7;
        public float angularSpeed = 270;
        private bool canMove = true;

        // Utils for rotation
        private float distanceFromCamera = 0;
        private Plane plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
        private float angularTreshold = 5;
        private float angle;
        // Movement in world space
        private Vector3 movement;
        // Movement in model Space
        private Vector3 modelMovement;
        // Time in second to stop or get full speed of the player
        private float movementSmoother = .4f;
        private float prevMagnitude =0;
        #endregion

        void Start()
        {
            this.rigidbd = model.GetComponent<Rigidbody>();
            this.animator = model.GetComponent<Animator>();
            // Weapon is created and sets the runtime animator controller to use
            weapon.Create(this.animator, this.RightHand, this.LeftHand);
        }
        

        private void Update()
        {
            #region Movement
            // Retrieve Movement Input
            float mH = Input.GetAxisRaw("Horizontal");
            float mV = Input.GetAxisRaw("Vertical");

            // Calculate absolute movement
            float absoluteMovement = Mathf.Max(Mathf.Abs(mH), Mathf.Abs(mV));
            // Calculate smoothed movement
            float magnitude = 0f;
            if (absoluteMovement > prevMagnitude)
                magnitude = Mathf.Min(1.0f, prevMagnitude + Time.deltaTime / movementSmoother);
            else if (absoluteMovement < prevMagnitude)
                magnitude = Mathf.Max(0f, prevMagnitude - Time.deltaTime / movementSmoother);
            else
                magnitude = absoluteMovement;
            
            this.prevMagnitude = magnitude;
            // Computing movement in world space
            movement = Vector3.ClampMagnitude(new Vector3(mH, 0, mV), 1.0f) * magnitude;
            #endregion

            #region Rotation with mouse
            //Get the Screen positions of the object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distanceFromCamera))
            {
                Vector3 worldPosition = ray.GetPoint(distanceFromCamera);
                Vector3 toMouse = worldPosition - this.model.transform.position;
                angle = Vector3.SignedAngle(toMouse, this.model.transform.forward, Vector3.up);

                // Calculate Variation Angle
                float variationAngle = angle > 0 ? Time.deltaTime * angularSpeed : -Time.deltaTime * angularSpeed;
                variationAngle = Mathf.Abs(variationAngle) > Mathf.Abs(angle) ? angle : variationAngle;
                variationAngle = Mathf.Abs(angle) > angularTreshold ? variationAngle : 0;

                // Finally Rotate the model
                this.model.transform.Rotate(Vector3.up, -variationAngle);
            }
            #endregion


            #region Special Move / Spells
            // If special animation was triggered, wait until it ends
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
            {
                // Reactivate the velocity
                canMove = true;
            }

            // Special Input Animation
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canMove = false;
                Animations.ResetTriggers(this.animator);
                this.animator.SetTrigger(Animations.Dodge);
            }
            #endregion


            #region Model Space Movement Animation
            modelMovement = model.transform.InverseTransformDirection(movement);

            if (canMove)
                this.weapon.AnimateMovement(animator, model, modelMovement, angle);
            #endregion

            Debug.Log(canMove);
        }

        private void FixedUpdate()
        {

            // Give directional smoothed velocity
            if (canMove)
                this.rigidbd.velocity = movement * speed;
        }
    }

}
