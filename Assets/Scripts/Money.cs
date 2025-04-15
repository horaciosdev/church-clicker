using TMPro;
using UnityEngine;
using System;

public class Money : MonoBehaviour
{
    private double currentMoney = 0;
    public double totalMoney = 0;
    public TextMeshProUGUI totalMoneyStr;
    public float smoothTime = 0.3f;
    private double velocity = 0;

    // Configurações de formatação
    private readonly string[] suffixes = { "", " k", " mi", " bi", " tri" };
    private const double MAX_MONEY = 666_666_000_000_000; // 666.666 trilhões

    void Start()
    {
        currentMoney = totalMoney;
        UpdateMoneyString();
    }

    void Update()
    {
        if (Math.Abs(currentMoney - totalMoney) > 0.001)
        {
            // Implementação customizada de SmoothDamp para double
            double diff = totalMoney - currentMoney;
            double change = diff * Time.deltaTime / smoothTime;

            // Limitar a velocidade máxima da mudança
            if (Math.Abs(change) > Math.Abs(diff))
                change = diff;

            currentMoney += change;

            // Garantir que não exceda os limites
            if (currentMoney > MAX_MONEY)
                currentMoney = MAX_MONEY;

            UpdateMoneyString();
        }
    }

    public double GetMoney()
    {
        return totalMoney;
    }

    public void AddMoney(double amount)
    {
        totalMoney = Math.Min(totalMoney + amount, MAX_MONEY);
    }

    public void RemoveMoney(double amount)
    {
        totalMoney = Math.Max(totalMoney - amount, 0);
    }

    public void UpdateMoneyString()
    {
        totalMoneyStr.text = FormatMoney(currentMoney);
    }

    public string FormatMoney(double amount)
    {
        int suffixIndex = 0;

        // Determina qual sufixo usar baseado no tamanho do número
        while (amount >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            amount /= 1000;
            suffixIndex++;
        }

        // Para valores menores que 1000, mostra sem casas decimais
        if (suffixIndex == 0)
        {
            return $"$ {amount:0}";
        }
        // Para valores maiores, mostra com 3 casas decimais
        else
        {
            return $"$ {amount:0.000}{suffixes[suffixIndex]}";
        }
    }
}