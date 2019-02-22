using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class AITargetController : MonoBehaviour
{

    public enum AIState { WANDERING, CHASING, DEAD, ALERTING };
    public AIManager managerAI;
    

    public enum AREA { Callout, Hangout }
    public AREA area;

    private GameObject player;
    private AICharacterControl playerCC;

    private Animator animator;

    private List<Transform> currentPath;
    private int currentWaypoint = 0;

    private NavMeshAgent navMeshRef;
    private ThirdPersonCharacter tpCharacter;
    private AIState state = AIState.WANDERING;

    private GameObject currentTarget, lastTarget, newTarget;
    [SerializeField]
    private float distanceTreshold;

    private float deathTimeout = 2.0f;
    public int pathIterator;
    bool playerBehind;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCC = GetComponent<AICharacterControl>();
        tpCharacter = GetComponent<ThirdPersonCharacter>();
        navMeshRef = GetComponent<NavMeshAgent>();

        navMeshRef.speed = 0.5f;

        currentTarget = this.gameObject;
        animator = GetComponent<Animator>();

       // currentTarget = currentPath[0].gameObject;
       // newTarget = currentPath[0].gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AIState.WANDERING:
                navMeshRef.speed = 0.5f;


                if (newTarget != currentTarget)
                {
                    currentTarget = newTarget;
                    pathIterator++;
                }

                if (currentTarget != null)
                {
                    playerCC.target = currentTarget.transform;
                } else
                {

                }


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

            case AIState.ALERTING:

                break;

            case AIState.CHASING:

                navMeshRef.speed = 1;
                if (playerCC.target != player.transform)
                {
                    playerCC.target = player.transform;
                }

                if (!CanSeePlayer() && !playerBehind)
                {
                    state = AIState.WANDERING;
                }
                break;

            case AIState.DEAD:

                this.GetComponent<NavMeshAgent>().speed = 0;
                this.GetComponent<AICharacterControl>().enabled = false;
                animator.SetBool("isDying", true);

                Destroy(gameObject, deathTimeout);

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && area == AREA.Callout)
        {
            Debug.Log("Ariwe");
            if (!player.GetComponent<ThirdPersonCharacter>().m_Crouching)
            {
                state = AIState.CHASING;
                playerBehind = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerBehind = false;
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

        if (Vector3.Angle(transform.forward, playerPos - transform.position) <= 90)
        {
            LayerMask layerMask = LayerMask.NameToLayer("Zombie");

            RaycastHit hit;

            if (Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), playerPos - transform.position, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.tag.Equals("Player") && !hit.collider.GetComponent<ThirdPersonCharacter>().m_Crouching) 
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
    }


}
