using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipieBookObject : MonoBehaviour
{

    public GameObject recc;
    public GameObject[] recipieCanvas;

    int pageRecepieCount = 0; //the amount of recipies on the current page
    int pagePairCount = 0; //the number of pairs we have gone through
    int pageCount = 0;//0 means leftpage 1 means rightpage

    int totalRecepieCount = 0;


    private void Start()
    {

    }
    private void Update()
    {
        if (recc.GetComponent<RecipeController>().book.KnownRecipes.Count > totalRecepieCount) 
        {
            totalRecepieCount++;
            string newRecepieName = recc.GetComponent<RecipeController>().book.KnownRecipes[totalRecepieCount-1].Output.Name;
            addNewRecipe(newRecepieName);
        }
    }

    public void addNewRecipe(string newRec)
    {
        for( int i = 0; i < recipieCanvas.Length; i++)
        {
            if (newRec == recipieCanvas[i].name)
            {
                Debug.Log("ahhhh");
                GameObject newElem = Instantiate(recipieCanvas[i]);
                newElem.transform.SetParent(transform.GetChild(pagePairCount).transform.GetChild(pageCount));
                newElem.GetComponent<RectTransform>().position = new Vector2(7.5f +(2.5f*pageCount), 6.1f-(0.9f * pageRecepieCount));
                if (pageRecepieCount < 4)
                {
                    pageRecepieCount++;
                }
                else if (pageCount == 0)
                {
                    pageCount++;
                    pageRecepieCount = 0;
                }
                else
                {
                    pageCount = 0;
                    
                }
            }
        }
    }


}
