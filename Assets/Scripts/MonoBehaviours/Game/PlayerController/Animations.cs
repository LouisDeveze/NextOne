
using UnityEngine;

namespace NextOne
{
    public class Animations
    {
        public static string Idle = "Idle";
        public static string RunBack = "Run Back";
        public static string RunFront = "Run Front";
        public static string StrafeRight = "Strafe Right";
        public static string StrafeLeft = "Strafe Left";
        public static string TurnRight = "Turn Right";
        public static string TurnLeft = "Turn Left";
        public static string PrimaryAction = "Primary Action";
        public static string SecondaryAction = "Secondary Action";
        public static string TertiaryAction = "Tertiary Action";
        public static string Stunned = "Stunned";
        public static string GettingUp = "Getting Up";
        public static string Dodge = "Dodge";


        /// <summary>
        /// Utils function to delete
        /// </summary>
        /// <param name="animator"></param>
        public static void ResetTriggers(Animator animator)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                    animator.ResetTrigger(parameter.name);
            }
        }
    }
}
