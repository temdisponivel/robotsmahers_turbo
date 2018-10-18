using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using RobotSmashers.GUI;
using RobotSmashers.Robots;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace RobotSmashers {
    public class LoopManager : MonoBehaviour {

        public GameplayGUIMaster GUIMaster;
        public Robot[] Robots;
        
        void Start() {
            Robots = FindObjectsOfType<Robot>();
            RobotUtil.SetupRobots(Robots);
            GUIUtil.SetupButtonGUI(Robots, GUIMaster);
        }

        void Update() {
            RobotUtil.UpdateRobots(Robots);
            GUIUtil.UpdateHealthBar(Robots, GUIMaster.HealthBar);
        }

        void FixedUpdate() {
            RobotUtil.FixedUpdateRobots(Robots);
        }
    }
}