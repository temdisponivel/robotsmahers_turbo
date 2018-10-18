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

        void Start() {
            GameData.Robots = FindObjectsOfType<Robot>(); // TODO: This will eventually be spawned from the template loaded from file
            RobotUtil.SetupRobots(GameData.Robots);

            // This can happen when the game is loaded from the arena scene instead of the title scene
            if (GameData.Match == null) {
                GameData.Match = new Match();
                MatchUtil.ResetMatch(GameData.Match, 2);
            }
            
            GameData.Match.Robots = GameData.Robots; // Update with the new robots
            GUIUtil.ChangeState(GUIMaster, GameData.Match, GameData.NextState);
        }

        void Update() {
            RobotUtil.UpdateRobots(GameData.Robots, GUIMaster);
            MatchUtil.UpdateMatch(GameData.Match, GUIMaster);
            GUIUtil.UpdateGUI(GUIMaster, GameData.Match);
        }

        void FixedUpdate() {
            RobotUtil.FixedUpdateRobots(GameData.Robots, GUIMaster);
        }

        public static void ReloadSceneAndChangeState(GameplayGUIState state, bool resetMatch) {
            GameData.NextState = state;
            SceneManager.LoadScene("_Arena");
        }
    }
}