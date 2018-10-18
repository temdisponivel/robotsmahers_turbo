using UnityEngine;

namespace RobotSmashers {
    
    public class Track  : MonoBehaviour {
        public ForceMode ForceMode;
        public float Force;

        public ForceMode TorqueMode;
        public float Torque;
        
        public bool Grounded;

        public int CollisionCount;

        private void OnCollisionEnter(Collision other) {
            CollisionCount++;
            Grounded = CollisionCount > 0; //(1 << other.gameObject.layer & Constants.GROUND_LAYER) == Constants.GROUND_LAYER;
        }

        private void OnCollisionExit(Collision other) {
            CollisionCount--;
            Grounded = CollisionCount <= 0; //(1 << other.gameObject.layer & Constants.GROUND_LAYER) != Constants.GROUND_LAYER;
        }
    }
}