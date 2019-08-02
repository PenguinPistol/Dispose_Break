using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    public float minLoadTime = 3f;

    void Start()
    {
        //Debug.Log("###############################################");
        //Debug.LogFormat("absolute : {0}", Application.absoluteURL);
        //Debug.LogFormat("data : {0}", Application.dataPath);
        //Debug.LogFormat("streaming : {0}", Application.streamingAssetsPath);
        //Debug.LogFormat("persistent :{0}", Application.persistentDataPath);
        //Debug.Log("###############################################");

        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        float time = Time.time;

        Database.ReadGameConst();
        CSVToSqlite parser = new CSVToSqlite();

        //Debug.Log("Read Block");
        yield return parser.Parse("Block");
        //Debug.Log("Read BallSkin");
        yield return parser.Parse("BallSkin");
        //Debug.Log("Read InifinityModeGroup");
        yield return parser.Parse("InfinityModeGroup");
        //Debug.Log("Read NoGuideChallenge");
        yield return parser.Parse("NoGuideChallenge");
        //Debug.Log("Read OneWayChallenge");
        yield return parser.Parse("OneWayChallenge");
        //Debug.Log("Read Coupon");
        yield return parser.Parse("Coupon");

        float afterTime = minLoadTime - (Time.time - time);

        //Debug.Log("Delay MinLoadTime");
        while (afterTime > 0)
        {
            afterTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("Game");
    }
}
