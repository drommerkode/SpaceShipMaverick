using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Camera curCamera; //камера уровня
    float heightRes; //высота экрана += 1
    float widthRes; //ширина экрана -= 0.8
    public GameObject Border;

    private void Start()
    {
        curCamera = Camera.main;
        heightRes = curCamera.orthographicSize + 1f;
        widthRes = curCamera.orthographicSize * curCamera.aspect - 0.8f;

    }
    void Update()
    {

    }
}
