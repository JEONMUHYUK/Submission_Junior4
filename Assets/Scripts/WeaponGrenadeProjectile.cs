using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenadeProjectile : MonoBehaviour
{
    [Header("Explosion Barrel")]
    [SerializeField]
    private GameObject      explosionPrefab;
    [SerializeField]
    private float           explosionRadius     = 10.0f;
    [SerializeField]        
    private float           explosionForce     = 500.0f;
    [SerializeField]
    private float           throwForce          = 1000.0f;

    private int             explsionDamage;
    private new Rigidbody   rigidbody;

    public void Setup(int damage, Vector3 rotation)
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(rotation * throwForce);

        explsionDamage = damage;
    }

    private void OnCollisionEnter(Collision collision) 
    {
        // 폭팔 이펙트 생성
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // 폭발 범위에 있는 모든 오브젝트의 Collider 정보를 받아와 폭발 효과 처리
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // 폭발 범위에 부딪힌 오브젝트가 플레이어일 때 처리
            PlayerController player = hit.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage((int)(explsionDamage * 0.2f));
                // continue 로 건너뛰도록 설정하여서 rigidbody를 가지고 있어도 날라가지 않는다.
                continue;
            }

            // 폭발 범위에 부딪힌 오브젝트가 적 캐릭터 일 때 처리
            EnemyFSM enemy = hit.GetComponentInParent<EnemyFSM>();
            if (enemy != null)
            {
                enemy.TakeDamage(explsionDamage);
                continue;
            }

            // 폭발 범위에 부딪힌 오브젝트가 상호작용 오브젝트이면 TakeDamge()로 피해를 줌
            InteractionObject interaction = hit.GetComponent<InteractionObject>();
            if (interaction != null)
            {
                interaction.TakeDamage(explsionDamage);
            }

            // 중력을 가지고 있는 오브젝트이면 힘을 받아 밀려나도록
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // 수류탄 오브젝트 삭제
        Destroy(gameObject);
    }
}   
