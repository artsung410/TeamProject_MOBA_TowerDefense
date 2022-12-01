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
    public BuffBlueprint buff;
    public BuffTooltip tooltip;
    public float coolTime;
    public float elapsedTime;
    public Image coolTimeImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buff != null)
        {
            tooltip.gameObject.SetActive(true);
            //TODO: ���� ��ũ��Ʈ �����ϱ�.
            tooltip.SetupBuffTooltip(buff);
            Debug.Log(GetType().Name);
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

        if (buff.Duration == 0)
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
            tooltip.gameObject.SetActive(false);
            gameObject.GetComponent<Image>().sprite = null;
            buff = null;
            coolTime = 0;
            elapsedTime = 0;
            StartCoroutine(AssemblyBuffAndApplyRandomDelay());
            return;
        }

        elapsedTime += Time.deltaTime;
        coolTimeImage.fillAmount = elapsedTime / coolTime;
    }

    // ��Ÿ���� ���ÿ� ����� �� �Լ�ȣ���� �ȵǴ� �����ذ�
    IEnumerator AssemblyBuffAndApplyRandomDelay()
    {
        float randNum = Random.Range(0.0f, 0.03f);
        yield return new WaitForSeconds(randNum);
        coolTimeImage.fillAmount = 0f;
        BuffManager.Instance.AssemblyBuff();
    }

    private void OnDisable()
    {
        if (buff == null)
        {
            return;
        }

        if (!BuffManager.Instance.buffDic.ContainsKey(buff))
        {
            BuffManager.Instance.buffDic.Add(buff, elapsedTime);
        }
    }
}
