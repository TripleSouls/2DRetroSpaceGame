using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] GameObject spriteObject;
    SpriteRenderer spriteRenderer;
    [Tooltip("0 = Normal\n1 = Upgraded")]
    [SerializeField] Sprite[] sprites = new Sprite[2];

    Collider2D _collider;
    Vector2 size = Vector2.zero;

    Vector2 LeftBarrier = Vector2.zero;
    Vector2 RightBarrier = Vector2.zero;
    Vector2 BottomBarrier = Vector2.zero;
    Vector2 TopBarrier = Vector2.zero;

    [SerializeField] bool DebugMode = true;

    [SerializeField] List<KeyCode> LeftMoveKeys = new List<KeyCode>() { KeyCode.A, KeyCode.LeftArrow };
    [SerializeField] List<KeyCode> RightMoveKeys = new List<KeyCode>() { KeyCode.D, KeyCode.RightArrow };
    [SerializeField] List<KeyCode> UpMoveKeys = new List<KeyCode>() { KeyCode.W, KeyCode.UpArrow };
    [SerializeField] List<KeyCode> DownMoveKeys = new List<KeyCode>() { KeyCode.S, KeyCode.DownArrow };
    [SerializeField] List<KeyCode> AttackKeys = new List<KeyCode>() { KeyCode.Space, KeyCode.Mouse0 };

    [Range(0f, 10f)]
    [SerializeField] float PlayerSpeed = 5f;

    [Header("Bullets")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject waveBullet;
    [SerializeField] GameObject PlayerBulletConnector;

    [Header("Level")]
    [SerializeField] GameObject Level;

    bool GoLeft = false;
    bool GoRight = false;
    bool GoUp = false;
    bool GoDown = false;
    bool attacked = false;

    float attackTimer = 0f;
    float attackTime = 0.2f;

    int selectedSprite = 0;

    bool weHitted = false;
    float hittedTimer = 0f;
    float hittedTime = 0.5f;

    private void Awake()
    {
        Application.targetFrameRate = 30;

        selectedSprite = 0;
        _collider = GetComponent<Collider2D>();
        size = _collider.bounds.size;
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        if(_camera != null)
        {
            LeftBarrier = (Vector2)_camera.ScreenToWorldPoint(new Vector3(0, 0, 0)) + new Vector2(0.25f, 0.25f);
            RightBarrier = (Vector2)_camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth, _camera.pixelHeight, 0)) - new Vector2(0.25f, 0.25f);
            BottomBarrier = (Vector2)_camera.ScreenToWorldPoint(Vector2.zero) + new Vector2(0f, 0.25f);
            TopBarrier = BottomBarrier + (size * 2.5f);
        }
    }

    void Start()
    {
        spriteRenderer.sprite = sprites[selectedSprite];
    }

    void Update()
    {
        #region Hitted
        if (weHitted)
        {
            if(hittedTimer == 0f)
            {
                PlayerBulletConnector.GetComponent<PlayerBuletConnector>().DownToPoint(5);
            }
            spriteRenderer.color = Color.red;
            hittedTimer += Time.deltaTime;
            if(hittedTimer >= hittedTime)
            {
                hittedTimer = 0f;
                weHitted = false;
                spriteRenderer.color = Color.white;
            }
        }
        #endregion
        if (attacked)
        {
            if(attackTimer == 0)
            {
                Attack();
            }
            attackTimer += Time.deltaTime;
            if(attackTimer > attackTime)
            {
                attackTimer = 0f;
                attacked = false;
                Debug.Log("Ateþ edebilir.");
            }
        }

        if (DebugMode)
        {
            DrawCorner();
        }
        #region KeyHandler
        GoLeft = false;
        GoRight = false;
        GoUp = false;
        GoDown = false;
        attacked = false;
        foreach (var key in LeftMoveKeys)
        {
            if (Input.GetKey(key))
            {
                GoLeft = true;
                break;
            }
        }
        foreach (var key in RightMoveKeys)
        {
            if (Input.GetKey(key))
            {
                GoRight = true;
                break;
            }
        }
        foreach (var key in UpMoveKeys)
        {
            if (Input.GetKey(key))
            {
                GoUp = true;
                break;
            }
        }
        foreach (var key in DownMoveKeys)
        {
            if (Input.GetKey(key))
            {
                GoDown = true;
                break;
            }
        }
        foreach (var key in AttackKeys)
        {
            if (Input.GetKey(key))
            {
                attacked = true;
                break;
            }
        }
        
        Move();
        #endregion
    }

    public void UpToPoint(float x = 5)
    {
        Level.GetComponent<Level>().UpToPoint(x);
    }

    private void Attack()
    {
        Vector3 pos = this.transform.position;
        pos += new Vector3(0f, size.y/2, 0f);
        pos += new Vector3(0f, 0.4f, 0f);
        Instantiate(bullet, pos, Quaternion.Euler(new Vector3(0, 0, 0)), (PlayerBulletConnector != null) ? PlayerBulletConnector.transform : null);
    }

    private void LateUpdate()
    {
        spriteRenderer.sprite = sprites[selectedSprite];
    }

    private void Move()
    {
        #region CheckDoubleDirection
        if (GoLeft && GoRight)
        {
            GoLeft = false;
            GoRight = false;
        }
        if (GoUp && GoDown)
        {
            GoUp = false;
            GoDown = false;
        }
        #endregion

        bool horizontalMove = (GoLeft ? true : GoRight ? true : false);
        bool verticalMove = (GoUp ? true : GoDown ? true : false);

        float xSpeed = (horizontalMove) ? PlayerSpeed : 0f;
        float ySpeed = (verticalMove) ? PlayerSpeed : 0f;

        xSpeed *= (GoLeft) ? -1f : 1f;
        ySpeed *= (GoDown) ? -1f : 1f;

        RotateMe(xSpeed);

        Vector3 speedPosition = new Vector3(xSpeed, ySpeed, 0f);

        MoveWithSpeed(speedPosition);
    }

    private void RotateMe(float x)
    {
        x = -1f * x;
        float dTime = Time.deltaTime;
        dTime *= 10;
        dTime = (dTime < 0.2f ? dTime + 0.2f : dTime);
        if (x < 0)
        {
            float nowY = transform.eulerAngles.y;
            nowY = (nowY > 180) ? nowY - 360 : nowY;
            float y = Mathf.Lerp(nowY, -50, dTime);
            this.transform.rotation = Quaternion.Euler(
                new Vector3(0, y, 0)
            );
        }
        else if(x > 0)
        {
            float y = Mathf.Lerp(this.transform.eulerAngles.y, 50, dTime);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, y, 0));
        }
        else
        {
            float nowY = transform.eulerAngles.y;
            nowY = (nowY > 180) ? nowY - 360 : nowY;
            dTime = (dTime < 0.5f) ? 0.5f + dTime : dTime;
            float y = Mathf.Lerp(nowY, 0, dTime * 2);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, y, 0));
        }
    }

    private void MoveWithSpeed(Vector3 speed)
    {
        Vector3 nPos = gameObject.transform.position + speed;
        nPos = Vector3.Lerp(gameObject.transform.position, nPos, Time.deltaTime);

        float rightX = RightBarrier.x + size.x / 2;
        float leftX = LeftBarrier.x + size.x/2;
        float topY = TopBarrier.y + size.y / 2;
        float bottomY = BottomBarrier.y + size.y/2;

        #region Barrier
        if (nPos.x > rightX)
        {
            nPos = new Vector3(rightX, nPos.y, nPos.z);
        }
        else if (nPos.x < leftX)
        {
            nPos = new Vector3(leftX, nPos.y, nPos.z);
        }
        if (nPos.y > topY)
        {
            nPos = new Vector3(nPos.x, topY, nPos.z);
        }
        else if (nPos.y < bottomY)
        {
            nPos = new Vector3(nPos.x, bottomY, nPos.z);
        }
        #endregion

        gameObject.transform.position = nPos;

    }

    public void EnemyAttacked()
    {
        weHitted = true;
    }

    private void DrawCorner()
    {
        #region TopBarrier
        Debug.DrawLine(
            new Vector2(LeftBarrier.x, TopBarrier.y),
            new Vector2(RightBarrier.x, TopBarrier.y),
            Color.blue,
            1000
            );
        #endregion
        #region BottomBarrier
        Debug.DrawLine(
             new Vector2(LeftBarrier.x, BottomBarrier.y),
             new Vector2(RightBarrier.x, BottomBarrier.y),
             Color.blue,
             1000
        );
        #endregion
    }
}
