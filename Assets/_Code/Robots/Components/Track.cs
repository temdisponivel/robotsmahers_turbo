using UnityEngine;

namespace RobotSmashers {
    
    public class Track  : MonoBehaviour {
        public ForceMode ForceMode;
        public float Force;

        public ForceMode TorqueMode;
        public float Torque;
        
        public bool Grounded;

        private void OnCollisionEnter(Collision other) {
            Grounded = other.gameObject.CompareTag(Constants.GROUND_TAG);
        }

        /*private void OnTriggerEnter(Collider other) {
            Grounded = other.gameObject.CompareTag(Constants.GROUND_TAG);
        }*/

        private void OnCollisionExit(Collision other) {
            Grounded = !other.gameObject.CompareTag(Constants.GROUND_TAG);
        }

        /*private void OnTriggerExit(Collider other) {
            Grounded = !other.gameObject.CompareTag(Constants.GROUND_TAG);
        }*/
    }
}