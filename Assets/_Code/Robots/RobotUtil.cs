using GamepadInput;
using UnityEngine;

namespace RobotSmashers.Robots {
    public static class RobotUtil {
        
        public static void UpdateRobots(Robot[] robots) {
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];

                if (robot.Chassi.CenterOfMass != null) {
                    Vector3 localCenterOfMass = robot.Chassi.Body.transform.InverseTransformPoint(robot.Chassi.CenterOfMass.position);
                    robot.Chassi.Body.centerOfMass = localCenterOfMass;                    
                }

                if (robot.LastCollision != null) {
                    robot.CurrentHP -= robot.LastCollision.relativeVelocity.magnitude / 10; // TODO: This will probably come from the shields of the robot
                    robot.LastCollision = null;
                }
                
                ComponentUtil.UpdateFlippers(robot);
                ComponentUtil.UpdateBlades(robot);
            }
        }

        public static void FixedUpdateRobots(Robot[] robots) {
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];
                //robot.Chassi.Body.velocity = robot.Chassi.Body.velocity / (robot.Chassi.Body.mass * 3.33f);
                ComponentUtil.UpdateTracks(robot);
            }
        }
    }
}