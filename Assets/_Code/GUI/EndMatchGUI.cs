using UnityEngine;
using UnityEngine.UI;

namespace RobotSmashers.GUI {
    public class EndMatchGUI : MonoBehaviour {
        public Text WinnerText;
        public bool RestartClicked;
        public AudioSource winner;

        public void Click() {
            RestartClicked = true;
        }
    }
}