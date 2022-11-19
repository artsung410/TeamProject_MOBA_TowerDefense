/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusorManager : MonoBehaviour
{
    // �̵� �Ϲ� Ŀ��
    public Texture2D cursorMoveNamal;
    // �̵� �Ʊ� Ŀ��
    public Texture2D cursorMoveAlly;
    // �̵� ���� Ŀ��
    public Texture2D cursorMoveEnemy;

    // ���� �Ϲ� Ŀ��
    public Texture2D cusorAttackNomal;
    // ���� �Ʊ� Ŀ��
    public Texture2D cusorAttackAlly;
    // ���� ���� Ŀ��
    public Texture2D cusorAttackEnemy;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        Cursor.SetCursor(cursorMoveNamal, hotSpot, cursorMode);
    }

    // SMS-------------------------------------------------------------------------------------
    private void OnMouseEnter()
    {
        if (photonView.IsMine)
        {
            Cursor.SetCursor(cursorMoveAlly, hotSpot, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(cursorMoveEnemy, hotSpot, CursorMode.ForceSoftware);
        }
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(cursorMoveNamal, hotSpot, CursorMode.ForceSoftware);
    }

    // SMS Start-------------------------------------------//
    public void ChangeMouseAMode()
    {
        Cursor.SetCursor(cusorAttackNomal, hotSpot, cursorMode);
    }
    // SMS End-----------------------------------------------//
}
*/