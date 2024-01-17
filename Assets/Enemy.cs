using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private Transform target;

    public static event Action OnPlayerHit;

    public static event Action<Enemy> OnEnemyHit;

    public float speed = 5f;

    public bool ActivelyHunting => activelyHunting;

    [SerializeField]
    private bool activelyHunting = false;

   public void SetData(Vector2 position, Transform target)
    {
        Debug.Log("we setting the data");
        // set the position of the enemy (outside of the view)
        this.transform.position = position;

        // set the target
        this.target = target;
        activelyHunting = true;
        this.gameObject.SetActive(true);
    }

        private void Update()
        {
            // To make sure we have a target
            if(target)
            {
                // move towards player

                // if distance is close to player
                if(Vector2.Distance(transform.position, target.position) < 1)
                {
                // player has to die
                Debug.Log("invoking shit");
                    OnPlayerHit?.Invoke();
                }   
            else
                {
                    // move towards player
                    // make this enemy move towards the player (target)
                    // something with this.transform.position
                    Vector3 direction = target.position - transform.position;
                    direction.Normalize();

                    transform.Translate(direction * speed * Time.deltaTime);
            }
            }
        }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a GameObject tagged as "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Bullet hit the enemy
            activelyHunting = false;
            OnEnemyHit?.Invoke(this);
        }
    }
}