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
    public GameObject NewItemSpawner; // cauldron


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
        Recipe recipe = RecipeController.book.getRecipe(Items);
        if (recipe == null)
        {
            Debug.Log("Recipe not found");
            return;
        }
        GameObject output = RecipeController.getIngredient(recipe.Output.Name);
        if (output)
        {
            
            NewItemSpawner.GetComponent<SpawnNew>().SpawnItem(output);
        }

        List<GameObject> usedIngredients = new List<GameObject>();
        for(int i = 0; i < recipe.Ingredients.Count; i++)
        {
            for(int j = 0; j < Items.Count; j++)
            {
                if (recipe.Ingredients[i].Name == Items[j].Name && recipe.Ingredients[i].Mod == Items[j].Mod)
                {
                    usedIngredients.Add(IngredientsInPot[j]);
                }
            }
        }
        for(int i = 0; i < usedIngredients.Count; i++)
        {
            Destroy(usedIngredients[i]);
        }
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
