using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public enum AttachmentPoint
    {
        LeftHand = 0,
        RightHand = 1
    }

    public class WeaponAnchors : MonoBehaviour
    {
        [SerializeField] private AttachmentPoint WeaponAttachmentPoint;

        public AttachmentPoint AttachmentPoint => WeaponAttachmentPoint;
    }
}