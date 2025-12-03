using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public static IntroManager instance;
    private bool introPlayed = false;

    public bool IntroPlayed
    {
        get { return introPlayed; }
        set { introPlayed = value; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}