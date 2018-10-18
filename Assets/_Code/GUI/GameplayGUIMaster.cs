using System;
using GamepadInput;
using UnityEngine;

namespace RobotSmashers.GUI {
    [Serializable]
    public class ComponentImageByType {
        public Sprite Image;
        public ComponentType Component;
    }

    [Serializable]
    public class ButtonImage {
        public GamePad.Button Button;
        public Sprite Image;
    }

    public enum GameplayGUIState {
        PLAYING,
        ROUND_START,
        ROUND_ENDED,
        MATCH_ENDED
    }
    
    public class GameplayGUIMaster : MonoBehaviour {
        public HealthBar HealthBar;
        public ButtonBindingsGUI ButtonBindings;
        public StartRoundGUI StartRoundGUI;
        public EndRoundGUI EndRoundGUI;
        public EndMatchGUI EndMatchGUI;
        
        public ComponentImageByType[] Images;
        public ButtonImage[] ButtonImages;

        public GameplayGUIState CurrentState;
    }
}