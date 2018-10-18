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

    private void OnTriggerEnter(Collider collider)
    {
        if (waiting == false)
        {
            StartCoroutine(Wait(waitSeconds));
        }

    }

    private void OnTriggerStay(Collider collider)
    {
        Collider[] colliders = new Collider[] { collider };
        ComponentUtil.ApplyDamage(null, colliders, damagePerSecond * Time.deltaTime, false);
    }

    private IEnumerator Wait(float seconds)
    {
        flames.Play(true);
        waiting = true;
        yield return new WaitForSeconds(seconds);
        flames.Stop(true);
        waiting = false;
    }
}
