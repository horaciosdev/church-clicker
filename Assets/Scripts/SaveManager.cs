using System.Collections;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Chaves para identificar os valores salvos no PlayerPrefs
    private const string TOTAL_DIZIMO_KEY = "TotalDizimo";
    private const string UPGRADE_QUANTITY_PREFIX = "UpgradeQuantity_"; // Usaremos um prefixo para salvar as quantidades dos upgrades
    private const string LAST_SAVE_TIME_KEY = "LastSaveTime";

    // Referência ao script Dizimos para acessar e modificar o total de dizimo
    private Dizimos dizimos;

    // Referência aos scripts de Upgrade (você precisará configurar isso no Inspector)
    public Upgrade[] upgrades;

    // Singleton para facilitar o acesso ao SaveManager de outros scripts
    public static SaveManager instance;

    void Awake()
    {
        // Garante que haja apenas uma instância do SaveManager
        if (instance == null)
        {
            instance = this;

            // Garante que o GameObject está na raiz da hierarquia
            transform.SetParent(null, false); // Substitui transform.parent = null

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Encontra o script Dizimos
        GameObject dizimosObject = GameObject.Find("Dizimos");
        if (dizimosObject != null)
        {
            dizimos = dizimosObject.GetComponent<Dizimos>();
        }
        else
        {
            Debug.LogError("Objeto 'Dizimos' não encontrado na cena!");
        }

        // Carrega os dados do jogo ao iniciar
        LoadGame();
    }

    // Função para salvar os dados do jogo
    public void SaveGame()
    {
        // Salva o total de dizimo
        if (dizimos != null)
        {
            PlayerPrefs.SetFloat(TOTAL_DIZIMO_KEY, dizimos.tootalDizimo);
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
        string currentTime = System.DateTime.UtcNow.ToBinary().ToString();
        PlayerPrefs.SetString(LAST_SAVE_TIME_KEY, currentTime);

        // É importante chamar Save() para garantir que os dados sejam escritos no disco
        PlayerPrefs.Save();

        ToastCreator.CreateToast("Jogo Salvo!");
    }

    // Função para carregar os dados do jogo
    public void LoadGame()
    {
        // 1. Primeiro carregamos o timestamp do último save
        string lastSaveTimeString = PlayerPrefs.GetString(LAST_SAVE_TIME_KEY, string.Empty);
        float offlineGains = 0f;

        // 2. Calculamos o tempo offline
        if (!string.IsNullOrEmpty(lastSaveTimeString))
        {
            System.DateTime lastSaveTime = System.DateTime.FromBinary(long.Parse(lastSaveTimeString));
            System.DateTime currentTime = System.DateTime.UtcNow;
            double secondsElapsed = (currentTime - lastSaveTime).TotalSeconds;

            // Limita o tempo offline máximo para 24 horas (86400 segundos)
            secondsElapsed = Mathf.Min((float)secondsElapsed, 86400f);

            // 3. Carregamos os upgrades primeiro, pois precisamos deles para calcular os ganhos offline
            if (upgrades != null)
            {
                foreach (Upgrade upgrade in upgrades)
                {
                    // Carrega a quantidade de cada upgrade
                    upgrade.quantity = PlayerPrefs.GetInt(UPGRADE_QUANTITY_PREFIX + upgrade.upgradeName, 0);

                    // Calcula os ganhos offline deste upgrade
                    float upgradeProduction = upgrade.GetProductionPerSecond() * upgrade.quantity;
                    offlineGains += upgradeProduction * (float)secondsElapsed;

                    // Atualiza a UI do upgrade
                    upgrade.upgradeQtdTMP.text = upgrade.quantity.ToString();
                    upgrade.upgradeCostTMP.text = "$ " + upgrade.GetCurrentCost().ToString();
                }
            }
        }

        // 4. Carregamos e atualizamos o total de dízimo
        if (dizimos != null)
        {
            // Carrega o valor base salvo
            dizimos.tootalDizimo = PlayerPrefs.GetFloat(TOTAL_DIZIMO_KEY, 0f);

            // Adiciona os ganhos offline
            if (offlineGains > 0)
            {
                dizimos.tootalDizimo += offlineGains;
                ShowOfflineGainsMessage(offlineGains); // Você precisa criar esta função para mostrar a UI
            }

            // Atualiza a UI
            dizimos.AtualizarTextoDizimo();
        }

        // 5. Inicia o salvamento automático
        StartCoroutine(AutomaticSave());

        ToastCreator.CreateToast("Jogo Carregado!");
    }

    // Função auxiliar para mostrar mensagem de ganhos offline
    private void ShowOfflineGainsMessage(float gains)
    {
        // Aqui você pode implementar uma UI para mostrar os ganhos offline
        ToastCreator.CreateToast($"Bem-vindo de volta! Você ganhou ${gains:F2} enquanto esteve fora!");
    }

    // Função para resetar os dados salvos (útil para testes)
    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();

        // Redefine os valores no script Dizimos
        if (dizimos != null)
        {
            dizimos.tootalDizimo = 0f;
            dizimos.AtualizarTextoDizimo();
        }

        // Redefine as quantidades dos upgrades
        if (upgrades != null)
        {
            foreach (Upgrade upgrade in upgrades)
            {
                upgrade.quantity = 0;
                upgrade.upgradeQtdTMP.text = "0";
                upgrade.upgradeCostTMP.text = "$ " + upgrade.GetCurrentCost().ToString();
            }
        }

        SaveGame(); // Salva os valores resetados
        ToastCreator.CreateToast("Save Resetado!");
        LoadGame(); // Recarrega os valores padrão (já definidos acima)
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