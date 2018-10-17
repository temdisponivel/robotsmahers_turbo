using GamepadInput;
using UnityEngine;

namespace RobotSmashers.Robots {
    public class Robot {
        public GamePad.Index ControllingPlayer;
        public RobotChassi Chassi;
    }
    
    public static class RobotUtil {
        
        public static void UpdateRobots(InputData inputData, Robot[] robots) {
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];
                GamepadState inputState = InputUtil.GetGamepadState(inputData, robot.ControllingPlayer);

                Transform robotTrans = robot.Chassi.transform;

                if (robot.Chassi.CenterOfMass != null) {
                    Vector3 localCenterOfMass = robot.Chassi.Body.transform.InverseTransformPoint(robot.Chassi.CenterOfMass.position);
                    robot.Chassi.Body.centerOfMass = localCenterOfMass;                    
                }
                
                Vector3 rightStickDir = new Vector3(inputState.RightStickAxis.x, 0, inputState.RightStickAxis.y);
                robotTrans.forward = Vector3.Lerp(robotTrans.forward, rightStickDir, robot.Chassi.RotationSpeed * Time.deltaTime);
                robotTrans.position += robotTrans.forward * robot.Chassi.MoveSpeed * Time.deltaTime * inputState.LeftStickAxis.y;
            }
        }
    }
}