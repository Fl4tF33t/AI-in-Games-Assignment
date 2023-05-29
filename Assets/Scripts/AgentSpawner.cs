using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{


    [SerializeField]
    private GameObject agentPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnAgent", 1, 20);
    }

    private void SpawnAgent()
    {
        Instantiate(agentPrefab, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
