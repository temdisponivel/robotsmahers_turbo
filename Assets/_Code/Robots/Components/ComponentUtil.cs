using System;
using System.Collections.Generic;
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
        public Shield[] AllShields;
    }

    public static class ComponentUtil {
        public static Collider[] GetRobotCollidersInArea(Vector3 position, float radius) {
            Collider[] colliders = Physics.OverlapSphere(position, radius, Constants.ROBOT_LAYER);
            return colliders;
        }
        
        public static List<Robot> ApplyDamage(Robot attacker, Collider[] robotColliders, float damageAmount) {
            List<Robot> processedRobots = new List<Robot>();
            
            for (int i = 0; i < robotColliders.Length; i++) {
                Collider collider = robotColliders[i];
                Robot enemy = collider.GetComponentInParent<Robot>();
                if (enemy == attacker) {
                    continue;
                }

                if (processedRobots.Contains(enemy)) {
                    continue;
                }

                processedRobots.Add(enemy);

                float damageToApply = damageAmount;
                if (collider.gameObject.CompareTag(Constants.SHIELD_TAG)) {
                    Shield shield = collider.gameObject.GetComponent<Shield>();

                    if (shield.CurrentShieldAmount > 0) {
                        float remaining = shield.CurrentShieldAmount - damageToApply;
                        if (remaining < 0) {
                            shield.CurrentShieldAmount = 0;
                            damageToApply = Mathf.Abs(remaining);
                        } else {
                            shield.CurrentShieldAmount -= damageToApply;
                            damageToApply = 0;
                        }
                    }
                }
                
                enemy.CurrentHP -= damageToApply;
            }

            return processedRobots;
        }
        
        public static void UpdateFlippers(Robot robot) {
            RobotChassi chassi = robot.Chassi;
            GamePad.Index playerIndex = robot.ControllingPlayer;

            for (int i = 0; i < chassi.Components.AllFlippers.Length; i++) {
                Flipper flipper = chassi.Components.AllFlippers[i];

                if (GamePad.GetButtonDown(flipper.UseButton, playerIndex)) {
                    flipper.DrawActivationGizmos = true;

                    Collider[] colliders = GetRobotCollidersInArea(flipper.transform.position, flipper.Radius);
                    List<Robot> damagedEnemies = ApplyDamage(robot, colliders, flipper.Force / 10);

                    for (int j = 0; j < damagedEnemies.Count; j++) {
                        Robot enemy = damagedEnemies[j];
                        enemy.Chassi.Body.AddForceAtPosition(flipper.transform.forward * flipper.Force, flipper.transform.position, flipper.ForceMode);
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

                if (track.GroundCollisions > 0) {
#if PHYSICS_MOVEMENT
                    Vector3 torque = chassi.ParentTransform.up * track.Torque * leftStick.x;
                    Vector3 force = chassi.ParentTransform.forward * track.Force * rightStick.y;
                    ownBody.AddRelativeTorque(torque, track.TorqueMode);
                    ownBody.AddForce(force, track.ForceMode);
#else
                    robot.Chassi.ParentTransform.forward = Vector3.Lerp(robot.Chassi.ParentTransform.forward, new Vector3(leftStick.x, 0, leftStick.y), track.Torque * Time.deltaTime);
                    float multiplier = (track.Force * Time.deltaTime * rightStick.y) / ownBody.mass;
                    robot.Chassi.ParentTransform.position += robot.Chassi.ParentTransform.forward * multiplier;
#endif
                }
            }
        }

        public static void UpdateBlades(Robot robot) {
            RobotChassi chassi = robot.Chassi;

            for (int i = 0; i < chassi.Components.AllBlades.Length; i++) {
                Blade blade = chassi.Components.AllBlades[i];
                
                List<Robot> damagedEnemies = ApplyDamage(robot, blade.CurrentCollisions.ToArray(), blade.DamagePerSecond * Time.deltaTime);
                
                for (int j = 0; j < damagedEnemies.Count; j++) {
                    Robot enemy = damagedEnemies[j];
                    
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

                    Collider[] colliders = GetRobotCollidersInArea(axe.transform.position, axe.Radius);
                    
                    List<Robot> damagedEnemies = ApplyDamage(robot, colliders, axe.Damage);
                    
                    for (int j = 0; j < damagedEnemies.Count; j++) {
                        Robot enemy = damagedEnemies[j];

                        if (enemy == robot) {
                            continue;
                        }

                        Rigidbody enemyBody = enemy.Chassi.Body;
                        enemyBody.AddForceAtPosition(-axe.transform.up * axe.PhysicsForce, axe.transform.position, ForceMode.Impulse);
                    }
                }
            }
        }

        public static void UpdateShields(Robot robot) {
            RobotChassi chassi = robot.Chassi;
            for (int i = 0; i < chassi.Components.AllShields.Length; i++) {
                Shield shield = chassi.Components.AllShields[i];
                if (shield.CurrentShieldAmount <= 0 && shield.Joint) {
                    UnityEngine.Object.Destroy(shield.Joint);
                    shield.transform.parent = null;
                    shield.gameObject.layer = LayerMask.NameToLayer(Constants.GROUND_LAYER_NAME);
                }
            }
        }
    }
}