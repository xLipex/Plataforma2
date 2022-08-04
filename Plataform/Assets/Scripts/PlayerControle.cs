using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;

public class PlayerControle : MonoBehaviour
{
    // Número de moedas coletados
    public int coins = 0;
    
    // Número de rubys coletados
    public int rubys = 0;
        
    // Guarda uma referência para os controles que criamos no InputAction
    private Controls _gameControls;
    
    // Guarda referência para o PlayerInput, que é quem conecta o dispositivo de controle ao código
    private PlayerInput _playerInput;
    
    // Referência para a Câmera Principal (main) do jogo
    private Camera _mainCamera;
    
    // Guardar o mobimento que está sendo lido do controle do jogador
    private Vector2 _moveInput;
    
    // Guarda a referência para o componente físico do jogador, usaremos para mover o jogador
    private Rigidbody _rigidbody;
    
    //Diz se o jogador está no chão ou não
    private bool _isGrounded;
    
    // Velocidade que o jogador vai se mover
    public float moveMultiplier;

    // Velocidade máxima que o jogador vai poder andar em cada eixo (x a z - somente esses, pois não queremos limitar o y)
    public float maxVelocity;
    
    // Distância que o raio vai percorrer procurando algo para bater
    public float rayDistance;
    
    // Máscara de colisão com o chão
    public LayerMask layerMask;
    
    // Força que o jogador vai usar para pular
    public float jumpForce;
    
    private void OnEnable()
    {
        // Associa a variável ao componente Rigidbody presente no objeto do jogador na Unity
        _rigidbody = GetComponent<Rigidbody>();
        
        // Instância de um novo objeto da classe GameControls
        _gameControls = new Controls();
        
        // Associa a variável ao componente PlayerInput presente no objeto do jogador na Unity
        _playerInput = GetComponent<PlayerInput>();
        
        // Associa a nossa variável o valor presente na variável main da classe Camera, que é
        _mainCamera = Camera.main;
        
        // Inscrever o delegate para a função que é chamada quando uma tecla ou botão 
        _playerInput.onActionTriggered += OnActionTriggered;
    }

    private void OnDisable()
    {
        // Desinscrever o delegate
        _playerInput.onActionTriggered -= OnActionTriggered;
    }

    // Delegate para adicionarmos funcionalidade qunado o jogador apertar um botão
    // o parâmetro obj, da classe InputAction.CallbackContext, traz às informações do notão
    // que foi apertado pelo jogador.
    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        
        
        // Compara a informação trazida pela obj, checando se o nome da ação executada
        // tem o mesmo nome de ação de moveer o jogador (Movemente.name)
        if (obj.action.name.CompareTo(_gameControls.Gameplay.Movement.name) == 0)
        {
            
            // Caso ocorra uma ação de movimento (mover), passamos o valor que está vindo no obg, que,
            // como definimos no InputAction, é um Vector2 para a variável _moveInput
            _moveInput = obj.ReadValue<Vector2>();
        }
        
