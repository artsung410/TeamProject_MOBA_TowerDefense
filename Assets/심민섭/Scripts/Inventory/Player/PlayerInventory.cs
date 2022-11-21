using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // �Է� ������
    public InputManager inputManagerDatabase;

    public GameObject craftSystem;
    public GameObject characterSystem;

    private CraftSystem cS;

    // �κ��丮 ���� ������Ʈ
    public GameObject inventory;

    // ����, �ɸ������â ��� �κ��丮�� ������ �ִ�.
    private Inventory craftSystemInventory;
    private Inventory characterSystemInventory;

    // �κ��丮 ��ũ��Ʈ�� ������
    private Inventory mainInventory;

    // ��ȹ�� ���� �۾���(�κ��丮 Ŭ�� ȿ����)
    public AudioClip clickSound;
    AudioSource audioSource;

    [SerializeField]
    private DragSlotsColorChange dragSlotsColorChange;

    // ���
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

    // ���� Ŭ�� �� ���� ������
    public void openInventory()
    {
        // ��ȹ�� ���� �۾���(�κ��丮 Ŭ�� ȿ����)
        audioSource.clip = clickSound;
        audioSource.Play();

        mainInventory.openInventory();
        characterSystemInventory.openInventory();
    }


    // ���ư�� �ٲٴٴ�..����
    public void BackButton()
    {
        mainInventory.closeInventory();
        characterSystemInventory.closeInventory();
    }

    // �κ��丮�� ���ư ��������
    public void closeInventory()
    {
        // ��ȹ�� ���� �۾���(�κ��丮 Ŭ�� ȿ����)
        audioSource.clip = clickSound;
        audioSource.Play();

        int stack = 0;
        // �������� ��� �����Ǿ� �ִ��� Ȯ���Ѵ�.
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
            // ���â �˾�
            backwarning2.SetActive(true);
            EquimentInventory.instance.setItemComplited = true;
            EquimentInventory.instance.AllItemCheck();
        }
        else
        {
            // ��� �˾�
            backWarning1.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // ���� ���콺 Ŭ�� �̺�Ʈ������ ȣ���� �� ����� ����
        if (Input.GetMouseButton(0))
        {
            dragSlotsColorChange.SlotsColorChange();
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragSlotsColorChange.SlotsWhiteColorChange();
        }
        
        // InputManager�� InventoryKeyCode "B"
        if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
        {
            // �κ��丮�� ����������
            if (!inventory.activeSelf && !characterSystem.activeSelf)
            {
                // �κ��丮�� ����.
                mainInventory.openInventory();
                characterSystemInventory.openInventory();
            }
            else
            {
                // �κ��丮�� �ݴ´�.
                mainInventory.closeInventory();
                characterSystemInventory.closeInventory();
            }
        }

        // InputManager�� CraftSystemKeyCode "K"
        if (Input.GetKeyDown(inputManagerDatabase.CraftSystemKeyCode))
        {
            if (!craftSystem.activeSelf)
            {
                craftSystemInventory.openInventory();
            }
            else
            {
                // CraftSystem ��ũ��Ʈ�� �������µ� �����ߴٸ�
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
