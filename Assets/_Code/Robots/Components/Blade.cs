using System.Collections.Generic;
using GamepadInput;
using UnityEngine;

namespace RobotSmashers {
    public class Blade : MonoBehaviour {
        public GamePad.Button UseButton;
        public float PhysicsForce;
        public float DamagePerSecond;
        public List<Collider> CurrentCollisions;
        public Animator Animator;

        void Awake() {
            CurrentCollisions = new List<Collider>();
        }

        private void OnCollisionEnter(Collision other) {
            ComponentUtil.ValidateCollisionEnter(other.collider, CurrentCollisions);
        }

        private void OnCollisionExit(Collision other) {
            ComponentUtil.ValidateCollisionExit(other.collider, CurrentCollisions);
        }
    }
}