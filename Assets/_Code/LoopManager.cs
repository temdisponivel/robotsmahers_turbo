using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using UnityEditor;
using UnityEngine;

namespace RobotSmashers {
    public class LoopManager : MonoBehaviour {

        public InputData InputData;
        public Transform[] PlayerObjects;
        
        void Start() {
            InputData.Players = new GamepadState[2];
        }

        void Update() {
            InputUtil.UpdateInput(InputData);

            for (int i = 0; i < InputData.Players.Length; i++) {
                GamepadState set = InputData.Players[i];

                Transform playerTransform = PlayerObjects[i];

                playerTransform.position += (Vector3) set.LeftStickAxis * 10 * Time.deltaTime;
                playerTransform.localScale += (Vector3) set.RightStickAxis;
                playerTransform.position += Vector3.right * set.RightTrigger;
                playerTransform.position += Vector3.left * set.LeftTrigger;

                if (set.LeftShoulder) {
                    playerTransform.Rotate(Vector3.left * 30);
                }

                if (set.RightShoulder) {
                    playerTransform.Rotate(Vector3.right * 30);
                }

                if (set.A) {
                    playerTransform.transform.forward = Vector3.forward;
                }

                if (set.B) {
                    playerTransform.transform.forward = Vector3.right;
                }

                if (set.X) {
                    playerTransform.transform.forward = Vector3.left;
                }

                if (set.Y) {
                    playerTransform.transform.forward = Vector3.back;
                }

                if (set.Start) {
                    EditorApplication.isPlaying = false;
                }
            }
        }
    }
}