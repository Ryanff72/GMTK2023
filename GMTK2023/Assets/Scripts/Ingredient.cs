using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public Item item;
    public string ItemName;
    public Modifier Mod;
    public bool canHeat;
    public bool canShake;

    public Transform leftGc;
    public Transform rightGc;
    public Sprite heatedSprite;
    public Sprite shakenSprite;

    float lerpIndex = 0.0f;
    bool isServing = false;
    Vector3 startPos;
    public Vector3 characterHandPos;
    public float lerpSpeed = 0.01f;


    // Start is called before the first frame update
    void Start()
    {
        item = new Item(ItemName, Mod);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if(isServing)
        {
            transform.position = Vector3.Lerp(startPos, characterHandPos, lerpIndex);
            lerpIndex += lerpSpeed;
            if(lerpIndex > 1.0)
            {
                isServing = false;
            }
        }
    }

    public void Heat()
    {
        if (heatedSprite)
        {
            GetComponent<SpriteRenderer>().sprite = heatedSprite;
        }
        item.Mod = Modifier.Heated;
        Mod = Modifier.Shaken;
    }

    public void Shake()
    {
        if (shakenSprite)
        {
            GetComponent<SpriteRenderer>().sprite = shakenSprite;
        }
        item.Mod = Modifier.Shaken;
        Mod = Modifier.Shaken;
    }

    public void serve()
    {
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        gameObject.GetComponent<IngredientGrabbing>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        startPos = transform.position;
        isServing = true;
    }
}
