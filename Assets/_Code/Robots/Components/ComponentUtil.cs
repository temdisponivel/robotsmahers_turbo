using System;
using GamepadInput;
using RobotSmashers.Robots;
using UnityEngine;

namespace RobotSmashers {
    [Serializable]
    public class ComponentSet {
        public Flipper[] AllFlippers;
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
                            collider.attachedRigidbody.AddForceAtPosition(flipper.transform.forward * flipper.Force, flipper.transform.position);                            
                        }
                    }
                    
                    ownBody.AddForceAtPosition(-(flipper.transform.forward * flipper.Force), flipper.transform.position);
                }
            }
        }
    }
}