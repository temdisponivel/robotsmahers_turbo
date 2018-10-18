using GamepadInput;
using UnityEngine;

namespace RobotSmashers {
    public class Axe : MonoBehaviour {
        public GamePad.Button UseButton;
        
        public float Damage;
        public float PhysicsForce;
        public float Radius;

        public bool AttackNow;

        // NOTE: Animation callback
        void OnAttack() {
            AttackNow = true;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, -transform.up * PhysicsForce);
        }
    }
}