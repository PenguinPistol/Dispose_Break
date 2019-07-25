using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockGroup
{
    public int index;
    public int unlock;
    public List<int> blockIndex;
    public List<int> blockHp;

    public int Count { get { return blockIndex.Count; } }

    public BlockGroup()
    {
        blockIndex = new List<int>();
        blockHp = new List<int>();
    }

    public bool ContainsBlock(int index)
    {
        return blockIndex.Contains(index);
    }
}
