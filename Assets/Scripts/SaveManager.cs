using System.Collections;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Chaves para identificar os valores salvos no PlayerPrefs
    private const string TOTAL_DIZIMO_KEY = "TotalDizimo";
    private const string UPGRADE_QUANTITY_PREFIX = "UpgradeQuantity_"; // Usaremos um prefixo para salvar as quantidades dos upgrades

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
            DontDestroyOnLoad(gameObject); // Mantém o objeto mesmo ao trocar de cenas
        }
        else
        {
            Destroy(gameObject);
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

        // É importante chamar Save() para garantir que os dados sejam escritos no disco
        PlayerPrefs.Save();

        Debug.Log("Jogo Salvo!");
    }

    // Função para carregar os dados do jogo
    public void LoadGame()
    {
        // Carrega o total de dizimo
        if (dizimos != null)
        {
            dizimos.tootalDizimo = PlayerPrefs.GetFloat(TOTAL_DIZIMO_KEY, 0f);
            dizimos.AtualizarTextoDizimo(); // Adicione essa função no seu script Dizimos para atualizar o texto da UI
        }

        // Carrega a quantidade de cada upgrade
        if (upgrades != null)
        {
            foreach (Upgrade upgrade in upgrades)
            {
                upgrade.quantity = PlayerPrefs.GetInt(UPGRADE_QUANTITY_PREFIX + upgrade.upgradeName, 0);
                upgrade.upgradeQtdTMP.text = upgrade.quantity.ToString(); // Atualiza o texto da quantidade do upgrade
                upgrade.upgradeCostTMP.text = "$ " + upgrade.getCurrentCost().ToString(); // Atualiza o custo do upgrade ao carregar
                // Você pode precisar fazer outras atualizações na UI do upgrade aqui, se necessário
            }
        }

        StartCoroutine(AutomaticSave()); // Inicia o salvamento automático

        Debug.Log("Jogo Carregado!");
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
                upgrade.upgradeCostTMP.text = "$ " + upgrade.getCurrentCost().ToString();
            }
        }

        SaveGame(); // Salva os valores resetados
        Debug.Log("Save Resetado!");
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