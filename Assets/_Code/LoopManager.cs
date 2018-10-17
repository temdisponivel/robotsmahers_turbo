using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using RobotSmashers.Robots;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace RobotSmashers {
    public class LoopManager : MonoBehaviour {
        
        public Robot[] Robots;
        
        void Start() {
            Robots = FindObjectsOfType<Robot>();
        }

        void Update() {
            RobotUtil.UpdateRobots(Robots);
            RobotUtil.FixedUpdateRobots(Robots);
        }

        void FixedUpdate() {
            //RobotUtil.FixedUpdateRobots(Robots);
        }
    }
}