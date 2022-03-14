using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private MovementTransform   movement;
    private float               projectileDistance = 30;    // 발사체 최대 사거리
    private int                 damage = 5;                // 발사체의 공격력

    public void Setup(Vector3 position)
    {
        movement = GetComponent<MovementTransform>();

        StartCoroutine("OnMove", position);
    }

    private IEnumerator OnMove(Vector3 tartgetPosition)
    {
        Vector3 start = transform.position;

        movement.MoveTo((tartgetPosition-transform.position).normalized);   // 이동방향 설정

        while (true)
        {
            // 발사체의 시작지점 부터 현재위치가 projectileDistance 보다 크거나 같아지면 발사체를 삭제한다.
            if ( Vector3.Distance(transform.position, start) >= projectileDistance )
            {
                Destroy(gameObject);

                yield break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        // 발사체가 플레이어와 충돌하면 "Player Hit"를 출력하고 발사체를 삭제한다.
        if ( other.CompareTag("Player") )
        {
            //Debug.Log("Player Hit");
            other.GetComponent<PlayerController>().TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
