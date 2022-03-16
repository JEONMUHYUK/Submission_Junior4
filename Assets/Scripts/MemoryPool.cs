using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    // 메모리 풀로 관리되는 오브젝트 정보
    private class PoolItem
    {
        public  bool        isActive;       // gameObject의 활성화/비활성화 정보
        public  GameObject  gameObject;     // 화면에 보이는 실제 게임 오브젝트
    }

    private int increaseCount = 5;          // 오브젝트가 부족할 때 Instance()로 추가 생성되는 오브젝트 개수
    private int maxCount;                   // 현재 리스트에 등록되어 있는 오브젝트 개수
    private int activeCount;                // 현재 게임에 사용되고 있는 (활성화)오브젝트 개수

    private GameObject poolObject;          // 오브젝트 풀링에서 관리하는 게임 오브젝트 프리팹
    private List<PoolItem> poolItemList;    // 관리되는 모든 오브젝트를 저장하는 리스트

    public int MaxCount => maxCount;        // 외부에서 현재 리스트에 등록되어 있는 오브젝트 개수 확인을 위한 프로퍼티
    public int ActiveCount => activeCount;  // 외부에서 현재 활성화 되어 있는 오브젝트 개수 확인을 위한 프로퍼티

    // 오브젝트가 임시로 보관되는 위치 
    // ? (플레이어와 위치가 겹치면 플레이어의 y축이 움직이기 때문이다.)
    private Vector3 tempPosition = new Vector3(48, 1, 48);

    public MemoryPool(GameObject poolObject)
    {
        // 메모리 풀 생성자에서 변수를 초기화
        maxCount        = 0;
        activeCount     = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObject();                // 최초 5개 아이템을 미리 생성.
    }

    /// <summary>                           //xml방식 summary안에 내용을 적으면 툴팁을 통해 나타난다
    /// increaseCount 단위로 오브젝트를 생성
    /// </summary>
    public void InstantiateObject()
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++ i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive =  false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);   //Instantiate() 메소드를 이용해 오브젝트 생성
            poolItem.gameObject.transform.position = tempPosition;
            poolItem.gameObject.SetActive(false);
            // 바로 사용 하지 않을 수도 있기 때문에 active를 false로 설정하여 보이지 않게 한다.

            poolItemList.Add(poolItem);
        }
    }
    /// <summary>                         
    /// 현재 관리중인(활성/비활성) 모든 오브젝트를 삭제
    /// </summary>
    public void DestroyObjects()
    {
        if ( poolItemList == null ) return;

        int count = poolItemList.Count;
        for ( int i = 0; i < count; ++ i )
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }
        
        poolItemList.Clear();
        // 씬이 바뀌거나 게임이 종료될 떄 한번만 수행하여 모든 게임 오브젝트를 한번에 삭제한다.
    }

    /// <summary>                         
    /// poolItemList에 저장되어 있는 오브젝트를 활성화해서 사용
    /// 현재 모든 오브젝트가 사용중이면 InstantiateObjects()로 추가 생성
    /// </summary>
    public GameObject ActivatePoolItem()
    {
        if ( poolItemList == null ) return null;                    //아이템리스트가 비어있으면 null을 return.

        // 현재 생성해서 관리하는 모든 오브젝트 개수와 현재 활성화 상태인 오브젝트 개수 비교
        // 모든 오브젝트가 활성화 상태이면 새로운 오브젝트 필요
        if ( maxCount == activeCount )
        {
            // 현재 리스트에 등록되어 있는 오브젝트 개수가 활성화 개수와 같다면 오브젝트 추가 생성.
            InstantiateObject();
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++ i)
        {
            PoolItem poolItem = poolItemList[i];

            if ( poolItem.isActive == false )
            {
                activeCount ++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;
    }

    /// <summary>                         
    /// 현재 사용이 완료된 오브젝트를 비활성화 상태로 설정
    /// </summary>
    public void DeactivatePoolItem(GameObject removeObject)
    {
        // 현재 활성상태인 reboveObject를 비활상태로 만들어 보이지 않게 한다.
        if( poolItemList == null || removeObject == null ) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++ i)
        {
            PoolItem poolItem = poolItemList[i];

            if ( poolItem.gameObject == removeObject )
            {
                activeCount --;

                poolItem.gameObject.transform.position = tempPosition;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    /// <summary>                         
    /// 게임에 사용중인 모든 오브젝트를 비활성화 상태로 설정
    /// </summary>
    public void DeactivateAllPoolItem()
    {
        if ( poolItemList == null ) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++ i)
        {
            PoolItem poolItem = poolItemList[i];

            if ( poolItem.gameObject != null && poolItem.isActive == true )
            {   
                poolItem.gameObject.transform.position = tempPosition;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;
    }
}
