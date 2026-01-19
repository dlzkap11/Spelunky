using UnityEngine;

public class Intro : MonoBehaviour
{
    
    void Start()
    {
        if(Managers.Intro.IntroPlayed == true)
            gameObject.SetActive(false);
    }
}
