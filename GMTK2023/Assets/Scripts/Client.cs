<<<<<<< Updated upstream
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using TMPro;
=======
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class DialogueParser
{
    public static string Dialogue;

    public static void initDialogue()
    {
        var fileStream = new FileStream(Application.dataPath + "/Dialogue.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            Dialogue = streamReader.ReadToEnd();
        }
    }
    public static List<string> LoadDialogue(string Name, int Hp, int Day, string LastPotion)
    {
        int CharacterDlgIndex = Dialogue.IndexOf("__" + Name) + Name.Length + 6;
        string CharacterStr = "";

        for (int i = CharacterDlgIndex; i < Dialogue.Length; i++)
        {
            if (i + 1 < Dialogue.Length && Dialogue[i] == '_' && Dialogue[i + 1] == '_')
            {
                break;
            }
            CharacterStr += Dialogue[i];
        }
        CharacterStr = CharacterStr.Replace("    ", "");
        CharacterStr = CharacterStr.Replace("\t", "");

        List<string> CharacterStrSplit = CharacterStr.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        List<string> loadedDialogue = new List<string>();
        bool skipping = true;
        bool foundDay = false;
        bool skippingDay = false;

        for (int i = 0; i < CharacterStrSplit.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(CharacterStrSplit[i]) || CharacterStrSplit[i][0] == '\n')
            {
                continue;
            }
            if (CharacterStrSplit[i][0] == '_')
            {

                switch (CharacterStrSplit[i].Substring(0, 4))
                {
                    case "_day":
                        if (foundDay)
                        {
                            return loadedDialogue;
                        }
                        if (CharacterStrSplit[i][5] - '0' != Day)
                        {
                            skipping = true;
                            skippingDay = true;
                            continue;
                        }
                        else
                        {
                            skipping = false;
                            foundDay = true;
                            skippingDay = false;

                            continue;
                        }
                    case "_hp ":
                        if (skippingDay)
                        {
                            continue;
                        }
                        if (CharacterStrSplit[i][4] - '0' != Hp)
                        {
                            skipping = true;
                            continue;
                        }
                        else
                        {
                            skipping = false;
                            continue;
                        }
                    case "_awf":
                        if (skippingDay)
                        {
                            continue;
                        }
                        if (LastPotion != "awf")
                        {
                            skipping = true;
                            continue;
                        }
                        else
                        {
                            skipping = false;
                            continue;
                        }
                    case "_mid":
                        if (skippingDay)
                        {
                            continue;
                        }
                        if (LastPotion != "mid")
                        {
                            skipping = true;
                            continue;
                        }
                        else
                        {
                            skipping = false;
                            continue;
                        }
                    case "_god":
                        if (skippingDay)
                        {
                            continue;
                        }
                        if (LastPotion != "god")
                        {
                            skipping = true;
                            continue;
                        }
                        else
                        {
                            skipping = false;
                            continue;
                        }
                    case "_itm":
                        if (!skipping)
                        {
                            return loadedDialogue;
                        }
                        break;
                    default:
                        if (skippingDay)
                        {
                            continue;
                        }
                        skipping = false;
                        continue;
                }
            }
            if (!skipping)
            {
                loadedDialogue.Add(CharacterStrSplit[i]);
            }
        }
        return loadedDialogue;
    }
}
>>>>>>> Stashed changes

public class Client : MonoBehaviour
{
    bool appearing = false;
    bool disappearing = false;
<<<<<<< Updated upstream
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


=======
    float appearLerp = 0.0f;
    public float appearSpeed;
    SpriteRenderer sprite;
    public int Hp = 3;
    string Name;
    public string wantedItemName;
    public Modifier wantedItemMod;
    public GameObject holdingItem;
    public string lastPotion;
    List<string> dialogue;
    [SerializeField] public Customer dialogueObject;
>>>>>>> Stashed changes

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
<<<<<<< Updated upstream
        initChar();
    }

    void initChar()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
    }

    void initDialogue()
    {
        dialogueObject.dialogueLines = DialogueParser.LoadDialogue(dialogueObject.name, Hp, GameObject.Find("GameController").GetComponent<GameController>().day, lastPotion).ToArray();
=======
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        DialogueParser.initDialogue();
        List<string> dialogue = DialogueParser.LoadDialogue(Name, Hp, GameObject.Find("GameController").GetComponent<GameController>().day, lastPotion);
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startDialogue()
    {
<<<<<<< Updated upstream
        initDialogue();
        //Do Dialogue(day, name, hp, lastPotion)
        FindObjectOfType<MessagesManager>().SpawnTextMessageAndStartDialogue(dialogueObject);
=======
        FindObjectOfType<MessagesManager>().SpawnTextMessageAndStartDialogue(dialogueObject);
        endDialogue();
>>>>>>> Stashed changes
    }

    public void endDialogue()
    {
<<<<<<< Updated upstream
        FindObjectOfType<MessagesManager>().CloseTextBox();
=======

>>>>>>> Stashed changes
    }

    public void receiveItem(GameObject holdItem)
    {
        Item item = holdItem.GetComponent<Ingredient>().item;
        holdingItem = holdItem;
        if(item.Name == wantedItemName && item.Mod == wantedItemMod)
        {
<<<<<<< Updated upstream
            lastPotion = "god";
            disappear();
            GameObject.Find(dialogueObject.name + "Review").GetComponent<TextMeshProUGUI>().text = goodReview;
            return;
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
=======
            disappear();
            lastPotion = "god";
            return;
        }

        Hp--;
        lastPotion = "mid";
        if (item.isPoisonous())
        {
            Hp--;
            lastPotion = "awf";
>>>>>>> Stashed changes
        }

        disappear();
    }


    public void FixedUpdate()
    {
        if (appearing)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, appearLerp);
            appearLerp += appearSpeed;
<<<<<<< Updated upstream
            if (appearLerp >= 1.0)
=======
            if(appearLerp >= 255)
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        endDialogue();
=======
>>>>>>> Stashed changes
    }
}
