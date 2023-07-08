using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;

public enum Modifier {
    None,
    Heated,
    Shaken
}

public class Item {
    public Modifier Mod;
    public string Name;

    public Item(string name, Modifier mod = Modifier.None) {
        Name = name;
        Mod = mod;
    }
}

public class Recipe {
    public List<Item> Ingredients;
    public Item Output;

    public Recipe(List<Item> ingredients, Item output) {
        Ingredients = ingredients;
        Output = output;
    }
}

public class RecipeBook {
    public List<Recipe> AllRecipes;
    public List<Recipe> KnownRecipes;

    public Recipe getRecipe(List<Item> Ingredients) {    // Gets corresponding recipe to list of items
        Debug.Log("Getting recipe with ingredients...");
        for(int i = 0; i < Ingredients.Count; i++)
        {
            Debug.Log(Ingredients[i].Name);
        }
        for (int i = 0; i < AllRecipes.Count; i++) {
            if (checkRecipe(AllRecipes[i], Ingredients)) {
                if(!isRecipeKnown(AllRecipes[i].Output, Ingredients)) {
                    KnownRecipes.Add(AllRecipes[i]);
                }
                Debug.Log("found recipe for: " + AllRecipes[i].Output.Name);
                return AllRecipes[i];
            }
        }
        Debug.Log("Could not get recipe");
        return null;
    }

    public bool isRecipeKnown(Item Output, List<Item> Ingredients) {
        for(int i = 0; i < KnownRecipes.Count; i++) {
            if(KnownRecipes[i].Output.Name == Output.Name && KnownRecipes[i].Ingredients == Ingredients) {
                return true;
            }
        }
        return false;
    }

    public bool checkRecipe(Recipe Rcp, List<Item> Ingrdns) {  // Checks for specific recipe if the items match it
        for(int i = 0; i < Rcp.Ingredients.Count; i++)
        {
            bool found = false;
            for(int j = 0; j < Ingrdns.Count; j++)
            {
                if (Ingrdns[j].Name == Rcp.Ingredients[i].Name && Ingrdns[j].Mod == Rcp.Ingredients[i].Mod)
                {
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                return false;
            }
        }
        return true;
    }

    public RecipeBook() {
        AllRecipes = new List<Recipe>();
        KnownRecipes = new List<Recipe>();

    }
}

public class RecipeController : MonoBehaviour
{
    public RecipeBook book;
    public List<string> IngredientNames;
    public List<GameObject> Ingredients;

    // Start is called before the first frame update
    void Start()
    {
        book = new RecipeBook();

        Item courageLeaf = new Item("courageLeaf");
        Item angryPepper = new Item("angryPepper");

        Item veggiePaste = new Item("veggiePaste");


        book.AllRecipes.Add(new Recipe(new List<Item>() {courageLeaf, angryPepper}, veggiePaste));


        List<Item> CrntIngredients = new List<Item>() { courageLeaf, angryPepper };
    }

    public GameObject getOutcome(List<Item> Ingrdnts)
    {
        Recipe recipe = book.getRecipe(Ingrdnts);
        if(recipe == null)
        {
            Debug.Log("Recipe not found");
            return null;
        }
        return getIngredient(recipe.Output.Name);
    }

    public GameObject getIngredient(string IngrName)
    {
        for(int i = 0; i < IngredientNames.Count; i++)
        {
            if (IngredientNames[i] == IngrName)
            {
                return Ingredients[i];
            }
        }
        return null;
    }

}
