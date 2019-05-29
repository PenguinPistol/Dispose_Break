using System.Text;
using System.Collections.Generic;

public static class SaveData
{
    public static int bestScore = 0 ;
    public static int oneWayClear = 0;
    public static int noGuideClear = 0;
    public static int goods = 0;
    public static int equipSkin = 1;
    public static List<int> unlockSkins = new List<int>();
    public static bool infoShield = false;
    public static bool infoHalf = false;
    public static bool infoReverse = false;

    public static string UnlockSkins
    {
        get
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < unlockSkins.Count; i++)
            {
                sb.AppendFormat("{0}", unlockSkins[i]);

                if(i < unlockSkins.Count - 1)
                {
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }
    }
}
