using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{
    public int unlockedLevel = 1; //Количество открытых уровней
    public LevelButton[] levelButtons; //список кнопок

    //настройка кнопок
    void Start()
    {
        //Загрузка
        SaveLoad saveLoad = this.gameObject.AddComponent<SaveLoad>();
        unlockedLevel = saveLoad.Load();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int level = i + 1;
            levelButtons[i].gameObject.SetActive(true);
            levelButtons[i].Setup(level, level <= unlockedLevel);
        }
    }
}
