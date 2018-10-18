using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using RobotSmashers.GUI;
using RobotSmashers.Robots;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace RobotSmashers {
    public class GameplayLoopManager : MonoBehaviour {
        public GameplayGUIMaster GUIMaster;
        public Robot[] Robots;
        public static Match Match; // Static so that it persists between scene loads
        public static GameplayGUIState NextState;
        public static bool ResetMatch;

        void Start() {
            Robots = FindObjectsOfType<Robot>();
            RobotUtil.SetupRobots(Robots);
            
            if (Match == null) {
                Match = new Match();
                NextState = GameplayGUIState.ROUND_START;
                ResetMatch = true;
            }
            
            Match.Robots = Robots;
            
            GUIUtil.ChangeState(GUIMaster, Match, NextState);


            if (ResetMatch) {
                ResetMatch = false;
                MatchUtil.ResetMatch(Robots, Match);
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

        public static void ReloadSceneAndChangeState(GameplayGUIState state, bool resetMatch) {
            NextState = state;
            ResetMatch = resetMatch;
            SceneManager.LoadScene("_Arena");
        }
    }
}