using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float O2Amount = 100f;
    [SerializeField] private float maxO2 = 100f;
    [SerializeField] private int fallY = -10;
    [SerializeField] public int money = 0;

    private bool falled = false;
    private float fallStartY = float.NaN;

    private float startTime;

    private Map map;

    public static PlayerHP instance;
    private Rigidbody2D rb;
    private PlayerController playerController;

    public float GetMaxO2() => maxO2;
    public float GetO2Amount() => O2Amount;
    public int GetHealth() => health;
    public int GetMaxHealth() => 100;

    private GameOverUI gameOverUI;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        startTime = Time.time;
        map = FindObjectOfType<Map>();
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        gameOverUI = FindObjectOfType<GameOverUI>();
    }
    void Update()
    {
        //시작 2초간은 낙사안함
        if (Time.time - startTime < 2f) return;
        Fall();
        Die();
        O2();
    }

    //낙사판정
    private void Fall()
    {
        if (!float.IsNaN(fallStartY))
        {
            //이미 추적 중
            if (playerController.GetIsGrounded())
            {
                float fallDistance = fallStartY - transform.position.y;
                if (fallDistance > Mathf.Abs(fallY))    //10이상 떨어지면 낙뎀
                {
                    health -= 20;
                }
                fallStartY = float.NaN; //초기화
            }
        }
        else if (rb.velocity.y < 0f)
        {
            //떨어지기 시작
            fallStartY = transform.position.y;
        }
    }

    private void O2()
    {
        //Map스크립트에서 산소소모량 함수
        float takeO2 = map.TakeO2(transform.position.y);

        //땅 위에서는 산소 회복
        if (transform.position.y >= 0f)
        {
            if (O2Amount < maxO2)
            {
                O2Amount += 10f * Time.deltaTime;
            }
        }
        //땅 아래서는 산소 줄기
        else
        {
            O2Amount -= takeO2 * Time.deltaTime;
        }

        //maxO2 ~ 0사이로 제한하기
        O2Amount = Mathf.Clamp(O2Amount, 0, maxO2);

        //산소 없으면 체력감소
        if(O2Amount <= 0)
        {
            //깊어질수록 산소소모량 많게
            health -= Mathf.RoundToInt(takeO2 * 2f * Time.deltaTime * 10f);
        }
    }
    private void Die()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);

            if (gameOverUI != null)
            {
                gameOverUI.Show();
            }
        }
    }

    public void ApplyO2Stat(float amount)
    {
        maxO2 += amount;
        O2Amount = Mathf.Min(O2Amount, maxO2);
    }

    public void ApplyO2Up(float amount)
    {
        O2Amount += amount;
        O2Amount = Mathf.Clamp(O2Amount, 0, maxO2); // 0~maxO2 사이로 제한
    }

    public void ApplyHeal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, GetMaxHealth()); // 0~최대 체력 사이로 제한
    }
}
