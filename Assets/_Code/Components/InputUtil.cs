using System;
using GamepadInput;
using UnityEngine;

namespace RobotSmashers {
    [Serializable]
    public class InputData {
        public GamepadState[] Players;
    }
    
    public static class InputUtil {
        
        public static void UpdateInput(InputData inputData) {
            for (int i = 0; i < inputData.Players.Length; i++) {
                inputData.Players[i] = GamePad.GetState((GamePad.Index) i + 1);
            }
        }
    }
}