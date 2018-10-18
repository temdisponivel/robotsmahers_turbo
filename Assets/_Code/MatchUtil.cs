using System;
using RobotSmashers.GUI;
using RobotSmashers.Robots;
using UnityEngine;

namespace RobotSmashers {
    [Serializable]
    public class Match {
        public Robot[] Robots;
        public int[] Wins;

        public Robot Winner;
    }

    public static class MatchUtil {
        public static void ResetMatch(Robot[] robots, Match match) {
            match.Wins = new int[robots.Length];
            match.Robots = robots;
        }

        public static void UpdateMatch(
            Match match,
            GameplayGUIMaster gui
        ) {
            if (gui.CurrentState == GameplayGUIState.PLAYING) {
                int deadRobots = 0;
                for (int i = 0; i < match.Robots.Length; i++) {
                    Robot robot = match.Robots[i];
                    if (robot.CurrentHP <= 0) {
                        deadRobots++;
                    }
                }

                // If there's only one left
                if (deadRobots == match.Robots.Length - 1) {
                    int aliveRobotIndex = 0;
                    for (int i = 0; i < match.Robots.Length; i++) {
                        Robot robot = match.Robots[i];
                        if (robot.CurrentHP > 0) {
                            aliveRobotIndex = i;
                        }
                    }

                    match.Wins[aliveRobotIndex]++;

                    match.Winner = match.Robots[aliveRobotIndex];

                    if (match.Wins[aliveRobotIndex] >= 2) {
                        GUIUtil.ChangeState(gui, match, GameplayGUIState.MATCH_ENDED);
                    } else {
                        GUIUtil.ChangeState(gui, match, GameplayGUIState.ROUND_ENDED);
                    }
                }
            }
        }
    }
}