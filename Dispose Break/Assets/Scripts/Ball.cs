using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    private enum State
    {
        Ready, Shot, Return
    }

    // 사용되는 태그들
    public const string TAG_SHOT_LINE = "Shot Line";
    public const string TAG_BLOCK = "Block";
    public const string TAG_WALL = "Wall";
    public const string TAG_GOODS = "Goods";

    // 인스펙터 항목
    public new Rigidbody2D rigidbody;
    public float degree;
    public int bounceCount;

    public UnityAction WallHitCallback;

    private State state;
    private Vector3 direction;
    private float speed;
    private bool isFinished;
    private new SpriteRenderer renderer;

    public bool Finished { get { return isFinished; } }

    public void Initialize(float degree)
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        speed = GameConst.DefaultSpeed;

        this.degree = degree;
        direction = Quaternion.AngleAxis(degree, Vector3.forward) * Vector3.right;
        
        if(GameManager.Instance.equipedBallSkin != null)
        {
            renderer.sprite = GameManager.Instance.equipedBallSkin.sprite;
        }

        state = State.Ready;
    }

    public void Shot()
    {
        if (state != State.Ready)
        {
            return;
        }

        state = State.Shot;
        isFinished = false;

        rigidbody.velocity = (direction * speed);
    }

    public void Reverse()
    {
        Vector2 vel = rigidbody.velocity;
        vel.x *= -1;
        rigidbody.velocity = vel;
    }

    public void SetDirection()
    {
        do
        {
            degree = Random.Range(GameConst.BallAngleMin, GameConst.BallAngleMax);
        }
        while (80f < degree && degree < 100f);

        direction = Quaternion.AngleAxis(degree, Vector3.forward) * Vector3.right;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals(TAG_WALL))
        {
            if(state == State.Shot)
            {
                WallHitCallback?.Invoke();

                SoundManager.Instance.PlaySe("Break");

                bounceCount++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(TAG_SHOT_LINE))
        {
            if (state == State.Shot)
            {
                rigidbody.velocity = Vector2.zero;

                // 발사된 후 발사라인에 도달 시
                bounceCount = 0;
                isFinished = true;

                state = State.Ready;
            }
        }
    }

    public void Drop()
    {
        rigidbody.velocity = Vector3.down * speed * 2;
    }
}
