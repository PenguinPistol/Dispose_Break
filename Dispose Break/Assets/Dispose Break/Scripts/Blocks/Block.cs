using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer disposeArea;
    public int hp = 1;
    public bool isDisposed;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(disposeArea == null)
        {
            disposeArea = transform.GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void Initialize(Sprite sprite, int hp)
    {
        gameObject.SetActive(true);

        spriteRenderer.sprite = sprite;
        this.hp = hp;

        isDisposed = false;
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("col");
        if(collider != null)
        {
            isDisposed = true;
            disposeArea.color = Color.red;
        }
        else
        {
            isDisposed = false;
            disposeArea.color = Color.green;
        }
    }
}
