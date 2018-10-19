using RobotSmashers.GUI;
using RobotSmashers.Robots;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RobotSmashers {
    public static class GameData {
        public static Match Match;
        public static Robot[] Robots;
        public static GameplayGUIState NextState;
        public static Robot[] ToSpawn;
    }
    
    public class TitleScene : MonoBehaviour {
        public int PlayerCount;
        public Robot[] Prefabs;

        public void Start() {
            GameData.Match = new Match(); 
            MatchUtil.ResetMatch(GameData.Match, PlayerCount);
            GameData.NextState = GameplayGUIState.ROUND_START;

            int playerCount = Mathf.Min(Prefabs.Length, PlayerCount);
            GameData.ToSpawn = new Robot[playerCount];

            for (int i = 0; i < playerCount; i++) {
                int randomIndex = UnityEngine.Random.Range(0, playerCount);
                GameData.ToSpawn[i] = Prefabs[randomIndex];
            }
        }

        public void PlayGame() {
            SceneManager.LoadScene("_Arena");
        }
    }
}