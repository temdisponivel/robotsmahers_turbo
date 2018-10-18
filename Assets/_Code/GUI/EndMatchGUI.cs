using UnityEngine;
using UnityEngine.UI;

namespace RobotSmashers.GUI {
    public class EndMatchGUI : MonoBehaviour {
        public Text WinnerText;
        public bool RestartClicked;

        public void Click() {
            RestartClicked = true;
        }
    }
}