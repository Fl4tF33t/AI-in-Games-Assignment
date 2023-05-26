using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterVisuals : MonoBehaviour
{
    public event EventHandler OnAnimationCompleted;

    AgentController agentController;
    Animator animator;

    const string IS_WALKING = "isWalking";

    private void Awake()
    {
        agentController = GetComponentInParent<AgentController>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        agentController.OnAgentState += AgentController_OnAgentState;
    }

    private void AgentController_OnAgentState(object sender, AgentController.OnAgentStateEventArgs e)
    {
        if(e.agentState == AgentController.State.Idle)
        {
            animator.SetBool(IS_WALKING, false);
        }
        if (e.agentState == AgentController.State.Walking)
        {
            animator.SetBool(IS_WALKING, true);
        }
        if (e.agentState == AgentController.State.Collecting)
        {
            animator.SetTrigger("isCollecting");
        }
    }

    public void OnCrouchAnimationFinished()
    {
        OnAnimationCompleted?.Invoke(this, EventArgs.Empty);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
