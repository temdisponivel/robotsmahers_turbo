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
                ComponentUtil.UpdateAxes(robot);
            }
        }

        public static void FixedUpdateRobots(Robot[] robots) {
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];

                Vector3 velocity = robot.Chassi.Body.velocity;
                float drag = (robot.Chassi.Body.mass * 2f);
                robot.Chassi.Body.velocity = new Vector3(robot.Chassi.Body.velocity.x / drag, velocity.y, robot.Chassi.Body.velocity.z / drag);
                ComponentUtil.UpdateTracks(robot);
            }
        }
    }
}