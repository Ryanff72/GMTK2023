using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeFromToBlack : MonoBehaviour
{
    public bool toBlack;

    public float fadeTime;

    public float color;

    public bool activated;

    private void Start()
    {
        if (toBlack)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            color = 0;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            color = 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (toBlack && activated == true)
        {
            color += Time.deltaTime * fadeTime;
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, color);
        }
        else if (activated)
        {
            color -= Time.deltaTime * fadeTime;
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, color);
        }
    }
}
