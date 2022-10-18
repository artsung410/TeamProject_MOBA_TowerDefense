using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    [SerializeField]
    private Transform Enemy;
    [SerializeField]
    private BlueAttackRange AttackEnemy;
    Slider _slider;
    private void Awake()
    {
        _slider = GetComponent<Slider>();   
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = Mathf.Lerp()
        transform.position = new Vector3(Enemy.position.x, 3f, Enemy.position.z);
        
    }
}
