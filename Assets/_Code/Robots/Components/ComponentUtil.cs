#define PHYSICS_MOVEMENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GamepadInput;
using RobotSmashers.Robots;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace RobotSmashers {
    public enum ComponentType {
        FLIPPER,
        TRACK,
        BLADE,
        AXE,
        SHIELD,
        FLAME_THROWER
    }

    [Serializable]
    public class ComponentSet {
        public Flipper[] AllFlippers;
        public Track[] AllTracks;
        public Blade[] AllBlades;
        public Axe[] AllAxes;
        public Shield[] AllShields;
        public FlameThrower[] AllFlameThrowers;
    }

    public static class ComponentUtil {
        public static void BindSlotToComponent<T>(T[] components, ComponentType type) where T : MonoBehaviour {
            for (int i = 0; i < components.Length; i++) {
                T component = components[i];
                ComponentSlot slot = component.gameObject.GetComponentInParent<ComponentSlot>();
                if (slot == null) {
                    slot = component.transform.parent.gameObject.AddComponent<ComponentSlot>();
                }

                Debug.Assert(slot != null, "A component must always be children of a ComponentSlot!");
                slot.Component = type;

                FieldInfo fieldInfo = typeof(T).GetField("UseButton");

                if (fieldInfo != null) {
                    fieldInfo.SetValue(component, slot.UseButton);
                }
            }
        }

        public static void ValidateCollisionEnter(Collider collider, List<Collider> colliderList) {
            if ((1 << collider.gameObject.layer & Constants.ROBOT_LAYER) != Constants.ROBOT_LAYER) {
                return;
            }

            for (int i = 0; i < colliderList.Count; i++) {
                if (colliderList[i] == collider) {
                    return;
                }
            }

            colliderList.Add(collider);
        }

        public static void ValidateCollisionExit(Collider collider, List<Collider> colliderList) {
            for (int i = 0; i < colliderList.Count; i++) {
                if (colliderList[i] == collider) {
                    colliderList.RemoveAt(i--);
                }
            }
        }

        public static Collider[] GetRobotCollidersInArea(Vector3 position, float radius) {
            Collider[] colliders = Physics.OverlapSphere(position, radius, Constants.ROBOT_LAYER);
            return colliders;
        }

        public static List<Robot> ApplyDamage(Robot attacker, Collider[] robotColliders, float damageAmount, bool canBeBlockedByShield = true) {
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

                if (canBeBlockedByShield) {
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

                flipper.CurrentCooldownTime -= Time.deltaTime;
                if (flipper.CurrentCooldownTime <= 0) {
                    if (GamePad.GetButtonDown(flipper.UseButton, playerIndex)) {
                        flipper.DrawActivationGizmos = true;

                        Collider[] colliders = GetRobotCollidersInArea(flipper.transform.position, flipper.Radius);
                        ApplyDamage(robot, colliders, flipper.Force / 10);
                        flipper.Animator.SetTrigger("Flip");
                        flipper.CurrentCooldownTime = flipper.DefaultCooldownTime;
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

            int notTouchingGroundCount = 0;
            for (int i = 0; i < chassi.Components.AllTracks.Length; i++) {
                Track track = chassi.Components.AllTracks[i];

                if (track.GroundCollisions > 0) {
#if PHYSICS_MOVEMENT
                    Vector3 torque = chassi.ParentTransform.up * track.Torque * leftStick.x;
                    Vector3 force = chassi.ParentTransform.forward * track.Force * rightStick.y;
                    ownBody.AddRelativeTorque(torque, track.TorqueMode);
                    ownBody.AddForce(force, track.ForceMode);
#else
                    robot.Chassi.ParentTransform.forward = Vector3.Lerp(robot.Chassi.ParentTransform.forward, new Vector3(rightStick.x, 0, rightStick.y), track.Torque * Time.deltaTime);
                    float multiplier = (track.Force * Time.deltaTime * leftStick.y) / ownBody.mass;
                    robot.Chassi.ParentTransform.position += robot.Chassi.ParentTransform.forward * multiplier;
#endif
                } else {
                    notTouchingGroundCount++;
                }
            }

            if (notTouchingGroundCount < chassi.Components.AllTracks.Length) {
                chassi.NotTochingGroundTimer = 0;
            } else {
                chassi.NotTochingGroundTimer += Time.deltaTime;
                if (chassi.NotTochingGroundTimer >= 10) {
                    robot.CurrentHP = 0;
                }
            }
        }

        public static void UpdateBlades(Robot robot) {
            RobotChassi chassi = robot.Chassi;
            GamePad.Index playerIndex = robot.ControllingPlayer;

            for (int i = 0; i < chassi.Components.AllBlades.Length; i++) {
                Blade blade = chassi.Components.AllBlades[i];

                if (GamePad.GetButton(blade.UseButton, playerIndex)) {
                    blade.Animator.SetBool("On", true);
                    List<Robot> damagedEnemies = ApplyDamage(robot, blade.CurrentCollisions.ToArray(), blade.DamagePerSecond * Time.deltaTime);

                    for (int j = 0; j < damagedEnemies.Count; j++) {
                        Robot enemy = damagedEnemies[j];

                        Rigidbody enemyBody = enemy.Chassi.Body;
                        Vector3 force = (enemy.Chassi.transform.position - chassi.transform.position) * blade.PhysicsForce;
                        enemyBody.AddForceAtPosition(force, blade.transform.position, ForceMode.Impulse);
                    }
                } else {
                    blade.Animator.SetBool("On", false);
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
                        Vector3 dir = Vector3.Lerp(-axe.transform.up, axe.transform.forward, .5f);
                        enemyBody.AddForceAtPosition(dir * axe.PhysicsForce, axe.transform.position, ForceMode.Impulse);
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

        public static void UpdateFlameThrower(Robot robot) {
            RobotChassi chassi = robot.Chassi;
            GamePad.Index index = robot.ControllingPlayer;

            for (int i = 0; i < chassi.Components.AllFlameThrowers.Length; i++) {
                FlameThrower flameThrower = chassi.Components.AllFlameThrowers[i];
                flameThrower.FireRenderer.enabled = false;
                
                if (flameThrower.CurrentFuel <= 0) {
                    float totalFlamethrower = flameThrower.CurrentFuel + flameThrower.RechargePerSecond * Time.deltaTime;
                    flameThrower.CurrentFuel = Mathf.Min(flameThrower.DefaultFuel, totalFlamethrower);
                } else {
                    if (GamePad.GetButton(flameThrower.UseButton, index)) {
                        flameThrower.CurrentFuel -= flameThrower.FuelPerSecond * Time.deltaTime;
                        flameThrower.FireRenderer.enabled = true;

                        ApplyDamage(robot, flameThrower.CurrentColliders.ToArray(), flameThrower.DamagePerSecond * Time.deltaTime);
                    }
                }
            }
        }
    }
}