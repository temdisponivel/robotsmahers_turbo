using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RobotSmashers.Robots {
    
    [RequireComponent(typeof(Rigidbody))]
    public class RobotChassi : MonoBehaviour {
        public Transform ParentTransform;
        public Transform CenterOfMass;
        public Rigidbody Body;

        public ComponentSet Components;

        [ContextMenu("BAKE")]
        void Reset() {
            ParentTransform = transform;
            Body = GetComponent<Rigidbody>();

            Components.AllFlippers = GetComponentsInChildren<Flipper>();
            Components.AllTracks = GetComponentsInChildren<Track>();
            Components.AllBlades = GetComponentsInChildren<Blade>();
        }
    }
}