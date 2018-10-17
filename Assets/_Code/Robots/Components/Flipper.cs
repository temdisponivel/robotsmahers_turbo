using GamepadInput;
using UnityEngine;

namespace RobotSmashers {
    public class Flipper : MonoBehaviour {
        public GamePad.Button UseButton;
        public float Force;
        public float Radius;

        public bool DrawActivationGizmos;

        void OnDrawGizmos() {
            if (DrawActivationGizmos) {
                Gizmos.color = Color.red;                
                DrawActivationGizmos = false;
            } else {
                Gizmos.color = Color.yellow;
            }
            
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
}