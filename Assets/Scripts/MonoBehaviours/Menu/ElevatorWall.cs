using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    /// <summary>
    /// This class controls the behaviour of the elevator in function of its speed
    /// </summary>
    public class ElevatorWall : MonoBehaviour
    {
        public MenuContext context = null;

        // Update is called once per frame
        void Update()
        {
            if(context != null)
            {
                Vector3 position = this.transform.position;
                // Send Back gameobject up
                if(position.y <= -12) {
                    position.y = 8 + (position.y + 12); }
                // Move it downwards
                position.y -= Time.deltaTime * context.elevatorSpeed;

                // Finally updates position
                this.transform.position = position;
            }
        }
    }

}
