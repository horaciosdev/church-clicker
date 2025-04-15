using System;
using System.Collections;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Chaves para identificar os valores salvos no PlayerPrefs
    private const string TOTAL_MONEY_KEY = "TotalMoneyDouble";
    private const string UPGRADE_QUANTITY_PREFIX = "UpgradeQuantity_";
    private const string LAST_SAVE_TIME_KEY = "LastSaveTime";

    // Referência ao script Money para acessar e modificar o total de money
    private Money money;

    // Referência aos scripts de Upgrade
    public Upgrade[] upgrades;

    // Singleton para facilitar o acesso ao SaveManager de outros scripts
    public static SaveManager instance;

    void Awake()
    {
        // Garante que haja apenas uma instância do SaveManager
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null, false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Encontra o script Money
        GameObject moneyObject = GameObject.Find("Money");
        if (moneyObject != null)
        {
            money = moneyObject.GetComponent<Money>();
        }
        else
        {
            Debug.LogError("Objeto 'Money' não encontrado na cena!");
        }

        // Carrega os dados do jogo ao iniciar
        LoadGame();
    }

    // Função para salvar os dados do jogo
    public void SaveGame()
    {
        // Salva o total de money
        if (money != null)
        {
            PlayerPrefs.SetString(TOTAL_MONEY_KEY, money.totalMoney.ToString("G17")); // Usando G17 para máxima precisão
        }

        // Salva a quantidade de cada upgrade
        if (upgrades != null)
        {
            foreach (Upgrade upgrade in upgrades)
            {
                PlayerPrefs.SetInt(UPGRADE_QUANTITY_PREFIX + upgrade.upgradeName, upgrade.quantity);
            }
        }

        // Salva o timestamp atual
        string currentTime = DateTime.UtcNow.ToBinary().ToString();
        PlayerPrefs.SetString(LAST_SAVE_TIME_KEY, currentTime);

        PlayerPrefs.Save();
    }

    // Função para carregar os dados do jogo
    public void LoadGame()
    {
        // Carrega o timestamp do último save
        string lastSaveTimeString = PlayerPrefs.GetString(LAST_SAVE_TIME_KEY, string.Empty);
        double offlineGains = 0.0;

        // Calcula o tempo offline
        if (!string.IsNullOrEmpty(lastSaveTimeString))
        {
            try
            {
                DateTime lastSaveTime = DateTime.FromBinary(long.Parse(lastSaveTimeString));
                DateTime currentTime = DateTime.UtcNow;
                double secondsElapsed = (currentTime - lastSaveTime).TotalSeconds;

                // Limita o tempo offline máximo para 24 horas (86400 segundos)
                secondsElapsed = Math.Min(secondsElapsed, 86400.0);

                // Carrega os upgrades primeiro, para calcular os ganhos offline
                if (upgrades != null)
                {
                    foreach (Upgrade upgrade in upgrades)
                    {
                        // Carrega a quantidade de cada upgrade
                        upgrade.quantity = PlayerPrefs.GetInt(UPGRADE_QUANTITY_PREFIX + upgrade.upgradeName, 0);

                        // Calcula os ganhos offline deste upgrade
                        double upgradeProduction = upgrade.GetProductionPerSecond() * upgrade.quantity;
                        offlineGains += upgradeProduction * secondsElapsed;

                        // Atualiza a UI do upgrade
                        upgrade.upgradeQtdTMP.text = upgrade.quantity.ToString();
                        upgrade.upgradeCostTMP.text = money.FormatMoney(upgrade.GetCurrentCost());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Erro ao processar timestamp: " + e.Message);
            }
        }

        // Carrega e atualiza o total de dinheiro
        if (money != null)
        {
            string savedMoney = PlayerPrefs.GetString(TOTAL_MONEY_KEY, "0");
            if (double.TryParse(savedMoney, out double loadedMoney))
            {
                money.totalMoney = loadedMoney;
            }
            else
            {
                Debug.LogError("Erro ao carregar dinheiro!");
                money.totalMoney = 0;
            }

            // Adiciona os ganhos offline
            if (offlineGains > 0)
            {
                money.AddMoney(offlineGains);
                ShowOfflineGainsMessage(offlineGains);
            }
        }

        // Inicia o salvamento automático
        StartCoroutine(AutomaticSave());

        ToastCreator.CreateToast("Jogo Carregado!", "bottom-left");
    }

    // Função auxiliar para mostrar mensagem de ganhos offline
    private void ShowOfflineGainsMessage(double gains)
    {
        ToastCreator.CreateToast($"Bem-vindo de volta! \n Você ganhou: \n {money.FormatMoney(gains)}", "top-center", 2f);
    }

    // Função para resetar os dados salvos
    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();

        // Redefine os valores no script Money
        if (money != null)
        {
            money.totalMoney = 0.0;
            money.UpdateMoneyString();
        }

        // Redefine as quantidades dos upgrades
        if (upgrades != null)
        {
            foreach (Upgrade upgrade in upgrades)
            {
                upgrade.quantity = 0;
                upgrade.upgradeQtdTMP.text = "0";
                upgrade.upgradeCostTMP.text = money.FormatMoney(upgrade.GetCurrentCost());
            }
        }

        SaveGame(); // Salva os valores resetados
        ToastCreator.CreateToast("Save Resetado!", "bottom-left");
    }

    IEnumerator AutomaticSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f); // Salva a cada 60 segundos
            SaveGame();
        }
    }
}