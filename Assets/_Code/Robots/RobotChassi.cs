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

        void Reset() {
            ParentTransform = transform;
            Body = GetComponent<Rigidbody>();
        }
    }
}