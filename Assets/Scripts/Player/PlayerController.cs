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

    public GameObject bombPrefab;
    public int GetDrillDamage() => drillDamage;
    public float GetMoveSpeed() => moveSpeed;
    public bool GetIsGrounded() => isGrounded;
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

    }
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (isGrounded)
        {
            isDrillingInput = (inputY < 0 || inputX != 0) && (Time.time - lastDrillTime >= drillDelay);
        }

        //Z키 누르면 폭탄 설치
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (bombPrefab == null) return;

            //플레이어 위치에 폭탄 생성
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        Move();
        Fly();

        if (isDrillingInput) Drill();
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
            bool destroyed = map.DamagedTile(targatTilePos, drillDamage);

            lastDrillTime = Time.time;  
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
}
