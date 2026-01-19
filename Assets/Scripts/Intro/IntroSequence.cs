using NUnit;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    private CancellationTokenSource cts;


    [Header("Rotation")]
    public Transform rotateTarget;
    public Vector3 localEuler;
    public float rotDur;

    [Header("Delay After Rotation")]
    public float delayAfterRot;

    [Header("Move")]
    public Transform moveTarget1;
    public Transform moveTarget2;
    public Vector3 moveDir1;
    public Vector3 moveDir2;
    public float moveDist;
    public float moveDur;

    void Start()
    {

        if (!Managers.Intro.IntroPlayed)
        {
            Invoke(nameof(StartIntro), 0.3f);
        }
        else
        {
            SkipIntro();
        }
        
    }

    void StartIntro()
    {
        StartCoroutine(Play());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            cts?.Cancel();
        }
    }

    IEnumerator Play()
    {
        cts = new CancellationTokenSource();
        var token = cts.Token;

        float elapsed = 0f;
        while (elapsed < 0.2f)
        {
            if (token.IsCancellationRequested)
            {
                SkipToEnd();
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }


        // 회전
        if (rotateTarget != null && rotDur > 0f)
        {
            Quaternion start = rotateTarget.rotation;
            Quaternion end = start * Quaternion.Euler(localEuler);

            float t = 0f;
            while (t < rotDur)
            {
                if (token.IsCancellationRequested)
                {
                    SkipToEnd();
                    yield break;
                }

                t += Time.deltaTime;
                rotateTarget.rotation = Quaternion.Slerp(start, end, t / rotDur);
                yield return null;
            }
        }

        elapsed = 0f;
        while (elapsed < delayAfterRot)
        {
            if (token.IsCancellationRequested)
            {
                SkipToEnd();
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }


        // 이동
        if (moveDur > 0f)
        {
            float t = 0f;
            float speed = moveDist / moveDur;
            while (t < moveDur)
            {
                if (token.IsCancellationRequested)
                {
                    SkipToEnd();
                    yield break;
                }

                t += Time.deltaTime;

                if (moveTarget1 != null)
                    moveTarget1.position += moveDir1.normalized * speed * Time.deltaTime;

                if (moveTarget2 != null)
                    moveTarget2.position += moveDir2.normalized * speed * Time.deltaTime;

                yield return null;
            }
        }

        Managers.Intro.IntroPlayed = true;
    }

    void SkipToEnd()
    {
        SkipIntro();

        Managers.Intro.IntroPlayed = true;
        //Debug.Log("End!");
    }

    void SkipIntro()
    {
        if (rotateTarget != null)
            rotateTarget.rotation = Quaternion.identity * Quaternion.Euler(localEuler);

        if (moveTarget1 != null)
            moveTarget1.position = moveDir1 * moveDist;

        if (moveTarget2 != null)
            moveTarget2.position = moveDir2 * moveDist;
    }

}
