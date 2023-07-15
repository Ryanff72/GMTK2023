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
    private AudioManager audioManager;
    public GameObject audioManagerReplacement;


    private void Awake()
    {
        GameObject audioManagertest = GameObject.Find("AudioManager");

        if (audioManagertest == null)
        {
            GameObject go = Instantiate(audioManagerReplacement);
            go.name = "AudioManager";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        lastSpoonX = Spoon.transform.position.x;
        
        audioManager.PlayMusic("morning");
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
        if (recipe == null && IngredientsInPot != null)
        {
            Debug.Log("Recipe not found");
            return;
        }
        GameObject output = RecipeController.getIngredient(recipe.Output.Name);
        if (output)
        {
            NewItemSpawner.GetComponent<SpawnNew>().SpawnItem(output);
            audioManager.PlaySoundEffect("newpotion", 0.8f);
        }

        for(int i = IngredientsInPot.Count-1; i >= 0; i--)
        {
            Destroy(IngredientsInPot[i]);
        }
        SpoonXTraveled = 0.0f;
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
