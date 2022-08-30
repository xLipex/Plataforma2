using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <sumarry>
/// Classe usada para gerenciar o jogo
/// </sumarry>

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // instância do singleton

    [SerializeField]
    private string guiName; // Name da fase da interface

    [SerializeField]
    private string levelName; // Nome da fase do jogo

    [SerializeField]
    private GameObject playerAndCameraPrefab; // Referência para o Prefab do jogador + câmera

    // Start is called before the first frame update
    private void Awake()
    {
        // Condição de criação do singleton
        if (Instance == null)
        {
            Instance = this;
            
            // Impede que o objeto indicado entre parênteses seja destruido
            DontDestroyOnLoad(this.gameObject); //referência para o objeto que contém o GameManager
        }
        else Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Se estiver iniciando a cena a partir de Initialization, carregue o jogo
        // do jeito de antes
        if (SceneManager.GetActiveScene().name == "Initialization")
        {
            StartGameFromInitialization();
        }
        // Caso contrário, está iniciando a partir do level, carregue o jogo do modo apropriado
        else
        {
            StartGameFromLevel();
        }
    }

    private void StartGameFromLevel()
    {
        // 1 - carrega a cena de interface de modo aditivo para não apagar a cena do level
        // da memória RAM
        SceneManager.LoadScene(guiName, LoadSceneMode.Additive);
        
        // 2 - Precisa instanciar o jogador na cena
        //Começa procurando o objeto PlayerStart na Cena do level
        Vector3 playerStartPosition = GameObject.Find("PlayerStart").transform.position;

        // Instancia o Prefab do jogador na posição player start com rotação zerada
        Instantiate(original: playerAndCameraPrefab, playerStartPosition, Quaternion.identity);
        
        // 3 - Inicia o jogo
    }

    public void StartGame()
    {
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

    private void StartGameFromInitialization()
    {
        SceneManager.LoadScene(("Splash"));
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
