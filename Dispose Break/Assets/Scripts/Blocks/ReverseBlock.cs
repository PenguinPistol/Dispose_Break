using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseBlock : MonoBehaviour
{
    public Block parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        {
            parent.isReversed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        {
            parent.isReversed = false;
        }
    }
}
