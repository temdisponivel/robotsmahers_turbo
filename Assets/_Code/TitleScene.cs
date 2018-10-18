using RobotSmashers.GUI;
using RobotSmashers.Robots;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RobotSmashers {
    public static class GameData {
        public static Match Match;
        public static Robot[] Robots;
        public static GameplayGUIState NextState;
    }
    
    public class TitleScene : MonoBehaviour {
        public int PlayerCount;

        public void Start() {
            GameData.Match = new Match(); 
            MatchUtil.ResetMatch(GameData.Match, PlayerCount);
            GameData.NextState = GameplayGUIState.ROUND_START;
        }

        public void PlayGame() {
            SceneManager.LoadScene("_Arena");
        }
    }
}