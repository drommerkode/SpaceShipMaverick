using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Скорость")]
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float moveX;
    private float moveY;

    private int health;

    //Объект Жизнь
    public GameObject live;
    private List<GameObject> liveElement;

    [Tooltip("Настройки пуль")]
    [SerializeField] private List<BulletSO> bulletSettings;

    [Tooltip("Размер пула")]
    [SerializeField] private int poolCount;

    [Tooltip("Префаб пули")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Время между спауном")]
    [SerializeField] private float spawnTime;

    //Словарь для скриптов астеройдов
    public static Dictionary<GameObject, Bullet> bullets;
    private Queue<GameObject> curBullets;

    //кнопки для активации
    public Button goMainMenuButton;
    public Button restartButton;
    public WinLose winLose;

    //джойстик
    public DynamicJoystick dynamicJoystick;

    private Camera curCamera; //камера уровня
    float heightRes; //высота экрана -= 0.8
    float widthRes; //ширина экрана -= 0.3

    void Start()
    {
        //получаем Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        //Устанавливаем Жизни
        health = 3;

        //Заполняем лист сердец
        liveElement = new List<GameObject>();
        for (int i = 0; i < health; i++) {
            Debug.Log("Health create");
            var le = Instantiate(live);
            liveElement.Add(le);
        }

        //Заполняем словарь
        bullets = new Dictionary<GameObject, Bullet>();
        curBullets = new Queue<GameObject>();
        for (int i = 0; i < poolCount; ++i)
        {
            var prefab = Instantiate(bulletPrefab);
            var script = prefab.GetComponent<Bullet>();
            prefab.SetActive(false);
            bullets.Add(prefab, script);
            curBullets.Enqueue(prefab);
        }

        Bullet.OnBulletOverFly += ReturnBullet;

        //запускаем спаун пуль
        StartCoroutine(Spawn());

        //Определяем камеру и границы
        curCamera = Camera.main;
        heightRes = curCamera.orthographicSize - 0.8f;
        widthRes = curCamera.orthographicSize * curCamera.aspect - 0.3f;
    }

    public int GetHealth()
    {
        return health;
    }

    void Update()
    { 
        PInputs();
        for (int i = 0; i < health; i++)
        {
            liveElement[i].transform.position = new Vector2(0.2f + transform.position.x - 0.2f * i, transform.position.y - 0.6f);
        }
    }

    private void FixedUpdate()
    {
        PMove();
        
    }

    //Ввод с клавиатуры или геймпада
    void PInputs() {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX + dynamicJoystick.Horizontal, moveY + dynamicJoystick.Vertical);
    }

    //Перемещение путем установки скорости
    void PMove() {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed,moveDirection.y * moveSpeed);
        if (transform.position.x > widthRes)
        {
            rb.transform.position = new Vector3(widthRes, transform.position.y,0);
        }
        if (transform.position.x < -widthRes)
        {
            rb.transform.position = new Vector3(-widthRes, transform.position.y, 0);
        }
        if (transform.position.y > heightRes)
        {
            rb.transform.position = new Vector3(transform.position.x, heightRes, 0);
        }
        if (transform.position.y < -heightRes)
        {
            rb.transform.position = new Vector3(transform.position.x, -heightRes, 0);
        }
    }

    //проверяем столкновение с Астеройдами
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //если объект является элементом пула астеройдов
        var obj = collision.gameObject;
        if (AsteroidSpawner.asteroids.ContainsKey(obj))
        {

            //то уменьшаем здоровье и при необходимости уничтожаем игрока
            health -= 1;
            if (health > -1) 
            {
                Destroy(liveElement[health]);
            }
            if (health <= 0)
            {
                PlayerDie();
            }
        }
    }

    //Спаун пуль
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
            //Пуля слева
            try
            {
                if (curBullets.Count > 0)
                {
                    //получаем и активируем пулю
                    var bull = curBullets.Dequeue();
                    var script = bullets[bull];
                    bull.SetActive(true);

                    //устанавливаем пуле  СО
                    script.Init(bulletSettings[0]);

                    //помещяем астеройд в случайную позицию в диапазоне
                    bull.transform.position = new Vector2(transform.position.x - 0.3f, transform.position.y + 0.2f);
                }
                //Пуля справа
                if (curBullets.Count > 0)
                {
                    //получаем и активируем пулю
                    var aster = curBullets.Dequeue();
                    var script = bullets[aster];
                    aster.SetActive(true);

                    //устанавливаем пуле  СО
                    script.Init(bulletSettings[0]);

                    aster.transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y + 0.2f);
                }
            }
            catch 
            {
                Debug.Log("Не удалось заспавнить пулю");
            }
            
        }
    }
    //Возврат в пул
    private void ReturnBullet(GameObject _bullet)
    {
        try
        {
            _bullet.transform.position = transform.position;
            _bullet.SetActive(false);
            curBullets.Enqueue(_bullet);
        }
        catch {
            Debug.Log("Не удалось вернуть пулю в пул, игрок не найден");
            //Destroy(_bullet);
        }
    }

    //смерть игрока
    void PlayerDie()
    {
        winLose.Setup(false);
        goMainMenuButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        Destroy(gameObject);
        StopCoroutine(Spawn());

    }
}
