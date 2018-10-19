using GamepadInput;
using UnityEngine;

namespace RobotSmashers {
    public class Flipper : MonoBehaviour {
        public GamePad.Button UseButton;

        public ForceMode ForceMode;
        public float Force;
        public float Radius;

        public float DefaultCooldownTime;
        public float CurrentCooldownTime;

        public bool DrawActivationGizmos;

        public Animator Animator;

        void OnDrawGizmosSelected() {
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