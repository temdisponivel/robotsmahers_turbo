using RobotSmashers.Robots;
using UnityEngine;

namespace RobotSmashers {
    
    [RequireComponent(typeof(Rigidbody))]
    public class Shield : MonoBehaviour {
        public float DefaultShieldAmount;
        public float CurrentShieldAmount;
        public FixedJoint Joint;

        void Reset() {
            Joint = GetComponent<FixedJoint>();
            Joint.connectedBody = GetComponentInParent<Robot>().Chassi.Body;
        }
    }
}