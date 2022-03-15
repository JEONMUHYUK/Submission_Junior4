using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType { Normal = 0, Obstacle, Enemy, InteractionObject, }

public class ImapctMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[]        impactPrefab;       // 피격 임펙트
    private MemoryPool[]        memoryPool;         // 피격 임펙트 메모리풀

    private void Awake() 
    {
        // 피격 이펙트가 여러 종류이면 종류별로 memoryPool 생성
        memoryPool = new MemoryPool[impactPrefab.Length];
        for (int i = 0; i < impactPrefab.Length; ++ i)
        {
            memoryPool[i] = new MemoryPool(impactPrefab[i]);
        }
    }

    public void SpawnImpact(RaycastHit hit)
    {
        // 부딪힌 오브젝트의 Tag 정보에 따라 다르게 처리
        if ( hit.transform.CompareTag("ImpactNormal") )
        {
            OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if ( hit.transform.CompareTag("ImpactObstacle") )
        {
            OnSpawnImpact(ImpactType.Obstacle, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if ( hit.transform.CompareTag("ImpactEnemy") )
        {
            OnSpawnImpact(ImpactType.Enemy, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if ( hit.transform.CompareTag("InteractionObject") )
        {
            // 상호작용 오브젝트의 종류가 많기 때문에 오브젝트별로 타격효과를 생성하지 않고,
            // 오브젝트 색상에 따라 색상만 바뀌도록 설정
            Color color = hit.transform.GetComponentInChildren<MeshRenderer>().material.color;
            OnSpawnImpact(ImpactType.InteractionObject, hit.point, Quaternion.LookRotation(hit.normal), color);
        }
    }

    public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation, Color color = new Color())
    {
        GameObject item = memoryPool[(int)type].ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Impact>().Setup(memoryPool[(int)type]);

        if ( type == ImpactType.InteractionObject )
        {   
            // ParticleSystem의 main 프로퍼티는 바로 접근할 수 없기 때문에 변수를 생성한 후 접근해서 사용한다.
            ParticleSystem.MainModule main = item.GetComponent<ParticleSystem>().main;
            // 스타트컬러를 매게변수로 받아온 컬러로 사용한다.
            main.startColor = color;
        }
    }
}
