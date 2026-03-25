 using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("캐릭터 설정")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float flyForce = 5.0f;
    [SerializeField] private int drillDamage = 25;
    [SerializeField] private float drillRange = 1.0f;
    [SerializeField] private float drillDelay = 0.2f;
    [SerializeField] private float flyAcceleration = 1f; //속도 증가율
    [SerializeField] private float flyMaxSpeed = 5f;      //최대 상승 속도

    [Header("바닥 체크")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;
    public bool isGrounded;

    private float inputX;
    private float inputY;
    private bool isDrillingInput = false;

    private Rigidbody2D rb;

    private Map map;
    private float lastDrillTime;

    private Animator anim;
    private SpriteRenderer sr;

    private InventoryShop invenShop;

    public int GetDrillDamage() => drillDamage;
    public float GetMoveSpeed() => moveSpeed;
    public bool GetIsGrounded() => isGrounded;

    [Header("오디오")]
    public AudioClip drillClip; //드릴 소리 파일
    private AudioSource audioSource;
    private bool isDrillingSoundPlaying = false;

    private void Awake()
    {
        if (invenShop == null)
        {
            invenShop = FindObjectOfType<InventoryShop>();
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        map = FindObjectOfType<Map>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;    //반복 재생 설정
        audioSource.playOnAwake = false; //자동 재생 해제
        audioSource.clip = drillClip;
    }
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        Move();
        Fly();

        bool shouldDrill = isGrounded && (inputY < 0 || inputX != 0);

        if (shouldDrill && Time.time - lastDrillTime >= drillDelay)
        {
            Drill();
            lastDrillTime = Time.time;
            //드릴 시작 시점에 소리 재생
            PlayDrillSound();
        }
        else if (!shouldDrill)
        {
            //드릴 조건이 아니면 소리 정지
            StopDrillSound();
        }

    }

    private void Move()
    {
        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);

        //좌우반전
        if (inputX != 0f)
        {
            if (inputX < 0.0f)
            {
                sr.flipX = true;

            }
            else
            {
                sr.flipX = false;
            }
        }
        if (inputX != 0)
        {
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
    }

    private void Fly()
    {
        if (inputY > 0f)
        {
            float targetSpeed = flyMaxSpeed;

            //현재 속도보다 작으면 가속
            if (rb.velocity.y < targetSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + flyAcceleration);

                //최대 속도 제한
                if (rb.velocity.y > targetSpeed)
                    rb.velocity = new Vector2(rb.velocity.x, targetSpeed);
            }
        }
    }
    

    private void Drill()
    {
        Vector2 direction = Vector2.zero;

        if(inputY < 0) direction = Vector2.down;
        else if(inputX<0) direction = Vector2.left;
        else if(inputX>0) direction = Vector2.right;
        //윗키는 무시하기
        if (direction == Vector2.zero) return;

        //플레이어 기준으로 드릴방향의 타일
        Vector3Int targatTilePos = map.groundTile.WorldToCell(transform.position + (Vector3)direction * drillRange);

        if (inputY < 0)
        {
            anim.SetTrigger("IsDrilling");
        }

        int tileHp = map.GetTileHp(targatTilePos);
        if (tileHp>0)
        {
            map.DamagedTile(targatTilePos, drillDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shop"))
        {
            invenShop.OpenShop();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Shop"))
        {
            invenShop.CloseAll();
        }
    }

    public void ApplyDrillStat(int amount)
    {
        drillDamage += amount;
        drillDamage = Mathf.Max(0, drillDamage);
    }

    public void ApplySpeedStat(float speedAmount, float flyAmount)
    {
        moveSpeed += speedAmount;
        flyForce += flyAmount;

        moveSpeed = Mathf.Max(1f, moveSpeed);
        flyForce = Mathf.Max(1f, flyForce);
    }

    private void PlayDrillSound()
    {
        if (!isDrillingSoundPlaying && drillClip != null)
        {
            audioSource.Play();
            isDrillingSoundPlaying = true;
            Debug.Log("드릴 소리 재생");
        }
    }

    private void StopDrillSound()
    {
        if (isDrillingSoundPlaying)
        {
            audioSource.Stop();
            isDrillingSoundPlaying = false;
            Debug.Log("드릴 소리 정지");
        }
    }
}
