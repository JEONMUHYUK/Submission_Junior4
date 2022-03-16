using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnifeCollider : MonoBehaviour
{
    [SerializeField]
    private ImapctMemoryPool    imapctMemoryPool;
    [SerializeField]
    private Transform           knifeTransform;

    private new Collider        collider;
    private int                 damage;

    private void Awake() {
        // 충돌범위 컴포넌트 비활성화
        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public void StartCollider(int damage)
    {
        // 단검 공격시 특정 프레임에서 호출
        this.damage         = damage;
        // 충돌 범위 컴포넌트 활성화
        collider.enabled    = true;

        StartCoroutine("DisablebyTime", 0.1f);
    }

    private IEnumerator DisablebyTime(float time)
    {   
        // 0.1초 후에 collider 충돌 범위를 비활성화.
        yield return new WaitForSeconds(time);

        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other) 
    {
        // 충돌범위에 부딪힌 오브젝트가 있을 때 실행
        imapctMemoryPool.SpawnImpact(other, knifeTransform);

        if ( other.CompareTag( "ImpactEnemy" ) )
        {
            other.GetComponentInParent<EnemyFSM>().TakeDamage(damage);
        }
        else if ( other.CompareTag( "InteractionObject" ) )
        {
            other.GetComponent<InteractionObject>().TakeDamage(damage);
        }
    }
}
