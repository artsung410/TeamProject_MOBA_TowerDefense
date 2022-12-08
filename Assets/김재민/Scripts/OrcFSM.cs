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
    int attackHandler; // 어택핸들러

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
        gameObject.tag = "Untagged"; //태그 바꿈
        transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = false; //처음에 데미지트리거끔
        transform.GetChild(1).gameObject.SetActive(false); // 이펙트
        InvokeRepeating("Targetdetection", 1f, 0.5f); // 감지함수 
        ResetPos = GameObject.FindGameObjectWithTag("Center").gameObject.transform; // 타켓없을때 중앙포지션

    }

    public void StateChange()
    {
        switch (_estate) // 코루틴순서  
        {
            case ESTATE.Attack:
                if (Imove != null)
                {

                    StopCoroutine(Imove);  // 3 
                }
                StartCoroutine(attack()); // 코루틴생성 4 
                Iattack = attack(); //  5코루틴 넣어줌 넣어주는 이유 삭제를 해주기 위해서 start코루틴할때마다 다른 주소값의 코루틴을 실행시킴
                break;

            default:
                if (Iattack != null)
                {
                    StopCoroutine(Iattack); // 있으면 삭제 0 
                }
                StartCoroutine(move()); // 시작  1
                Imove = move(); // 코루틴 저장 2
                break;
        }

    }
    private IEnumerator move() // 1 
    {
        while (_estate == ESTATE.Move) // 
        {
            _navMeshAgent.isStopped = false;
            
            if (Target == null) // 타겟이 널이면
            {
                TargetOn = false; //타켓온 해제
                Target = ResetPos.transform; // 중심포지션으로 바꿈
            }

            transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z)); //타켓을 바라봄
            _navMeshAgent.SetDestination(Target.position); // 타겟쪽으로 걸어감
            float distanceNosql = Vector2.Distance(new Vector2(Target.transform.position.x, Target.transform.position.z), new Vector2(transform.position.x, transform.position.z)); // 거리구함
            if (distanceNosql >= attackRange * 2) // 거리가 공격범위에 2배면 TargetNull로 만들엇 ㅓ센터가게끔 설정
            {
                Target = null;
            }

            if (distanceNosql <= attackRange && Target != ResetPos) //공격사거리거나 센터가 아닐때만 공격
            {
                _estate = ESTATE.Attack; //공격상태로 전환
                StateChange(); // FSM실행
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

            transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z)); //타켓을 바라봄
            float atkDistanceNoSql = Vector2.Distance(new Vector2(Target.transform.position.x, Target.transform.position.z), new Vector2(transform.position.x, transform.position.z));
            _navMeshAgent.isStopped = true; //멈춰서
            Animationact("Attack", true); // 공격애니메이션 재생
            if (atkDistanceNoSql >= attackRange) //공격사거리가 멀어지면
            {
                Animationact("Attack", false); // 공격애니메이션 끄고 
                _estate = ESTATE.Move; // 무브상태로 전환 따라가야하기때문
                TargetOn = false;
                StateChange();
                break;
            }
            yield return null;
        }
    }
    private void Targetdetection() // 타켓 범위 감지
    {
        if (TargetOn == true) // 타켓있으면 안돌아감 
        {
            return;
        }
        Collider[] Enemys = Physics.OverlapSphere(transform.position, 20f, mask);
        foreach (Collider col in Enemys)
        {
            if (col.gameObject.name == "monster_orc") // 자신제외하고
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

    private void Animationact(string animation, bool value) //애니메이션 동기화
    {
        photonView.RPC(nameof(RPC_Animaion), RpcTarget.All, animation, value);
    }


    [PunRPC]
    private void RPC_Animaion(string animation, bool value)
    {
        _animator.SetBool(animation, value);
    }

    public void BoxColliderON() //데미지 박스트리거 on
    {
        transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).GetComponent<BoxCollider>().enabled = true;
    }


    public void BoxColliderOff() // 데미지 박스 트리거 off
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
