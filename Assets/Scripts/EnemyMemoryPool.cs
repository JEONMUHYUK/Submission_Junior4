using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private Transform   target;                             // 적의 목표 (플레이어)
    [SerializeField]
    private GameObject  enemySpawnPointPrefab;              // 적이 등장하기 전 적의 등장 위치를 알려주는 프리펩
    [SerializeField]
    private GameObject  enemyPrefab;                        // 생성되는 적 프리펩
    [SerializeField]
    private float       enemySpawnTime = 1;                 // 적 생성주기
    [SerializeField]
    private float       enemySapwnLatency = 1;              // 타일 생성 후 적이 등장하기 까지 대기시간

    private MemoryPool  spawnPointMemoryPool;               // 적 등장 위치를 알려주는 오브젝트 생성, 활성/ 비활성 관리
    private MemoryPool  enemyMemoryPool;                    // 적 생성, 활성/비활성 관리

    private int         numberOfEnemiesSpawnedAtOnce = 1;   // 동시에 생성되는 적 숫자
    private Vector2Int  mapSize = new Vector2Int(100, 100); // 맵 크기

    private void Awake() {
        spawnPointMemoryPool = new MemoryPool(enemySpawnPointPrefab);
        enemyMemoryPool      = new MemoryPool(enemyPrefab);

        StartCoroutine("SpawnTile");
    }

    private IEnumerator SpawnTile()
    {
        int currentNumber = 0;
        int maximumNumber = 50;

        while (true)
        {
            // 동시에 numberOfEnemiesSpawnedAtOnce 숫자만큼 적이 생성되도록 반복문 사용
            for (int i = 0; i < numberOfEnemiesSpawnedAtOnce; ++ i)
            {
                // ActivatePoolItem 메서드를 호출해 기둥을 생성할 수 있게 하고,
                GameObject item = spawnPointMemoryPool.ActivatePoolItem();

                // 기둥의 위치는 맵 내부 랜덤하게 배치한다.
                item.transform.position = new Vector3(Random.Range(-mapSize.x*0.49f, mapSize.x*0.49f), 1,
                                                    Random.Range(-mapSize.y*0.49f, mapSize.y*0.49f) );

                // 기둥의 위치에 적이 생성되게 한다.
                StartCoroutine("SpawnEnemy", item);
            }

            //  for문이 끝날 때마다 currentNumber의 숫자를 1씩 증가시킨다.
            currentNumber ++;

            // currentNumber가 maximumNumber 보다 크거나 같아질때 currentNumber 을 초기화하고 
            // 적 동시생산 변수인 numberOfEnemiesSpawnedAtOnce 를 1씩 증가시켜 준다.
            if ( currentNumber >= maximumNumber )
            {   
                currentNumber = 0;
                numberOfEnemiesSpawnedAtOnce ++ ;
            }

            // 이 과정을 enemySpawnTime마다 반복
            yield return new WaitForSeconds(enemySpawnTime);
        }
    }

    private IEnumerator SpawnEnemy(GameObject point)
    {
        // enemySapwnLatency 시간동안 대기.
        yield return new WaitForSeconds(enemySapwnLatency);

        // 적 오브젝트를 생성하고, 적의 위치를 point(기둥)의 위치로 설정
        GameObject item = enemyMemoryPool.ActivatePoolItem();
        item.transform.position = point.transform.position;

        item.GetComponent<EnemyFSM>().Setup(target, this);

        // 타일 오브젝트를 비활성화
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }

    public void DeactivateEnemy(GameObject enemy)
    {
        enemyMemoryPool.DeactivatePoolItem(enemy);
    }
}
