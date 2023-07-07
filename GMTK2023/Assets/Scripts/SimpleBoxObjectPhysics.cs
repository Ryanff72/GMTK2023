using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static DraggableObject;

public class SimpleBoxObjectPhysics : MonoBehaviour
{
    //added stuff
    public enum ingredientStatus { Dragging, NotDragging };
    public ingredientStatus ingStatus;
    bool hovering = false;
    Vector3 worldMousePos;
    Vector2 oldPosition; //used to calculate throw speed


    //god what happened here
    public Vector2 velocity;
    private Rigidbody2D rb2d;
    bool grounded;
    bool nearGrounded;
    [SerializeField] bool crushable;
    bool hasSpawnedSmoke = false;
    public float gravity;
    [SerializeField] Transform leftGc;
    [SerializeField] Transform rightGc;
    [SerializeField] Transform WCLR;
    [SerializeField] Transform WCHL;
    [SerializeField] Transform WCLL;
    [SerializeField] Transform WCHR;
    [SerializeField] Transform CCR;
    [SerializeField] Transform CCL;
    bool velHasDiminished = false;
    //[SerializeField] GameObject landingSmoke;
    //[SerializeField] GameObject breakSmoke;
    //[SerializeField] GameObject SoundCreator;
    bool crushed;
    float crushtime;
    public AudioClip landSound;
    // Start is called before the first frame update
    void Start()
    {
        //SoundCreator = Instantiate(SoundCreator, transform.position, Quaternion.identity);
        rb2d = GetComponent<Rigidbody2D>();
        ingStatus = ingredientStatus.NotDragging;
    }

    public void StateMachine()
    {
        switch (ingStatus)
        {
            case ingredientStatus.NotDragging:
                applyPhysics();
                break;
            case ingredientStatus.Dragging:
                syncPosition();
                StartCoroutine("getOldDistance", 0.05f);
                break;
        }
    }

    public void applyGravity()
    {
        if (!grounded)
        {
            float yspeed = rb2d.velocity.y - gravity * Time.deltaTime;
            rb2d.velocity = new Vector2(rb2d.velocity.x, yspeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();

        Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        worldMousePos = Camera.main.ScreenToWorldPoint(screenPoint);
        
        //pick up the object
        if (Input.GetButtonDown("Fire1") && hovering)
        {
            ingStatus = ingredientStatus.Dragging;
        }
        if (Input.GetButtonUp("Fire1") && ingStatus == ingredientStatus.Dragging)
        {
            ingStatus = ingredientStatus.NotDragging;
            velocity = new Vector2((transform.position.x-oldPosition.x)/0.2f, (transform.position.y - oldPosition.y) / 0.2f);
        }
    }

    private IEnumerator getOldDistance(float waitTime)
    {
        Vector2 position = transform.position;
        yield return new WaitForSeconds(waitTime);
        oldPosition = position;

    }

    private void OnMouseEnter()
    {
        hovering = true;
    }
    private void OnMouseExit()
    {
        hovering = false;
    }

    public void syncPosition()
    {
        transform.position = worldMousePos;
    }

    public void applyPhysics()
    {
        if (crushed == false)
        {
            RaycastHit2D GroundCheck = Physics2D.Linecast(leftGc.position, rightGc.position, 1 << LayerMask.NameToLayer("Ground"));
            if (GroundCheck.collider != null)
            {
                grounded = true;
                if (hasSpawnedSmoke == false)
                {
                    hasSpawnedSmoke = true;
                    //Instantiate(landingSmoke, transform.position + new Vector3(0, -0.72f, 0), Quaternion.Euler(-90, 0, 0));
                    //SoundCreator.transform.position = transform.position;
                    //SoundCreator.GetComponent<AudioProximity>().PlaySound(landSound, 150f, 6f);
                }
                if (GroundCheck.collider.gameObject.tag == "Platform")
                {
                    rb2d.velocity = rb2d.velocity + GroundCheck.collider.gameObject.GetComponent<Rigidbody2D>().velocity * Time.fixedDeltaTime;
                }
                if (GroundCheck.collider.gameObject.tag == "Platform")
                {
                    transform.SetParent(GroundCheck.collider.gameObject.transform);
                }
                else
                {
                    if (crushable == true)
                    {
                        transform.parent = null;
                    }
                }

            }
            else
            {
                if (crushable == true)
                {
                    transform.parent = null;
                }
                hasSpawnedSmoke = false;
                grounded = false;
            }
            RaycastHit2D NearGroundCheck = Physics2D.Linecast(leftGc.transform.position + new Vector3(0, -0.1f, 0), rightGc.transform.position + new Vector3(0, -0.1f, 0), 1 << LayerMask.NameToLayer("Ground"));
            RaycastHit2D WallCheckRight = Physics2D.Linecast(WCLR.position, WCHR.position, 1 << LayerMask.NameToLayer("Ground"));
            RaycastHit2D WallCheckLeft = Physics2D.Linecast(WCLL.position, WCHL.position, 1 << LayerMask.NameToLayer("Ground"));
            RaycastHit2D CeilingCheck = Physics2D.Linecast(CCR.position, CCL.position, 1 << LayerMask.NameToLayer("Ground"));

            if (NearGroundCheck.collider != null)
            {
                nearGrounded = true;
                velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * 3f);
            }
            else
            {
                nearGrounded = false;
            }
            if (grounded == true && CeilingCheck.collider != null && crushable == true)
            {
                Crushed();
            }
            if (grounded == true)
            {
                if (velocity.y < -15f)
                {
                    velocity.y = Mathf.Abs(velocity.y);
                    velHasDiminished = false;
                }
                else
                {
                    velocity.y = Mathf.Lerp(velocity.y, -1, Time.deltaTime * 10);
                    velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * 2f);
                }
                if (CeilingCheck.collider != null && grounded == true)
                {
                    crushtime += Time.deltaTime;
                    if (crushtime > 0.1f)
                    {
                        Crushed();
                    }

                }
                else
                {
                    crushtime = 0;
                }
            }
            else if (velHasDiminished == false)
            {
                velHasDiminished = true;
                velocity.x *= 0.5f;
                velocity.y *= 0.6f;
            }
            if (WallCheckLeft.collider != null)
            {
                velocity.x = Mathf.Abs(velocity.x);
                if (WallCheckLeft.collider.gameObject.tag == "Platform")
                {
                    transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, 0f);
                }
            }
            if (WallCheckRight.collider != null)
            {
                velocity.x = -Mathf.Abs(velocity.x);
                if (WallCheckRight.collider.gameObject.tag == "Platform")
                {
                    transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, 0f);
                }
            }
            if (CeilingCheck.collider != null)
            {
                velocity.y = -Mathf.Abs(velocity.y);
            }
            if (nearGrounded == false)//workaround for gravity being wonky
            {
                velocity.y += gravity * Time.deltaTime;
            }
            rb2d.velocity = velocity;
        }
    }


    public void Crushed()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        crushed = true;
        rb2d.bodyType = RigidbodyType2D.Static;
        transform.GetChild(0).gameObject.SetActive(false);
        //Instantiate(breakSmoke, transform.position, Quaternion.identity);
;    }

 }
