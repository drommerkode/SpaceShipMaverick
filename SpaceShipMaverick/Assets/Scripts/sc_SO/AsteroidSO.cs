
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Asteroid", fileName = "New asteroid")]
public class AsteroidSO : ScriptableObject
{
    
    [Tooltip("Анимация")]
    [SerializeField] private AnimatorController anim;
    public AnimatorController Animation
    {
        get { return anim; }
        protected set { }
    }

    [Tooltip("Скорость")]
    [SerializeField] private float speed;
    public float Speed
    {
        get { return speed; }
        protected set { }
    }

    [Tooltip("Очки")]
    [SerializeField] private int score;
    public int Score
    {
        get { return score; }
        protected set { }
    }

    [Tooltip("Размер")]
    [SerializeField] private float size;
    public float Size
    {
        get { return size; }
        protected set { }
    }

    [Tooltip("Здоровье")]
    [SerializeField] private int health;
    public int Health
    {
        get { return health; }
        protected set { }
    }
}
