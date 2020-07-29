using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public NavMeshAgent agent;
    public GameObject police;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(police.transform.position);
    }
}
