using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using RobotSmashers.Robots;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace RobotSmashers {
    public class LoopManager : MonoBehaviour {

        [FormerlySerializedAs("RobotReferences")] public RobotChassi[] RobotChassi;
        
        public InputData InputData;
        public Robot[] Robots;
        
        
        void Start() {
            RobotChassi = FindObjectsOfType<RobotChassi>();
            
            InputData.Players = new GamepadState[RobotChassi.Length];
            Robots = new Robot[RobotChassi.Length];

            for (int i = 0; i < Robots.Length; i++) {
                Robots[i] = new Robot();
                Robots[i].ControllingPlayer = (GamePad.Index) i + 1;
                Robots[i].Chassi = RobotChassi[i];
            }
        }

        void Update() {
            InputUtil.UpdateInput(InputData);
        }

        void FixedUpdate() {
            RobotUtil.UpdateRobots(InputData, Robots);
        }
    }
}