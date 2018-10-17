using GamepadInput;
using UnityEngine;

namespace RobotSmashers.Robots {
    public class Robot : MonoBehaviour {
        public float HP;
        public GamePad.Index ControllingPlayer;
        public RobotChassi Chassi;
    }
}