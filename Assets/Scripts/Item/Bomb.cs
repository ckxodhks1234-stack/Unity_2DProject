using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float fuseTime = 1.0f;     //1초 뒤 폭발
    public int damage = 50;           //폭발 데미지

    private Animator anim;
    private bool exploded = false;

    public AudioClip explosionSound;
    private AudioSource audioSource;

    void Start()
    {
        //1초 뒤 폭발
        Invoke(nameof(Explosion), fuseTime);

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Explosion()
    {
        if (exploded) return; //이미 폭발했으면 무시
        exploded = true;

        anim.SetTrigger("Explode"); //폭발 애니메이션 재생
        audioSource.PlayOneShot(explosionSound); //폭발음 재생

        //폭발 범위 내의 적들에게 데미지 적용
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        foreach (var hit in hits)
        {
            if (hit == null) continue;

            var player = hit.GetComponent<PlayerHP>();
            if (player != null)
            {
                //플레이어가 맞으면 피해
                player.ApplyHeal(-damage); //HP 감소
            }
        }

        //폭탄 오브젝트 제거
        Destroy(gameObject, 1f);
    }

    //폭발 범위 확인용
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
