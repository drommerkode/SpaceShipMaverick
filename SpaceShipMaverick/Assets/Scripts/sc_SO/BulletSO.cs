
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet", fileName = "New Bullet")]
public class BulletSO : ScriptableObject
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
}
