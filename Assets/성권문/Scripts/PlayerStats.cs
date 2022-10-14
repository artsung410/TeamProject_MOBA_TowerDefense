using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("소지 골드")]
    public static int Money;

    [Header("처음 골드")]
    public int startMoney = 400;

    public static int Lives;

    [Header("처음 체력")]
    public int startLives = 20;


    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
    }

}
