using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Block data;

    public void Initialize(Block data)
    {
        this.data = data;

        if(spriteRenderer != null)
        {
            spriteRenderer.sprite = data.sprite;
        }
    }
}