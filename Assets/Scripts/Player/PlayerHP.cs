using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float O2Amount = 100f;
    [SerializeField] private float maxO2 = 100f;
    [SerializeField] private int fallY = -10;
    [SerializeField] public int money = 0;

    private bool falled = false;

    private float startTime;

    private Map map;

    public static PlayerHP instance;

    public float GetMaxO2() => maxO2;

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
        //10이상 떨어지면 체력닳기
        if (!falled && transform.position.y < fallY)
        {
            health -= 20;
            falled = true;
        }

        //한번 떨어지고 나서 낙사판정 초기화하기
        if(falled && transform.position.y > fallY)
        {
            falled = false;
        }
    }

    private void O2()
    {
        //Map스크립트에서 산소소모량 함수
        float takeO2 = map.TakeO2(transform.position.y);

        //땅 위에서는 산소 회복
        if (transform.position.y >= 0)
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
        O2Amount = Mathf.Max(O2Amount, 0, maxO2);

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
        }
    }

    public void ApplyO2Stat(float amount)
    {
        maxO2 += amount;
        if (O2Amount > maxO2)
            O2Amount = maxO2;
    }
}
