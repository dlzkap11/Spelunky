using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class IntroDoor : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Roate180(1.5f, 3.0f));
    }

    IEnumerator Roate180(float rotDur, float posDur)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = transform.rotation * Quaternion.Euler(0f, 0f, -180f);
        float timeElapsed = 0f;

        

        while (timeElapsed < rotDur)
        {
            timeElapsed += Time.deltaTime;
            
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / rotDur);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Transform door1 = transform.GetChild(0);
        Transform door2 = transform.GetChild(1);

        timeElapsed = 0f; // 초기화
        float totalDist = 5f;

        while (timeElapsed < posDur)
        {
            timeElapsed += Time.deltaTime;

            door1.position += Vector3.right
                          * (totalDist / posDur)  // 초당 거리
                          * Time.deltaTime;

            door2.position += Vector3.left
                          * (totalDist / posDur)  // 초당 거리
                          * Time.deltaTime;


            yield return null;
        }
        

    }


}
