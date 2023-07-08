using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipieBookObject : MonoBehaviour
{

    public GameObject recc;
    public GameObject[] recipieCanvas;

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
            Debug.Log("why");
            totalRecepieCount++;
            string newRecepieName = recc.GetComponent<RecipeController>().book.KnownRecipes[totalRecepieCount].Output.Name;
            
        }
    }

    public void addNewRecipe(string newRec)
    {
        for( int i = 0; i < recipieCanvas.Length; i++)
        {
            if (newRec == recipieCanvas[i].name)
            {
                Debug.Log("ahhhh");
                Instantiate(recipieCanvas[i]);
            }
        }
    }


}
