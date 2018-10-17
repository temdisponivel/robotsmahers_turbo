using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RobotSmashers.Robots {
    
    [RequireComponent(typeof(Rigidbody))]
    public class RobotChassi : MonoBehaviour {
        public float MoveSpeed;
        public float RotationSpeed;
        public Transform ParentTransform;
        public Transform CenterOfMass;
        public Rigidbody Body;

        public ComponentSet Components;

        [ContextMenu("BAKE")]
        void Reset() {
            ParentTransform = transform;
            Body = GetComponent<Rigidbody>();

            Components.AllFlippers = GetComponentsInChildren<Flipper>();
        }
    }
}