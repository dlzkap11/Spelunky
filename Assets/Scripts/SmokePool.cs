using System.Collections;
using UnityEngine;

public class SmokePool : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private float timer = 0f;


    private Vector3 direction = Vector3.up;  // 초기방향
    private float acceleration = 2f;         // 가속도
    private float curveAmount;        // 왼쪽 휘어짐 세기
    private Vector3 velocity;               // 현재 속도

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            0.3f
        );
    }

    private void OnEnable()
    {
        Clear();
        
        spriteRenderer.sortingOrder = Random.Range(1, 3);

        StartCoroutine("DisableObject");

    }

    void Clear()
    {
        direction = Vector3.up;
        velocity = Vector3.zero;
        timer = 0f;
        transform.position = new Vector3(
            Random.Range(-4.0f, -3.3f),
            Random.Range(1.9f, 2.25f),
            0f
            );

        curveAmount = Random.Range(-10f, 60f);

    }

    private void Update()
    {
        timer += Time.deltaTime;
        //가속도 + 왼쪽으로 조금씩 휘어지게

        velocity += direction * acceleration * Time.deltaTime; // 1. 가속도 적용 (속도 증가)
        direction = Quaternion.Euler(0, 0, curveAmount * Time.deltaTime) * direction; // 2. 왼쪽으로 휘어짐 (방향 벡터 회전)
        transform.position += velocity * Time.deltaTime; // 3. 위치 업데이트

    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
    }

}
