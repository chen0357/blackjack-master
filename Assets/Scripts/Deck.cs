using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Deck : MonoBehaviour
{
    public Sprite[] faces; // Caras de las cartas
                           // 牌面的图像数组
    public GameObject dealer; // Repartidor
                              // 庄家对象
    public GameObject player; // Jugador
                              // 玩家对象
    public Button hitButton; // Botón de "Pedir carta"
                             // “要牌”按钮
    public Button stickButton; // Botón de "Plantarse"
                               // “停牌”按钮
    public Button playAgainButton; // Botón de "Jugar de nuevo"
                                   // “再玩一次”按钮
    public Text finalMessage; // Mensaje final
                              // 最终消息文本
    public Text probMessage; // Mensaje de probabilidad
                             // 概率消息文本
    public Text playerPointsText; // Texto de puntos del jugador
                                  // 玩家分数文本
    public int[] values = new int[52]; // Valores de las cartas
                                       // 牌的值数组
    int cardIndex = 0; // Índice de carta
                       // 牌的索引

    public InputField betInputField; // Campo de entrada de apuesta
                                     // 押注输入字段
    public Text jingeText;  // Texto de dinero actual
                            // 当前金额文本
    private int playerMoney = 1000; // Dinero inicial del jugador
                                    // 玩家初始金额

    private int bet = 0; // Variable de apuesta

    private bool gameStarted = false;  // Bandera para verificar si el juego ha comenzado



    // Inicialización al inicio del script
    // 脚本启动时的初始化
    private void Awake()
    {
        InitCardValues(); // Inicializar valores de las cartas
                          // 初始化牌的值
    }

    // Método que se ejecuta al inicio del juego
    // 游戏开始时执行的方法
    private void Start()
    {
        ShuffleCards(); // Barajar cartas
                        // 洗牌
        StartGame(); // Iniciar juego
                     // 开始游戏
        UpdateMoneyDisplay(); // Actualizar visualización del dinero
                              // 更新金额显示
        CheckInitialBlackjack(); // Comprobar si hay Blackjack inicial
                                 // 检查初始是否有黑杰克
    }

    // Inicializar los valores de las cartas
    // 初始化牌的值
    private void InitCardValues()
    {
        for (int i = 0; i < 52; i++)
        {
            int value = (i % 13) + 1;
            if (value == 1)
                values[i] = 11; // As se inicializa en 11
                                // Ace 被初始化为 11
            else if (value > 10)
                values[i] = 10; // J, Q, K se inicializan en 10
                                // J, Q, K 被初始化为 10
            else
                values[i] = value;
        }
    }

    // Barajar las cartas
    // 洗牌
    private void ShuffleCards()
    {
        for (int i = 0; i < faces.Length; i++)
        {
            Sprite tempFace = faces[i];
            int tempValue = values[i];
            int randomIndex = Random.Range(i, faces.Length);
            faces[i] = faces[randomIndex];
            values[i] = values[randomIndex];
            faces[randomIndex] = tempFace;
            values[randomIndex] = tempValue;
        }
    }

    // Comenzar el juego repartiendo las cartas
    // 开始游戏并发牌
    void StartGame()
    {
        // Repartir cartas solo si es la primera vez que se juega
        if (!gameStarted)
        {
            for (int i = 0; i < 2; i++)
            {
                PushPlayer(); // Repartir carta al jugador
                PushDealer(); // Repartir carta al repartidor
            }

            CalculateProbabilities(); // Calcular probabilidad inicial después de repartir
            gameStarted = true; // Marcar el juego como iniciado
        }
    }


    // Comprobar si hay Blackjack al inicio del juego
    // 检查游戏开始时是否有黑杰克
    void CheckInitialBlackjack()
    {
        if (player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "¡Ambos tienen Blackjack! ¡Es un empate!";
            DisableButtons(); // Desactivar botones
                              // 禁用按钮
        }
        else if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "¡Blackjack! ¡Ganaste!";
            DisableButtons(); // Desactivar botones
                              // 禁用按钮
        }
        else if (dealer.GetComponent<CardHand>().points == 21)
        {
            dealer.GetComponent<CardHand>().InitialToggle();  // Asegurar que se muestre la primera carta del repartidor
            finalMessage.text = "¡El repartidor tiene Blackjack! ¡Perdiste!";
            DisableButtons(); // Desactivar botones
                              // 禁用按钮
        }
    }

    // Desactivar botones
    // 禁用按钮
    void DisableButtons()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;
    }

    // Repartir carta al repartidor
    // 给庄家发牌
    void PushDealer()
    {
        dealer.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        cardIndex++;
    }

    // Repartir carta al jugador
    // 给玩家发牌
    void PushPlayer()
    {
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        cardIndex++;
        playerPointsText.text = "Puntos: " + player.GetComponent<CardHand>().points.ToString();
    }

    // Método para pedir carta
    // 请求更多牌的方法
    public void Hit()
    {
        PushPlayer(); // Repartir carta al jugador

        // Actualizar visualización de los puntos del jugador
        // 更新玩家点数显示
        playerPointsText.text = "Puntos: " + player.GetComponent<CardHand>().points.ToString();

        // Comprobar si se ha pasado de 21
        // 检查是否超过 21 点
        if (player.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "¡Te has pasado!";
            DisableButtons(); // Desactivar botones
                              // 禁用按钮
        }
        // Comprobar si se ha alcanzado 21
        // 检查是否达到 21 点
        else if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "¡21 puntos! ¡Te plantas con una puntuación perfecta!";
            Stand(); // Llamar automáticamente al método Stand para terminar el turno del jugador
                     // 自动调用 Stand 方法结束玩家的回合
        }
        CalculateProbabilities();
    }

    // Método para plantarse
    // 停牌方法
    public void Stand()
    {
        while (dealer.GetComponent<CardHand>().points <= 16)
        {
            PushDealer(); // Repartir carta al repartidor mientras su puntuación sea menor o igual a 16
                          // 继续给庄家发牌直到分数大于 16
        }
        // Mostrar la primera carta del repartidor
        // 显示庄家的第一张牌
        dealer.GetComponent<CardHand>().InitialToggle();
        CompareHands(); // Comparar las manos del jugador y del repartidor
                        // 比较玩家和庄家的手牌
    }

    // Comparar las manos del jugador y del repartidor
    // 比较玩家和庄家的手牌
    void CompareHands()
    {
        int playerTotal = player.GetComponent<CardHand>().points;
        int dealerTotal = dealer.GetComponent<CardHand>().points;

        if (playerTotal > 21)
        {
            finalMessage.text = "¡Perdiste!";
        }
        else if (dealerTotal > 21 || playerTotal > dealerTotal)
        {
            finalMessage.text = "¡Ganaste!";
            playerMoney += bet * 2;  // Se duplica la apuesta ganada
        }
        else if (dealerTotal == playerTotal)
        {
            finalMessage.text = "Empate";
            playerMoney += bet;  // Recupera la apuesta
        }
        else
        {
            finalMessage.text = "¡Perdiste!";
        }

        bet = 0; // Reiniciar la apuesta para la siguiente ronda
        hitButton.interactable = false;
        stickButton.interactable = false;

        UpdateMoneyDisplay(); // Asegurar que la UI se actualice
    }



    // Método para jugar de nuevo
    // 再玩一次的方法
    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";

        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();

        cardIndex = 0;
        bet = 0; // La apuesta se reinicia, pero el dinero no
        ShuffleCards();

        gameStarted = false;  // Restablecer la bandera para la próxima ronda
                              // Solo se reinicia la interfaz, no se reparten cartas automáticamente aquí
    }




    // Actualizar visualización del dinero
    // 更新金额显示
    void UpdateMoneyDisplay()
    {
        jingeText.text = "Dinero: " + playerMoney.ToString();
    }

    // Método para apostar
    // 押注的方法
    // Método para apostar
    public void Bet()
    {
        int betAmount;
        if (int.TryParse(betInputField.text, out betAmount) && betAmount % 10 == 0 && betAmount <= playerMoney && betAmount > 0)
        {
            if (bet == 0) // Si no hay una apuesta en curso
            {
                playerMoney -= betAmount; // Restar dinero apostado
                bet = betAmount; // Registrar la apuesta
                UpdateMoneyDisplay(); // Actualizar UI del dinero

                // Asegúrate de que el juego no se haya iniciado antes
                if (!gameStarted)
                {
                    StartGame();  // Empezar el juego si no ha comenzado
                    gameStarted = true;  // Marcar el juego como comenzado
                }
            }
            else
            {
                Debug.Log("Ya has realizado una apuesta. Espera a que termine la ronda.");
            }
        }
        else
        {
            Debug.Log("Cantidad de apuesta inválida.");
        }
    }







    // Finalizar ronda y actualizar dinero según el resultado
    // 结束回合并根据结果更新金额
    IEnumerator EndRound(int betAmount)
    {
        yield return new WaitUntil(() => !hitButton.interactable); // Esperar hasta que termine la ronda
                                                                   // 等到回合结束
        if (finalMessage.text.Contains("ganaste"))
        {
            playerMoney += betAmount * 2; // Ganar el doble de la cantidad apostada
                                          // 赢得押注金额的两倍
        }
        // Si se pierde, no es necesario hacer nada más ya que la cantidad apostada ya ha sido deducida
        // 如果输了，无需做其他处理，因为押注金额已经被扣除
        UpdateMoneyDisplay(); // Actualizar visualización del dinero
                              // 更新金额显示
    }


    // Calcular la probabilidad de no pasarse al pedir una carta
    // 计算抽到不爆牌的概率
    public void CalculateProbabilities()
    {
        int currentPoints = player.GetComponent<CardHand>().points;
        int safeCards = 0;
        int remainingCards = values.Length - cardIndex;

        for (int i = cardIndex; i < values.Length; i++)
        {
            int testValue = values[i];
            // Si el valor de la carta actual hace que el jugador no se pase
            // 如果当前牌不会让玩家爆牌
            if (currentPoints + testValue <= 21)
            {
                safeCards++;
            }
        }

        float probability = (float)safeCards / remainingCards * 100f;

        probMessage.text = $"Probabilidad de no pasarse: {probability:F1}%";
        // 显示不会爆牌的概率（保留 1 位小数）
    }

   


}
