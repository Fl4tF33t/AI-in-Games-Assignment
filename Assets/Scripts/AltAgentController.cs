using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AltAgentController : BaseAgent
{
    [SerializeField]
    private int axeDamage;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterVisuals = GetComponentInChildren<CharacterVisuals>();
    }
    void Start()
    {
        FindTargetsAndSetIndex("Tree", 0);
        targetIndex = Random.Range(0, targetArray.Length);
        navMeshAgent.destination = targetArray[targetIndex].transform.position;
        AgentState = State.Walking;
        characterVisuals.OnAnimationCompleted += CharacterVisuals_OnAnimationCompleted;
    }

    private void CharacterVisuals_OnAnimationCompleted(object sender, System.EventArgs e)
    {
        TreeData treeData = targetArray[targetIndex].GetComponent<TreeData>();
        treeData.DamageTaken(axeDamage);
    }


    // Update is called once per frame
    void Update()
    {
        if (ArrivedToDestination())
        {
            AgentState = State.WoodChopping;
        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

    }
}
