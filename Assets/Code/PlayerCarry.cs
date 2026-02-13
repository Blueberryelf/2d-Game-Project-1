using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCarry : MonoBehaviour
{
    public Transform holdPoint;
    public float pickupRadius = 0.5f;
    public LayerMask carryLayer;

    private Rigidbody2D carriedRb;
    private Transform carriedObj;

    public float lastMoveDir = 1f;
    public float dropDistance = 0.6f;

    private PlayerMovement playerMovement;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && playerMovement.isGrounded)
        {


            if (carriedRb == null)
            {
                
                TryPickup();
                
            }
            else
            {
                
                Drop();
                
            }
                
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
            lastMoveDir = Mathf.Sign(moveInput);

        if (carriedObj != null)
        {
            carriedObj.position = holdPoint.position;
        }

    }

    void TryPickup()
    {
        
        if (carriedObj != null) return;

        

        Collider2D hit = Physics2D.OverlapCircle(
            holdPoint.position,
            pickupRadius,
            carryLayer
            );

        if (hit == null) return;


        Animals animal = hit.GetComponent<Animals>();
        if (animal == null) return;

        //block pickup if there is any other object of the same type nearby (stacked)
        Collider2D[] others = Physics2D.OverlapCircleAll(hit.transform.position, 2f, carryLayer);
        foreach (Collider2D other in others)
        {
            if (other == hit) continue; //skip itself
            Animals otherAnimal = other.GetComponent<Animals>();
            if (otherAnimal != null && otherAnimal.objectType == animal.objectType)
            {
                return;
            }
        }


        carriedRb = hit.GetComponent<Rigidbody2D>();
        carriedObj = hit.transform;

        //disable physics

        carriedRb.bodyType = RigidbodyType2D.Kinematic;
        
        carriedRb.simulated = false;

        animator.SetBool("isCarrying", true);

    }

    void Drop()
    {
        
        if (carriedObj == null) return;

        

        //determine drop position
        Vector3 dropPos = transform.position;
        dropPos.x += lastMoveDir * dropDistance;

        //check if theres an object at that position
        Collider2D hit = Physics2D.OverlapBox(dropPos, new Vector2(0.5f, 0.5f), 0f);
        if (hit != null && hit.CompareTag("Animal"))
        {
            //stack on top
            float topY = hit.bounds.max.y;
            dropPos.y = topY + carriedObj.GetComponent<Collider2D>().bounds.extents.y;
        }


        

        carriedObj.position = dropPos;

        carriedRb.simulated = true;
        carriedRb.bodyType = RigidbodyType2D.Dynamic;
        carriedRb.freezeRotation = true;

        carriedRb = null;
        carriedObj = null;

        animator.SetBool("isCarrying", false);
    }

    private void OnDrawGizmosSelected()
    {
        if (holdPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, pickupRadius);
        }
    }
}
