using System.Collections.Generic;
using GamepadInput;
using UnityEngine;

namespace RobotSmashers {


    public class FlameThrower : MonoBehaviour {
        public GamePad.Button UseButton;
        public AudioSource Sound;
        public ParticleSystem flames;

        public float DefaultFuel;
        public float CurrentFuel;

        public float FuelPerSecond;
        public float DamagePerSecond;

        public float RechargePerSecond;

        public float DefaultDelay = 3;
        public float CurrentDelay;

        public MeshRenderer FireRenderer;

        public List<Collider> CurrentColliders;

        private void OnTriggerEnter(Collider other) {
            ComponentUtil.ValidateCollisionEnter(other, CurrentColliders);
        }

        private void OnTriggerExit(Collider other) {
            ComponentUtil.ValidateCollisionExit(other, CurrentColliders);
        }
    }
}