﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public class WeaponController
    {
        //ATTACH WEAPON
        private List<GameObject> weaponsGO = new List<GameObject>();
        private List<Weapon> WeaponsModel = new List<Weapon>();
        private float WeaponDamage;
        private EWeaponAnimation EWeaponAnimation;
        private AnimatorOverrideController WeaponAnimator;

        // Angle in degrees before running animation becomes strafe
        private float thresholdStrafe = .5f;

        /// T'as pas le droit de faire un  constructor, utilise start ou une fonction Init
        public WeaponController(List<Weapon> _weapons, EWeaponAnimation _weaponAnimation, AnimatorOverrideController controller)
        {
            Weapons = _weapons;
            EWeaponAnimation = _weaponAnimation;
            WeaponAnimator = controller;
        }

        public void AnimateMovement(Animator _animator, Vector3 _movement, float _angle)
        {
            DefaultAnimation(_animator, _movement, _angle);
            /*  switch (WeaponAnimation)
              {
                  case EWeaponAnimation.None:
                      None();
                      break;
                  case EWeaponAnimation.OneHandedMelee:
                      OneHandedMelee();
                      break;
                  case EWeaponAnimation.TwoHandedMelee:
                      TwoHandedMelee(_animator, _movement, _angle);
                      break;
                  case EWeaponAnimation.OneHandedRanged:
                      OneHandedRanged();
                      break;
                  case EWeaponAnimation.TwoHandedRanged:
                      TwoHandedRanged();
                      break;
                  case EWeaponAnimation.TwoHandedMix:
                      TwoHandedMix();
                      break;
                  default:
                      None();
                      break;
              }*/
        }

        private void DefaultAnimation(Animator _animator, Vector3 _movement, float _angle)
        {
            string trigger = Animations.GetStringEquivalent(EAnimation.Idle);


            // If movement in right is superior to the Strafe threshold, Strafe Right
            if (_movement.x > thresholdStrafe)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.StrafeRight);
            }
            // If movement in Left is superior to the Strafe threshold, Strafe Left
            else if (_movement.x < -thresholdStrafe)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.StrafeLeft);
            }
            // Else check the movement in Z to now if player is running backward or frontward
            else if (_movement.z > 0)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.RunFront);
            }
            // Else check the movement in Z to now if player is running backward or frontward
            else if (_movement.z < 0)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.RunBack);
            }
            else if (_movement.magnitude == 0 && _angle < -10)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.TurnRight);
            }
            // Else if idle and turning a lot
            else if (_movement.magnitude == 0 && _angle > 10)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.TurnLeft);
            }
            // Idle triggered when there are no movement
            else
            {
                trigger = Animations.GetStringEquivalent(EAnimation.Idle);
            }

            Animations.ResetTriggers(_animator);
            _animator.SetTrigger(trigger);
        }

        public void SetActive(bool _active)
        {
            foreach (var weapon in Weapons)
            {
                weapon.Model.SetActive(_active);
            }
        }
        
        public void ActiveWeaponTrigger(bool _active)
        {
            foreach (var weapon in Weapons)
            {
                weapon.ActiveTrigger(_active);
            }
        }
        

        private void None()
        {
        }

        private void OneHandedMelee()
        {
        }

        private void TwoHandedMelee(Animator _animator, Vector3 _movement, float _angle)
        {
            DefaultAnimation(_animator, _movement, _angle);
        }

        private void OneHandedRanged()
        {
        }

        private void TwoHandedRanged()
        {
        }

        private void TwoHandedMix()
        {
        }

        public Weapon Weapon(int _index)
        {
            if (WeaponsModel.Count < _index)
                throw new IndexOutOfRangeException();
            return Weapons[_index];
        }

        public List<Weapon> Weapons
        {
            get => WeaponsModel;
            set => WeaponsModel = value;
        }

        public Weapon GetWeaponAt(int _index)
        {
            if (_index < Weapons.Count)
                return Weapons[_index];
            else return null;
        }

        public EWeaponAnimation WeaponAnimation => EWeaponAnimation;

        public AnimatorOverrideController WeaponAnimatorOverride => WeaponAnimator;
        public List<Transform> GetCastPoint()
        {
            List<Transform> castPoints = new List<Transform>();
            foreach (var weapon in Weapons)
            {
                castPoints.Add(weapon.GetCastPoint());
            }

            return castPoints;
        }

   
    }
}