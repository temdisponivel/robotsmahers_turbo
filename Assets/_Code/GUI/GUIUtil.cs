using RobotSmashers.Robots;
using UnityEngine.UI;

namespace RobotSmashers.GUI {
    public static class GUIUtil {
        public static void UpdateHealthBar(Robot[] robots, HealthBar healthBars) {
            UnityEngine.Debug.Assert(robots.Length == healthBars.HealthBarImages.Length, "There's not enough health bars images. There need to be at least: " + robots.Length);
            
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];
                float normalizedHp = robot.CurrentHP / robot.DefaultHP;

                Text robotNameText = healthBars.RobotNames[i];
                robotNameText.text = robot.Name;

                Image image = healthBars.HealthBarImages[i];
                image.fillAmount = normalizedHp;
            }
        }
    }
}