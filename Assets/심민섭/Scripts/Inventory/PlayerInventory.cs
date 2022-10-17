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
        // InputManager의 InventoryKeyCode "I"
        if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
        {
            // 인벤토리가 닫혀있으면
            if (!inventory.activeSelf)
            {
                // 인벤토리를 연다.
                mainInventory.openInventory();
            }
            else
            {
                // 인벤토리를 닫는다.
                mainInventory.closeInventory();
            }
        }

        // InputManager의 CharacterSystemKeyCode "C"
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
