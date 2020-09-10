using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    //public static float CONTRACT_TARGET_CONST = 10.0f;    //test value
    public static float CONTRACT_TARGET_CONST = 1000000000.0f;    
    public static float msToBoosterWait = 900000.0f;    //15 min
    public static float msToDailyReward = 86400000.0f;  //24h
    public static float percentageContractValue = 1.2f; //120%

    public static readonly string[] BULK_UPGREADE_OPTIONS = { "x1", "x5", "x10", "MAX" };
    public const int NUMBER_BULK_UPGREADE_OPTIONS = 4;
    public static readonly int[] BULK_UPGRADE_LEVELS = { 1, 5, 10 };
}

public struct GameColors
{
    public static readonly Color managerOrUpgradeSlot_TitleColor_v1 = HexToColor("#429BFF");
    public static readonly Color managerOrUpgradeSlot_TitleColor_v2 = HexToColor("#1D589F");

    public static readonly Color disableColor = HexToColor("#fff");
    public static readonly Color availableColor = HexToColor("#e3d729");
    public static readonly Color availableColorGreen = HexToColor("#0E9800");
    public static readonly Color availableColorBuyButton = HexToColor("#B3FF9E");
    public static readonly Color disableColorBuyButton = HexToColor("#fff");

    //for buttons in manager and upgradePanel
    public static readonly Color availableColorForButtons = HexToColor("#fff");
    public static readonly Color disableColorForButtons = HexToColor("#ff002b");

    //public static readonly Color currentUserInLeaderboard = tempColor;

    private static Color HexToColor(string hex)
    {
        Color color = Color.white;
        ColorUtility.TryParseHtmlString(hex, out color);

        return color;
    }
}
