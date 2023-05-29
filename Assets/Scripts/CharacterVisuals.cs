using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterVisuals : MonoBehaviour
{
    public event EventHandler OnAnimationCompleted;

    BaseAgent baseAgent;
    Animator animator;

    const string IS_WALKING = "isWalking";

    private void Awake()
    {
        baseAgent = GetComponentInParent<BaseAgent>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        baseAgent.OnAgentState += baseAgent_OnAgentState;
    }

    private void baseAgent_OnAgentState(object sender, BaseAgent.OnAgentStateEventArgs e)
    {
        if(e.agentState == BaseAgent.State.Idle)
        {
            animator.SetBool(IS_WALKING, false);
        }
        if (e.agentState == BaseAgent.State.Walking)
        {
            animator.SetBool(IS_WALKING, true);
        }
        if (e.agentState == BaseAgent.State.Collecting)
        {
            animator.SetTrigger("isCollecting");
        }
        if (e.agentState == BaseAgent.State.WoodChopping)
        {
            animator.SetBool("isChopping", true);
        }
    }

    public void OnCrouchAnimationFinished()
    {
        OnAnimationCompleted?.Invoke(this, EventArgs.Empty);
    }

    public void OnChopAnimationFinished()
    {
        OnAnimationCompleted?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
