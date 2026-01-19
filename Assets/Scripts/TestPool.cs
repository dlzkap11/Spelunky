using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class TestPool : MonoBehaviour
{
    public GameObject[] prefab;
    public GameObject[] gameObjects;
    public int objNum = 50;
    public float time = 1.0f;

    [SerializeField]
    int _monsterCount = 0;
    [SerializeField]
    int _reserveCount = 0;

    [SerializeField]
    int _keepMonsterCount = 50;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }


    //Action 활용하면 더 편할 듯;;
    public static TestPool Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void AddMon()
    {
        _monsterCount++;
    }

    public void RemoveMonster()
    {
        _monsterCount--;
    }

    void Start() // 씬 전환시 중복되는 버그
    {
        RemoveMonster();
        AddMon();
        Managers.Pool.CreatePool(prefab[0], objNum);
        Managers.Pool.CreatePool(prefab[1], objNum);
    }

    private void Update()
    {
        if (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;

        yield return new WaitForSeconds(time);
        GameObject obj = Managers.Pool.Instantiate(prefab[0]);

        _reserveCount--;
    }

}
