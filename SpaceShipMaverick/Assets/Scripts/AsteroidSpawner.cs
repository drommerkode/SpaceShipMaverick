using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsteroidSpawner : MonoBehaviour
{
    [Tooltip("Настройки астеройдов")]
    [SerializeField] private List<AsteroidSO> asterSettings;

    [Tooltip("Размер пула")]
    [SerializeField] private int poolCount;

    [Tooltip("Префаб астеройда")]
    [SerializeField] private GameObject asterPrefab;

    [Tooltip("Время между спауном")]
    [SerializeField] private float spawnTime;

    //Словарь для скриптов астеройдов
    public static Dictionary<GameObject, Asteroid> asteroids;
    private Queue<GameObject> curAsteroids;

    private Camera curCamera; //камера уровня
    float heightRes; //высота экрана += 1
    float widthRes; //ширина экрана -= 0.6

    //Счёт
    public int needScore;//сколько нужно
    private int score;//сколько есть
    private bool canGetScore = true;//
    public Text scoreText;

    public WinLose winLose;

    private void Start()
    {

        //Определяем камеру и границы спавна
        curCamera = Camera.main;
        heightRes = curCamera.orthographicSize + 1f;
        widthRes = curCamera.orthographicSize * curCamera.aspect - 0.6f;

        //Заполняем словарь
        asteroids = new Dictionary<GameObject, Asteroid>();
        curAsteroids = new Queue<GameObject>();
        for (int i = 0; i < poolCount; ++i) 
        {
            var prefab = Instantiate(asterPrefab);
            var script = prefab.GetComponent<Asteroid>();
            prefab.SetActive(false);
            asteroids.Add(prefab, script);
            curAsteroids.Enqueue(prefab);
        }

        Asteroid.OnAsterOverFly += ReturnAster;

        //Запускаем спаун астеройдов
        StartCoroutine(Spawn());

        scoreText.text = "Счёт: " + score.ToString() + "/" + needScore.ToString();
    }

    private IEnumerator Spawn()
    {
        //Для того чтобы не повесить юнити
        if (spawnTime == 0)
        {
            Debug.Log("Не установлено время спауна");
            spawnTime = 1f;
        }

        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            if (curAsteroids.Count > 0)
            {
                try
                {
                    //получаем и активируем амтеройд
                    var aster = curAsteroids.Dequeue();
                    var script = asteroids[aster];
                    aster.SetActive(true);

                    //устанавливаем астеройду случайный СО
                    int rand = Random.Range(0, asterSettings.Count);
                    script.Init(asterSettings[rand]);

                    //помещяем астеройд в случайную позицию в диапазоне
                    float posX = Random.Range(-widthRes, widthRes);
                    float rot = Random.Range(0f, 45f);
                    aster.transform.position = new Vector2(posX, heightRes);
                    aster.transform.Rotate(0f, 0f, rot, Space.Self);
                }
                catch
                {
                    Debug.Log("Не удалось заспавнить астеройд в пул, игрок не найден");
                }
            }
        }
    }
    //Возврат в пул
    private void ReturnAster(GameObject _aster, int _score)
    {
        try
        {
            _aster.transform.position = transform.position;
            _aster.SetActive(false);
            curAsteroids.Enqueue(_aster);
            if (canGetScore)
            {
                score += _score;
                if (score < 0)
                {
                    score = 0;
                }
                if (score >= needScore)
                {
                    canGetScore = false;
                    score = needScore;
                    StopAllCoroutines();
                    winLose.Setup(true);
                    StartCoroutine(Win());
                    
                    //Сохраняемся
                    SaveLoad saveLoad = this.gameObject.AddComponent<SaveLoad>();
                    int curLevel = SceneManager.GetActiveScene().buildIndex;
                    saveLoad.Save(curLevel);
                }
                scoreText.text = "Счёт: " + score.ToString() + "/" + needScore.ToString();
            }

        }
        catch 
        {
            Debug.Log("Не удалось вернуть астеройд в пул");
            //Destroy(_aster);
        }
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
       
    }
