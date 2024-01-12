using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentNavigation : MonoBehaviour
{
    [SerializeField]
    private Vector3 desiredDestination;

     void Start()
    {
        GetComponent<NavMeshAgent>().destination = desiredDestination;
    }

}
