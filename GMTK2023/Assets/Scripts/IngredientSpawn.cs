using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class IngredientSpawn : MonoBehaviour
{

    bool hovering;
    public GameObject spawnedIngredient;

    Vector2 worldMousePos;

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
            newIngredient.GetComponent<IngredientGrabbing>().ingStatus = IngredientGrabbing.ingredientStatus.Dragging;
        }
    }


    private void OnMouseEnter()
    {
        hovering = true;
    }
    private void OnMouseExit()
    {
        hovering = false;
    }
}
