using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipieBookObject : MonoBehaviour
{

    public GameObject recc;

    int pageRecepieCount = 0;
    int pagePairCount = 0;
    int pageCount = 0;
    int totalRecepieCount = 0;


    private void Start()
    {
        for(int i = 0; i < recc.GetComponent<RecipeController>().book.AllRecipes.Count;  i++)
        {
            Debug.Log(recc.GetComponent<RecipeController>().book.AllRecipes[i]);
        }
    }
    private void Update()
    {
        if (recc.GetComponent<RecipeController>().book.KnownRecipes.Count > totalRecepieCount) 
        {
            totalRecepieCount++;
            Debug.Log("new recepie: "+recc.GetComponent<RecipeController>().book.KnownRecipes[0].Output.Name);
        }
    }

    public void addNewRecipe(string RecipeName, string Buffs, Sprite Mod1, Sprite Mod2, Sprite Item1, Sprite Item2)
    {
        //gameObject.getch
    }


}
