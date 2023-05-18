using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Vector2 BottomDestroyPos;
    [SerializeField] EnemyBullet bullet;
    [SerializeField] AudioClip[] audioClips;

    private float attackWaitingTimeMin = 1f;
    private float attackWaitingTimeMax = 1f;
    private float waitAttack = 1f;

    private bool attacked = false;
    private float attackTimer = 0f;

    private float speed = 1f;

    private AudioSource _as;
    private SpriteRenderer _sr;
    private bool iDestoryed = false;
    private float destroyTimer = 0f;
    private float destroyTime = 0.6f;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
        _sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        attackWaitingTimeMax = Random.Range(0.8f, 2f);
        attackWaitingTimeMin = Random.Range(0.4f, attackWaitingTimeMax);
        speed = Random.Range(1f, 3.5f);
    }

    void Update()
    {
        DestroyCheck();
        if (!iDestoryed)
        {
            ExistsControl();
            Move();
            if (attacked)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= waitAttack)
                {
                    attacked = false;
                    attackTimer = 0f;
                }
            }

            if (!attacked)
            {
                Attack();
            }
        }

    }

    private void ExistsControl()
    {
        if(this.transform.position.y <= BottomDestroyPos.y)
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        Vector3 speedVector = new Vector3(0f, -1f * speed, -1f);
        speedVector += transform.position;
        transform.position = Vector3.Lerp(transform.position, speedVector, Time.deltaTime);
    }

    private void Attack()
    {
        _as.Stop();
        _as.clip = audioClips[1]; //laser sound
        _as.Play();
        waitAttack = Random.Range(attackWaitingTimeMin, attackWaitingTimeMax);
        attacked = true;
        Vector3 pos = transform.position;
        pos += new Vector3(0f, -1f * transform.GetComponent<BoxCollider2D>().bounds.size.y, 0f);
        Instantiate(bullet, pos, Quaternion.Euler(new Vector3(0, 180, 0)), transform.parent);
    }

    private void DestroyCheck()
    {
        if (iDestoryed)
        {
            _sr.color = Color.red;
            gameObject.transform.localScale = new Vector3(Mathf.Lerp(1f, 0f, destroyTimer), Mathf.Lerp(1f, 0f, destroyTimer), 1f);
            if(destroyTimer == 0)
            {
                _as.Stop();
                _as.clip = audioClips[0];//explosion sound
                _as.Play();
            }
            destroyTimer += Time.deltaTime;
            if(destroyTimer >= destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void DestroyYourself()
    {
        iDestoryed = true;
    }
}
