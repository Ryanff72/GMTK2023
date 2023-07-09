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
    public GameObject statusIndicatorPrefab;
    private GameObject statusIndicator;



    // Start is called before the first frame update
    void Start()
    {
        statusIndicator = Instantiate(statusIndicatorPrefab,transform.position + new Vector3(-.5f,0.8f,0), Quaternion.identity);
        statusIndicator.transform.parent = transform;
        item = new Item(ItemName, Mod);
    }

    // Update is called once per frame
    void Update()
    {
        statusIndicator.GetComponent<SpriteRenderer>().sortingOrder = transform.GetChild(8).GetComponent<SpriteRenderer>().sortingOrder + 1;
        if (Mod == Modifier.Heated)
        {
            statusIndicator.GetComponent<SpriteRenderer>().sprite = heatedSprite;
        }
        else if(Mod == Modifier.Shaken)
        {
            statusIndicator.GetComponent<SpriteRenderer>().sprite = shakenSprite;
        }
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
        item.Mod = Modifier.Heated;
        Mod = Modifier.Heated;
        
    }

    public void Shake()
    {
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
