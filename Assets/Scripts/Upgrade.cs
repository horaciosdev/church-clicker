using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Upgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float baseCost = 10f;
    public float factor = 1.5f;
    public int quantity = 0;
    public float incomePerSecond = 0.1f;

    public string upgradeName = "Upgrade";
    public string description = "Upgrade description!";

    public TextMeshProUGUI upgradeNameTMP;
    public TextMeshProUGUI upgradeDescriptionTMP;
    public TextMeshProUGUI upgradeQtdTMP;
    public TextMeshProUGUI upgradeCostTMP;

    public GameObject disableOverlayImage;

    private Money money;

    private Color originalColor; // Para armazenar a cor original da imagem
    public Color hoverColor;
    private Image upgradeImage; // Referência para o componente Image

    void Start()
    {
        GameObject gameObject = GameObject.Find("Money");
        money = gameObject.GetComponent<Money>();

        upgradeImage = GetComponent<Image>(); // Obtenha a referência ao componente Image
        if (upgradeImage != null)
        {
            originalColor = upgradeImage.color; // Salve a cor original
        }

        int currentCost = GetCurrentCost();

        upgradeNameTMP.text = upgradeName;
        upgradeDescriptionTMP.text = description;

        upgradeCostTMP.text = "$ " + currentCost.ToString();
        upgradeQtdTMP.text = quantity.ToString();

        StartCoroutine(StartGenerate());
    }

    void Update()
    {
        int currentCost = GetCurrentCost();

        // Verifica se o jogador tem dinheiro suficiente para comprar o upgrade
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
        int currentCost = GetCurrentCost();

        if (money.GetMoney() < currentCost) return;

        money.RemoveMoney(currentCost);

        quantity++;

        // Atualiza o texto
        this.upgradeQtdTMP.text = quantity.ToString();
        upgradeCostTMP.text = "$ " + GetCurrentCost().ToString();

        SaveManager.instance.SaveGame();
        ToastCreator.CreateToast("Novo upgrade adquirido!", "top-center");
    }

    // Implementação correta de OnPointerEnter
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (upgradeImage != null)
        {
            upgradeImage.color = hoverColor;
        }
    }

    // Implementação correta de OnPointerExit
    public void OnPointerExit(PointerEventData eventData)
    {
        if (upgradeImage != null)
        {
            upgradeImage.color = originalColor; // Restaura a cor original
        }
    }

    IEnumerator StartGenerate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (quantity > 0)
            {
                money.AddMoney(GetProductionPerSecond());
            }
        }
    }

    public int GetCurrentCost()
    {
        return (int)(baseCost * Mathf.Pow(factor, quantity));
    }

    public float GetProductionPerSecond()
    {
        return incomePerSecond * quantity;
    }
}