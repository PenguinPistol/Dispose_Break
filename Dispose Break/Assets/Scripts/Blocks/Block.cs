using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using System;

public class Block : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer disposeArea;
    public TextMeshPro hpText;
    public ParticleSystem breakEffect;
    public int index;
    public string blockName;
    public int hp = 1;

    public bool isMoved;
    public bool isDisposed;
    public bool isBreaked;
    public bool isReversed;
    public bool isGaurd;
    public UnityAction breakedAction;

    private Vector3 prevPosition;

    public Sprite Sprite { get { return spriteRenderer.sprite; } }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isDisposed = true;
        isBreaked = false;
        isGaurd = false;
        isReversed = false;

        if (disposeArea == null)
        {
            disposeArea = transform.GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (hp > 1)
        {
            hpText.text = string.Format("{0}", hp);
        }
        else
        {
            hpText.text = "";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        {
            if (isMoved || isGaurd)
            {
                return;
            }

            collision.gameObject.GetComponent<Ball>().bounceCount = 0;

            if (isReversed)
            {
                collision.gameObject.GetComponent<Ball>().Reverse();
            }

            hp -= GameConst.BallDamage;

            if (hp <= 0)
            {
                if(breakEffect != null)
                {
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                }
                SoundManager.Instance.PlaySe("Break");
                breakedAction?.Invoke();

                isBreaked = true;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider != null)
        {
            isDisposed = false;
            disposeArea.color = new Color(1, 0, 0, 0.3f);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isDisposed = true;
        disposeArea.color = new Color(0, 1, 0, 0.3f);
    }

    public void StartMoved()
    {
        isMoved = true;
        prevPosition = transform.position;

        disposeArea.gameObject.SetActive(true);
    }

    public bool CheckPosition()
    {
        bool result = true;

        if(isDisposed == false)
        {
            transform.position = prevPosition;
            result = false;
        }

        isMoved = false;
        disposeArea.gameObject.SetActive(false);

        return result;
    }
}
