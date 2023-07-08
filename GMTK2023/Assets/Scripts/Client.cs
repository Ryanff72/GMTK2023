using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Text;
using Unity.VisualScripting;


public class Client : MonoBehaviour
{
    bool appearing = false;
    bool disappearing = false;
    float appearLerp = 0.0f;
    public float appearSpeed;
    SpriteRenderer sprite;
    public int Hp = 3;
    string Name;
    public string wantedItemName;
    public Modifier wantedItemMod;
    public GameObject holdingItem;
    [SerializeField] Customer dialogueObject;
    string lastPotion;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        initChar();
    }

    void initChar()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        Debug.Log("day: " + GameObject.Find("GameController").GetComponent<GameController>().day);
        dialogueObject.dialogueLines = DialogueParser.LoadDialogue(dialogueObject.name, Hp, GameObject.Find("GameController").GetComponent<GameController>().day, lastPotion).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startDialogue()
    {
        //Do Dialogue(day, name, hp, lastPotion)
        FindObjectOfType<MessagesManager>().SpawnTextMessageAndStartDialogue(dialogueObject);
        endDialogue();
    }

    public void endDialogue()
    {

    }

    public void receiveItem(GameObject holdItem)
    {
        Item item = holdItem.GetComponent<Ingredient>().item;
        holdingItem = holdItem;
        if(item.Name == wantedItemName && item.Mod == wantedItemMod)
        {
            lastPotion = "god";
            disappear();
            return;
        }
        lastPotion = "mid";
        Hp--;
        if (item.isPoisonous())
        {
            lastPotion = "awf";
            Hp--;
        }

        disappear();
    }


    public void FixedUpdate()
    {
        if (appearing)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, appearLerp);
            appearLerp += appearSpeed;
            if(appearLerp >= 255)
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
    }
}
