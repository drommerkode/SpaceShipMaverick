using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletSO bulletData;
    private Rigidbody2D rb;
    public void Init(BulletSO _bulletData)
    {
        bulletData = _bulletData;
        GetComponent<Animator>().runtimeAnimatorController = bulletData.Animation;

        rb = GetComponent<Rigidbody2D>();
    }

    public static Action<GameObject> OnBulletOverFly;
    private void FixedUpdate()
    {
        //задаем скорость
        rb.velocity = new Vector2(0f, bulletData.Speed);

        //проверяем необходимось существования )))
        //если её нет возвращаем в пул
        if (transform.position.y > 5 && OnBulletOverFly != null)
        {
            BackToPool();
        }
    }

    void BackToPool()
    {
        OnBulletOverFly(gameObject);
    }

    //проверяем столкновение с Астеройдами
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //если объект является элементом пула астеройдов
        var obj = collision.gameObject;
        if (AsteroidSpawner.asteroids.ContainsKey(obj))
        {
            BackToPool();
        }
    }
}
