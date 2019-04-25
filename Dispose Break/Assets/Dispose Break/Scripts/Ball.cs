using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public const string TAG_SHOT_LINE = "Shot Line";
    public const string TAG_BLOCK = "Block";
    public const string TAG_WALL = "Wall";

    public float shotDegree;
    public float speed;
    public Vector3 direction;
    public UnityAction wallAction;

    private bool isShoted;
    private Vector3 shotPosition;
    private new Rigidbody2D rigidbody2D;
    private bool isFinished;

    public bool Finished { get { return isFinished; } }

    private void Start()
    {
        shotPosition = transform.position;
        speed = GameConst.DefaultSpeed;
        direction = Quaternion.AngleAxis(shotDegree, Vector3.forward) * Vector3.right;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Shot()
    {
        if (isShoted)
        {
            return;
        }

        isShoted = true;
        isFinished = false;

        rigidbody2D.velocity = (direction * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals(TAG_WALL))
        {
            wallAction?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(TAG_SHOT_LINE) && isShoted)
        {
            rigidbody2D.velocity = Vector2.zero;

            // 발사된 후 발사라인에 도달 시
            isFinished = true;
        }
    }

    public IEnumerator ResetShot()
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(transform.position, shotPosition);
        float startTime = Time.time;

        while (Vector3.Distance(shotPosition, transform.position) > 0.1f)
        {
            float distcoverd = (Time.time - startTime) * GameConst.DefaultSpeed * 2;
            float fracJouney = distcoverd / distance;

            transform.position = Vector3.Lerp(startPosition, shotPosition, fracJouney);

            yield return null;
        }

        transform.position = shotPosition;

        shotDegree = Random.Range(20f, 160f);
        direction = Quaternion.AngleAxis(shotDegree, Vector3.forward) * Vector3.right;

        isShoted = false;
    }
}
