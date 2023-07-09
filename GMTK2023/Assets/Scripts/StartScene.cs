using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Menu");
        }
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
        yield return new WaitForSeconds(delayBoard[lineIndex]);
        lineIndex++;
        text.text = "";
        charIndex = 0;
        boardIndex++;
        if(boardIndex >= boards.Count)
        {
            yield return new WaitForSeconds(4.0f);
        }
        Board.sprite = boards[boardIndex];
        StartCoroutine(nextBoard());
    }
}
