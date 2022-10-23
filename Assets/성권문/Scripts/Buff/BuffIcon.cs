using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class BuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BuffData buff;
    public BuffTooltip tooltip;
    public float coolTime;
    private float elapsedTime;
    public Image coolTimeImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buff != null)
        {
            tooltip.gameObject.SetActive(true);
            tooltip.SetupTooltip(buff.Name, buff.Desc);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buff != null)
        {
            tooltip.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (buff == null)
        {
            return;
        }

        if (buff.Unlimited)
        {
            return;
        }

        ApplyCooldown();
    }

    private void ApplyCooldown()
    {
        if (elapsedTime >= coolTime)
        {
            BuffManager.Instance.removeBuff(buff);
            gameObject.GetComponent<Image>().sprite = null;
            buff = null;
            coolTime = 0;
            elapsedTime = 0;
            coolTimeImage.fillAmount = 0f;
            StartCoroutine(AssemblyBuffAndApplyRandomDelay());
            return;
        }

        elapsedTime += Time.deltaTime;
        coolTimeImage.fillAmount = elapsedTime / coolTime;
    }

    // 쿨타임이 동시에 적용될 때 함수호출이 안되는 문제해결
    IEnumerator AssemblyBuffAndApplyRandomDelay()
    {
        float randNum = Random.Range(0f, 0.3f);
        yield return new WaitForSeconds(randNum);
        BuffManager.Instance.AssemblyBuff();
    }
}
