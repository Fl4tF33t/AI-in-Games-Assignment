using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WoodCutterAgentVisuals : MonoBehaviour
{
    public event EventHandler OnAnimationCompleted;

    WoodCutterAgent woodCutterAgent;
    Animator animator;

    const string IS_WALKING = "isWalking";

    private void Awake()
    {
        woodCutterAgent = GetComponentInParent<WoodCutterAgent>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        woodCutterAgent.OnAgentState += WoodCutterAgent_OnAgentState;
    }

    private void WoodCutterAgent_OnAgentState(object sender, WoodCutterAgent.OnAgentStateEventArgs e)
    {
        if (e.agentState == WoodCutterAgent.State.Idle)
        {
            animator.SetBool(IS_WALKING, false);
        }
        if (e.agentState == WoodCutterAgent.State.Walking)
        {
            animator.SetBool(IS_WALKING, true);
        }
        if (e.agentState == WoodCutterAgent.State.Collecting)
        {
            animator.SetTrigger("isCollecting");
        }
        if (e.agentState == WoodCutterAgent.State.Chopping)
        {
            animator.SetBool("isChopping", true);
        }
    }


    public void OnChopAnimationFinished()
    {
        OnAnimationCompleted?.Invoke(this, EventArgs.Empty);
        Debug.Log("Choppped");
    }



    // Update is called once per frame
    void Update()
    {

    }
}
