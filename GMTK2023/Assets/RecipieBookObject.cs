using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipieBookObject : MonoBehaviour
{

    public GameObject recc;
    public GameObject[] recipieCanvas;
    public GameObject turnLeftButton;
    public GameObject turnRightButton;

    int pageRecepieCount = 0; //the amount of recipies on the current page
    int pagePairCount = 0; //the number of pairs we have gone through
    int pageCount = 0;//0 means leftpage 1 means rightpage

    int totalRecepieCount = 0;

    int currentpage = 0;



    private void Update()
    {
        if (recc.GetComponent<RecipeController>().book.KnownRecipes.Count > totalRecepieCount) 
        {
            totalRecepieCount++;
            string newRecepieName = recc.GetComponent<RecipeController>().book.KnownRecipes[totalRecepieCount-1].Output.Name;
            addNewRecipe(newRecepieName);
        }

        if (currentpage == 0)
        {
            turnLeftButton.gameObject.SetActive(false);
        }
        else
        {
            turnLeftButton.gameObject.SetActive(true);
        }
        if(currentpage == 2) 
        {
            turnRightButton.gameObject.SetActive(false);
        }
        else
        {
            turnRightButton.gameObject.SetActive(true);
        }

    }

    public void addNewRecipe(string newRec)
    {
        for( int i = 0; i < recipieCanvas.Length; i++)
        {
            if (newRec == recipieCanvas[i].name)
            {
                GameObject newElem = Instantiate(recipieCanvas[i]);

                
                if (pageRecepieCount <= 4)
                {
                    pageRecepieCount++;
                }
                if (pageCount == 0 && pageRecepieCount > 4)
                {
                    pageCount = 1;
                    pageRecepieCount = 1;
                }
                if (pageCount == 1 && pageRecepieCount > 4)
                {
                    newPage();
                }
                else
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    gameObject.transform.GetChild(pagePairCount).gameObject.SetActive(true);
                    currentpage = pagePairCount;
                }
                Debug.Log("page count: " + pageCount);
                Debug.Log("page pair count: " + pagePairCount);
                Debug.Log("pageRecepieCount: " + pageRecepieCount);
                newElem.transform.SetParent(transform.GetChild(pagePairCount).transform.GetChild(pageCount));
                newElem.GetComponent<RectTransform>().position = new Vector2(7.35f + (2.95f * pageCount), 7.1f - (1.1f * pageRecepieCount));
            }
        }
    }

    void newPage()
    {
        pagePairCount++;
        pageCount = 0;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(pagePairCount).gameObject.SetActive(true);
        currentpage = pagePairCount;
        turnLeftButton.gameObject.SetActive(true);
        pageRecepieCount = 1;

        
    }

    public void turnLeft()
    {
        gameObject.transform.GetChild(currentpage).gameObject.SetActive(false);
        gameObject.transform.GetChild(currentpage-1).gameObject.SetActive(true);
        TurnSFX();
        currentpage--;
    }

    public void turnRight()
    {
        gameObject.transform.GetChild(currentpage).gameObject.SetActive(false);
        gameObject.transform.GetChild(currentpage + 1).gameObject.SetActive(true);
		TurnSFX();
		currentpage++;
    }


    void TurnSFX()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.pageTurn, transform.position);
    }

}
