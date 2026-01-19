using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FxSequence : MonoBehaviour
{
    [Header("Life Time")]
    public bool autoDisable = true;      // 끝나면 자동 비활성화

    [Header("Movement")]
    public bool useMove = true;                // 이동 사용 여부
    public Vector3 startDirection = Vector3.up;
    public float acceleration = 2f;
    public float curveAmount = 30f;            // Z축 회전 속도(도/초)
    public float randomPosXMin = -4.3f;
    public float randomPosXMax = -3f;
    public float randomPosYMin = 1.0f;
    public float randomPosYMax = 2.0f;

    [Header("Visual")]
    [Range(0f, 1f)]
    public float startAlpha = 0.5f;

    SpriteRenderer spriteRenderer;
    Vector3 direction;
    Vector3 velocity;
    float timer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        ResetState();
        StartCoroutine(LifeRoutine());
    }

    void ResetState()
    {
        // 위치 랜덤
        transform.position = new Vector3(
            Random.Range(randomPosXMin, randomPosXMax),
            Random.Range(randomPosYMin, randomPosYMax),
            transform.position.z
        );

        // 방향/속도/타이머 초기화
        direction = startDirection.normalized;
        velocity = Vector3.zero;
        timer = 0f;

        // 스케일 & 알파 & 소팅 오더

        var c = spriteRenderer.color;
        c.a = startAlpha;
        spriteRenderer.color = c;

        curveAmount = Random.Range(0f, 50f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        // 이동
        if (useMove)
        {
            
            velocity += direction * acceleration * Time.deltaTime;
            direction = Quaternion.Euler(0, 0, curveAmount * Time.deltaTime) * direction;
            transform.position += velocity * Time.deltaTime;
        }

    }

    IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        if (autoDisable)
            gameObject.SetActive(false);
    }
}
