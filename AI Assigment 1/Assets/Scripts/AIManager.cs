using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    public List<Transform> waypointCallout = new List<Transform>();
    public List<Transform> waypointHangout = new List<Transform>();
    public GameObject[] zombiesObj;
    public List<AITargetController> zombies;

    [SerializeField]
    private int callAreaLimit, hangAreaLimit; 

    // Start is called before the first frame update
    void Awake()
    {
        zombiesObj = GameObject.FindGameObjectsWithTag("Zombie");

        foreach (GameObject obj in zombiesObj)
        {
            AITargetController zombie = obj.GetComponent<AITargetController>();
            if (zombie == null)
            {
                Debug.Log("no zombie");
            }
            zombies.Add(zombie);
        }

        GameObject[] wpPool = GameObject.FindGameObjectsWithTag("Waypoint");
        
        foreach (GameObject obj in wpPool) {
            Debug.Log(obj.name);
            if (obj.GetComponent<Waypoint>().area == Waypoint.AREATYPE.Callout)
            {
                waypointCallout.Add(obj.transform);
            } else if (obj.GetComponent<Waypoint>().area == Waypoint.AREATYPE.Hangout)
            {
                waypointHangout.Add(obj.transform);
            } else
            {
                continue;
            }
        }

        //zombies[0].GetComponent<AITargetController>().SetPath(waypointCallout);

        for (int i = 0; i < zombies.Count; i++)
        {
            if (i < callAreaLimit)
            {
                zombies[i].SetPath(waypointCallout);
                zombies[i].SetDirection(waypointCallout[i].gameObject);
                zombies[i].pathIterator = i;
                zombies[i].area = AITargetController.AREA.Callout;

            } else
            {
                zombies[i].SetPath(waypointHangout);
                zombies[i].SetDirection(waypointHangout[i].gameObject);
                zombies[i].pathIterator = Random.Range(2, waypointHangout.Count);
                zombies[i].area = AITargetController.AREA.Hangout;
            }
        }
    }

}
