using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAgent : MonoBehaviour, IDamage
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
        WoodChopping,
    }
    
    private State agentState;
    protected State AgentState
    {
        get
        {
            return agentState;
        }
        set
        {
            agentState = value;
            OnAgentState?.Invoke(this, new OnAgentStateEventArgs
            {
                agentState = value
            });
        }
    }

    protected NavMeshAgent navMeshAgent;
    protected CharacterVisuals characterVisuals;
    protected GameObject[] targetArray;
    protected int targetIndex;

    public int health = 100;

    public void FindTargetsAndSetIndex(string tag, int index)
    {
        targetArray = GameObject.FindGameObjectsWithTag(tag);
        targetIndex = index;
    }

    public void DamageTaken(int damage)
    {
        health -= damage;
    }

    public bool ArrivedToDestination()
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

    private void Update()
    {
        
    }
}
