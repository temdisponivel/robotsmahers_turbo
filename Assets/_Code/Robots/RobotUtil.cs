using GamepadInput;
using UnityEngine;

namespace RobotSmashers.Robots {
    public static class RobotUtil {
        
        public static void UpdateRobots(Robot[] robots) {
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];

                Transform robotTrans = robot.Chassi.transform;

                if (robot.Chassi.CenterOfMass != null) {
                    Vector3 localCenterOfMass = robot.Chassi.Body.transform.InverseTransformPoint(robot.Chassi.CenterOfMass.position);
                    robot.Chassi.Body.centerOfMass = localCenterOfMass;                    
                }

                Vector2 rightStick = GamePad.GetAxis(GamePad.Axis.RightStick, robot.ControllingPlayer);
                Vector2 leftStick = GamePad.GetAxis(GamePad.Axis.LeftStick, robot.ControllingPlayer);
                
                Vector3 rightStickDir = new Vector3(rightStick.x, 0, rightStick.y);
                robotTrans.forward = Vector3.Lerp(robotTrans.forward, rightStickDir, robot.Chassi.RotationSpeed * Time.deltaTime);
                robotTrans.position += robotTrans.forward * robot.Chassi.MoveSpeed * Time.deltaTime * leftStick.y;
                
                ComponentUtil.UpdateFlippers(robot);
            }
        }
    }
}