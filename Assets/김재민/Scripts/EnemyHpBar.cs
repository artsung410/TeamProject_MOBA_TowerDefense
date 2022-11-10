using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class EnemyHpBar : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    [SerializeField]
    private Transform Enemy;
    Slider _slider;
    [SerializeField]
    Enemybase enemybase;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    public void Update()
    {
        if(Enemy == null)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        _slider.value = enemybase.CurrnetHP / enemybase.HP;
        if (Enemy == null || _slider == null)
        {
            return;
        }

        if (transform == null || transform.gameObject == null || this == null)
        {
            return;
        }

        if(transform.parent == null)
        {
            Destroy(gameObject.GetComponent<RectTransform>());
        }
        transform.position = new Vector3(Enemy.position.x, 7f, Enemy.position.z);
        
    }
}