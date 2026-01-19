using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;

public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }
        public Transform SceneRoot { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            SceneRoot = new GameObject().transform;
            SceneRoot.name = $"{original.name}_SceneRoot";

            for (int i = 0; i < count; i++)
                Push(Create());
        }


        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();

        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            //DontDestroyOnLoad 해제용
            if (parent == null)
            {
                // 부모 제거해 루트로 만듦
                if (poolable.transform.parent != null)
                {
                    poolable.transform.SetParent(null);
                }

                Scene activeScene = SceneManager.GetActiveScene();
                SceneManager.MoveGameObjectToScene(poolable.gameObject, activeScene);
                poolable.transform.SetParent(SceneRoot, true);
            }

            if (parent != null)
            {
                poolable.transform.parent = parent;
            }

            
            poolable.IsUsing = true;

            return poolable;
        }


    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;
    Transform _sceneRoot;
    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            _sceneRoot = new GameObject { name = "Pool_SceneRoot"}.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;
        pool.SceneRoot.parent = _sceneRoot;

        _pool.Add(original.name, pool);
    }


    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }
        _pool[name].Push(poolable);

    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);


        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Original;
    }


    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }

    // ResourceManager 만들면 이관
    public GameObject Instantiate(GameObject original, Transform parent = null)
    {
        TestPool.Instance.AddMon(); // TODO 다른 곳으로 보내야함

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent); //prefab 생성
        go.name = original.name;

        return go;
    }

    // ResourceManager 만들면 이관
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Poolable poolalbe = go.GetComponent<Poolable>();
        if (poolalbe != null)
        {
            Managers.Pool.Push(poolalbe);
            return;
        }
        
        Object.Destroy(go);
    }
}
