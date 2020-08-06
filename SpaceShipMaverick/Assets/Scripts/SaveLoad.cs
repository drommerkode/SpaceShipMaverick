using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    private string keyName = "SSMavericSave";
    public void Save(int _curLevel)
    {
        //Если следующий уровень закрыт 
        //то увеличиваем число сохраненных уровней на 1 и сохраняем
        if (_curLevel < 3)//проверяем что это не 3,последний уровень
        {
            int curSave = PlayerPrefs.GetInt(keyName);
            if (curSave < _curLevel + 1)
            {
                PlayerPrefs.SetInt(keyName, _curLevel + 1);
            }
        }
    }

    //Загрузка
    public int Load()
    {
        //проверка наличия сохранений
        //и запись их если они не найдены
        //Если найдены то считываем их
        if (!PlayerPrefs.HasKey(keyName))
        {
            PlayerPrefs.SetInt(keyName, 1);
            return 1;
        }
        else
        {
            return PlayerPrefs.GetInt(keyName);
        }
        
    }
}
