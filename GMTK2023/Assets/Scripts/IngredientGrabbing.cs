using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static DraggableObject;

public class IngredientGrabbing : MonoBehaviour
{

    //added stuff
    public enum ingredientStatus { Dragging, NotDragging, Animating };
    public ingredientStatus ingStatus;
    public bool hovering = false;
    Vector3 worldMousePos;
    Vector2 oldPosition; //used to calculate throw speed

    // CamShakeStuff
    public GameObject cam;
    private float shakeDuration = 0f;
    private float shakeMagnitude;
    private float dampingSpeed = 1.0f;
    Vector3 initialPosition;
    public float weight;

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
    private void Awake()
    {
        ingStatus = ingredientStatus.NotDragging;
        shakeMagnitude = weight;
        rb2d = GetComponent<Rigidbody2D>();
        cam = Camera.main.gameObject;
        initialPosition = cam.transform.position;
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
            case ingredientStatus.Animating:
                break;
        }
    }

    public void applyGravity()
    {
        if (!grounded)
        {
            float yspeed = rb2d.velocity.y - gravity * Time.deltaTime;
            rb2d.velocity = new Vector2(rb2d.velocity.x, yspeed);
            ingStatus = ingredientStatus.NotDragging;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();

        //screen shake

        if (shakeDuration > 0)
        {
            Vector3 targetpos = initialPosition + Random.insideUnitSphere * shakeMagnitude; 
            cam.transform.position = new Vector3(targetpos.x, targetpos.y, cam.transform.localPosition.z);
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else if (shakeDuration < 0)
        {
            shakeDuration = 0f;
            cam.transform.position = initialPosition;
        }


        //mousepos to worldpos
        Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        worldMousePos = Camera.main.ScreenToWorldPoint(screenPoint);
        
        //pick up the object
        if (Input.GetButtonDown("Fire1") && hovering)
        {
            ingStatus = ingredientStatus.Dragging;
        }
        if (Input.GetButtonUp("Fire1") && ingStatus == ingredientStatus.Dragging)
        {
            GetComponent<BoxCollider2D>().enabled = true;
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
        Vector2 direction = worldMousePos - transform.position; 
        Vector2 transitionaryVector = direction.normalized * 45 * Mathf.Sqrt(Mathf.Pow(worldMousePos.x - transform.position.x, 2f) + Mathf.Pow(worldMousePos.y - transform.position.y, 2f)); //makes the squares go to the mouse but not jitter (based)
        velocity = transitionaryVector;
        rb2d.velocity = velocity;
    }

    public void applyPhysics()
    {
        if (crushed == false)
        {
            RaycastHit2D GroundCheck = Physics2D.Linecast(leftGc.position, rightGc.position, 1 << 6);
            if (GroundCheck.collider != null)
            {
                if (!grounded)
                {
                    shakeDuration = 0.1f;
                }
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
            RaycastHit2D NearGroundCheck = Physics2D.Linecast(leftGc.transform.position + new Vector3(0, -0.1f, 0), rightGc.transform.position + new Vector3(0, -0.1f, 0), 1 << 6);
            RaycastHit2D WallCheckRight = Physics2D.Linecast(WCLR.position, WCHR.position, 1 << 6);
            RaycastHit2D WallCheckLeft = Physics2D.Linecast(WCLL.position, WCHL.position,  1 << 6 );
            RaycastHit2D CeilingCheck = Physics2D.Linecast(CCR.position, CCL.position, 1 << 6);

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
                    shakeDuration = 0.1f;
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
                if (velocity.x < -0.5f)
                {
                    shakeDuration = 0.1f;
                }
                velocity.x = Mathf.Abs(velocity.x);
                if (WallCheckLeft.collider.gameObject.tag == "Platform")
                {
                    transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, 0f);
                }
            }
            if (WallCheckRight.collider != null)
            {
                if (velocity.x > 0.5f)
                {
                    shakeDuration = 0.1f;
                }
                velocity.x = -Mathf.Abs(velocity.x);

                if (WallCheckRight.collider.gameObject.tag == "Platform")
                {
                    transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, 0f);
                }
            }
            if (CeilingCheck.collider != null)
            {
                if(velocity.y > 1)
                {
                    shakeDuration = 0.1f;
                }
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
        //GetComponent<BoxCollider2D>().enabled = false;
        //crushed = true;
        //rb2d.bodyType = RigidbodyType2D.Static;
        //transform.GetChild(0).gameObject.SetActive(false);
        //Instantiate(breakSmoke, transform.position, Quaternion.identity);
;    }

    public void getGrabbed()
    {
        ingStatus = IngredientGrabbing.ingredientStatus.Dragging;
    }

 }