        // Compara se a informação trazida pelo obj é referente ao comando de pular
        if (obj.action.name.CompareTo(_gameControls.Gameplay.Jump.name) == 0)
        {
            // Se o jogador apertar e soltar o botão de pulo, chamamos a função de pular Jump()
            if (obj.performed) Jump();
        }
    }

    // Executa a movimentação do jogador através da física
    private void Move()
    {
        
        // Pegamos o vetor que aponta para a direção que a camera está olhando
        Vector3 canForward = _mainCamera.transform.forward;
        
        // Pegamos o vetor que aponta para a direita da câmera
        Vector3 canRight = _mainCamera.transform.right;
        
        canForward.y = 0;
        canRight.y = 0;
        
        // Usamos AddForce para adicionar uma força gradual para o jogador, quanto mais
        // tempo seguramos a tecla, mais rápido a bolinha irá
        _rigidbody.AddForce(
            // Multiplicamos o input que move o jogador para frente pelo vetor que aponta
            // para a frente da câmera
            (_mainCamera.transform.forward * _moveInput.y +
             // Multiplica o input que move o jogador para a direita pelo vetor que aponta
             // para a direta da câmera
                            _mainCamera.transform.right * _moveInput.x)
            // Multiplica esse resultado pela velocidade e pela variável de deltaTime
                            * moveMultiplier * Time.fixedDeltaTime);

    }

    // Função que é executado todo loop de física da Unity
    private void FixedUpdate()
    {
        
        // Quando a física for atualizada, chama a função de Mover
        Move();
        LimitVelocity();
    }

    // Função que vai limitar a velocidade do rigidbody
    private void LimitVelocity()
    {
        // Pega a velocidade atual do player através do rigidbody
        Vector3 velocity = _rigidbody.velocity;
        
        // Compara a velocidade do eixo x (usando a função abs para ignorar o sinal negativo caso tenha)
        // Utilizamos a função Sign para recuperar o valor do sinal da velocidade e multuplicar pela velocidade máxima permitida
        if (Mathf.Abs(velocity.x) > maxVelocity) velocity.x = Mathf.Sign(velocity.x) * maxVelocity;
        // mesma coisa de cima, mas para o eixo z, porém usando o método Clone.
        // onde -maxVelocity < velocity.z < maxVelocity
        velocity.z = Mathf.Clamp(value:velocity.z, min:-maxVelocity, maxVelocity);
        
        // Atribui o vetor que alteramos de volta ao rigidbody
        _rigidbody.velocity = velocity;
    }
    
    /* O que eu preciso para pular?
     * -  Jogador precisa estar no chão;
     *  1º -> Usar a colisão do rigidbody do jogador com o chão e, se a colisão estuver acontecendo, faz uso variável
     *   que checa se o jogador está no chão ficar verdadeira;
     *  2º -> Usar um raycast = ()---|
     *   █ Atira um raio em alguma direção (no nosso caso, vai ser sempre para baixo);
     *   █ Caso atinja algum objeto, ela retorna uma colisão que podemos usar para fazer que checa se o jogador está no
     *   no chão ficar verdadeiro caso o objeto colidido seja um objeto que representa o chão;
     *   █ Podemos usar LayerMask para somente verificar colisões com certos tipos de objeto
     * - Jogador precisa apertar o botão de pular;
     *  1º -> Usamos a função OnActionTriggered e comparamos se o nome da ação tem o mesmo nome da ação de pulo;
     *   █ Caso tenha, checamos se o botão foi apertado (storted), foi solto (canceled) ou foi pressionado e solto (perfomed)
     */
    
    //Vai ser usada para checar se o jogador está no chão ou não
    private void CheckGround()
    {
        // variável que guarda o resultado da colisão da raycast
        RaycastHit collision;
        
        // vamos atirar um raio para baixo e vamos checar se ele bate em algo
        if (Physics.Raycast(transform.position, Vector3.down, out collision, rayDistance, layerMask))
        {
            // Se executou esse código é porquê o jogador estará no chão
            _isGrounded = true;
        }
        else
        {
            // Se executou esse código, o jogador não estará no chão
            _isGrounded = false;
        }
    }
    
    
    // Função que vai ser chamada para fazer o jogador pular
    private void Jump()
    {
        // Se o jogador estiver no são (isGrounded for verdadeiro)
        if (_isGrounded)
        {
            // adicionamos uma força do tipo Impulso para fazer o jogador pular
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        CheckGround();
    }

    private void OnDrawGizmos()
    {
        // Desenha a linha do raycast no editor do unity para ficar mais fácil visualizarmos o tamanho do raycast
        Debug.DrawRay(start:transform.position, dir:Vector3.down * rayDistance, Color.red);
    }

    // Função que checa se o objeto entrou em um colisor setado como IsTrigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            //Aumente o número de coins do jogador em uma unidade
            coins++;
            
            // Manda a notificação da mudança do valor de coins
            PlayerObserverManager.CoinsChanged(coins);

            // Destrua o objeto da coin
            Destroy(other.gameObject);
        }
        
        if (other.CompareTag("Ruby"))
        {
            rubys++;
            
            PlayerObserverManager.RubysChanged(rubys);
            
            Destroy(other.gameObject);
        }
    }
}
