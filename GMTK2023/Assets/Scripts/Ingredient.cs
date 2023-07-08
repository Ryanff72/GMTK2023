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

    // Start is called before the first frame update
    void Start()
    {
        item = new Item(ItemName, Mod);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D GroundCheck = Physics2D.Linecast(leftGc.position, rightGc.position, 1 << 6);
        if(GroundCheck && GroundCheck.collider.tag == "Heater" && canHeat)
        {
            Mod = Modifier.Heated;
            GetComponent<SpriteRenderer>().sprite = heatedSprite;
        }
    }
}
