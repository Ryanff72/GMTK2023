using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int CharactersPerDay = 6;
    public float newDayWaitTime = 5.0f;

    public Sprite MorningBg;
    public Sprite SunsetBg;
    public Sprite NightBg;
    public List<Sprite> tablesTops;
    public List<Sprite> tablesBots;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startDay());
    }

    public IEnumerator startDay()
    {
        AudioManager.instance.SetMusic(MusicEnum.MORNING);

        GameObject.Find("background").GetComponent<SpriteRenderer>().sprite = MorningBg;
        GameObject.Find("table").GetComponent<SpriteRenderer>().sprite = tablesTops[0];
        GameObject.Find("tableBottom").GetComponent<SpriteRenderer>().sprite = tablesBots[0];

        yield return new WaitForSeconds(2.5f);
        currentCustomers = new List<GameObject>(Customers.Count);
        for(int i = 0; i < Customers.Count; i++)
        {
            currentCustomers.Add(null);
        }
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
                currentCustomers[i] = (Instantiate(Customers[i], CustomerPos, Quaternion.identity));
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
        yield return new WaitForSeconds(1.3f);
        currentCustomers[CustomerIndex].GetComponent<Client>().receiveItem(serveItem);
        yield return new WaitForSeconds(2f);

        if (CustomerIndex + 1 < currentCustomers.Count)
        {
            CustomerIndex++;
            if(CustomerIndex == 2)
            {
                //sunset
                AudioManager.instance.SetMusic(MusicEnum.NOON);
                GameObject.Find("background").GetComponent<SpriteRenderer>().sprite = SunsetBg;
                GameObject.Find("table").GetComponent<SpriteRenderer>().sprite = tablesTops[1];
                GameObject.Find("tableBottom").GetComponent<SpriteRenderer>().sprite = tablesBots[1];
            }
            else if(CustomerIndex == 3)
            {
				//night
				AudioManager.instance.SetMusic(MusicEnum.NIGHT);
				GameObject.Find("background").GetComponent<SpriteRenderer>().sprite = NightBg;
                GameObject.Find("table").GetComponent<SpriteRenderer>().sprite = tablesTops[2];
                GameObject.Find("tableBottom").GetComponent<SpriteRenderer>().sprite = tablesBots[2];
            }
            if(CustomerIndex % CharactersPerDay == 0)
            {
                day++;
                if(day == 4)
                {
                    yield return new WaitForSeconds(newDayWaitTime / 3);

                    int sumOfHealth = 0;
                    for(int i = 5; i < currentCustomers.Count; i++)
                    {
                        sumOfHealth += currentCustomers[i].GetComponent<Client>().Hp;
                    }
                    if(sumOfHealth <= 5)
                    {
                        // bad ending
				        AudioManager.instance.SetMusic(MusicEnum.BAD_ENDING);
						SceneManager.LoadScene(4);
                    }
                    else
                    {
						// good ending
						AudioManager.instance.SetMusic(MusicEnum.GOOD_ENDING);
						SceneManager.LoadScene(3);

					}
				}
                yield return new WaitForSeconds(newDayWaitTime / 3);
                GameObject.Find("ReviewPage").GetComponent<Animator>().SetTrigger("ShowResult");
                yield return new WaitForSeconds(newDayWaitTime / 2);
                GameObject.Find("background").GetComponent<SpriteRenderer>().sprite = MorningBg;
                GameObject.Find("table").GetComponent<SpriteRenderer>().sprite = tablesTops[0];
                GameObject.Find("tableBottom").GetComponent<SpriteRenderer>().sprite = tablesBots[0];
                yield return new WaitForSeconds(newDayWaitTime);

            }
            currentCustomers[CustomerIndex].GetComponent<Client>().appear();
        } 
        else
        {
            endedDay = true;
        }
        canServe = true;
    }
}
