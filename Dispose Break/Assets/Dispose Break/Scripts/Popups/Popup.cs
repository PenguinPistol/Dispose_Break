using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Popup : MonoBehaviour
{
    public Text message;

    public Animator animator;
    public string openAnimation = "Open";
    public string closeAnimation = "Close";

    public void Show(params object[] data)
    {
        gameObject.SetActive(true);

        if(animator != null)
        {
            animator.Play(openAnimation);
        }

        OnOpend(data);
    }

    public void Close()
    {
        if (animator != null)
        {
            animator.Play(closeAnimation);
        }

        OnClosed();
        gameObject.SetActive(false);
    }

    public void SetMessage(string message)
    {
        if(this.message == null)
        {
            return;
        }

        this.message.text = message;
    }

    public virtual void OnOpend(params object[] data) { }
    public virtual void OnClosed() { }
}
