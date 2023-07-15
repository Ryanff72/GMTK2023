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

    public bool isPoisonous()
    {
        if(Name == "Acidic Rust" || Name == "Veggie Paste" || Name == "Rust Catalyst" || Name == "Acidic Gunk")
        {
            return true;
        }
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
        int biggestRecipeSize = -1;
        int biggestRecipeIndex = -1;

        for (int i = 0; i < AllRecipes.Count; i++) {
            if (AllRecipes[i].Ingredients.Count > biggestRecipeSize && checkRecipe(AllRecipes[i], Ingredients)) {
                biggestRecipeIndex = i;
                biggestRecipeSize = AllRecipes[i].Ingredients.Count;
            }
        }
        if(biggestRecipeSize == -1)
        {
            Debug.Log("Could not get recipe");
            Item veggiepaste = new Item("Veggie Paste", Modifier.None);
            Recipe veggieRecipe = new Recipe(Ingredients, veggiepaste);
            return veggieRecipe;
        }

        if (!isRecipeKnown(AllRecipes[biggestRecipeIndex].Output, Ingredients))
        {
            KnownRecipes.Add(AllRecipes[biggestRecipeIndex]);
        }
        Debug.Log("found recipe for: " + AllRecipes[biggestRecipeIndex].Output.Name);
        return AllRecipes[biggestRecipeIndex];
    }

    public bool isRecipeKnown(Item Output, List<Item> Ingredients) {
        for(int i = 0; i < KnownRecipes.Count; i++) {
            if(KnownRecipes[i].Output.Name == Output.Name && KnownRecipes[i].Output.Mod == Output.Mod) {
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
        Item HotRavensTears = new Item("Raven's Tears", Modifier.Heated);
        Item ShakenRavensTears = new Item("Raven's Tears", Modifier.Shaken);



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



        /*DP + RT = [LJ] Lemon Juice (hate, vengeance, sour)
        DP + #RT = [LJ] Lemon Juice (hate, vengeance, sour)*/
        Item lemonJuice = new Item("Lemon Juice");
        book.AllRecipes.Add(new Recipe(new List<Item>() { demonPepper, ravensTears }, lemonJuice));
        book.AllRecipes.Add(new Recipe(new List<Item>() { demonPepper, HotRavensTears }, lemonJuice));

        Item HotLemonJuice = new Item("Lemon Juice", Modifier.Heated);
        Item ShakenLemonJuice = new Item("Lemon Juice", Modifier.Shaken);


        /*PS + RT = [HC] Handsome Caramel (charisma)
        PS + $RT = [HC] Handsome Caramel (charisma)*/
        Item handsomeCaramel = new Item("Handsome Caramel");
        book.AllRecipes.Add(new Recipe(new List<Item>() {pixieSugar, ravensTears}, handsomeCaramel));
        book.AllRecipes.Add(new Recipe(new List<Item>() { pixieSugar, ShakenRavensTears }, handsomeCaramel));

        /*PS + #DP = [SH] [HC] Hot Coffee (restless, productive, depressant)
        SH + RT = [SH] [HC] Hot Coffee (restless, productive, depressant)
        SH + #RT = [SH] [HC] Hot Coffee (restless, productive, depressant)*/
        Item hotCoffee = new Item("Hot Coffee");
        book.AllRecipes.Add(new Recipe(new List<Item>() { pixieSugar, HotDemonPepper }, hotCoffee));
        book.AllRecipes.Add(new Recipe(new List<Item>() { pixieSugar, ravensTears }, hotCoffee));
        book.AllRecipes.Add(new Recipe(new List<Item>() { pixieSugar, HotRavensTears }, hotCoffee));


        /*PS + $DP = [SH] Crushed Beans (lunatic)
        #PS + $DP = [SH] Crushed Beans (lunatic)
        $PS + $DP = [SH] Crushed Beans (lunatic)*/
        Item crushedBeans = new Item("Crushed Beans");
        book.AllRecipes.Add(new Recipe(new List<Item>() { pixieSugar, ShakenDemonPepper }, crushedBeans));
        book.AllRecipes.Add(new Recipe(new List<Item>() { HotPixieSugar, ShakenDemonPepper }, crushedBeans));
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenPixieSugar, ShakenDemonPepper }, crushedBeans));


        /*$PS + PI = [VG] Vitamin Gummy (health)
        $PS + $PI = [VG] Vitamin Gummy (health)
        PS + PI = [VG] Vitamin Gummy (health)*/
        Item vitaminGummy = new Item("Vitamin Gummy");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenPixieSugar, powderedIron }, vitaminGummy));
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenPixieSugar, ShakenPowderedIron }, vitaminGummy));
        book.AllRecipes.Add(new Recipe(new List<Item>() { pixieSugar, powderedIron }, vitaminGummy));


        /*$DP + PI = [GP] Gunpowder (potent energy, pressure, danger)
        #DP + PI = [GP] Gunpowder (potent energy, pressure, danger)
        $DP + $PI = [GP] Gunpowder (potent energy, pressure, danger)
        #DP + #PI = [GP] Gunpowder (potent energy, pressure, danger)
        DP + PI = [GP] Gunpowder (potent energy, pressure, danger)
        DP + $PI = [GP] Gunpowder (potent energy, pressure, danger)*/
        Item gunpowder = new Item("Gunpowder");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenDemonPepper, powderedIron }, gunpowder));
        book.AllRecipes.Add(new Recipe(new List<Item>() { HotDemonPepper, powderedIron }, gunpowder));
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenDemonPepper, ShakenPowderedIron }, gunpowder));
        book.AllRecipes.Add(new Recipe(new List<Item>() { HotDemonPepper, HotPowderedIron }, gunpowder));
        book.AllRecipes.Add(new Recipe(new List<Item>() { demonPepper, ShakenPowderedIron }, gunpowder));



        /*RT + $PI = [RC] Rust Catalyst (decay, infection, bitter)
        $RT + $PI = [RC] Rust Catalyst (decay, infection, bitter)*/
        Item rustCatalyst = new Item("Rust Catalyst");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ravensTears, ShakenPowderedIron }, rustCatalyst));
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenRavensTears, ShakenPowderedIron }, rustCatalyst));


        /*RT + #PI = [AG] Acidic Gunk (poison, melt)
        #RT + #PI = [AG] Acidic Gunk (poison, melt)*/
        Item acidicGunk = new Item("Acidic Gunk");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ravensTears, HotPowderedIron }, acidicGunk));
        book.AllRecipes.Add(new Recipe(new List<Item>() { HotRavensTears, HotPowderedIron }, acidicGunk));


        /*
        PI + #RT = [PB] Paint Bomb (mischief, explosive)
        #PI + #RT = [PB] Paint Bomb (mischief, explosive)
        PI + $RT = [PB] Paint Bomb (mischief, explosive)
        $PI + $RT = [PB] Paint Bomb (mischief, explosive)
        PI + RT = [PB] Paint Bomb (mischief, explosive)*/
        Item paintBomb = new Item("Paint Bomb");
        book.AllRecipes.Add(new Recipe(new List<Item>() { powderedIron, HotRavensTears }, paintBomb));
        book.AllRecipes.Add(new Recipe(new List<Item>() { HotPowderedIron, HotRavensTears }, paintBomb));
        book.AllRecipes.Add(new Recipe(new List<Item>() { powderedIron, ShakenRavensTears }, paintBomb));
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenPowderedIron, ShakenRavensTears }, paintBomb));
        book.AllRecipes.Add(new Recipe(new List<Item>() { powderedIron, ravensTears }, paintBomb));



        /*$PS + RT = [HP] Happy Pill (antidepressant)
        $PS + $RT = [HP] Happy Pill (antidepressant)
        PS + #RT = [HP] Happy Pill (antidepressant)
        #PS + #RT = [HP] Happy Pill (antidepressant)*/
        Item happyPill = new Item("Happy Pill");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenPixieSugar, ravensTears }, happyPill));
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenPixieSugar, ShakenRavensTears }, happyPill));
        book.AllRecipes.Add(new Recipe(new List<Item>() { pixieSugar, HotRavensTears }, happyPill));
        book.AllRecipes.Add(new Recipe(new List<Item>() { HotPixieSugar, ShakenRavensTears }, happyPill));



        /*LJ + PS = [EM] Elegant Martini (luxury)
        LJ + DP = [EM] Elegant Martini (luxury)
        LJ + RT = [EM] Elegant Martini (luxury)
        #LJ + PS = [EM] Elegant Martini (luxury)
        $LJ + PS = [EM] Elegant Martini (luxury)
        #LJ + DP = [EM] Elegant Martini (luxury)
        $LJ + DP = [EM] Elegant Martini (luxury)
        LJ + #RT = [EM] Elegant Martini (luxury)*/
        Item martini = new Item("Elegant Martini");
        book.AllRecipes.Add(new Recipe(new List<Item>() { lemonJuice, pixieSugar }, martini));
        book.AllRecipes.Add(new Recipe(new List<Item>() { lemonJuice, demonPepper }, martini));
        book.AllRecipes.Add(new Recipe(new List<Item>() { lemonJuice, ravensTears }, martini));
        book.AllRecipes.Add(new Recipe(new List<Item>() { HotLemonJuice, pixieSugar }, martini));
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenLemonJuice, pixieSugar }, martini));
        book.AllRecipes.Add(new Recipe(new List<Item>() { HotLemonJuice, demonPepper }, martini));
        book.AllRecipes.Add(new Recipe(new List<Item>() { ShakenLemonJuice, demonPepper }, martini));
        book.AllRecipes.Add(new Recipe(new List<Item>() { lemonJuice, ShakenRavensTears }, martini));


        //RT + DP + PS + PI = [RS] Rainbow Sludge (confusion, thrill, surprise)
        Item rainbow = new Item("Rainbow Sludge");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ravensTears, demonPepper, powderedIron, pixieSugar }, rainbow));

        /*RT + DP + PS = [PrSp] Primordial Soup (wisdom, life, experience)
        DP + PS + PI = [PrSp] Primordial Soup (wisdom, life, experience)`*/
        Item soup = new Item("Primordial Soup");
        book.AllRecipes.Add(new Recipe(new List<Item>() { ravensTears, demonPepper, pixieSugar }, soup));
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
