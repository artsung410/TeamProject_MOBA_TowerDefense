using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool doMovement = true;

    [Header("ī�޶� �̵� �ӵ�")]
    public float panSpeed = 30f;

    [Header("ī�޶� ����")]
    public float panBorderThickness = 10f;

    [Header("ī�޶� Zoom In/Out �ӵ�")]
    public float scrollSpeed = 5f;

    [Header("ī�޶� Zoom �ּڰ�")]
    public float minY = 10f;

    [Header("ī�޶� Zoom �ִ�")]
    public float maxY = 80f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            doMovement = !doMovement;
        }

        if(!doMovement)
        {
            return;
        }


        // ī�޶� ���콺 ���󰡰�, ����Ű�� �����̰Բ� ����.
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }


        // wna
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;

        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
  