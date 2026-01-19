using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_UI : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(StartIntro), 0.5f);
    }

    void StartIntro()
    {
        Transform Enter = transform.GetChild(0);
        Transform Start = transform.GetChild(1);


        Enter.gameObject.SetActive(true);
        Start.gameObject.SetActive(true);
    }
}
