using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.TeamPlug.Input;

public class InfinityMode : GameState
{
    public enum State
    {
        Ready, Play, Pause, Finished
    }
    
    public Animator animator;
    public Text scoreText;

    private int ralleyScore = 0;
    private int currentScore = 0;
    private State state = State.Ready;

    public override IEnumerator Initialize(params object[] _data)
    {
        GameManager.Instance.currentGameMode = this;

        inventoryBlocks = new List<Block>
        {
            usedBlocks[0]
            , usedBlocks[1]
            , usedBlocks[1]
        };

        ball.wallAction = () =>
        {
            ralleyScore++;
        };

        isShot = false;

        selectBlock = null;

        yield return null;
    }

    public override void Begin()
    {
        inventory.Initialize(inventoryBlocks);
        shotButton.interactable = false;
        CalculatePath();

        TouchController.Instance.AddObservable(this);
    }

    public override void Execute()
    {
        switch(state)
        {
            case State.Ready:
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PopupContoller.Instance.Show("QuitPopup");
                }
                break;

            case State.Play:
                scoreText.text = string.Format("{0}", currentScore);
                break;
        }
    }

    public override void Release()
    {
    }

    public override void TouchBegan(Vector3 touchPosition, int touchIndex)
    {
        var hitCollider = TouchController.Raycast2D(touchPosition);

        if(hitCollider != null)
        {
            if(hitCollider.tag.Equals("Block"))
            {
                // 이동 
                selectBlock = hitCollider.GetComponent<Block>();
                selectBlock.StartMoved();
            }
        }
    }


    public override void TouchMoved(Vector3 touchPosition, int touchIndex)
    {
        if(selectBlock != null)
        {
            float z = selectBlock.transform.position.z;

            selectBlock.transform.position = new Vector3(touchPosition.x, touchPosition.y, z);
        }
    }

    public override void TouchEnded(Vector3 touchPosition, int touchIndex)
    {
        if(selectBlock != null)
        {
            selectBlock.CheckPosition();
        }

        selectBlock = null;

        if (inventory.Count == 0)
        {
            shotButton.interactable = true;
        }
    }

    public override void DisposeBlock(Block block)
    {
        if(state == State.Ready)
        {
            state = State.Play;
            animator.Play("Play");
        }

        selectBlock = Instantiate(block, transform);
        selectBlock.StartMoved();
        selectBlock.breakedAction = () => {
            ralleyScore++;
        };

        disposedBlocks.Add(selectBlock);
    }

    protected override IEnumerator Shot()
    {
        ball.Shot();

        while (ball.Finished == false)
        {
            yield return null;
        }

        if (disposedBlocks.FindAll(x => x.isBreaked).Count == disposedBlocks.Count)
        {
            // 성공
            yield return ball.ResetShot();

            currentScore += ralleyScore;
            ralleyScore = 0;

            CalculatePath();
            path.gameObject.SetActive(true);

            foreach (var item in disposedBlocks)
            {
                Destroy(item.gameObject);
            }
            disposedBlocks.Clear();

            inventory.Initialize(inventoryBlocks);

        }
        else
        {
            // 실패
            PopupContoller.Instance.Show("InfinityResultPopup", currentScore);
        }

        shotButton.interactable = false;
        isShot = false;
    }
}