using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    public bool isKicking;

    private void OnTriggerEnter(Collider other)
    {
        if (isKicking && other.tag == "Zombie")
        {
            other.GetComponent<AITargetController>().kill();
        }
    }
}
