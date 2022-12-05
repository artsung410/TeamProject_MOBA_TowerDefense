using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerManager : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 배팅 호출 프리펩
    public GameObject WinPostAPICallerPre;

    private void Update()
    {
        if (GameManager.Instance.isGameEnd)
        {
            Instantiate(WinPostAPICallerPre, Vector3.zero, Quaternion.identity);
            CurencyChange.instance.amountCurrency = GameObject.FindGameObjectWithTag("APIStorage").GetComponent<APIStorage>().amount_won;
            Destroy(GameObject.FindGameObjectWithTag("APIStorage").gameObject);
            Destroy(GameObject.FindGameObjectWithTag("GetCaller").gameObject);
            Destroy(gameObject);
        }
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(WinPostAPICallerPre, Vector3.zero, Quaternion.identity);
            Destroy(gameObject);
        }*/
    }
}
