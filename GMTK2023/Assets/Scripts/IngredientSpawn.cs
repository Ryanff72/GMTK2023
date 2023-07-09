using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class IngredientSpawn : MonoBehaviour
{

    bool hovering;
    public GameObject spawnedIngredient;

    private Sprite idleSprite;
    public Sprite openSprite;

    Vector2 worldMousePos;

    private void Start()
    {
        idleSprite = GetComponent<SpriteRenderer>().sprite;    
    }

    // Update is called once per frame
    void Update()
    {

        //spawn a new ingredient
        if (Input.GetButtonDown("Fire1") && hovering)
        {
            //mousepos to worldpos
            Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            worldMousePos = Camera.main.ScreenToWorldPoint(screenPoint);
            GameObject newIngredient = Instantiate(spawnedIngredient, worldMousePos, Quaternion.identity);
            newIngredient.GetComponent<BoxCollider2D>().enabled = false;
            newIngredient.GetComponent<IngredientGrabbing>().ingStatus = IngredientGrabbing.ingredientStatus.Dragging;
        }
    }

    private void OnMouseOver()
    {
        hovering = true;
        GetComponent<SpriteRenderer>().sprite = openSprite;
    }
    private void OnMouseExit()
    {
        hovering = false;
        GetComponent<SpriteRenderer>().sprite = idleSprite;
    }
}
