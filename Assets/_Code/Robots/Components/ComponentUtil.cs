using System;
using GamepadInput;
using RobotSmashers.Robots;
using UnityEngine;
using UnityEngine.Assertions.Must;

//#define PHYSICS_MOVEMENT

namespace RobotSmashers {
    [Serializable]
    public class ComponentSet {
        public Flipper[] AllFlippers;
        public Track[] AllTracks;
        public Blade[] AllBlades;
        public Axe[] AllAxes;
    }

    public static class ComponentUtil {
        public static void UpdateFlippers(Robot robot) {
            RobotChassi chassi = robot.Chassi;
            GamePad.Index playerIndex = robot.ControllingPlayer;

            for (int i = 0; i < chassi.Components.AllFlippers.Length; i++) {
                Flipper flipper = chassi.Components.AllFlippers[i];

                if (GamePad.GetButtonDown(flipper.UseButton, playerIndex)) {
                    flipper.DrawActivationGizmos = true;

                    Collider[] colliders = Physics.OverlapSphere(flipper.transform.position, .5f, Constants.ROBOT_LAYER);

                    for (int j = 0; j < colliders.Length; j++) {
                        Collider collider = colliders[j];
                        Robot enemy = collider.gameObject.GetComponentInParent<Robot>();

                        if (enemy == robot) {
                            continue;
                        }

                        enemy.Chassi.Body.AddForceAtPosition(flipper.transform.forward * flipper.Force, flipper.transform.position, flipper.ForceMode);
                        enemy.CurrentHP -= flipper.Force / 10;
                    }
                }
            }
        }

        public static void UpdateTracks(Robot robot) {
            RobotChassi chassi = robot.Chassi;
            GamePad.Index playerIndex = robot.ControllingPlayer;
            Vector2 leftStick = GamePad.GetAxis(GamePad.Axis.LeftStick, playerIndex);
            Vector2 rightStick = GamePad.GetAxis(GamePad.Axis.RightStick, playerIndex);

            Rigidbody ownBody = robot.Chassi.Body;

            for (int i = 0; i < chassi.Components.AllTracks.Length; i++) {
                Track track = chassi.Components.AllTracks[i];

                if (track.Grounded) {
#if PHYSICS_MOVEMENT
                    Vector3 torque = chassi.ParentTransform.up * track.Torque * leftStick.x;
                    Vector3 force = chassi.ParentTransform.forward * track.Force * rightStick.y;
                    ownBody.AddRelativeTorque(torque, track.TorqueMode);
                    ownBody.AddForce(force, track.ForceMode);
#else
                    robot.Chassi.ParentTransform.forward = Vector3.Lerp(robot.Chassi.ParentTransform.forward, new Vector3(leftStick.x, 0, leftStick.y), track.Torque * Time.deltaTime);
                    robot.Chassi.ParentTransform.position += robot.Chassi.ParentTransform.forward * track.Force * Time.deltaTime * rightStick.y;
#endif
                }
            }
        }

        public static void UpdateBlades(Robot robot) {
            RobotChassi chassi = robot.Chassi;

            for (int i = 0; i < chassi.Components.AllBlades.Length; i++) {
                Blade blade = chassi.Components.AllBlades[i];

                for (int j = 0; j < blade.CurrentCollisions.Count; j++) {
                    Collision collision = blade.CurrentCollisions[j];
                    Robot enemy = collision.collider.gameObject.GetComponentInParent<Robot>();

                    if (enemy == robot) {
                        continue;
                    }

                    enemy.CurrentHP -= blade.DamagePerSecond * Time.deltaTime;
                    Rigidbody enemyBody = enemy.Chassi.Body;

                    Vector3 force = (enemy.Chassi.transform.position - chassi.transform.position) * blade.PhysicsForce;
                    enemyBody.AddForceAtPosition(force, blade.transform.position, ForceMode.Impulse);
                }
            }
        }

        public static void UpdateAxes(Robot robot) {
            RobotChassi chassi = robot.Chassi;
            GamePad.Index playerIndex = robot.ControllingPlayer;

            for (int i = 0; i < chassi.Components.AllAxes.Length; i++) {
                Axe axe = chassi.Components.AllAxes[i];

                if (GamePad.GetButtonDown(axe.UseButton, playerIndex)) {
                    // TODO: Play animation, and the animation will set AttackNow to true
                    axe.AttackNow = true;
                }
            }

            for (int i = 0; i < chassi.Components.AllAxes.Length; i++) {
                Axe axe = chassi.Components.AllAxes[i];

                if (axe.AttackNow) {
                    axe.AttackNow = false;

                    Collider[] colliders = Physics.OverlapSphere(axe.transform.position, axe.Radius, Constants.ROBOT_LAYER);
                    for (int j = 0; j < colliders.Length; j++) {
                        Collider collider = colliders[j];
                        Robot enemy = collider.gameObject.GetComponentInParent<Robot>();

                        if (enemy == robot) {
                            continue;
                        }

                        Rigidbody enemyBody = enemy.Chassi.Body;
                        enemyBody.AddForceAtPosition(-axe.transform.up * axe.PhysicsForce, axe.transform.position, ForceMode.Impulse);
                        enemy.CurrentHP -= axe.Damage;
                    }
                }
            }
        }
    }
}