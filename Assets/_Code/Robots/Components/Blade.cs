using System.Collections.Generic;
using UnityEngine;

namespace RobotSmashers {
    public class Blade : MonoBehaviour {
        public float PhysicsForce;
        public float DamagePerSecond;
        public List<Collider> CurrentCollisions;

        void Awake() {
            CurrentCollisions = new List<Collider>();
        }

        private void OnCollisionEnter(Collision other) {
            if ((1 << other.gameObject.layer & Constants.ROBOT_LAYER) != Constants.ROBOT_LAYER) {
                return;
            }

            for (int i = 0; i < CurrentCollisions.Count; i++) {
                if (CurrentCollisions[i] == other.collider) {
                    return;
                }
            }

            CurrentCollisions.Add(other.collider);
        }

        private void OnCollisionExit(Collision other) {
            for (int i = 0; i < CurrentCollisions.Count; i++) {
                if (CurrentCollisions[i] == other.collider) {
                    CurrentCollisions.RemoveAt(i--);
                }
            }
        }
    }
}