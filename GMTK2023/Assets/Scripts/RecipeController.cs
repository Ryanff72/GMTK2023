using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;
using FMOD.Studio;
using UnityEditor.PackageManager.UI;

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

    public bool isPoisonous()
    {
        return false;
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
        if(Ingredients.Count == 0)
        {
            return null;
        }
        Debug.Log("Getting recipe with ingredients...");
        for(int i = 0; i < Ingredients.Count; i++)
        {
            Debug.Log(Ingredients[i].Name);
        }
        for (int i = 0; i < AllRecipes.Count; i++) {
            Debug.Log(Ingredients[i]);
            if (checkRecipe(AllRecipes[i], Ingredients)) {
                if(!isRecipeKnown(AllRecipes[i].Output, Ingredients)) {
                    KnownRecipes.Add(AllRecipes[i]);
                }
                Debug.Log("found recipe for: " + AllRecipes[i].Output.Name);
                return AllRecipes[i];
            }
        }
        Debug.Log("Could not get recipe");
        Item veggiepaste = new Item("Veggie Paste", Modifier.None);
        Recipe veggieRecipe = new Recipe(Ingredients, veggiepaste);
        return veggieRecipe;
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
    void Awake()
    {
        book = new RecipeBook();

        //base items
        Item ravensTears = new Item("Raven's Tears");

        Item demonPepper = new Item("Demon Pepper");
        Item ShakenDemonPepper = new Item("Demon Pepper", Modifier.Shaken);
        Item HotDemonPepper = new Item("Demon Pepper", Modifier.Heated);

        Item pixieSugar = new Item("Pixie Sugar");
        Item ShakenPixieSugar = new Item("Pixie Sugar", Modifier.Shaken);
        Item HotPixieSugar = new Item("Pixie Sugar", Modifier.Heated);


        Item powderedIron = new Item("Powdered Iron");
        Item HotPowderedIron = new Item("Powdered Iron", Modifier.Heated);
        Item ShakenPowderedIron = new Item("Powdered Iron", Modifier.Shaken);

        Item veggiePaste = new Item("Veggie Paste");



        //DP + RT = [LJ] Lemon Juice (hate, vengeance, sour)
        Item lemonJuice = new Item("Lemon Juice");
        book.AllRecipes.Add(new Recipe(new List<Item>() { demonPepper, ravensTears }, lemonJuice));

        //PS + RT = [HC] Handsome Caramel (charisma)
        Item handsomeCaramel = new Item("Handsome Caramel");
        book.AllRecipes.Add(new Recipe(new List<Item>() {pixieSugar, ravensTears}, handsomeCaramel));

        //PS + #DP = [SH] [HC] Hot Coffee (restless, productive, depressant)
        Item hotCoffee = new Item("Hot Coffee");
        book.AllRecipes.Add(new Recipe(new List<Item>() { pixieSugar, HotDemonPepper }, hotCoffee));

        //PS + $DP = [SH] Crushed Beans (lunatic)
        Item crushedBeans = new Item("Crushed Beans");
        book.AllRecipes.Add(new Recipe(new List<Item>() { pixieSugar, ShakenDemonPepper }, crushedBeans));

        //$PS + PI = [VG] Vitamin Gummy (health)
        Item vitaminGummy = new Item("Vitamin Gummy");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenPixieSugar, powderedIron }, vitaminGummy));

        //#DP + PI = [GP] Gunpowder (potent energy, pressure, danger)
        Item gunpowder = new Item("Gunpowder");
        book.AllRecipes.Add(new Recipe(new List<Item>() { HotDemonPepper, powderedIron }, gunpowder));

        //RT + $PI = [RC] Rust Catalyst (decay, infection, bitter)
        Item rustCatalyst = new Item("Rust Catalyst");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ravensTears, ShakenPixieSugar }, rustCatalyst));

        //RT + #PI = [AG] Acidic Gunk (poison, melt)
        Item acidicGunk = new Item("Acidic Gunk");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ravensTears, HotPixieSugar }, acidicGunk));

        //#PI + RT = [PB] Paint Bomb (mischief, explosive)
        Item paintBomb = new Item("Paint Bomb");
        book.AllRecipes.Add(new Recipe(new List<Item>() { HotPowderedIron, ravensTears }, paintBomb));

        //$PS + RT = [HP] Happy Pill (antidepressant)
        Item happyPill = new Item("Happy Pill");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenPixieSugar, ravensTears }, happyPill));

        //LJ + PS = [EM] Elegant Martini (luxury)
        Item martini = new Item("Elegant Martini");
        book.AllRecipes.Add(new Recipe(new List<Item>() { lemonJuice, pixieSugar }, martini));

        //RT + DP + PS + PI = [RS] Rainbow Sludge (confusion, thrill, surprise)
        Item rainbow = new Item("Rainbow Sludge");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ravensTears, demonPepper, powderedIron, pixieSugar }, rainbow));

        //RT + DP + PS = [PrSp] Primordial Soup(wisdom, life, experience)
        Item soup = new Item("Primordial Soup");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ravensTears, demonPepper, pixieSugar }, soup));

        //DP + PS + PI = [PrSp] Primordial Soup (wisdom, life, experience)
        book.AllRecipes.Add(new Recipe(new List<Item>() { demonPepper, pixieSugar, powderedIron }, soup));
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
