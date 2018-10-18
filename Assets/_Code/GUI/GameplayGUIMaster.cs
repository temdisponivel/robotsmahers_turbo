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
    
    public class GameplayGUIMaster : MonoBehaviour {
        public HealthBar HealthBar;
        public ButtonBindingsGUI ButtonBindings;
        
        public ComponentImageByType[] Images;
        public ButtonImage[] ButtonImages;
    }
}