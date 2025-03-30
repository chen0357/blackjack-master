using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();  // Lista de cartas en la mano
                                                             // 手中的牌列表
    public GameObject card; // Plantilla de carta para instanciar
                            // 用于实例化的牌模板
    public bool isDealer = false; // Si este objeto es el repartidor
                                  // 该对象是否为庄家
    public int points; // Puntos totales de la mano
                       // 手上的总分
    private int coordY; // Coordenada Y para la posición de las cartas
                        // 牌的Y坐标位置

    private void Awake()
    {
        points = 0;
        //Definimos dónde posicionamos las cartas de cada uno
        if (!isDealer)
            coordY = 3;
        else
            coordY = -1;
    }

    public void Clear()
    {
        points = 0; // Restablecer los puntos a 0
                    // 重置分数为0
        if (!isDealer)
            coordY = 3;  // Restablecer posición Y para las cartas del jugador
                         // 重置玩家牌的Y位置
        else
            coordY = -1; // Restablecer posición Y para las cartas del repartidor
                         // 重置庄家牌的Y位置
        foreach (GameObject g in cards)
        {
            Destroy(g); // Destruir el objeto de la carta
                        // 销毁牌对象
        }
        cards.Clear(); // Limpiar la lista de cartas
                       // 清空牌列表                        
    }

    public void InitialToggle()
    {
        cards[0].GetComponent<CardModel>().ToggleFace(true);              
    }

    public void Push(Sprite front, int value)
    {
        //Creamos una carta y la añadimos a nuestra mano
        GameObject cardCopy = (GameObject)Instantiate(card);
        cards.Add(cardCopy);

        //La posicionamos en el tablero 
        float coordX = (float)1.4 * (float)(cards.Count - 4);
        Vector3 pos = new Vector3(coordX, coordY);               
        cardCopy.transform.position = pos;

        //Le ponemos la imagen y el valor asignado
        cardCopy.GetComponent<CardModel>().front = front;
        cardCopy.GetComponent<CardModel>().value = value;
        
        //La cubrimos si es la primera del dealer
        if (isDealer && cards.Count <= 1)
            cardCopy.GetComponent<CardModel>().ToggleFace(false);
        else
            cardCopy.GetComponent<CardModel>().ToggleFace(true);

        //Calculamos la puntuación de nuestra mano
        int val = 0;
        int aces = 0;
        foreach (GameObject f in cards)
        {            

            if (f.GetComponent<CardModel>().value != 11)
                val += f.GetComponent<CardModel>().value;
            else
                aces++;
        }

        for (int i = 0; i < aces; i++)
        {
            if (val + 11 <= 21)
            {
                val = val + 11;
            }
            else
            {
                val = val + 1;
            }
        }

        points = val;
       
    }
     

}
