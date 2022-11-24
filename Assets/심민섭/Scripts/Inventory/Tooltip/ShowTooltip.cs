using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{         //Tooltip

    public Tooltip tooltip;                                     //The tooltip script
    public GameObject tooltipGameObject;                        //the tooltip as a GameObject
    public RectTransform canvasRectTransform;                    //the panel(Inventory Background) RectTransform
    public RectTransform tooltipRectTransform;                  //the tooltip RectTransform
    private Item item;


    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
        {
            tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
            tooltipGameObject = GameObject.FindGameObjectWithTag("Tooltip");
            tooltipRectTransform = tooltipGameObject.GetComponent<RectTransform>() as RectTransform;
        }
        canvasRectTransform = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>() as RectTransform;
    }




    public void OnPointerEnter(PointerEventData data)                               //if you hit a item in the slot
    {
        if (tooltip != null)
        {
            item = GetComponent<ItemOnObject>().item;                   //we get the item
            tooltip.item = item;                                        //set the item in the tooltip
            tooltip.activateTooltip();                                  //set all informations of the item in the tooltip
            if (canvasRectTransform == null)
                return;


            Vector3[] slotCorners = new Vector3[4];                     //get the corners of the slot
            GetComponent<RectTransform>().GetWorldCorners(slotCorners); //get the corners of the slot                

            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, slotCorners[3], data.pressEventCamera, out localPointerPosition))   // and set the localposition of the tooltip...
            {
                /*if (transform.parent.Get.GetComponent<Hotbar>() == null)
                    tooltipRectTransform.localPosition = localPointerPosition;
                else*/
                //tooltipRectTransform.localPosition = new Vector3(localPointerPosition.x, localPointerPosition.y + tooltip.tooltipHeight);
            }
        }

        if (this.gameObject.transform.parent.parent.parent.gameObject.tag == "EquipmentSystem")
        {
            tooltip.deactivateTooltip();
        }

    }

    public void OnPointerExit(PointerEventData data)
    {
        if (tooltip != null)
            tooltip.deactivateTooltip();
    }

}
