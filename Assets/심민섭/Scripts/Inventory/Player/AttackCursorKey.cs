using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCursorKey : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    public Texture2D cursorTextureOriginal;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;


    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ChangeMouseAMode();
        }
        if (Input.GetMouseButtonDown(0))
            Cursor.SetCursor(cursorTextureOriginal, hotSpot, cursorMode);
    }

    public void ChangeMouseAMode()
    {
        // 커서를 변경하고
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
}
