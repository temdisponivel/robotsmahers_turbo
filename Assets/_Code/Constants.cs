using UnityEngine;

namespace RobotSmashers {
    public static class Constants {
        public static int GROUND_LAYER = LayerMask.GetMask("Ground");
        public static int ROBOT_LAYER = LayerMask.GetMask("Robot");
        public const string SHIELD_TAG = "Shield";
    }
}