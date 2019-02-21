using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    public List<Transform> waypointCallout = new List<Transform>();
    public List<Transform> waypointHangout = new List<Transform>();
    public GameObject[] zombies;

    // Start is called before the first frame update
    void Start()
    {
        zombies = GameObject.FindGameObjectsWithTag("Zombie");

        GameObject wpParent = GameObject.FindGameObjectWithTag("WaypointsPool");
        foreach (Transform child in wpParent.transform)
        {
            if (child.GetComponent<Waypoint>().area == Waypoint.AREATYPE.Callout)
            {
                waypointCallout.Add(child);
            } else if (child.GetComponent<Waypoint>().area == Waypoint.AREATYPE.Hangout)
            {
                waypointHangout.Add(child);
            } else
            {
                continue;
            }
        }

        zombies[0].GetComponent<AITargetController>().SetPath(waypointCallout);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
