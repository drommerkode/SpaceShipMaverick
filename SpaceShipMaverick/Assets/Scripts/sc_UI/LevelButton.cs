using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Sprite locSprite; // закрытый уровень
    public Sprite unlocSprite; //открытый уровень
    public Sprite onPressSprite; //нажатая кнопка
    public Text levelText; //текст кнопки
    private int level = 0; //номер уровня
    private Button button; //компонент кнопка
    private Image image; //компонент картинка

    void OnEnable()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    //настройка кнопки
    public void Setup(int level, bool isUnLock)
    {
        //устанавливаем текст кнопки
        this.level = level;
        levelText.text = level.ToString();

        //открыт ли уровень
        if (isUnLock)
        {
            image.sprite = unlocSprite;
            button.enabled = true;
            levelText.gameObject.SetActive(true);
        }
        else 
        {
            image.sprite = locSprite;
            button.enabled = false;
            levelText.gameObject.SetActive(false);
        }
        
    }

    //переход на новый уровень
    public void OnClick()
    {
        //Останавливаем все карутины
        StopAllCoroutines();
        image.sprite = onPressSprite;
        SceneManager.LoadScene(level);
    }

}
