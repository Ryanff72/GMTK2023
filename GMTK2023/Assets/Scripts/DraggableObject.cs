using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DraggableObject : MonoBehaviour
{

    public enum ingredientStatus {Dragging, NotDragging};
    public ingredientStatus ingStatus;
    public bool grounded = false;
    public float gravity;
    public GameObject leftGc;
    public GameObject rightGc;

    //components
    public Rigidbody2D rb2d;
    


    // Start is called before the first frame update
    void Start()
    {
        ingStatus = ingredientStatus.NotDragging;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
        grounded = Physics2D.Linecast(leftGc.transform.position, rightGc.transform.position, 1 << LayerMask.NameToLayer("Ground"));

    }

    public void StateMachine()
    {
        switch (ingStatus)
        {
            case ingredientStatus.NotDragging:
                applyGravity();
                break;
            case ingredientStatus.Dragging:
                
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
}
