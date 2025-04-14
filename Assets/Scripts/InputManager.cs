using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameObject exitMenuInstance; // Armazena a referência do menu criado

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitMenuInstance == null)
            {
                // Carrega o prefab do ExitMenu da pasta Resources
                GameObject exitMenuPrefab = Resources.Load<GameObject>("ExitMenu");

                // Verifica se o Canvas existe
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    // Instancia o menu dentro do Canvas
                    exitMenuInstance = Instantiate(exitMenuPrefab, canvas.transform);
                    exitMenuInstance.name = "ExitMenu"; // Remove "(Clone)" do nome
                }
                else
                {
                    Debug.LogError("Canvas não encontrado na cena!");
                }
            }
            else
            {
                // Se o menu já existe, destrua-o
                Destroy(exitMenuInstance);
                exitMenuInstance = null;
            }
        }
    }
}