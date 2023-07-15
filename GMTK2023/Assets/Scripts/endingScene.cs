using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endingScene : MonoBehaviour
{
    public List<string> lines;
    int charIndex = 0;
    int boardIndex = 0;
    int lineIndex = 0;
    public List<Sprite> boards;
    public float charTime;
    SpriteRenderer Board;
    TextMeshProUGUI text;
    public List<float> delayBoard;

    // Start is called before the first frame update
    void Start()
    {
        Board = GameObject.Find("Board").GetComponent<SpriteRenderer>();
        text = GameObject.Find("bottomText").GetComponent<TextMeshProUGUI>();
        text.text = "";
        Board.sprite = boards[boardIndex];
        StartCoroutine(nextBoard());
        StartCoroutine(nextChar());
    }


    IEnumerator nextChar()
    {
        yield return new WaitForSeconds(charTime);
        if (charIndex < lines[lineIndex].Length)
        {
            text.text += lines[lineIndex][charIndex];
        }
        charIndex++;
        StartCoroutine(nextChar());
    }

    IEnumerator nextBoard()
    {
        yield return new WaitForSeconds(delayBoard[boardIndex]);
        boardIndex++;
        lineIndex++;
        if (boardIndex >= boards.Count)
        {
            //titlemusic
            SceneManager.LoadScene("Menu");
        }
        else
        {
            text.text = "";
            charIndex = 0;
            Board.sprite = boards[boardIndex];
            StartCoroutine(nextBoard());
        }

    }
}
