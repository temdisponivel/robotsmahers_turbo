using UnityEngine;

namespace RobotSmashers {
    
    public class Track  : MonoBehaviour {
        public ForceMode ForceMode;
        public float Force;

        public ForceMode TorqueMode;
        public float Torque;
        
        public int GroundCollisions;

        private void OnCollisionEnter(Collision other) {
            if ((1 << other.gameObject.layer & Constants.GROUND_LAYER) == Constants.GROUND_LAYER) {
                GroundCollisions++;
            }
        }

        private void OnCollisionExit(Collision other) {
            if ((1 << other.gameObject.layer & Constants.GROUND_LAYER) == Constants.GROUND_LAYER) {
                GroundCollisions--;
            }
        }
    }
}