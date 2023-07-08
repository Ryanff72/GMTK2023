using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public Item item;
    public string ItemName;
    public Modifier Mod;

    // Start is called before the first frame update
    void Start()
    {
        item = new Item(ItemName, Mod);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
