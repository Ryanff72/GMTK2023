using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

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
        for(int i = 0; i < AllRecipes.Count; i++) {
            if(checkRecipe(AllRecipes[i], Ingredients)) {
                if(!isRecipeKnown(AllRecipes[i].Output, Ingredients)) {
                    KnownRecipes.Add(AllRecipes[i]);
                }
                return AllRecipes[i];
            }
        }
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
        if(Rcp.Ingredients.Count != Ingrdns.Count) {
            return false;
        }

        return (Rcp.Ingredients.All(Ingrdns.Contains) && Ingrdns.All(Rcp.Ingredients.Contains));
    }

    public RecipeBook() {
        AllRecipes = new List<Recipe>();
        KnownRecipes = new List<Recipe>();

    }
}

public class RecipeController : MonoBehaviour
{
    public RecipeBook book;

    // Start is called before the first frame update
    void Start()
    {
        RecipeBook book = new RecipeBook();

        Item egg = new Item("egg");
        Item coldMilk = new Item("milk");
        Item hotMilk = new Item("milk", Modifier.Heated);

        Item coldCake = new Item("cold cake");
        Item hotCake = new Item("hot cake");


        book.AllRecipes.Add(new Recipe(new List<Item>() {egg, coldMilk}, coldCake));
        book.AllRecipes.Add(new Recipe(new List<Item>() {egg, hotMilk}, hotCake));


        List<Item> CrntIngredients = new List<Item>() {egg, coldMilk};
        Debug.Log(book.getRecipe(CrntIngredients).Output.Name);
        CrntIngredients = new List<Item>() {egg, hotMilk};
        Debug.Log(book.getRecipe(CrntIngredients).Output.Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
