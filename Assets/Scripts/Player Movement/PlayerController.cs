using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Go to destination with pathfinding
    public void GoToDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
    }
}
