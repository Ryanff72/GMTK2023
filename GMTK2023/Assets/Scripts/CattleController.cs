using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CattleController : MonoBehaviour
{
    public RecipeController RecipeController;
    public GameObject Spoon;
    public List<GameObject> IngredientsInPot;
    public float SpoonXTraveled = 0.0f;
    float lastSpoonX;
    bool SpoonInPot = false;
    public float SpoonStirDistance = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        lastSpoonX = Spoon.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(SpoonInPot)
        {
            SpoonXTraveled += Mathf.Abs(Spoon.transform.position.x - lastSpoonX);
            lastSpoonX = Spoon.transform.position.x;
            if(SpoonXTraveled >= SpoonStirDistance)
            {
                Stirred();
                SpoonXTraveled = 0.0f;
            }
        }
    }

    void Stirred()
    {
        List<Item> Items = new List<Item>();
        for(int i = 0; i < IngredientsInPot.Count; i++) {
            Items.Add(IngredientsInPot[i].GetComponent<Ingredient>().item);
        }

        Instantiate(RecipeController.getOutcome(Items), new Vector2(0, 0), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Ingredient>() != null)
        {
            SpoonXTraveled = 0.0f;
            IngredientsInPot.Add(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Spoon")
        {
            SpoonInPot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(IngredientsInPot.Contains(collision.gameObject))
        {
            SpoonXTraveled = 0.0f;
            IngredientsInPot.Remove(collision.gameObject);
        } 
        else if(collision.gameObject.tag == "Spoon")
        {
            SpoonInPot = false;
        }
    }
}
