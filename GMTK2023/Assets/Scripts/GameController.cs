using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> Customers;
    List<GameObject> currentCustomers;
    public int CustomerIndex = 0;
    public Vector2 CustomerPos;
    public int day = 1;
    public GameObject serveItem;
    bool canServe = true;
    bool endedDay = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startDay());
    }

    public IEnumerator startDay()
    {
        yield return new WaitForSeconds(1.0f);
        currentCustomers = new List<GameObject>();
        for (int i = 0; i < Customers.Count; i++)
        {
            if (GameObject.Find(Customers[i].name + "(Clone)"))
            {
                currentCustomers[i] = GameObject.Find(Customers[i].name + "(Clone)");
            }
            else if(GameObject.Find(Customers[i].name))
            {
                currentCustomers[i] = GameObject.Find(Customers[i].name);
            }
            else
            {
                currentCustomers.Add(Instantiate(Customers[i], CustomerPos, Quaternion.identity));
            }
        }
        currentCustomers[0].GetComponent<Client>().appear();
        DialogueParser.initDialogue();
    }



    // Update is called once per frame
    void Update()
    {
        if(serveItem != null && canServe && !endedDay)
        {
            canServe = false;
            StartCoroutine(nextCustomer());
        }
    }

    IEnumerator nextCustomer()
    {
        serveItem.GetComponent<Ingredient>().serve();
        yield return new WaitForSeconds(0.5f);
        currentCustomers[CustomerIndex].GetComponent<Client>().receiveItem(serveItem);
        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(1.5f);
        if (CustomerIndex + 1 < currentCustomers.Count)
        {
            CustomerIndex++;
            currentCustomers[CustomerIndex].GetComponent<Client>().appear();
        } else
        {
            endedDay = true;
        }
        canServe = true;
    }
}
