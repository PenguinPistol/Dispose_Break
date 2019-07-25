using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePath : MonoBehaviour
{
    public SpriteRenderer path;
    public SpriteRenderer path2;
    public Transform pathHead;
    public Transform hitPoint;
    public float length;

    //private void Start()
    //{
    //    hitPoint.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.equipedBallSkin.sprite;
    //}

    public void Calculate(float targetAngle)
    {
        Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * targetAngle), Mathf.Sin(Mathf.Deg2Rad * targetAngle));
        Ray2D ray = new Ray2D(transform.position, dir);
        RaycastHit2D hit = Physics2D.CircleCast(ray.origin, 0.47f, ray.direction, length, 1 << 10);

        float angle = Vector3.Angle(Vector3.right, dir);
        path.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(hit)
        {
            path.size = new Vector2(hit.distance, path.size.y);

            Vector3 point = ray.origin + ray.direction * hit.distance;
            float remainWidth = length - hit.distance;
            var reflect = Vector3.Reflect(ray.direction, hit.normal);
            var refAngle = Vector3.Angle(Vector3.right, reflect);

            path2.transform.position = point;
            path2.transform.rotation = Quaternion.AngleAxis(refAngle, Vector3.forward);
            path2.size = new Vector2(remainWidth, path2.size.y);
            path2.gameObject.SetActive(true);

            pathHead.position = point + reflect * remainWidth;
            pathHead.rotation = path2.transform.rotation;

            hitPoint.gameObject.SetActive(true);
            hitPoint.position = point;
        }
        else
        {
            path.size = new Vector2(length, path.size.y);

            pathHead.position = transform.position + dir * length;
            pathHead.rotation = path.transform.rotation;

            path2.gameObject.SetActive(false);
            hitPoint.gameObject.SetActive(false);
        }

        gameObject.SetActive(true);
    }
}
