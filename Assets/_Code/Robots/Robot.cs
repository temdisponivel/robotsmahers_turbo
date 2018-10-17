using GamepadInput;
using UnityEngine;

namespace RobotSmashers.Robots {
    public class Robot : MonoBehaviour {
        public float DefaultHP;
        public float CurrentHP;
        public string Name;
        public GamePad.Index ControllingPlayer;
        public RobotChassi Chassi;
        public Collision LastCollision;

        private void OnCollisionEnter(Collision other) {
            LastCollision = other;
        }
    }
}