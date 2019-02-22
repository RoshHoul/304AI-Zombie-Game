using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{

    Animator animator;
    private HitEnemy enemyScript;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyScript = GetComponentInChildren<HitEnemy>();
        if (enemyScript != null)
        {
            enemyScript.isKicking = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool kicking = Input.GetMouseButton(0);
        animator.SetBool("isKicking", kicking);
        if (enemyScript != null)
        {
            enemyScript.isKicking = kicking;
        }

    }
}
