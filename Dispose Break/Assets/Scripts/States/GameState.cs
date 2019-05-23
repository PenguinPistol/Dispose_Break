using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using com.TeamPlug.Patterns;
using UnityEngine.Events;

public abstract class GameState : State
{
    // 공 예상 이동경로 길이
    public const float PATH_DISTANCE = 10f;

    // 공
    public Ball ball;
    // 공 발사 방향화살표
    public Transform directionArrow;
    // 블럭 인벤토리
    public BlockInventory inventory;
    // 해당 모드에서 사용되는 블럭들
    public List<Block> usedBlocks;
    // 발사 버튼
    public Button shotButton;
    // 발사 경로
    public GuidePath path;

    [HideInInspector]
    // 선택한 블록
    public Block selectBlock;

    // 인벤토리에 블럭들
    protected List<Block> inventoryBlocks;
    // 배치된 블럭들
    protected List<Block> disposedBlocks = new List<Block>();
    // 발사 여부
    protected bool isShot;

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
        selectBlock = null;
    }

    public virtual void DisposeBlock(Block block)
    {
        selectBlock = Instantiate(block, transform);
        selectBlock.StartMoved();

        disposedBlocks.Add(selectBlock);
    }

    protected abstract IEnumerator Shot();

    public void ShotBall()
    {
        if (isShot)
        {
            return;
        }

        isShot = true;

        if(path != null)
        {
            path.gameObject.SetActive(false);
        }
        StartCoroutine(Shot());
    }
}
