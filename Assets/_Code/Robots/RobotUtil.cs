using GamepadInput;
using UnityEngine;

namespace RobotSmashers.Robots {
    public class Robot {
        public GamePad.Index ControllingPlayer;
        public RobotReferences References;
    }
    
    public static class RobotUtil {
        private const float ROBOT_SPEED = 10;
        
        public static void UpdateRobots(InputData inputData, Robot[] robots) {
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];
                GamepadState inputState = InputUtil.GetGamepadState(inputData, robot.ControllingPlayer);

                Transform robotTrans = robot.References.transform;

                Vector3 leftStickDir = new Vector3(inputState.LeftStickAxis.x, 0, inputState.LeftStickAxis.y);
                robotTrans.forward = leftStickDir;

                Vector3 force = robotTrans.forward * robot.References.ForceToApply * Time.deltaTime * leftStickDir.magnitude;
                robot.References.Body.AddForce(force);
                
                // robotTrans.position += robotTrans.forward * ROBOT_SPEED * Time.deltaTime * leftStickDir.magnitude;
            }
        }
    }
}