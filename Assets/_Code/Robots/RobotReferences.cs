using UnityEngine;

namespace RobotSmashers.Robots {
    [RequireComponent(typeof(Rigidbody))]
    public class RobotReferences : MonoBehaviour {
        public float ForceToApply;
        public Transform ParentTransform;
        public Rigidbody Body;

        void Reset() {
            ParentTransform = transform;
            Body = GetComponent<Rigidbody>();
        }
    }
}