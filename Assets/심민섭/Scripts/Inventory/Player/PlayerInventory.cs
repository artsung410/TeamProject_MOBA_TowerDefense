using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // 입력 관리자
    public InputManager inputManagerDatabase;

    public GameObject craftSystem;
    public GameObject characterSystem;

    private CraftSystem cS;

    // 인벤토리 게임 오브젝트
    public GameObject inventory;

    // 제작, 케릭터장비창 모두 인벤토리를 가지고 있다.
    private Inventory craftSystemInventory;
    private Inventory characterSystemInventory;

    // 인벤토리 스크립트를 가져옴
    private Inventory mainInventory;

    // 기획팀 사운드 작업본(인벤토리 클릭 효과음)
    public AudioClip clickSound;
    AudioSource audioSource;

    [SerializeField]
    private DragSlotsColorChange dragSlotsColorChange;

    // 경고문
    [SerializeField]
    private GameObject backWarning1;

    [SerializeField]
    private GameObject backwarning2;

    void Start()
    {
        if (inventory != null)
            mainInventory = inventory.GetComponent<Inventory>();

        if (characterSystem != null)
            characterSystemInventory = characterSystem.GetComponent<Inventory>();

        if (craftSystem != null)
            craftSystemInventory = craftSystem.GetComponent<Inventory>();

        if (craftSystem != null)
            cS = craftSystem.GetComponent<CraftSystem>();

        this.audioSource = GetComponent<AudioSource>();
    }

    // 가방 클릭 할 때만 쓸거임
    public void openInventory()
    {
        // 기획팀 사운드 작업본(인벤토리 클릭 효과음)
        audioSource.clip = clickSound;
        audioSource.Play();

        mainInventory.openInventory();
        characterSystemInventory.openInventory();
    }


    // 백버튼을 바꾸다니..에휴
    public void BackButton()
    {
        mainInventory.closeInventory();
        characterSystemInventory.closeInventory();
    }

    // 인벤토리의 백버튼 눌렀을떄
    public void closeInventory()
    {
        // 기획팀 사운드 작업본(인벤토리 클릭 효과음)
        audioSource.clip = clickSound;
        audioSource.Play();

        int stack = 0;
        // 아이템이 모두 장착되어 있는지 확인한다.
        for (int i = 0; i < characterSystem.transform.GetChild(1).childCount; i++)
        {
            if (characterSystem.transform.GetChild(1).GetChild(i).childCount != 0)
            {
                stack++;
            }
        }
        if (stack == 8)
        {
            //mainInventory.closeInventory();
            //characterSystemInventory.closeInventory();
            // 경고창 팝업
            backwarning2.SetActive(true);
            EquimentInventory.instance.setItemComplited = true;
            EquimentInventory.instance.AllItemCheck();
        }
        else
        {
            // 경고문 팝업
            backWarning1.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // 왼쪽 마우스 클릭 이벤트있으면 호출할 떄 사용할 거임
        if (Input.GetMouseButton(0))
        {
            dragSlotsColorChange.SlotsColorChange();
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragSlotsColorChange.SlotsWhiteColorChange();
        }
        
        // InputManager의 InventoryKeyCode "B"
        if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
        {
            // 인벤토리가 닫혀있으면
            if (!inventory.activeSelf && !characterSystem.activeSelf)
            {
                // 인벤토리를 연다.
                mainInventory.openInventory();
                characterSystemInventory.openInventory();
            }
            else
            {
                // 인벤토리를 닫는다.
                mainInventory.closeInventory();
                characterSystemInventory.closeInventory();
            }
        }

        // InputManager의 CraftSystemKeyCode "K"
        if (Input.GetKeyDown(inputManagerDatabase.CraftSystemKeyCode))
        {
            if (!craftSystem.activeSelf)
            {
                craftSystemInventory.openInventory();
            }
            else
            {
                // CraftSystem 스크립트를 가져오는데 성공했다면
                if (cS != null)
                {
                    // 
                    cS.backToInventory();
                }
                /*if (toolTip != null)
                    toolTip.deactivateTooltip();*/
                craftSystemInventory.closeInventory();
            }
        }
    }
}
