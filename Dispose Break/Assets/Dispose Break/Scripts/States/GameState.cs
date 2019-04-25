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
    // 공 이동 경로
    public Transform path;
    // 발사 버튼
    public Button shotButton;

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

        path.gameObject.SetActive(false);
        StartCoroutine(Shot());
    }

    // 경로 계산
    public void CalculatePath()
    {
        var ray = new Ray2D(path.position, ball.direction);
        var hit = Physics2D.CircleCast(ray.origin, 0.25f, ray.direction, PATH_DISTANCE, (1 << 10));

        if (hit)
        {
            Vector3 reflect = Vector3.Reflect(ray.direction, hit.normal);

            float travelDistance = Vector3.Distance(ray.origin, hit.point);
            float remainingDistance = PATH_DISTANCE - travelDistance;

            int travelCount = (int)(travelDistance / PATH_DISTANCE * path.childCount);
            int remainCount = path.childCount - travelCount;

            for (int i = 0; i < path.childCount; i++)
            {
                float t = (float)(i + 1) / travelCount;

                if (i < travelCount)
                {
                    path.GetChild(i).localPosition = Vector3.Lerp(Vector3.zero, ball.direction * travelDistance, t);
                }
                else
                {
                    t = (float)(i - travelCount + 1) / remainCount;

                    path.GetChild(i).position = Vector3.Lerp(hit.point, (Vector3)hit.point + reflect * remainingDistance, t) + Vector3.forward * 100;
                }
            }
        }
        else
        {
            for (int i = 0; i < path.childCount; i++)
            {
                float t = (float)i / path.childCount;
                path.GetChild(i).localPosition = Vector3.Lerp(ball.direction, ball.direction * ball.speed, t);
            }
        }
    }
}
