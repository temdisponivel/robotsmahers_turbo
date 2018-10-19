using System;
using UnityEngine;
using UnityEngine.UI;

namespace RobotSmashers.GUI {
    [Serializable]
    public class RoundWonsGUI {
        public Image[] Images;
    }
    public class HealthBar : MonoBehaviour {
        public Image[] HealthBarImages;
        public Text[] RobotNames;
        public RoundWonsGUI[] RoundsGUI;
    }
}