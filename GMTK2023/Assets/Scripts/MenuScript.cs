using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    private void Start()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayMusic("title");
    }
    public void StartGame()
    {
        //morningsong
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
