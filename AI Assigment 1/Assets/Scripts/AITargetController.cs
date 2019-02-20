using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class AITargetController : MonoBehaviour
{

    public enum AIState { WANDERING, CHASING, DEAD };

    private GameObject player;
    private AICharacterControl playerCC;
    
    private GameObject[] allWaypoints;
    private int currentWaypoint = 0;
    private ThirdPersonCharacter tpCharacter;
    private AIState state = AIState.WANDERING;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCC = GetComponent<AICharacterControl>();
        tpCharacter = GetComponent<ThirdPersonCharacter>();

        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        System.Random rnd = new System.Random(System.DateTime.Now.Millisecond);
        allWaypoints = allWaypoints.OrderBy(x => rnd.Next()).ToArray();

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AIState.WANDERING:

                if (CanSeePlayer())
                {
                    state = AIState.CHASING;
                }
                break;
            case AIState.CHASING:

                if (playerCC.target != player.transform)
                {
                    playerCC.target = player.transform;
                }

                if (!CanSeePlayer())
                {
                    state = AIState.WANDERING;
                }
                break;

            case AIState.DEAD:

                break;
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 playerPos = player.transform.position;

        if (Vector3.Angle(transform.forward, playerPos - transform.position) <= 45)
        {
            LayerMask layerMask = LayerMask.NameToLayer("Zombie");

            RaycastHit hit;

            if (Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), playerPos - transform.position, out hit, Mathf.Infinity, layerMask))
            {
                return (hit.collider.tag.Equals("Player"));
            }
        }
        return false;
    }

    public void kill()
    {
        this.state = AIState.DEAD;
    }

    public AIState getState()
    {
        return state;
    }


}
