using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public void StartGame()
    {
        AudioManager.instance.SetMusic(MusicEnum.MORNING);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
