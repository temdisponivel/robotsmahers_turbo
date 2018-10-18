using System.Collections.Generic;
using UnityEngine;

namespace RobotSmashers {
    public class Blade : MonoBehaviour {
        public float PhysicsForce;
        public float DamagePerSecond;
        public float Radius;
        public List<Collision> CurrentCollisions;

        void Awake() {
            CurrentCollisions = new List<Collision>();
        }

        private void OnCollisionEnter(Collision other) {
            if ((1 << other.gameObject.layer & Constants.ROBOT_LAYER) != Constants.ROBOT_LAYER) {
                return;
            }

            for (int i = 0; i < CurrentCollisions.Count; i++) {
                if (CurrentCollisions[i].collider == other.collider) {
                    return;
                }
            }

            CurrentCollisions.Add(other);
        }

        private void OnCollisionExit(Collision other) {
            for (int i = 0; i < CurrentCollisions.Count; i++) {
                if (CurrentCollisions[i].collider == other.collider) {
                    CurrentCollisions.RemoveAt(i--);
                }
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
}