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
        public Transform[] SpawnPoints;

        void Start() {
            if (GameData.ToSpawn != null && GameData.ToSpawn.Length > 0) {
                GameData.Robots = new Robot[GameData.ToSpawn.Length];
                for (int i = 0; i < GameData.ToSpawn.Length; i++) {
                    Transform spawnPoint = SpawnPoints[GameData.ToSpawn.Length - i - 1];
                    GameData.Robots[i] = Instantiate(GameData.ToSpawn[i], spawnPoint.position, spawnPoint.rotation);
                    GameData.Robots[i].ControllingPlayer = (GamePad.Index) i + 1;
                }
            } else {
                GameData.Robots = FindObjectsOfType<Robot>();
            }
            
            RobotUtil.SetupRobots(GameData.Robots);

            // This can happen when the game is loaded from the arena scene instead of the title scene
            if (GameData.Match == null) {
                GameData.Match = new Match();
                GameData.NextState = GameplayGUIState.ROUND_START;
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