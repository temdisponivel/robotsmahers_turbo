using System;
using GamepadInput;
using RobotSmashers.GUI;
using UnityEngine;

namespace RobotSmashers.Robots {
    public static class RobotUtil {
        public static void SetupRobots(Robot[] robots) {
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];

                if (i == 0) {
                    robot.Name = "Rob";
                    robot.Color = Color.blue;
                } else if (i == 1) {
                    robot.Name = "Betty";
                    robot.Color = Color.red;
                }

                robot.CurrentHP = robot.DefaultHP;

                for (int j = 0; j < robot.Chassi.Components.AllShields.Length; j++) {
                    Shield shield = robot.Chassi.Components.AllShields[j];
                    shield.CurrentShieldAmount = shield.DefaultShieldAmount;
                }

                for (int j = 0; j < robot.Chassi.Components.AllFlameThrowers.Length; j++) {
                    FlameThrower flameThrower = robot.Chassi.Components.AllFlameThrowers[j];
                    flameThrower.CurrentFuel = flameThrower.DefaultFuel;
                }
                
                robot.Chassi.Renderer.material.SetColor("_MainColor", robot.Color);
            }
        }

        public static void UpdateRobots(Robot[] robots, GameplayGUIMaster gui) {
            if (gui.CurrentState != GameplayGUIState.PLAYING) {
                return;
            }

            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];

                if (robot.CurrentHP <= 0) {
                    Joint[] joint = robot.GetComponentsInChildren<Joint>();
                    for (int j = 0; j < joint.Length; j++) {
                        UnityEngine.Object.Destroy(joint[j]);                        
                    }
                    continue;
                }

                if (robot.Chassi.CenterOfMass != null) {
                    Vector3 localCenterOfMass = robot.Chassi.Body.transform.InverseTransformPoint(robot.Chassi.CenterOfMass.position);
                    robot.Chassi.Body.centerOfMass = localCenterOfMass;
                }

                if (robot.LastCollision != null) {
                    if (robot.LastCollision.gameObject.CompareTag("Death")) {
                        robot.CurrentHP = 0;
                    } else {
                        robot.CurrentHP -= robot.LastCollision.relativeVelocity.magnitude / 5;
                    }
                    
                    robot.LastCollision = null;                        
                }

                ComponentUtil.UpdateFlippers(robot);
                ComponentUtil.UpdateBlades(robot);
                ComponentUtil.UpdateAxes(robot);
                ComponentUtil.UpdateShields(robot);
                ComponentUtil.UpdateFlameThrower(robot);
            }
        }

        public static void FixedUpdateRobots(Robot[] robots, GameplayGUIMaster gui) {
            if (gui.CurrentState != GameplayGUIState.PLAYING) {
                return;
            }

            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];

                if (robot.CurrentHP <= 0) {
                    continue;
                }

                Vector3 velocity = robot.Chassi.Body.velocity;
                float drag = (robot.Chassi.Body.mass * 2f);
                robot.Chassi.Body.velocity = new Vector3(robot.Chassi.Body.velocity.x / drag, velocity.y, robot.Chassi.Body.velocity.z / drag);
                ComponentUtil.UpdateTracks(robot);
            }
        }
    }
}