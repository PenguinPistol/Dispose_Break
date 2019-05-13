using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;

public class Shop : State
{
    public BallList ballList;
    public List<BallSkin> ballSkins;

    public override IEnumerator Initialize(params object[] _data)
    {
        ballSkins = new List<BallSkin>();

        string query = string.Format(Database.SELECT_TABLE_ALL, "BallSkin");

        Database.Query(query, (reader) => {
            BallSkin skin = new BallSkin();

            skin.index = int.Parse(reader.GetString(0));
            skin.name = reader.GetString(1);
            skin.grade = reader.GetString(2);
            skin.unlockType = int.Parse(reader.GetString(3));
            skin.unlockLevel = int.Parse(reader.GetString(4));
            skin.sprite = Resources.Load<Sprite>("Ball/Ball_"+skin.index);

            ballSkins.Add(skin);
        });

        int selectIndex = 0;

        if (GameManager.Instance.equipedBallSkin != null)
        {
            selectIndex = GameManager.Instance.equipedBallSkin.index-1;
        }

        yield return ballList.Init(ballSkins, selectIndex);

        ballList.UnlockItem(0);
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
}