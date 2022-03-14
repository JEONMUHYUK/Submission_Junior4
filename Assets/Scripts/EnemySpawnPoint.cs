using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float               fadeSpeed = 4;
    private MeshRenderer        meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable() {
        StartCoroutine("OnFadeEffect");
    }

    private void OnDisable() {
        StopCoroutine("OnFadeEffect");
    }

    private IEnumerator OnFadeEffect()
    {
        while (true)
        {
            Color color = meshRenderer.material.color; // 메테리얼 컬러를 변수에 저장
            // color.a = 색상창의 Alpha(투명도)값을 PingPong 메서드를 통해 0과1로 반복, 
            // Lerp로 퍼센테이지를 0~1로 맞추어 사용한다.
            color.a     = Mathf.Lerp(1, 0, Mathf.PingPong(Time.time * fadeSpeed, 1));
            meshRenderer.material.color = color;

            yield return null;
        }
    }
}
