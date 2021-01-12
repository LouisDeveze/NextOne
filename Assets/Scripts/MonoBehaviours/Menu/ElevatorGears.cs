using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    /// <summary>
    /// Elevator Gears update
    /// </summary>
    public class ElevatorGears : MonoBehaviour
    {
        public MenuContext context;

        public static float speedFactor = -200;

        private void Update()
        {
            float angle = context.elevatorSpeed * speedFactor * Time.deltaTime;
            transform.Rotate(Vector3.up, angle);
        }
    }
}
