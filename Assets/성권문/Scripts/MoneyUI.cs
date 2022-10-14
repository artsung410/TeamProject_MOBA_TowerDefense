using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################
public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    private void Update()
    {
        moneyText.text = "&" + PlayerStats.Money.ToString();
    }
}
