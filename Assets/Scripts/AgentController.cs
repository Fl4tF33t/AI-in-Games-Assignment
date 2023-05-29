using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.UIElements;

public class AgentController : BaseAgent
{
    //Agent movement and Logic
    private NavMeshPath path;

    //Box moving logic
    [SerializeField]
    private GameObject moveableBox;
    [SerializeField]
    private GameObject moveableBoxTarget;
    [SerializeField]
    private GameObject navMeshLink; 

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterVisuals = GetComponentInChildren<CharacterVisuals>();
        path = new NavMeshPath();
    }

    private void Start()
    {
        characterVisuals.OnAnimationCompleted += CharacterVisuals_OnAnimationCompleted;
        FindTargetsAndSetIndex("Collectable", 0);
        AgentState = State.Idle;
        FindNextCollectable();      
    }

    private void FindNextCollectable()
    {
        //Check if there are any points to go to 
        if(targetArray.Length == 0)
        {
            AgentState = State.Idle;
            return;
        }

        //Checks for the pressence of a collectable and validates the path that has been created
        if (CheckIfCanCollect(FindClosestCollectableIndex()))
        {
            if (CheckIfPathIsValid(FindClosestCollectableIndex()))
            {
                Debug.Log("Closest collectable in range being collected");
                navMeshAgent.destination = targetArray[FindClosestCollectableIndex()].transform.position;
                AgentState = State.Walking;
            }
            else
            {
                if (CheckIfCanCollect(CalculateNextValidPath()))
                {
                    if (CheckIfPathIsValid(CalculateNextValidPath()))
                    {
                        Debug.Log("There is a valid path to take for the next closest collectable");
                        navMeshAgent.destination = targetArray[CalculateNextValidPath()].transform.position;
                        AgentState = State.Walking;
                    }
                    else
                    {
                        Debug.Log("Collectable present with no vaalid path, perform something else");
                        NextCollectableInNonValidPath();
                    }
                }
                else
                {
                    Debug.Log("No valid paths, cause there are no collectables");
                    AgentState = State.Idle;
                }           
            }
        }
        else
        {
            Debug.Log("No valid paths, cause there are no collectable");
            AgentState = State.Idle;
        }
            
    }

    private void NextCollectableInNonValidPath()
    {
        if (moveableBox == null || moveableBoxTarget == null)
        {
            AgentState = State.Idle;
            return;
        }
        else
        {
            AgentState = State.Walking;
            navMeshAgent.destination = moveableBox.transform.position - FindMoveableLocationNormalized();
        }
    }
    

    private Vector3 FindMoveableLocationNormalized()
    {
        Vector3 moveableBoxPos = moveableBox.transform.position;
        Vector3 moveableBoxTargetPos = moveableBoxTarget.transform.position;
        Vector3 direction = moveableBoxTargetPos - moveableBoxPos;

        return direction.normalized;
    }

    private void MovingMoveable()
    {
        MoveableBox moveableBoxScript = moveableBox.GetComponent<MoveableBox>();
        if (moveableBoxScript != null)
        {
            moveableBoxScript.Target((FindMoveableLocationNormalized()));
            moveableBoxScript.OnTriggerReached += MoveableBoxScript_OnTriggerReached;
        }
    }

    private void MoveableBoxScript_OnTriggerReached(object sender, EventArgs e)
    {
        navMeshAgent.areaMask = 5;
        navMeshLink.gameObject.SetActive(true);
        FindNextCollectable();
    }

    private int FindClosestCollectableIndex()
    {
        //Finding the distance of collectables and comparing, setting the index of the closest collectable
        float closestCollectableDistance = float.MaxValue;

        for (int i = 0; i < targetArray.Length; i++)
        {
            //skips the deleted GameObjects in the array
            if (targetArray[i] == null)
            {
                continue;
            }

            //adds the distance for the path using from vertex-vertex path.corners
            if (NavMesh.CalculatePath(transform.position, targetArray[i].transform.position, navMeshAgent.areaMask, path))
            {
                float distance = Vector3.Distance(transform.position, path.corners[0]);

                for (int cornerPoints = 1; cornerPoints < path.corners.Length; cornerPoints++)
                {
                    distance += Vector3.Distance(path.corners[cornerPoints - 1], path.corners[cornerPoints]);
                }

                if (distance < closestCollectableDistance)
                {
                    closestCollectableDistance = distance;
                    targetIndex = i;   
                } 
            }
        }
        return targetIndex;
    }

    private int CalculateNextValidPath()
    {
        //Finding the distance of collectables and comparing, setting the index of the closest collectable
        float closestCollectableDistance = float.MaxValue;

        for (int i = 0; i < targetArray.Length; i++)
        {
            //skips the deleted GameObjects in the array
            if (targetArray[i] == null)
            {
                continue;
            }

            //adds the distance for the path using from vertex-vertex path.corners
            if (NavMesh.CalculatePath(transform.position, targetArray[i].transform.position, navMeshAgent.areaMask, path))
            {
                if (CheckIfPathIsValid(i))
                {
                    float distance = Vector3.Distance(transform.position, path.corners[0]);

                    for (int cornerPoints = 1; cornerPoints < path.corners.Length; cornerPoints++)
                    {
                        distance += Vector3.Distance(path.corners[cornerPoints - 1], path.corners[cornerPoints]);
                    }

                    if (distance < closestCollectableDistance)
                    {
                        closestCollectableDistance = distance;
                        targetIndex = i;
                    } 
                }
            }
        }
        return targetIndex;
    }

    //unity animation event after the crouch animation has finished
    private void CharacterVisuals_OnAnimationCompleted(object sender, EventArgs e)
    {
        FindNextCollectable();
    }

    private bool CheckIfPathIsValid(int collectableIndex)
    {
        //Check if the Calculated path is partial or completed
        if (!CheckIfCanCollect(collectableIndex))
        {
            return false;
        }

        navMeshAgent.CalculatePath(targetArray[collectableIndex].transform.position, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        return false;
    }

    private bool CheckIfCanCollect(int collectableIndex)
    {
        return targetArray[collectableIndex] != null;   
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (ArrivedToDestination())
        {
            Debug.Log("arrivedd");
            if (CheckIfCanCollect(FindClosestCollectableIndex()) && CheckIfPathIsValid(FindClosestCollectableIndex()))
            {
                AgentState = State.Collecting;
                Destroy(targetArray[FindClosestCollectableIndex()]);
            }
            else if(navMeshAgent.areaMask == 1)
            {
                AgentState = State.Idle;
                MovingMoveable();    
            }
            else
            {
                AgentState = State.Idle;
            }
        }
    }
}
