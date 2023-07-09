using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using TMPro;

public class Client : MonoBehaviour
{
    bool appearing = false;
    bool disappearing = false;
    public float appearLerp = 0.0f;
    public float appearSpeed;
    SpriteRenderer sprite;
    public int Hp = 3;
    public string wantedItemName;
    public Modifier wantedItemMod;
    public GameObject holdingItem;
    [SerializeField] Customer dialogueObject;
    public string lastPotion = "god";
    public string goodReview = "goodReview";
    public string midReview = "midReview";
    public string awfReview = "awfulReview";
    public List<string> wantedItemNamesDayOne;
    public List<Modifier> wantedItemModsDayOne;
    public List<string> wantedItemNamesDayTwo;
    public List<Modifier> wantedItemModsDayTwo;
    public List<string> wantedItemNamesDayThree;
    public List<Modifier> wantedItemModsDayThree;




    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        initChar();
    }

    void initChar()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
    }

    void initDialogue()
    {
        dialogueObject.dialogueLines = DialogueParser.LoadDialogue(dialogueObject.name, Hp, GameObject.Find("GameController").GetComponent<GameController>().day, lastPotion).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startDialogue()
    {
        initDialogue();
        //Do Dialogue(day, name, hp, lastPotion)
        FindObjectOfType<MessagesManager>().SpawnTextMessageAndStartDialogue(dialogueObject);
    }

    public void endDialogue()
    {
        FindObjectOfType<MessagesManager>().CloseTextBox();
    }

    public void receiveItem(GameObject holdItem)
    {
        Item item = holdItem.GetComponent<Ingredient>().item;
        holdingItem = holdItem;
        int crntDay = GameObject.Find("GameController").GetComponent<GameController>().day;
        List<string> wantedItemNames = wantedItemNamesDayOne;
        List<Modifier> wantedItemMods = wantedItemModsDayOne;

        if (crntDay == 2)
        {
            wantedItemNames = wantedItemNamesDayTwo;
            wantedItemMods = wantedItemModsDayTwo;
        } 
        else
        {
            wantedItemNames = wantedItemNamesDayThree;
            wantedItemMods = wantedItemModsDayThree;
        }
        for (int i = 0; i < wantedItemNames.Count; i++)
        {
            if (wantedItemNames[i] == item.Name && wantedItemMods[i] == item.Mod)
            {
                lastPotion = "god";
                disappear();
                GameObject.Find(dialogueObject.name + "Review").GetComponent<TextMeshProUGUI>().text = goodReview;
                return;
            }
        }


        if (item.isPoisonous())
        {
            lastPotion = "awf";
            Hp-=2;
            GameObject.Find(dialogueObject.name + "Review").GetComponent<TextMeshProUGUI>().text = awfReview;
        } else
        {
            lastPotion = "mid";
            Hp--;
            GameObject.Find(dialogueObject.name + "Review").GetComponent<TextMeshProUGUI>().text = midReview;
        }

        disappear();
    }


    public void FixedUpdate()
    {
        if (appearing)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, appearLerp);
            appearLerp += appearSpeed;
            if (appearLerp >= 1.0)
            {
                appearing = false;
                startDialogue();
            }
        }
        else if(disappearing)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, appearLerp);
            appearLerp -= appearSpeed;
            if (appearLerp <= 0)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
                disappearing = false;
                if(holdingItem)
                {
                    Destroy(holdingItem);
                }
            }
        }
    }

    public void appear()
    {
        appearing = true;
        appearLerp = 0.0f;
    }

    public void disappear()
    {
        disappearing = true;
        appearing = false;
        appearLerp = 1.0f;
        endDialogue();
    }
}
