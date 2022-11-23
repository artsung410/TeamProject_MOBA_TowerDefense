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

    [Header("Hp��")]
    public Sprite[] healthbarImages = new Sprite[3];
    public Image healthbarImage; // hp�� 
    public Image hitbarImage; // hit��
    public GameObject ui; // Hp�� ĵ����

    Slider _slider;
    [SerializeField]
    Enemybase enemybase;
    private void Awake()
    {
   
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            healthbarImage.sprite = healthbarImages[0]; // �ʷ� 
            hitbarImage.sprite = healthbarImages[1]; //����
        }
        else
        {
            healthbarImage.sprite = healthbarImages[1];
            hitbarImage.sprite = healthbarImages[2];
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if(Enemy == null)
        {
            Destroy(gameObject);
        }

        healthbarImage.fillAmount = enemybase.CurrnetHP / enemybase.HP;
        ApplyHitBar(healthbarImage.fillAmount);



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
        
        if (enemybase._eminontpye != EMINIONTYPE.Netural)
        {
            transform.position = new Vector3(Enemy.position.x, 7f, Enemy.position.z);
        }
        else if(enemybase._eminontpye == EMINIONTYPE.Netural)
        {
            transform.position = new Vector3(Enemy.position.x,14f,Enemy.position.z);
        }
    }

    private IEnumerator ApplyHitBar(float value)
    {
        float prevValue = hitbarImage.fillAmount;
        float delta = prevValue / 100f;

        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            prevValue -= delta;
            hitbarImage.fillAmount = prevValue;

            if (prevValue - value < 0.001f)
            {
                break;
            }
        }
    }

}