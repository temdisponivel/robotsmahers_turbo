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
                    robot.CurrentHP -= robot.LastCollision.relativeVelocity.magnitude;
                    robot.LastCollision = null;
                }
                
                ComponentUtil.UpdateFlippers(robot);
                ComponentUtil.UpdateBlades(robot);
            }
        }

        public static void FixedUpdateRobots(Robot[] robots) {
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];
                ComponentUtil.UpdateTracks(robot);
            }
        }
    }
}