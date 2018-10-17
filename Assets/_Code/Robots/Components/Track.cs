using UnityEngine;

namespace RobotSmashers {
    
    public class Track  : MonoBehaviour {
        public ForceMode ForceMode;
        public float Force;

        public ForceMode TorqueMode;
        public float Torque;
        
        public bool Grounded;

        private void OnCollisionEnter(Collision other) {
            Grounded = (1 << other.gameObject.layer & Constants.GROUND_LAYER) == Constants.GROUND_LAYER;
        }

        private void OnCollisionExit(Collision other) {
            Grounded = (1 << other.gameObject.layer & Constants.GROUND_LAYER) != Constants.GROUND_LAYER;
        }
    }
}