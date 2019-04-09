using UnityEngine;
using System.Collections;

namespace com.TeamPlug.Patterns
{
    public class LoadingView : MonoBehaviour
    {
        public Animator animator;

        public AnimationClip startClip;
        public AnimationClip finishClip;

        public IEnumerator StartAnimation()
        {
            animator.Play(startClip.name);

            // 해당 애니메이션클립 으로 진입했는지 체크
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(startClip.name))
            {
                yield return null;
            }

            //  애니메이션이 끝났는지 체크
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
        }

        public IEnumerator FinishAnimation()
        {
            animator.Play(finishClip.name);

            // 해당 애니메이션클립 으로 진입했는지 체크
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(finishClip.name))
            {
                yield return null;
            }

            //  애니메이션이 끝났는지 체크
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
        }
    }

}