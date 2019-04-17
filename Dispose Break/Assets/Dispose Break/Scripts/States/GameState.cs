using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.TeamPlug.Patterns;

public abstract class GameState : State
{
    // 공
    public Ball ball;
    // 공 발사 방향화살표
    public Transform directionArrow;
    // 선택한 블록
    public Block selectBlock;
    // 블럭 인벤토리
    public BlockInventory inventory;
    // 해당 모드에서 사용되는 블럭들
    public List<Block> usedBlocks;
    // 인벤토리에 블럭들
    protected List<Block> inventoryBlocks;
    // 배치된 블럭들
    protected List<Block> disposedBlocks = new List<Block>();

    public override IEnumerator Initialize(params object[] _data)
    {
        yield return null;
    }

    public override void Begin()
    {
    }

    public override void Execute()
    {
    }

    public override void Release()
    {
    }

    public void DisposeBlock(Block block)
    {
        selectBlock = Instantiate(block, transform);
        selectBlock.StartMoved();

        disposedBlocks.Add(selectBlock);
    }
}
