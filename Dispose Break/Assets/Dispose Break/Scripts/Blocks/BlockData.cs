using UnityEngine;
using System.Collections;

public class BlockData : ScriptableObject
{
    public Sprite sprite;
    public int hp = 1;

    public IEnumerator BlockControl()
    {
        yield return null;
    }
}
