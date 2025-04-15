using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Upgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public double baseCost = 10.0;
    public double factor = 1.5;
    public int quantity = 0;
    public double incomePerSecond = 0.1;

    public string upgradeName = "Upgrade";
    public string description = "Upgrade description!";

    public TextMeshProUGUI upgradeNameTMP;
    public TextMeshProUGUI upgradeDescriptionTMP;
    public TextMeshProUGUI upgradeQtdTMP;
    public TextMeshProUGUI upgradeCostTMP;

    public GameObject disableOverlayImage;

    private Money money;

    private Color originalColor;
    public Color hoverColor;
    private Image upgradeImage;

    void Start()
    {
        GameObject moneyObject = GameObject.Find("Money");
        money = moneyObject.GetComponent<Money>();

        upgradeImage = GetComponent<Image>();
        if (upgradeImage != null)
        {
            originalColor = upgradeImage.color;
        }

        upgradeNameTMP.text = upgradeName;
        upgradeDescriptionTMP.text = description;

        upgradeCostTMP.text = money.FormatMoney(GetCurrentCost());
        upgradeQtdTMP.text = quantity.ToString();

        StartCoroutine(StartGenerate());
    }

    void Update()
    {
        double currentCost = GetCurrentCost();

        if (money.GetMoney() < currentCost)
        {
            disableOverlayImage.SetActive(true);
        }
        else
        {
            disableOverlayImage.SetActive(false);
        }
    }

    public void Click()
    {
        double currentCost = GetCurrentCost();

        if (money.GetMoney() < currentCost) return;

        money.RemoveMoney(currentCost);
        quantity++;

        // Atualiza o texto
        upgradeQtdTMP.text = quantity.ToString();
        upgradeCostTMP.text = money.FormatMoney(GetCurrentCost());

        SaveManager.instance.SaveGame();
        ToastCreator.CreateToast("Novo upgrade adquirido!", "top-center");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (upgradeImage != null)
        {
            upgradeImage.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (upgradeImage != null)
        {
            upgradeImage.color = originalColor;
        }
    }

    IEnumerator StartGenerate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (quantity > 0)
            {
                money.AddMoney(incomePerSecond * quantity);
            }
        }
    }

    public double GetCurrentCost()
    {
        return baseCost * System.Math.Pow(factor, quantity);
    }

    public double GetProductionPerSecond()
    {
        return incomePerSecond;
    }
}