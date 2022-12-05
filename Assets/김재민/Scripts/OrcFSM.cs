using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class OrcFSM : Enemybase
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    public Transform Target;
    private Transform ResetPos;
    [SerializeField]
    private bool TargetOn = false;

    public GameObject HealthUI;
    public Image healthbarImage;
    public Image hitbarImage;
    public TextMeshProUGUI HealthText1;
    public TextMeshProUGUI HealthText2;

    int mask = (1 << 7);


    WaitForSeconds Delay100 = new WaitForSeconds(1f);
    WaitForSeconds Delay200 = new WaitForSeconds(2f);

    public enum ESTATE
    {
        Move,
        Attack,

    }

    public ESTATE _estate;
    public IEnumerator IstateChange;
    private IEnumerator Imove;
    private IEnumerator Iattack;
    int attackHandler; // �����ڵ鷯

    private bool activity = false;
    protected override void Awake()
    {
        base.Awake();

        _estate = ESTATE.Move;
        CurrnetHP = HP;

        setNeturalMonsterHealthBar();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Target = null;
        _estate = ESTATE.Move;

    }

    void Start()
    {
        Iattack = attack();
        Imove = move();
        gameObject.tag = "Untagged"; //�±� �ٲ�
        transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = false; //ó���� ������Ʈ���Ų�
        transform.GetChild(1).gameObject.SetActive(false); // ����Ʈ
        InvokeRepeating("Targetdetection", 1f, 0.5f); // �����Լ� 
        ResetPos = GameObject.FindGameObjectWithTag("Center").gameObject.transform; // Ÿ�Ͼ����� �߾�������

    }

    public void StateChange()
    {
        switch (_estate) // �ڷ�ƾ����  
        {
            case ESTATE.Attack:
                if (Imove != null)
                {

                    StopCoroutine(Imove);  // 3 
                }
                StartCoroutine(attack()); // �ڷ�ƾ���� 4 
                Iattack = attack(); //  5�ڷ�ƾ �־��� �־��ִ� ���� ������ ���ֱ� ���ؼ� start�ڷ�ƾ�Ҷ����� �ٸ� �ּҰ��� �ڷ�ƾ�� �����Ŵ
                break;

            default:
                if (Iattack != null)
                {
                    StopCoroutine(Iattack); // ������ ���� 0 
                }
                StartCoroutine(move()); // ����  1
                Imove = move(); // �ڷ�ƾ ���� 2
                break;
        }

    }
    private IEnumerator move() // 1 
    {
        while (_estate == ESTATE.Move) // 
        {
            _navMeshAgent.isStopped = false;
            
            if (Target == null) // Ÿ���� ���̸�
            {
                TargetOn = false; //Ÿ�Ͽ� ����
                Target = ResetPos.transform; // �߽����������� �ٲ�
            }

            transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z)); //Ÿ���� �ٶ�
            _navMeshAgent.SetDestination(Target.position); // Ÿ�������� �ɾ
            float distanceNosql = Vector2.Distance(new Vector2(Target.transform.position.x, Target.transform.position.z), new Vector2(transform.position.x, transform.position.z)); // �Ÿ�����
            if (distanceNosql >= attackRange * 2) // �Ÿ��� ���ݹ����� 2��� TargetNull�� ����� �ü��Ͱ��Բ� ����
            {
                Target = null;
            }

            if (distanceNosql <= attackRange && Target != ResetPos) //���ݻ�Ÿ��ų� ���Ͱ� �ƴҶ��� ����
            {
                _estate = ESTATE.Attack; //���ݻ��·� ��ȯ
                StateChange(); // FSM����
                break;
            }
            else if (distanceNosql <= attackRange && Target == ResetPos)
            {
                
                TargetOn = false; 
                _animator.SetBool("Idle",true);
                transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.transform.position.z));
                
            }
            else
            {
                _animator.SetBool("Idle",false);
            }

            

            yield return null;
        }
    }

    private IEnumerator attack()
    {
        while (_estate == ESTATE.Attack)
        {
            
            if (Target == null)
            {
                _estate = ESTATE.Move;
                StateChange();

            }

            transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z)); //Ÿ���� �ٶ�
            float atkDistanceNoSql = Vector2.Distance(new Vector2(Target.transform.position.x, Target.transform.position.z), new Vector2(transform.position.x, transform.position.z));
            _navMeshAgent.isStopped = true; //���缭
            Animationact("Attack", true); // ���ݾִϸ��̼� ���
            if (atkDistanceNoSql >= attackRange) //���ݻ�Ÿ��� �־�����
            {
                Animationact("Attack", false); // ���ݾִϸ��̼� ���� 
                _estate = ESTATE.Move; // ������·� ��ȯ ���󰡾��ϱ⶧��
                TargetOn = false;
                StateChange();
                break;
            }
            yield return null;
        }
    }
    private void Targetdetection() // Ÿ�� ���� ����
    {
        if (TargetOn == true) // Ÿ�������� �ȵ��ư� 
        {
            return;
        }
        Collider[] Enemys = Physics.OverlapSphere(transform.position, 20f, mask);
        foreach (Collider col in Enemys)
        {
            if (col.gameObject.name == "monster_orc") // �ڽ������ϰ�
            {
                continue;
            }

            if (col.gameObject.layer == 7)

            {
                Target = Enemys[0].transform; // 
                float shortDistacenosql = Vector2.SqrMagnitude(new Vector2(Target.transform.position.x, Target.transform.position.z) - new Vector2(transform.position.x, transform.position.z));
                float distanceNoSql = Vector2.SqrMagnitude(new Vector2(col.transform.position.x, col.transform.position.z) - new Vector2(transform.position.x, transform.position.z));
                float shortDistance = shortDistacenosql * shortDistacenosql;
                float distance = distanceNoSql * distanceNoSql;
                if (distance < shortDistance)
                {
                    Target = col.transform;
                    shortDistance = distance;
                    _animator.SetBool("Idle", false);
                    TargetOn = true;

                }

            }

        }
       

    }

    private void Animationact(string animation, bool value) //�ִϸ��̼� ����ȭ
    {
        photonView.RPC(nameof(RPC_Animaion), RpcTarget.All, animation, value);
    }


    [PunRPC]
    private void RPC_Animaion(string animation, bool value)
    {
        _animator.SetBool(animation, value);
    }

    public void BoxColliderON() //������ �ڽ�Ʈ���� on
    {
        transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).GetComponent<BoxCollider>().enabled = true;
    }


    public void BoxColliderOff() // ������ �ڽ� Ʈ���� off
    {
        transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;
    }

    public void setNeturalMonsterHealthBar()
    {
        healthbarImage.fillAmount = CurrnetHP / HP;

        if ((CurrnetHP <= 0))
        {
            HealthText1.text = HealthText2.text = $"0 / 0";
        }
        else
        {
            HealthText1.text = HealthText2.text = $"{ CurrnetHP} / {HP}";
        }

        StartCoroutine(ApplyHitBar(healthbarImage.fillAmount));
    }

    private IEnumerator ApplyHitBar(float value)
    {
        float prevValue = hitbarImage.fillAmount;
        float delta = prevValue / 400f;

        while (true)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            prevValue -= delta;
            hitbarImage.fillAmount = prevValue;

            if (prevValue - value < 0.001f)
            {
                hitbarImage.fillAmount = healthbarImage.fillAmount;
                break;
            }
        }
    }
}
