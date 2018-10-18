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
            RobotUtil.SetupRobots(Robots);
        }

        void Update() {
            RobotUtil.UpdateRobots(Robots);
            GUIUtil.UpdateHealthBar(Robots, HealthBar);

            bool pressed = GamePad.GetButton(GamePad.Button.A, GamePad.Index.One);
            if (pressed)
            {
                UnityEngine.Debug.Log("PRESSED ==A");
            }
        }

        void FixedUpdate() {
            RobotUtil.FixedUpdateRobots(Robots);
        }
    }
}