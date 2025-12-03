using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class IntroStatue : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Roate180(1.5f, 3.0f));
    }

    IEnumerator Roate180(float rotDur, float posDur)
    {
        Transform head = transform.GetChild(0);
        Transform body = transform.GetChild(1);

        Quaternion startRotation = head.rotation;
        Quaternion endRotation = head.rotation * Quaternion.Euler(0f, 0f, 180f);
        float timeElapsed = 0f;

        while (timeElapsed < rotDur)
        {
            timeElapsed += Time.deltaTime;
            // Lerp를 사용하여 시작점에서 끝점까지 부드럽게 보간합니다.
            head.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / rotDur);
            yield return null; // 다음 프레임까지 대기
        }


        yield return new WaitForSeconds(3.6f);


        timeElapsed = 0f; // 초기화
        float totalDist = 7f;


        while (timeElapsed < posDur)
        {
            timeElapsed += Time.deltaTime;

            transform.position += Vector3.down
                          * (totalDist / posDur)  // 초당 거리
                          * Time.deltaTime;
            yield return null;
        }


    }


}
