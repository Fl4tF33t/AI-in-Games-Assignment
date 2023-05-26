using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class AgentController : MonoBehaviour
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
    }

    [SerializeField]
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

    //Agent movement and Logic
    private NavMeshAgent navMeshAgent;
    private GameObject[] collectablesArray;
    private int collectableIndex = 0;

    //subscribe to animation finished
    private CharacterVisuals characterVisuals;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterVisuals = GetComponentInChildren<CharacterVisuals>();
    }

    private void Start()
    {
        characterVisuals.OnAnimationCompleted += CharacterVisuals_OnAnimationCompleted;
        collectablesArray = GameObject.FindGameObjectsWithTag("Collectable");
        AgentState = State.Idle;
        FindNextCollectable();
    }

    private void FindNextCollectable()
    {
        //Check if there are any points to go to and sets the next points
        if(collectablesArray.Length == 0 || !CheckIfCanCollect(FindClosestCollectableIndex()))
        {
            AgentState = State.Idle;
            return;
        }

        navMeshAgent.destination = collectablesArray[FindClosestCollectableIndex()].transform.position;
        AgentState = State.Walking;
    }

    private void ArrivedToDestination()
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
                        //Checks if there is a collectable object and increase index to the next collectable
                        if (CheckIfCanCollect(collectableIndex))
                        {
                            AgentState = State.Collecting;
                            Destroy(collectablesArray[collectableIndex]);
                            
                        }
                        
                    }
                }
            }
        }
    }

    private int FindClosestCollectableIndex()
    {
        //Finding the distance of collectables and comparing, setting the index of the closest collectable
        float closestCollectableDistance = float.MaxValue;
        NavMeshPath path = new NavMeshPath();

        for (int i = 0; i < collectablesArray.Length; i++)
        {
            //skips the deleted GameObjects in the array
            if (collectablesArray[i] == null)
            {
                continue;
            }

            //adds the distance for the path using from vertex-vertex path.corners
            if (NavMesh.CalculatePath(transform.position, collectablesArray[i].transform.position, navMeshAgent.areaMask, path))
            {
                float distance = Vector3.Distance(transform.position, path.corners[0]);

                for (int cornerPoints = 1; cornerPoints < path.corners.Length; cornerPoints++)
                {
                    distance += Vector3.Distance(path.corners[cornerPoints - 1], path.corners[cornerPoints]);
                }

                if (distance < closestCollectableDistance)
                {
                    closestCollectableDistance = distance;
                    collectableIndex = i;
                }

            }
        }
        return collectableIndex;
    }

    //unity animation event after the crouch animation has finished
    private void CharacterVisuals_OnAnimationCompleted(object sender, EventArgs e)
    {
        //check if there is a next collectable target before starting the method
        if (CheckIfCanCollect(FindClosestCollectableIndex()))
        {
            FindNextCollectable();
        }
        else
        {
            AgentState = State.Idle;
        }
    }

    private bool CheckIfCanCollect(int collectableIndex)
    {
        return collectablesArray[collectableIndex] != null;   
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        ArrivedToDestination();
    }

   


}
