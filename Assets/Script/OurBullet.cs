using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurBullet : MonoBehaviour
{
    [SerializeField] float destroyY = 8f;
    [SerializeField] float speed = 5f;
    GameObject parent = null;

    private BoxCollider2D _collider;

    RaycastHit2D[] hits;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        parent = this.transform.parent.gameObject ?? null;
    }

    void Start()
    {
        
    }


    void Update()
    {
        AttackCheck();
        CheckDestroy();
        Move();
    }

    private void AttackCheck()
    {
        hits = Physics2D.RaycastAll(transform.position, Vector2.up, _collider.size.x / 2);
        if(hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.transform.gameObject.GetComponent<Enemy>().DestroyYourself();
                    parent.GetComponent<PlayerBuletConnector>().UpToPoint(5);
                    Destroy(gameObject);
                    break;
                }else if (hit.transform.CompareTag("BigEnemy"))
                {
                    hit.transform.gameObject.GetComponent<Enemy>().DestroyYourself();
                    parent.GetComponent<PlayerBuletConnector>().UpToPoint(10);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }

    private void Move()
    {
        Vector3 speedVector = new Vector3(0f, speed, -1f);
        Vector3 nPos = this.transform.position + speedVector;
        this.transform.position = Vector3.Lerp(this.transform.position, nPos, Time.deltaTime);
    }

    private void CheckDestroy()
    {
        if(this.transform.position.y >= destroyY)
        {
            Destroy(this.gameObject);
        }
    }


}
