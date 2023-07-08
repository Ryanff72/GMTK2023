using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startDialogue()
    {
        //Do Dialogue(day, name, hp, lastPotion)
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
            disappear();
            return;
        }

        Hp--;
        if (item.isPoisonous())
        {
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
