using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : MonoBehaviour
{
    public int life;
    public Animator animator;
    public BoxCollider2D spawnRange;
    public ParticleSystem effect;

    private Rect spawnRect;

    public bool IsShow { get { return gameObject.activeSelf; } }

    private void Awake()
    {
        Vector2 size = spawnRange.size / 2f;
        float offsetY = spawnRange.transform.localPosition.y;

        Debug.LogFormat("size : {0}", spawnRange.size);

        spawnRect = new Rect(-size.x, -size.y+offsetY, size.x, size.y-offsetY);

        gameObject.SetActive(false);
    }

    public void Show()
    {
        if(IsShow)
        {
            return;
        }

        // 랜덤 위치
        float x = Random.Range(spawnRect.x, spawnRect.width);
        float y = Random.Range(spawnRect.y, spawnRect.height);

        Debug.LogFormat("spanwnX ({0} - {1})", spawnRect.x, spawnRect.width);
        Debug.LogFormat("spanwnY ({0} - {1})", spawnRect.y, spawnRect.height);

        transform.localPosition = new Vector3(x, y);
        gameObject.SetActive(true);
        life = GameConst.GoodsLife;

        animator.Play("Idle");
        SoundManager.Instance.PlaySe("Goods");
    }


    public void Pass()
    {
        if(gameObject.activeSelf == false)
        {
            return;
        }

        life -= 1;

        switch(life)
        {
            case 1:
                animator.Play("Blink");
                break;
            case 0:
                gameObject.SetActive(false);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Ball"))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            SaveData.goods += 1;
        }
    }
}
