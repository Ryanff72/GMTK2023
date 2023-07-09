using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public List<string> lines;
    int charIndex = 0;
    int boardIndex = 0;
    int lineIndex = 0;
    public List<Sprite> boards;
    public float boardTime;
    public float charTime;
    SpriteRenderer Board;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        Board = GameObject.Find("Board").GetComponent<SpriteRenderer>();
        text = GameObject.Find("bottomText").GetComponent<TextMeshProUGUI>();
        text.text = "";
        Board.sprite = boards[boardIndex];
        boardIndex++;
        StartCoroutine(nextBoard());
        StartCoroutine(nextChar());
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if(boardIndex == 1)
        {
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(boardTime);
        lineIndex++;
        text.text = "";
        charIndex = 0;
        boardIndex++;
        if(boardIndex >= boards.Count)
        {
            yield return new WaitForSeconds(4.0f);
            SceneManager.LoadScene("Menu");
        }
        Board.sprite = boards[boardIndex];
        StartCoroutine(nextBoard());
    }
}
