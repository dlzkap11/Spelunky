using System.Collections;
using UnityEngine;

public class FlamePool : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // 월드 기준 삼각형 꼭짓점
    private Vector3 pointA = new Vector3(-3.535f, 1.827f, 0f);
    private Vector3 pointB = new Vector3(-4.281f, 1.077f, 0f);
    private Vector3 pointC = new Vector3(-3.029f, 1.059f, 0f);
    private Vector3 pos;

    private float timer = 0f;
    public float targetScale = 0.1f;
    private float shrinkDuration;       // 작아지는데 걸리는 시간

    public float startScale = 0.5f;
    public float growDuration = 0.4f;       // 커지는데 걸리는 시간

    public Vector3 direction = Vector3.up;  // 초기방향
    public float acceleration = 2f;         // 가속도
    public float curveAmount = 30f;        // 왼쪽 휘어짐 세기
    private Vector3 velocity;               // 현재 속도

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            0.7f
        );
    }

    private void OnEnable()
    {
        //TestPool.Instance.AddMon();
        Clear();
        transform.position = pos;
        spriteRenderer.sortingOrder = Random.Range(1, 3);

        StartCoroutine("DisableObject");

    }

    void Clear()
    {
        direction = Vector3.up;
        velocity = Vector3.zero;
        transform.localScale = Vector3.one * startScale;
        timer = 0f;
        pos = GetRandomPointInTriangle(
               pointA,
               pointB,
               pointC
           );

        curveAmount = Random.Range(10f, 30f);
        shrinkDuration = Random.Range(1.2f, 3f);


    }

    private void Update()
    {
        timer += Time.deltaTime;
        //가속도 + 왼쪽으로 조금씩 휘어지게
        
        velocity += direction * acceleration * Time.deltaTime; // 1. 가속도 적용 (속도 증가)
        direction = Quaternion.Euler(0, 0, curveAmount * Time.deltaTime) * direction; // 2. 왼쪽으로 휘어짐 (방향 벡터 회전)
        transform.position += velocity * Time.deltaTime; // 3. 위치 업데이트
        

        if (timer <= growDuration)
        {
            // 작 -> 1
            float t = timer / growDuration;
            transform.localScale =
                Vector3.Lerp(Vector3.one * startScale, Vector3.one, t);
        }
        else if (timer <= growDuration + shrinkDuration)
        {
            // 1 -> targetScale
            float t = (timer - growDuration) / shrinkDuration;
            transform.localScale =
                Vector3.Lerp(Vector3.one, Vector3.one * targetScale, t);
        }

    }


    Vector3 GetRandomPointInTriangle(Vector3 A, Vector3 B, Vector3 C)
    {
        float r1 = Random.value;
        float r2 = Random.value;

        // 균일 분포를 위한 변형 (barycentric 기반)
        float sqrtR1 = Mathf.Sqrt(r1);
        float u = 1f - sqrtR1;
        float v = sqrtR1 * (1f - r2);
        float w = sqrtR1 * r2;

        // P = u*A + v*B + w*C
        return u * A + v * B + w * C;
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(shrinkDuration + growDuration + 0.1f);
        //gameObject.SetActive(false);
        TestPool.Instance.RemoveMonster();
        Managers.Pool.Destroy(gameObject);
    }

}