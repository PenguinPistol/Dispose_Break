using UnityEngine;

public class BlockData
{
    public int index;
    public string blockName;
    public string description;
    public int hp = 1;

    public BlockData(int index)
    {
        this.index = index;
    }
}
