using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class AITargetController : MonoBehaviour
{

    public enum AIState { WANDERING, CHASING, DEAD };

    public enum AREA { Callout, Hangout }
    public AREA area;

    private GameObject player;
    private AICharacterControl playerCC;
    
    private List<Transform> currentPath;
    private int currentWaypoint = 0;

    private ThirdPersonCharacter tpCharacter;
    private AIState state = AIState.WANDERING;

    private GameObject currentTarget, lastTarget, newTarget;
    [SerializeField]
    private float distanceTreshold;

    private float deathTimeout = 2.0f;
    private int pathIterator = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCC = GetComponent<AICharacterControl>();
        tpCharacter = GetComponent<ThirdPersonCharacter>();

        currentTarget = currentPath[0].gameObject;
        newTarget = currentPath[0].gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AIState.WANDERING:

                if (newTarget != currentTarget)
                {
                    currentTarget = newTarget;
                    pathIterator++;
                }

                playerCC.target = currentTarget.transform;

                if (IsDestinationReached())
                {
                    if (pathIterator >= currentPath.Count)
                        pathIterator = 0;

                    SetDirection(currentPath[pathIterator].gameObject);
                }

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

                this.GetComponent<NavMeshAgent>().enabled = false;
                this.GetComponent<AICharacterControl>().enabled = false;

                Destroy(gameObject, deathTimeout);

                break;
        }
    }

    private bool IsDestinationReached()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        if(distanceToTarget < distanceTreshold)
        {
            return true;
        }

        return false;
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

    public void SetDirection(GameObject dir) 
    {
        newTarget = dir;
    }

    public void SetPath(List<Transform> path)
    {
        currentPath = path;
        SetDirection(currentPath[Random.Range(0, currentPath.Count)].gameObject);
    }


}
