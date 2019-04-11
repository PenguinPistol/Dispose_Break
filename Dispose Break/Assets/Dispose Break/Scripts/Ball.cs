using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public const string TAG_SHOT_LINE = "Shot Line";
    public const string TAG_BLOCK = "Block";

    public float shotDegree;
    public float speed;
    public Vector3 direction;

    private bool isShoted;
    private Vector3 shotPosition;

    private void Start()
    {
        shotPosition = transform.position;
        speed = GameConst.DefaultSpeed;
        direction = Quaternion.AngleAxis(shotDegree, Vector3.forward) * Vector3.right;
    }

    private void Update()
    {
    }

    public void Shot()
    {
        if (isShoted)
        {
            return;
        }

        isShoted = true;
        

        GetComponent<Rigidbody2D>().velocity = (direction * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(TAG_SHOT_LINE) && isShoted)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            // 발사된 후 발사라인에 도달 시
            StartCoroutine(ResetShot());
        }
    }

    private IEnumerator ResetShot()
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(transform.position, shotPosition);
        float startTime = Time.time;

        while (Vector3.Distance(shotPosition, transform.position) > 0.1f)
        {
            float distcoverd = (Time.time - startTime) * GameConst.DefaultSpeed;
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
