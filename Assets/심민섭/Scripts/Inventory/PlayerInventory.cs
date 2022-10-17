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
    }

    // Update is called once per frame
    void Update()
    {
        // InputManager�� InventoryKeyCode "I"
        if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
        {
            // �κ��丮�� ����������
            if (!inventory.activeSelf)
            {
                // �κ��丮�� ����.
                mainInventory.openInventory();
            }
            else
            {
                // �κ��丮�� �ݴ´�.
                mainInventory.closeInventory();
            }
        }

        // InputManager�� CharacterSystemKeyCode "C"
        if (Input.GetKeyDown(inputManagerDatabase.CharacterSystemKeyCode))
        {
            if (!characterSystem.activeSelf)
            {
                characterSystemInventory.openInventory();
            }
            else
            {
                /*if (toolTip != null)
                    toolTip.deactivateTooltip();*/
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
