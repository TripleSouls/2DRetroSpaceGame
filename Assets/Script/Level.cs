using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{

    [SerializeField] GameObject backgroundObject;
    [SerializeField] Camera cam;
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject EscapePanel;
    [SerializeField] GameObject DroppenPoint;
    [SerializeField] GameObject SpawnLevel;

    Vector2 playerSize = Vector2.zero;
    Vector2 LeftBarrier = Vector2.zero;
    Vector2 RightBarrier = Vector2.zero;
    Vector2 BottomBarrier = Vector2.zero;
    Vector2 TopBarrier = Vector2.zero;

    Vector2 enemySize = Vector2.zero;

    [SerializeField] TMP_Text pointText;
    [SerializeField] TMP_Text maxPointText;

    bool spawn1 = false;
    bool spawn2 = false;
    bool pointSpawn = false;

    float spawnTimer1 = 0f;
    float spawnTimer2 = 0f;
    float pointTimer = 0f;

    float spawnTimer1Max = 4f;
    float spawnTimer2Max = 2.6f;
    float pointTimerMax = 5f;

    public float MaxPoint = 0f;
    public float Point = 0f;
    public float PointHitEnemy = 2f;
    public float PointTime = 0.75f;

    private float lifeTime = 0f;

    private bool GameOver = false;
    private bool StopGame = false;

    private void Awake()
    {

        EscapePanel.SetActive(false);

        playerSize = player.transform.GetComponent<Collider2D>().bounds.size;
        enemySize = enemy.transform.GetComponent<Collider2D>().bounds.size;

        spawnTimer1Max = Random.RandomRange(3f, 4f);
        spawnTimer2Max = Random.RandomRange(1.7f, 2.8f);
    }

    void Start()
    {
        #region bgprepare
        if (cam != null && backgroundObject != null)
        {
            cam = Camera.main;
            Vector3 r0 = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
            Vector3 r1 = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, 0));
            Vector2 newSizeForBackground = (Vector2)((r1 - r0)) * 2;
            newSizeForBackground = new Vector2(Mathf.Abs(newSizeForBackground.x / 2), Mathf.Abs(newSizeForBackground.y));
            
            backgroundObject.transform.localScale = newSizeForBackground;
            backgroundObject.transform.localPosition = new Vector3(
                cam.transform.transform.localPosition.x,
                (r0 + backgroundObject.transform.localScale / 2).y,
                backgroundObject.transform.position.z
                );
        }
        #endregion

        #region SpawnPoints
        if (cam != null)
        {
            LeftBarrier = (Vector2)cam.ScreenToWorldPoint(Vector2.zero) + (playerSize);
            RightBarrier = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, 0)) - (playerSize);
            BottomBarrier = (Vector2)cam.ScreenToWorldPoint( new Vector3(0, (cam.pixelHeight), 0) ) + enemySize;
            TopBarrier = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight * 4 / 3, 0)) + enemySize;

        }
        #endregion

    }

    private void Update()
    {
        if(!GameOver && !StopGame && SpawnLevel.active == false)
        {
            SpawnLevel.SetActive(true);
        }
        if (!GameOver && !StopGame)
        {
            lifeTime += Time.deltaTime;
            PointCalculator();
            #region Spawner
            if (spawn1)
            {
                spawnTimer1 += Time.deltaTime;
                if (spawnTimer1 >= spawnTimer1Max)
                {
                    spawnTimer1 = 0;
                    spawn1 = false;
                }
            }
            if (spawn2)
            {
                spawnTimer2 += Time.deltaTime;
                if (spawnTimer2 >= spawnTimer2Max)
                {
                    spawnTimer2 = 0;
                    spawn2 = false;
                }
            }
            if (pointSpawn)
            {
                pointTimer += Time.deltaTime;
                if(pointTimer >= pointTimerMax)
                {
                    pointTimer = 0;
                    pointSpawn = false;
                    pointTimerMax = Random.Range(4f, 10f);
                }
            }
            if (!spawn1)
            {
                spawn1 = true;
                Spawn();
                if (Time.deltaTime % 2 == 0)
                {
                    Spawn();
                }
            }
            if (!spawn2)
            {
                spawn2 = true;
                Spawn();
            }
            if (!pointSpawn)
            {
                pointSpawn = true;
                Spawn(1);
            }
            #endregion
        }
        else
        {
            escape();
        }
    }

    public void Continue()
    {
        Debug.Log("continue");
        if(Point < 0)
        {
            Point = 0;
        }
        Cont();
    }

    private void Cont()
    {
        EscapePanel.SetActive(false);
        StopGame = false;
        GameOver = false;
    }

    private void escape()
    {
        SpawnLevel.SetActive(false);
        EscapePanel.SetActive(true);
    }

    private void OnGUI()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            StopGame = true;
        }
    }

    private void LateUpdate()
    {
        MaxPoint = (MaxPoint < Point) ? Point : MaxPoint;
        if (pointText != null)
        {
            pointText.text = "POINT: " + Point.ToString("F2");
            maxPointText.text = "MaxP: " + MaxPoint.ToString("F2");
        }

        if(Point < 0)
        {
            GameOver = true;
        }
    }

    private void PointCalculator()
    {
        Point += Time.deltaTime * PointTime;
    }

    public void UpToPoint(float x = 5f)
    {
        Point += x;
    }

    public void DownToPoint(float x = 5f)
    {
        Point -= x;
    }

    private void Spawn(int type = 0)
    {

        float x = Random.RandomRange(LeftBarrier.x, RightBarrier.x);
        float y = Random.RandomRange(BottomBarrier.y, TopBarrier.y);

        if(type == 0)
        {
            Instantiate(enemy, new Vector3(x, y, 1), Quaternion.identity, SpawnLevel.transform);
        }
        else
        {
            Instantiate(DroppenPoint, new Vector3(x, y, 1), Quaternion.identity, SpawnLevel.transform);
        }

    }
}
