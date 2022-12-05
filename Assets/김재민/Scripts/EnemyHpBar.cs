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
    [SerializeField]
    private Enemybase enemybase;


    [Header("Hp��")]
    public Sprite[] healthbarImages = new Sprite[3];
    private Image healthbarImage; // hp�� 
    private Image hitbarImage; // hit��
    public GameObject ui; // Hp�� ĵ����

    float elaspedTime = 0f;

    private IEnumerator _ApplyHitBar;

    private void Awake()
    {
        hitbarImage = transform.GetChild(0).GetComponent<Image>();
        healthbarImage = transform.GetChild(1).GetComponent<Image>();
        
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

        if(enemybase._eminontpye == EMINIONTYPE.Netural)
        {
            healthbarImage.sprite = healthbarImages[1];
            hitbarImage.sprite = healthbarImages[2];
        }
        else
        {
            return;
        }



    }

    // Update is called once per frame
    public void Update()
    {
        if (Enemy == null)
        {
            Destroy(gameObject);
        }

        healthbarImage.fillAmount = enemybase.CurrnetHP / enemybase.HP; //ü�� ���� ���� ��Ʈ�ٰ� 1�� �ڿ� ������Ʈ

        if(healthbarImage.fillAmount != hitbarImage.fillAmount)
        {

            float prevValue = hitbarImage.fillAmount; //1f
            float delta = prevValue / 100f; // 0.01

            elaspedTime += Time.deltaTime * 2;
            if (elaspedTime >= 0.01f)
            {
                elaspedTime = 0f;
                prevValue -= (delta * 2f); //0.8- 0.008 // 0.01 ���
                hitbarImage.fillAmount = prevValue;  // 0.01���� ����ȭ 
                if(prevValue - healthbarImage.fillAmount < 0.001f)
                {
                    hitbarImage.fillAmount = healthbarImage.fillAmount;
                    return;
                }

            }


        }

       



        if (Enemy == null || ui == null)
        {
            return;
        }

        if (transform == null || transform.gameObject == null || this == null)
        {
            return;
        }

        if (transform.parent == null)
        {
            Destroy(gameObject.GetComponent<RectTransform>());
        }

        if (enemybase._eminontpye != EMINIONTYPE.Netural)
        {
            transform.position = new Vector3(Enemy.position.x,Enemy.position.y + 5f, Enemy.position.z);
        }
        else if (enemybase._eminontpye == EMINIONTYPE.Netural)
        {
            transform.position = new Vector3(Enemy.position.x, 17f, Enemy.position.z);
        }
        
        if(enemybase._capsuleCollider.height >= 2f)
        {
            transform.position = new Vector3(Enemy.position.x, Enemy.position.y + 8f, Enemy.position.z);
        }
        
    }

    

}