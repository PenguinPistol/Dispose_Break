using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer disposeArea;
    public string blockName;
    public int hp = 1;
    public bool isMoved;
    public bool isDisposed;

    private Vector3 prevPosition;

    public Sprite Sprite { get { return spriteRenderer.sprite; } }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isDisposed = true;

        if (disposeArea == null)
        {
            disposeArea = transform.GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        {
            //hp -= 1;

            //if (hp <= 0)
            //{
            //    gameObject.SetActive(false);
            //}
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider != null)
        {
            isDisposed = false;
            disposeArea.color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isDisposed = true;
        disposeArea.color = Color.green;
    }

    public void StartMoved()
    {
        isMoved = true;
        prevPosition = transform.position;

        disposeArea.gameObject.SetActive(true);
    }

    public void CheckPosition()
    {
        Debug.Log("disposed : " + isDisposed);

        if(isDisposed == false)
        {
            transform.position = prevPosition;
        }

        isMoved = false;
        disposeArea.gameObject.SetActive(false);
    }
}
