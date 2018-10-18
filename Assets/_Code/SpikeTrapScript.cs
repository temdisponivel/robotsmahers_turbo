using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using RobotSmashers.Robots;
using UnityEngine;
using UnityEngine.Assertions.Must;
using RobotSmashers;

public class SpikeTrapScript : MonoBehaviour
{
    public Animator Animation;
    public bool waiting;
    public float waitSeconds;
    public float damage;

    private void OnTriggerEnter(Collider collider)
    {
        Collider[] colliders = new Collider[] { collider };
        ComponentUtil.ApplyDamage(null, colliders, damage, false);
        if (waiting == false)
        {
            StartCoroutine(Wait(waitSeconds));
        }

    }

    private IEnumerator Wait(float seconds)
    {
        waiting = true;
        yield return new WaitForSeconds(.25f);
        Animation.CrossFade("Activate", 0);
        yield return new WaitForSeconds(seconds);        
        waiting = false;
    }
}
