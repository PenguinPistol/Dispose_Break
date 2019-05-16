using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using com.TeamPlug.Patterns;

public class Shop : State
{
    public BallList ballList;
    public List<BallSkin> ballSkins;
    public Text goodsText;
    public AudioSource sePlayer;

    public override IEnumerator Initialize(params object[] _data)
    {
        SoundManager.Instance.sePlayer = sePlayer;
        SoundManager.Instance.PlaySe("Scroll");

        ballSkins = new List<BallSkin>();

        string query = string.Format(Database.SELECT_TABLE_ALL, "BallSkin");

        Database.Query(query, (reader) => {
            BallSkin skin = new BallSkin();

            skin.index = int.Parse(reader.GetString(0));
            skin.name = reader.GetString(1);
            skin.grade = reader.GetString(2);
            skin.unlockType = int.Parse(reader.GetString(3));
            skin.unlockLevel = int.Parse(reader.GetString(4));
            skin.sprite = Resources.Load<Sprite>("Sprites/Ball/Ball_" + skin.index);

            ballSkins.Add(skin);
        });

        int selectIndex = 0;

        if (GameManager.Instance.equipedBallSkin != null)
        {
            selectIndex = GameManager.Instance.equipedBallSkin.index-1;
        }

        yield return ballList.Init(ballSkins, selectIndex);

        foreach(var index in SaveData.unlockSkins)
        {
            Debug.Log("unlock : " + index);
            ballList.UnlockItem(index);
        }
    }

    public override void Begin()
    {
    }

    public override void Execute()
    {
        goodsText.text = string.Format("{0}", SaveData.goods);
    }

    public override void Release()
    {
    }

    public void LootSkin()
    {
        if (SaveData.goods < GameConst.LootCost)
        {
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(Database.SELECT_TABLE_ALL, "BallSkin");
        sb.Append(" WHERE UnlockType = \'0\' AND ");

        for(int i = 0; i < SaveData.unlockSkins.Count; i++)
        {
            sb.AppendFormat("SkinIndex != \'{0}\'", SaveData.unlockSkins[i]);

            if(i < SaveData.unlockSkins.Count - 1)
            {
                sb.Append(" AND ");
            }
        }
        sb.Append(";");

        List<int> result = new List<int>();

        Database.Query(sb.ToString(), (reader) => {
            result.Add(int.Parse(reader.GetString(0)));
        });

        if(result.Count == 0)
        {
            return;
        }

        int lootIndex = Random.Range(0, result.Count);

        SaveData.goods -= GameConst.LootCost;
        ballList.UnlockItem(result[lootIndex]);

        PopupContoller.Instance.Show("GetBallPopup", result[lootIndex]);
        SoundManager.Instance.PlaySe("GetBall");
    }
}