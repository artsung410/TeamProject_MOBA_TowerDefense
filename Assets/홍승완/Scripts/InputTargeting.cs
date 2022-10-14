using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTargeting : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject selectedHero;
    public bool heroPlayer;

    RaycastHit hit;

    private void Awake()
    {
        selectedHero = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        
    }

    private void Update()
    {
        MinionTargeting();
    }


    /// <summary>
    /// �̴Ͼ��� Ÿ�����ϸ� �÷��̾� targeted Enemy�� ���� �̴Ͼ� ������Ʈ ����
    /// </summary>
    private void MinionTargeting()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;

                // �̴Ͼ��� Ÿ�ٰ����ϸ�
                if (hit.collider.GetComponent<Targetable>() != null)
                {
                    if (hit.collider.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Minion)
                    {
                        selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;
                    }
                }
                else if (hit.collider.gameObject.GetComponent<Targetable>() == null)
                {
                    selectedHero.GetComponent<HeroCombat>().targetedEnemy = null;
                }
            }

        }
    }
}
