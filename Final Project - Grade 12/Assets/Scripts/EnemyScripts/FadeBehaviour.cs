using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBehaviour : StateMachineBehaviour
{
    public float fadeTime = 3f;
    public float startFade = 1f;

    private float timeElapsed = 0f;
    SpriteRenderer spriteRenderer;
    GameObject remover;
    Color startColour;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColour = spriteRenderer.color;
        remover = animator.gameObject;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed += Time.deltaTime;

        float newAlpha = startColour.a * (1 - (timeElapsed-startFade) / fadeTime);
        if(timeElapsed > startFade)
        {
            spriteRenderer.color = new Color(startColour.r, startColour.g, startColour.b, newAlpha);
        }

        if(timeElapsed > fadeTime + startFade)
        {
            Destroy(remover);
        }
    }

}
