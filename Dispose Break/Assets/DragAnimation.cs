using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAnimation : MonoBehaviour
{
    public static bool isFinished = false;

    public Animator animator;

    private void Start()
    {
        if(isFinished)
        {
            gameObject.SetActive(false);
        }
        else
        {
            animator.Play("Drag");
            StartCoroutine(CheckFinish());
        }
    }

    private IEnumerator CheckFinish()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Drag"))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        isFinished = true;
    }
}
