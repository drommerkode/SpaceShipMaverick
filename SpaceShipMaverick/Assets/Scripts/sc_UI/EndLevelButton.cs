using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelButton : MonoBehaviour
{
    public Sprite noPressSpriteRest; //не нажатая кнопка
    public Sprite onPressSpriteResr; //нажатая кнопка
    public Sprite noPressSpriteMaimM; //не нажатая кнопка
    public Sprite onPressSpriteMaimM; //нажатая кнопка
    private Button button; //компонент кнопка
    private Image image; //компонент картинка
    public bool restart; //Если 1 - рестарт уровня Если 0 - переход в главное меню

    
    void OnEnable()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        if (restart)
        {
            image.sprite = noPressSpriteRest;
        }
        else
        {
            image.sprite = noPressSpriteMaimM;
        }
    }
    private void Start()
    {
        button.gameObject.SetActive(false);
    }
    public void OnClick()
    {
        //Останавливаем все карутины
        StopAllCoroutines();
        if (restart)
        {
            image.sprite = onPressSpriteResr;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            image.sprite = onPressSpriteMaimM;
            SceneManager.LoadScene(0);
        }
    }
}
