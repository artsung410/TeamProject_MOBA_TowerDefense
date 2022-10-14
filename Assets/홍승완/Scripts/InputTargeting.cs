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
    /// 미니언을 타게팅하면 플레이어 targeted Enemy에 찍은 미니언 오브젝트 담음
    /// </summary>
    private void MinionTargeting()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;

                // 미니언이 타겟가능하면
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
