using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour
{
    SpriteRenderer spriteRenderer; // Componente para renderizar sprites
                                   // 用于渲染精灵的组件

    public Sprite cardBack; // Sprite de la parte trasera de la carta
                            // 牌背的精灵
    public Sprite front; // Sprite del frente de la carta
                         // 牌面的精灵
    public int value; // Valor numérico de la carta
                      // 牌的数值

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el componente SpriteRenderer al inicio
                                                         // 在开始时获取SpriteRenderer组件
    }

    public void ToggleFace(bool showFace)
    {
        if (showFace)
        {
            spriteRenderer.sprite = front; // Mostrar el frente de la carta si showFace es verdadero
                                           // 如果showFace为真，则显示牌面
        }
        else
        {
            spriteRenderer.sprite = cardBack; // Mostrar el dorso de la carta si showFace es falso
                                              // 如果showFace为假，则显示牌背
        }
    }

}

