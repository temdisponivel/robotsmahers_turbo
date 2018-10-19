using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using RobotSmashers.Robots;
using UnityEngine;
using UnityEngine.Assertions.Must;
using RobotSmashers;

public class FlameTrapScript : MonoBehaviour
{

    public Animator Controller;
    public bool waiting;
    public ParticleSystem flames;
    public float waitSeconds;
    public float damagePerSecond;
    public AudioSource flameSounds;

    private void OnTriggerStay(Collider collider)
    {
        if (waiting) {
            Collider[] colliders = new Collider[] { collider };
            ComponentUtil.ApplyDamage(null, colliders, damagePerSecond * Time.deltaTime, false);            
        } else {
            StartCoroutine(Wait(waitSeconds));
        }
    }

    private IEnumerator Wait(float seconds)
    {
        flames.Play(true);
        flameSounds.Play();
        waiting = true;
        yield return new WaitForSeconds(seconds);
        flames.Stop(true);
        flameSounds.Stop();
        waiting = false;
    }
}
