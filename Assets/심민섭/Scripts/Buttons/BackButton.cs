using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڷ� ����
public class BackButton : MonoBehaviour
{
    private GameObject selectMatch;

    void Start()
    {
        selectMatch = gameObject.transform.parent.gameObject;
    }

    public void BackSpace()
    {
        selectMatch.SetActive(false);
    }
}
