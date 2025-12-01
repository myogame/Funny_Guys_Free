using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BOTScript : MonoBehaviour
{
    [SerializeField] 
    private Transform movePositionTransfrom;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.destination = movePositionTransfrom.position;
    }
}
