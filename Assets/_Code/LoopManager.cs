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

        public HealthBar HealthBar;
        public Robot[] Robots;
        
        void Start() {
            Robots = FindObjectsOfType<Robot>();
            for (int i = 0; i < Robots.Length; i++) {
                Robots[i].CurrentHP = Robots[i].HP;
            }
        }

        void Update() {
            RobotUtil.UpdateRobots(Robots);
            
            GUIUtil.UpdateHealthBar(Robots, HealthBar);
        }

        void FixedUpdate() {
            RobotUtil.FixedUpdateRobots(Robots);
        }
    }
}