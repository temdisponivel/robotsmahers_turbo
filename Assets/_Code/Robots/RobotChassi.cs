using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RobotSmashers.Robots {
    
    [RequireComponent(typeof(Rigidbody))]
    public class RobotChassi : MonoBehaviour {
        public Transform ParentTransform;
        public Transform CenterOfMass;
        public Rigidbody Body;

        public ComponentSlot[] AllSlots;
        
        public ComponentSet Components;

        [ContextMenu("BAKE")]
        void Reset() {
            ParentTransform = transform;
            
            AllSlots = GetComponentsInChildren<ComponentSlot>();
            Body = GetComponent<Rigidbody>();
            
            // TODO: This should be called after the components have been spawned on their slots!
            Components.AllFlippers = GetComponentsInChildren<Flipper>();
            ComponentUtil.BindSlotToComponent(Components.AllFlippers, ComponentType.FLIPPER);
            
            Components.AllTracks = GetComponentsInChildren<Track>();
            ComponentUtil.BindSlotToComponent(Components.AllTracks, ComponentType.TRACK);
            
            Components.AllBlades = GetComponentsInChildren<Blade>();
            ComponentUtil.BindSlotToComponent(Components.AllBlades, ComponentType.BLADE);
            
            Components.AllAxes = GetComponentsInChildren<Axe>();
            ComponentUtil.BindSlotToComponent(Components.AllAxes, ComponentType.AXE);
            
            Components.AllShields = GetComponentsInChildren<Shield>();
            ComponentUtil.BindSlotToComponent(Components.AllShields, ComponentType.SHIELD);
            
            Components.AllFlameThrowers = GetComponentsInChildren<FlameThrower>();
            ComponentUtil.BindSlotToComponent(Components.AllFlameThrowers, ComponentType.FLAME_THROWER);
        }
    }
}