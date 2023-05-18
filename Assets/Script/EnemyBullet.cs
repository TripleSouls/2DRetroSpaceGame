using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] Vector2 destroyArea = new Vector2(0, -8f);

    float speed = 5f;
    [SerializeField] float maxSpeed = 8f;
    [SerializeField] float minSpeed = 4f;

    private RaycastHit2D[] hits;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        speed = Random.RandomRange(minSpeed, maxSpeed);
    }

    void Update()
    {
        AttackCheck();
        Check();
        Move();
    }

    private void Check()
    {
        if(this.transform.position.y <= destroyArea.y)
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        Vector3 speedVector = new Vector3(0f, -1f * speed, -1f);
        speedVector += this.transform.position;

        this.transform.position = Vector3.Lerp(this.transform.position, speedVector, Time.deltaTime);
    }


    private void AttackCheck()
    {
        hits = Physics2D.RaycastAll(transform.position, Vector2.up, _collider.size.x / 2);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.transform.CompareTag("Player"))
                {
                    Player x = hit.transform.gameObject.GetComponent<Player>();
                    x.EnemyAttacked();
                }
            }
        }
    }
}
