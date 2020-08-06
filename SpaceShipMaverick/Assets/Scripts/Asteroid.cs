using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private AsteroidSO asterData;
    private Rigidbody2D rb;
    private int health;
    public CircleCollider2D col2d;
    private int score;

    public void Init(AsteroidSO _asterData)
    {
        asterData = _asterData;
        GetComponent<Animator>().runtimeAnimatorController = asterData.Animation;

        rb = GetComponent<Rigidbody2D>();

        col2d.radius = asterData.Size;

        health = asterData.Health;

        score = asterData.Score;

    }

    public static Action<GameObject, int> OnAsterOverFly;
    private void FixedUpdate()
    {
        //задаем скорость
        rb.velocity = new Vector2(0f,-asterData.Speed);

        //проверяем необходимось существования )))
        //если её нет возвращаем в пул
        if (transform.position.y < -8 && OnAsterOverFly != null) 
        {
            BackToPool(-score);
        }
    }

    void BackToPool(int _score)
    {
        OnAsterOverFly(gameObject, _score);
       

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;
        if (obj.tag == "Player")
        {
            BackToPool(-score);
        }

        //если объект является элементом пула пуль
        var obj2 = collision.gameObject;
        if (PlayerController.bullets.ContainsKey(obj2))
        {
            health -= 1;
            if (health <= 0)
            {
                BackToPool(score);
            }
        }
    }
}
