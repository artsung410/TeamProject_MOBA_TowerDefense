using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDrawBox : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 드로우버튼을 누르면 카드가 뽑히는데 일단은 아이템 소모부터 구현할꼬임

    // 드로우 박스 닫기
    [SerializeField]
    private GameObject closeObj;
    // 뽑기 창 띄우기
    [SerializeField]
    private GameObject openObj_ten;
    [SerializeField]
    private GameObject openObj_one;

    [SerializeField]
    private RandomSelect randomSelect_one;
    [SerializeField]
    private RandomSelect randomSelect_ten;

    private void OnEnable()
    {
        gameObject.GetComponent<Button>().interactable = true;
    }


    // 카드 뽑기 함수 실행 (드로우 매니저)
    public void BoxOpen()
    {
        // 아이템 소모
        DrawManager.instance.OpenBoxDisCount();
        // 카드 뽑기
        closeObj.SetActive(false);

        if (DrawManager.instance.boxCount == 10)
        {
            openObj_ten.SetActive(true);
            randomSelect_ten.SkillResultSelect();
        }
        else if (DrawManager.instance.boxCount == 1)
        {
            openObj_one.SetActive(true);
            randomSelect_one.SkillResultSelect();
        }
    }

    private void Update()
    {
        // 자기 자신이 활성화 되어 있고
        if (gameObject.activeSelf == true && DrawManager.instance.boxItem.item.itemValue < 10 && gameObject.GetComponent<Button>().interactable == true && DrawManager.instance.boxCount == 10)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        if (gameObject.name == "OneRetry - Button" && DrawManager.instance.boxItem.item.itemValue < 1)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void RetryButton()
    {
        StartCoroutine(RetryButtonIE());
    }

    public IEnumerator RetryButtonIE()
    {
        // 자동 오픈 이후
        gameObject.GetComponent<AutoCardOpen>().AutoOpenButton();

        yield return new WaitForSeconds(2f);

        // 아이템 소모
        DrawManager.instance.OpenBoxDisCount();
        // 카드 뽑기 창 초기화
        DrawManager.instance.CardDataInit();
        // 카드 뽑기 실행
        if (DrawManager.instance.boxCount == 10)
        {
            randomSelect_ten.SkillResultSelect();
        }
        else if (DrawManager.instance.boxCount == 1)
        {
            randomSelect_one.SkillResultSelect();
        }
    }
}
