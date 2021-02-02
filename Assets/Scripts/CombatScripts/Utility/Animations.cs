using System;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public enum EAnimation
    {
        Idle,
        RunBack,
        RunFront,
        StrafeRight,
        StrafeLeft,
        TurnRight,
        TurnLeft,
        AutoAttack,
        AutoAttackTwo,
        PrimaryAction,
        SecondaryAction,
        TertiaryAction,
        Stunned,
        GettingUp,
        Dodge
    }

    public class Animations
    {
        public const string IDLE = "Idle";
        public const string RUNBACK = "Run Back";
        public const string RUNFRONT = "Run Front";
        public const string STRAFERIGHT = "Strafe Right";
        public const string STRAFELEFT = "Strafe Left";
        public const string TURNRIGHT = "Turn Right";
        public const string TURNLEFT = "Turn Left";
        public const string AUTOATTACK = "Auto Attack";
        public const string AUTOATTACKTWO = "Auto Attack Two";
        public const string PRIMARYACTION = "Primary Action";
        public const string SECONDARYACTION = "Secondary Action";
        public const string TERTIARYACTION = "Tertiary Action";
        public const string STUNNED = "Stunned";
        public const string GETTINGUP = "Getting Up";
        public const string DODGE = "Dodge";

        private static Animations instance;

        private static Dictionary<EAnimation, string> EnumMap;


        private static Animations Instance()
        {
            if (instance != null) return instance;

            instance = new Animations();
            instance.SetMap();

            return instance;
        }


        private void SetMap()
        {
            EnumMap = new Dictionary<EAnimation, string>
            {
                {EAnimation.Idle, IDLE},
                {EAnimation.RunBack, RUNBACK},
                {EAnimation.RunFront, RUNFRONT},
                {EAnimation.StrafeRight, STRAFERIGHT},
                {EAnimation.StrafeLeft, STRAFELEFT},
                {EAnimation.TurnRight, TURNRIGHT},
                {EAnimation.TurnLeft, TURNLEFT},
                {EAnimation.AutoAttack, AUTOATTACK},
                {EAnimation.AutoAttackTwo, AUTOATTACKTWO},
                {EAnimation.PrimaryAction, PRIMARYACTION},
                {EAnimation.SecondaryAction, SECONDARYACTION},
                {EAnimation.TertiaryAction, TERTIARYACTION},
                {EAnimation.Stunned, STUNNED},
                {EAnimation.GettingUp, GETTINGUP},
                {EAnimation.Dodge, DODGE}
            };
        }

        public static string GetStringEquivalent(EAnimation _enum)
        {
            if (instance == null)
                Instance();

            if (EnumMap.TryGetValue(_enum, out var enumEquivalent))
            {
                return enumEquivalent;
            }

            throw new Exception();
        }

        /// <summary>
        /// Utils function to delete
        /// </summary>
        /// <param name="_animator"></param>
        public static void ResetTriggers(Animator _animator)
        {
            foreach (AnimatorControllerParameter parameter in _animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                    _animator.ResetTrigger(parameter.name);
            }
        }
    }
}