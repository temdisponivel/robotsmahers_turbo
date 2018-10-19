using UnityEngine;
using UnityEngine.UI;

namespace RobotSmashers.GUI {
    public class StartRoundGUI : MonoBehaviour {
        public float DefaultStartMatchCooldown;
        public float CurrentStartMatchCooldown;
        public Text CountdownText;
        public AudioSource ding;
        public bool Played;
    }
}