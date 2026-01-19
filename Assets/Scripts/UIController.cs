using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject[4] go;



    void Start()
    {


        for(int i = 0; i < go.Length; i++)
        {
            go[i] = GameObject.Find(go[i].name);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.transform.position += Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.transform.position += Vector3.up;
        }
    }
}
