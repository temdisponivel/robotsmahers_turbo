using System;
using GamepadInput;
using RobotSmashers.Robots;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace RobotSmashers {
    [Serializable]
    public class ComponentSet {
        public Flipper[] AllFlippers;
        public Track[] AllTracks;
        public Blade[] AllBlades;
    }

    public static class ComponentUtil {
        
        public static void UpdateFlippers(Robot robot) {
            RobotChassi chassi = robot.Chassi;
            GamePad.Index playerIndex = robot.ControllingPlayer;

            Rigidbody ownBody = robot.Chassi.Body;
            
            for (int i = 0; i < chassi.Components.AllFlippers.Length; i++) {
                Flipper flipper = chassi.Components.AllFlippers[i];

                if (GamePad.GetButtonDown(flipper.UseButton, playerIndex)) {
                    flipper.DrawActivationGizmos = true;
                    
                    Collider[] colliders = Physics.OverlapSphere(flipper.transform.position, .5f);

                    for (int j = 0; j < colliders.Length; j++) {
                        Collider collider = colliders[j];

                        if (collider.attachedRigidbody != null && collider.attachedRigidbody != ownBody) {
                            collider.attachedRigidbody.AddForceAtPosition(flipper.transform.forward * flipper.Force, flipper.transform.position, flipper.ForceMode);
                            Robot enemy = collider.gameObject.GetComponentInParent<Robot>();
                            enemy.CurrentHP -= flipper.Force / 10;
                        }
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
                    Vector3 torque = chassi.ParentTransform.up * track.Torque * leftStick.x;
                    Vector3 force = chassi.ParentTransform.forward * track.Force * rightStick.y;
                    ownBody.AddRelativeTorque(torque, track.TorqueMode);
                    ownBody.AddForce(force, track.ForceMode);
                }
            }
        }

        public static void UpdateBlades(Robot robot) {
            RobotChassi chassi = robot.Chassi;
            
            for (int i = 0; i < chassi.Components.AllBlades.Length; i++) {
                Blade blade = chassi.Components.AllBlades[i];

                for (int j = 0; j < blade.CurrentCollisions.Count; j++) {
                    Collision collision = blade.CurrentCollisions[j];
                    if ((1 << collision.collider.gameObject.layer & Constants.ROBOT_LAYER) != 0) {
                        Robot enemy = collision.collider.gameObject.GetComponentInParent<Robot>();

                        enemy.CurrentHP -= blade.DamagePerSecond * Time.deltaTime;
                    }
                }
            }
        }
    }
}