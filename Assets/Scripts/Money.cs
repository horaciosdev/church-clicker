using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    public float tootalMoney = 0f;
    public TextMeshProUGUI totalMoneyStr;

    public float GetMoney()
    {
        return tootalMoney;
    }

    public void AddMoney(float valor)
    {
        tootalMoney += valor;
        UpdateMoneyString();
    }

    public void RemoveMoney(int valor)
    {
        tootalMoney -= valor;
        UpdateMoneyString();
    }

    public void UpdateMoneyString()
    {
        totalMoneyStr.text = "$ " + ((int)tootalMoney).ToString();
    }
}
