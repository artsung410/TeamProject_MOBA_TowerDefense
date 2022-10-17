using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDataBaseList : ScriptableObject
{             //The scriptableObject where the Item getting stored which you create(ItemDatabase)

    [SerializeField]
    public List<Item> itemList = new List<Item>();              //List of it


    // 들어온 아이템과 DB에 있는 id를 비교하고 있으면 아이템의 오브젝트를 복사해서 리턴해주는 함수
    public Item getItemByID(int id)
    {
        // 아이템 데이터베이스가 가지고 있는 아이템의 id 갯수 만큼 반복
        for (int i = 0; i < itemList.Count; i++)
        {
            // 같은 id가 있으면
            if (itemList[i].itemID == id)
            {
                // 같은것이 있으면 해당 id의 오브젝트를 복사해서 리턴해준다.
                return itemList[i].getCopy();
            }
        }
        return null;
    }

    public Item getItemByName(string name)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemName.ToLower().Equals(name.ToLower()))
                return itemList[i].getCopy();
        }
        return null;
    }
}
