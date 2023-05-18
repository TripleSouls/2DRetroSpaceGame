using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppenPoint : MonoBehaviour
{

    float speed = 1f;
    [SerializeField] float maxSpeed = 2.5f;
    [SerializeField] float minSpeed = 1f;
    [SerializeField] Vector2 destroyArea = new Vector2(0, -8f);
    [SerializeField] Vector2 myBounds = new Vector2(1, 1);

    [SerializeField] bool debugMode = true;

    private RaycastHit2D[] hits;


    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        RayCast();
        Move();
        DestroyCheck();
    }

    private void RayCast()
    {
        hits = Physics2D.BoxCastAll(transform.position, myBounds, 0, transform.forward, 1f);
        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Player"))
            {
                hits[i].transform.GetComponent<Player>().UpToPoint(15);
                Destroy(this.transform.gameObject);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(this.transform.position, myBounds);
    }

    private void Move()
    {
        Vector3 speedVector = new Vector3(0f, -1f * speed, 1f);
        speedVector += transform.position;
        speedVector = new Vector3(speedVector.x, speedVector.y, 1f);
        transform.position = Vector3.Lerp(transform.position, speedVector, Time.deltaTime);
    }

    private void DestroyCheck()
    {
        if(transform.position.y <= destroyArea.y)
        {
            Destroy(gameObject);
        }
    }
}
