using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("���� ���")]
    public static int Money;

    [Header("ó�� ���")]
    public int startMoney = 400;

    public static int Lives;

    [Header("ó�� ü��")]
    public int startLives = 20;


    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
    }

}
