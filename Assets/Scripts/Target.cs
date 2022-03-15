using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : InteractionObject
{
    [SerializeField]
    private AudioClip   clipTartgetUP;
    [SerializeField]
    private AudioClip   clipTartgetDown;
    [SerializeField]
    private float       targetUPDelayTime = 3;

    private AudioSource audioSource;
    private bool        isPossibleHit = true;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public override void TakeDamage(int damage)
    {
        currentHP -= damage;

        if ( currentHP <= 0 && isPossibleHit == true)
        {
            isPossibleHit = false;

            StartCoroutine("OnTargetDown");
        }
    }

    private IEnumerator OnTargetDown()
    {
        audioSource.clip = clipTartgetDown;
        audioSource.Play();

        yield return StartCoroutine(OnAnimation(0, 90));

        StartCoroutine("OnTargetUp");
    }

    private IEnumerator OnTargetUp()
    {
        yield return new WaitForSeconds(targetUPDelayTime);

        audioSource.clip = clipTartgetUP;
        audioSource.Play();

        yield return StartCoroutine(OnAnimation(90, 0));

        //OnAnimation 코루틴이 완전히 종료된후 isPossibleHit가 true로 변경된다.
        isPossibleHit = true;
    }

    private IEnumerator OnAnimation(float start, float end)
    {
        float percent = 0;
        float current = 0;
        float time    = 1;

        while ( percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;
            
            // Tip. 과녁이 아래 축을 기준으로 회전하도록 빈 오브젝트를 부모로 설정하였다.
            // Slerp = start 각도부터 end각도까지 percent 시간동안 회전한다.
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(start, 0, 0), Quaternion.Euler(end, 0, 0), percent);

            yield return null;
        }
    }


}
