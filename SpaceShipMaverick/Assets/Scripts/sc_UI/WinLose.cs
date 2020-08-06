using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLose : MonoBehaviour
{
    private Image image;//компонент картинка
    public Sprite loseSprite;//надпись Lose

    private void Start()
    {
        image = GetComponent<Image>();
        image.gameObject.SetActive(false);
    }

    public void Setup(bool _win) 
    {
        image.gameObject.SetActive(true);
        if (!_win)
        {
            image.sprite = loseSprite;
        }
    }


}
