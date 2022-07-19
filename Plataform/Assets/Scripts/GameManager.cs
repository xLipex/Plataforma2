using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <sumarry>
/// Classe usada para gerenciar o jogo
/// </sumarry>

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string guiName; // Name da fase da interface

    [SerializeField]
    private string levelName; // Nome da fase do jogo

    [SerializeField]
    private GameObject playerAndCameraPrefab; // Referência para o Prefab do jogador + câmera

    // Start is called before the first frame update
    void Start()
    {
        // Impede que o objeto indicado entre parênteses seja destruido
        DontDestroyOnLoad(this.gameObject); //referência para o objeto que contém o GameManager
        // 1 - Carregar a cena de interface e do jogo
        SceneManager.LoadScene(guiName);
        // SceneManager.LoadScene(levelName, LoadSceneMode.Additive); // Additive carrega uma

        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive).completed += operation =>
        {
            // Inicializa a variável para guardar a cena do level com o valor padrão (default)
            Scene levelScene = default;

            // Encontrar a cena de level que está carregada
            // O "for" que intera no array de cenas abertas
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                // Se o nome da cena na posição i do array for igual ao nome do level
                if (SceneManager.GetSceneAt(i).name == levelName)
                {
                    // Associe a cena na posição i do array à variável
                    levelScene = SceneManager.GetSceneAt(i);
                    break;
                }
            }

            // Se a variável estiver com um valor diferente do padrão, significa que ela foi alterada
            // e a cena do level atual foi encontrada no array, então faça ela ser a nova cena ativa.
            if (levelScene != default) SceneManager.SetActiveScene(levelScene);

            // 2 - Precisa instanciar o jogador na cena
            //Começa procurando o objeto PlayerStart na Cena do level
            Vector3 playerStartPosition = GameObject.Find("PlayerStart").transform.position;

            // Instancia o Prefab do jogador na posição player start com rotação zerada
            Instantiate(original: playerAndCameraPrefab, playerStartPosition, Quaternion.identity);
            // 3 - Começar a partida

        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
