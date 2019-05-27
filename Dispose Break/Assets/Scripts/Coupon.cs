using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coupon : MonoBehaviour
{
    public InputField inputCode;
    public Text errCode;
    public GameObject reward;

    public void Show()
    {
        gameObject.SetActive(true);

        inputCode.text = "";
        inputCode.gameObject.SetActive(true);
        errCode.text = "";
        errCode.gameObject.SetActive(false);

        reward.SetActive(false);

        inputCode.Select();
    }

    public void CheckCode()
    {
        string code = inputCode.text.ToUpper();
        string query = string.Format("SELECT * FROM Coupon WHERE Code=\'{0}\';", code);

        bool isValid = false;
        bool isUsed = false;
        string type = "Goods";
        string value = "0";
        
        Database.Query(query, (reader) =>
        {
            isValid = true;
            isUsed = bool.Parse(reader.GetString(1));
            type = reader.GetString(2);
            value = reader.GetString(3);
        });

        Debug.Log(isValid + " / " + isUsed);

        if(isValid == false || isUsed)
        {
            string errMessage = "This code is not valid";

            if (isUsed)
            {
                errMessage = "This code is already used";
            }

            errCode.text = errMessage;

            inputCode.gameObject.SetActive(false);
            errCode.gameObject.SetActive(true);
        }
        else
        {
            SaveData.goods += int.Parse(value);

            query = string.Format("UPDATE Coupon SET Used=\'True\' WHERE Code=\'{0}\';", code);

            Database.Query(query, (reader) =>
            {
            });

            gameObject.SetActive(false);
            reward.SetActive(true);
        }
    }
}
