using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using RobotSmashers.GUI;
using RobotSmashers.Robots;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace RobotSmashers {
    public class GameplayLoopManager : MonoBehaviour {
        public GameplayGUIMaster GUIMaster;
        public Robot[] Robots;
        public static Match Match; // Static so that it persists between scene loads

        void Start() {
            Robots = FindObjectsOfType<Robot>();
            RobotUtil.SetupRobots(Robots);
            
            if (Match == null) {
                Match = new Match();
                MatchUtil.ResetMatch(Robots, Match);
                GUIUtil.ChangeState(GUIMaster, Match, GameplayGUIState.ROUND_START);
            }
        }

        void Update() {
            RobotUtil.UpdateRobots(Robots, GUIMaster);
            MatchUtil.UpdateMatch(Match, GUIMaster);
            GUIUtil.UpdateGUI(GUIMaster, Match);
        }

        void FixedUpdate() {
            RobotUtil.FixedUpdateRobots(Robots, GUIMaster);
        }
    }
}