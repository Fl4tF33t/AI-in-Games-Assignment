using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class WoodCutterAgent : MonoBehaviour
{
    public event EventHandler<OnAgentStateEventArgs> OnAgentState;
    public class OnAgentStateEventArgs : EventArgs
    {
        public State agentState;
    }

    public enum State
    {
        Idle,
        Walking,
        Collecting,
        Chopping,
    }

    private State agentState;
    public State AgentState
    {
        get
        {
            return agentState;
        }
        private set
        {
            agentState = value;
            OnAgentState?.Invoke(this, new OnAgentStateEventArgs
            {
                agentState = value
            });
        }
    }

    NavMeshAgent navMeshAgent;
    GameObject[] destination;
    int randomIndex;
    WoodCutterAgentVisuals woodCutterVisuals;
    Tree tree;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        woodCutterVisuals = GetComponentInChildren<WoodCutterAgentVisuals>();
    }

    // Start is called before the first frame update
    void Start()
    {
        woodCutterVisuals.OnAnimationCompleted += WoodCutterVisuals_OnAnimationCompleted;
        destination = GameObject.FindGameObjectsWithTag("Tree");
        randomIndex = UnityEngine.Random.Range(0, destination.Length);
        navMeshAgent.destination = destination[randomIndex].transform.position;
        tree = destination[randomIndex].GetComponent<Tree>();
        AgentState = State.Walking;
    }

    private void WoodCutterVisuals_OnAnimationCompleted(object sender, EventArgs e)
    {
        tree.TakeDamage(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (ArrivedToDestination())
        {
            AgentState = State.Chopping;
        }
    }

    private bool ArrivedToDestination()
    {
        //Checks that the agent has arrrived, sends a notification that it is idle
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    if (AgentState == State.Walking)
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        else return false;
    }
}
