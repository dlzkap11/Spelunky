using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; //유일성이 보장됨
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    void Start()
    {
        Init();
    }

    #region core
    IntroManager _intro = new IntroManager();
    PoolManager _pool = new PoolManager();

    public static IntroManager Intro { get { return Instance._intro; }  }
    public static PoolManager Pool { get { return Instance._pool; } }
    #endregion

    void Update()
    {
        
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }


        s_instance._pool.Init();
    }

    public static void Clear()
    {
        Pool.Clear();
    }
}
