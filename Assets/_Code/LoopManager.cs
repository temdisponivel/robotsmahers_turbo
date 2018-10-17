using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using RobotSmashers.Robots;
using UnityEditor;
using UnityEngine;

namespace RobotSmashers {
    public class LoopManager : MonoBehaviour {

        public RobotReferences[] RobotReferences;
        
        public InputData InputData;
        public Robot[] Robots;
        
        
        void Start() {
            RobotReferences = FindObjectsOfType<RobotReferences>();
            
            InputData.Players = new GamepadState[RobotReferences.Length];
            Robots = new Robot[RobotReferences.Length];

            for (int i = 0; i < Robots.Length; i++) {
                Robots[i] = new Robot();
                Robots[i].ControllingPlayer = (GamePad.Index) i + 1;
                Robots[i].References = RobotReferences[i];
            }
        }

        void Update() {
            InputUtil.UpdateInput(InputData);
            RobotUtil.UpdateRobots(InputData, Robots);
        }
    }
}