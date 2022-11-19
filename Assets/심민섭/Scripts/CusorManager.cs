/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusorManager : MonoBehaviour
{
    // 이동 일반 커서
    public Texture2D cursorMoveNamal;
    // 이동 아군 커서
    public Texture2D cursorMoveAlly;
    // 이동 적군 커서
    public Texture2D cursorMoveEnemy;

    // 공격 일반 커서
    public Texture2D cusorAttackNomal;
    // 공격 아군 커서
    public Texture2D cusorAttackAlly;
    // 공격 적군 커서
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