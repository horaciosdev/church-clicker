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

    private Dizimos dizimos;

    private Color originalColor; // Para armazenar a cor original da imagem
    public Color hoverColor;
    private Image upgradeImage; // Referência para o componente Image

    void Start()
    {
        GameObject gameObject = GameObject.Find("Dizimos");
        dizimos = gameObject.GetComponent<Dizimos>();

        upgradeImage = GetComponent<Image>(); // Obtenha a referência ao componente Image
        if (upgradeImage != null)
        {
            originalColor = upgradeImage.color; // Salve a cor original
        }

        int currentCost = getCurrentCost();

        upgradeNameTMP.text = upgradeName;
        upgradeDescriptionTMP.text = description;

        upgradeCostTMP.text = "$ " + currentCost.ToString();
        upgradeQtdTMP.text = quantity.ToString();

        StartCoroutine(StartGenerate());
    }

    void Update()
    {
        int currentCost = getCurrentCost();

        // Verifica se o jogador tem dinheiro suficiente para comprar o upgrade
        if (dizimos.GetDizimo() < currentCost)
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
        int currentCost = getCurrentCost();

        if (dizimos.GetDizimo() < currentCost) return;

        dizimos.RemoveDizimo(currentCost);

        quantity++;

        // Atualiza o texto
        this.upgradeQtdTMP.text = quantity.ToString();
        upgradeCostTMP.text = "$ " + getCurrentCost().ToString();
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
            if(quantity > 0)
            {
                dizimos.AddDizimo(incomePerSecond * quantity);
            }
        }
    }

    private int getCurrentCost()
    {
        return (int)(baseCost * Mathf.Pow(factor, quantity));
    }
}
