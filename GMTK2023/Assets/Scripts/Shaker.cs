using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public enum shakerStatus { Shaking, NotShaking, Dormant, Output };
    public shakerStatus shksts;

    Vector3 worldMousePos;
    bool hovering;

    float Ytraveled;
    float lastPosY;

    float totaltraveled;
    public float shakeRequired;

    public Sprite ShakerClosed;
    private Sprite ShakerOpen;

    bool hasOutputItems;
    public List<GameObject> shakenObjects = new List<GameObject>();

    private Sprite OpenSprite;
    public Sprite ClosedSprite;

    private void Start()
    {
        OpenSprite = transform.GetChild(3).GetComponent<SpriteRenderer>().sprite;
        ShakerOpen = transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite;
        hasOutputItems = false;
        shakenObjects.Clear();
        shksts = shakerStatus.Dormant;
    }
    public void StateMachine()
    {
        switch (shksts)
        {
            case shakerStatus.Dormant:
                transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = OpenSprite;
                break;
            case shakerStatus.NotShaking:
                transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = ShakerOpen;
                goBack();
                break;
            case shakerStatus.Shaking:
                transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = ClosedSprite;
                shakeIngredients();
                transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = ShakerClosed;
                break;
            case shakerStatus.Output:
                transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = OpenSprite;
                transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = ShakerOpen;
                goBack();
                StartCoroutine("SpawnOutputs");
                break;
        }
    }

    private void Update()
    {
        StateMachine();

        //mousepos to worldpos
        Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        worldMousePos = Camera.main.ScreenToWorldPoint(screenPoint);

        //pick up the object
        if (Input.GetButtonDown("Fire1") && hovering && shksts != shakerStatus.Output && shakenObjects.Count!=0)
        {
            shksts = shakerStatus.Shaking;
        }
        if (Input.GetButtonUp("Fire1") && shksts == shakerStatus.Shaking)
        {
            shksts = shakerStatus.NotShaking;
        }
    }
    private void OnMouseEnter()
    {
        hovering = true;
    }
    private void OnMouseExit()
    {
        hovering = false;
    }

    private IEnumerator SpawnOutputs()
    {
        if (!hasOutputItems)
        {
            hasOutputItems = true;
            for (int i = 0; i < shakenObjects.Count; i++)
            {
                yield return new WaitForSeconds(0.5f);
                shakenObjects[i].GetComponent<IngredientGrabbing>().ingStatus = IngredientGrabbing.ingredientStatus.NotDragging;
                if (shakenObjects[i].GetComponent<IngredientGrabbing>().shakeable == true)
                {
                    shakenObjects[i].GetComponent<Ingredient>().Mod = Modifier.Shaken;
                    shakenObjects[i].GetComponent<Ingredient>().item.Mod = Modifier.Shaken;
                }
                shakenObjects[i].gameObject.transform.position = new Vector3(-1.22000003f, -0.439999998f, 0);
                shakenObjects[i].GetComponent<IngredientGrabbing>().velocity = new Vector2(Random.Range(12, 18), Random.Range(17, 20));
            }
            shakenObjects.Clear();
            yield return new WaitForSeconds(0.5f);
            shakenObjects.Clear();
            shksts = shakerStatus.NotShaking;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.layer == 8 && (collision.gameObject.GetComponent<IngredientGrabbing>() != null && shksts == shakerStatus.NotShaking || shksts == shakerStatus.Dormant))
        {
            hasOutputItems = false;
            shakenObjects.Add(collision.gameObject);
            collision.gameObject.GetComponent<IngredientGrabbing>().ingStatus = IngredientGrabbing.ingredientStatus.Static;
            
        }

    }
    void goBack()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(-1.34526443f, -0.74000001f, 0.0367328897f), Time.deltaTime * 30);
    }

    void shakeIngredients()
    {
        if (GetComponentInChildren<BoxCollider2D>().enabled)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        //sees if the thing has travelled far enough
        Ytraveled += Mathf.Abs(transform.position.y - lastPosY);
        lastPosY = transform.position.y;

        if (Ytraveled >= shakeRequired)
        {
            shksts = shakerStatus.Output;
            Ytraveled = 0.0f;
        }
        transform.position = new Vector2(transform.position.x,worldMousePos.y);
    }



}
