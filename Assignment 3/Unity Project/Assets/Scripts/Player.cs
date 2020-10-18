using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private const int MAX_HEALTH = 3;
    
    public int speed = 3;
    private float gravity = 9.8f;
    public int jumpSpeed = 3;
    private float vSpeed = 0f;
    private CharacterController controller;
    private bool isJump = false;
    private int right = 1;
    private Vector3 vel;
    private Transform transform;
    private Collider collider;
    public Vector3 startPos;
    private bool canMove = true;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    private float timer = 0f;
    public float groundDistance = 1f;
    private bool jumpFrame = false;
    private int health = 3;
    public Image[] healthImages = new Image[3];
    public GameObject deathMenu;
    public Image deathScreen;
    public Image respawnButton;
    public Image endButton;
    public Text deathText;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        collider = gameObject.GetComponent<Collider>();
        transform = gameObject.GetComponent<Transform>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpFrame == true)
            jumpFrame = false;

        if (canMove)
        {
            //Reset velocity
            vel = Vector3.zero;

            //Jump Code
            if (Input.GetKeyDown("space") && isJump == false)
            {
                jumpFrame = true;
                vSpeed = jumpSpeed;
                isJump = true;
                Vector3 adjust = new Vector3(0, groundDistance, 0);
                transform.position += adjust;

                if (gravity != 9.8f)
                    gravity = 9.8f;
            }

            //Landing Code
            if (CheckGround() && jumpFrame == false)
            {
                vSpeed = 0;
                isJump = false;

                if (gravity != 0f)
                    gravity = 0f;
            }
            else if (jumpFrame == false)
            {
                if (vSpeed < -1f)
                    isJump = true;

                if (gravity != 9.8f)
                    gravity = 9.8f;
            }

            //Applying Gravity
            vSpeed -= gravity * Time.deltaTime;

            if (transform.position.x < -6)
                canMoveLeft = false;
            else
                canMoveLeft = true;

            if (transform.position.x > 190)
                canMoveRight = false;
            else
                canMoveRight = true;

            //Left and Right Code
            if (Input.GetKey("d") && canMoveRight)
                vel.x = right * speed;
            else if (Input.GetKey("a") && canMoveLeft)
                vel.x = -right * speed;

            //Applying vertical speed to velocity
            vel.y = vSpeed;
            //Moving character
            controller.Move(vel * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            health--;

            if (health <= 0)
            {
                Death();
            }
            else
            {
                ShowHealth();
            }
        }
        else if (other.gameObject.tag == "Trap")
        {
            health = 0;
            ShowHealth();

            if (health <= 0)
            {
                Death();
            }
        }
        else if (other.gameObject.tag == "KillPlane")
        {
            health = 0;
            ShowHealth();
            Death();
        }
    }

    private void Death()
    {
        canMove = false;
        deathMenu.SetActive(true);

        deathScreen.canvasRenderer.SetAlpha(0.01f);
        deathScreen.CrossFadeAlpha(1f, .5f, false);

        deathText.canvasRenderer.SetAlpha(0.01f);
        deathText.CrossFadeAlpha(1f, .5f, false);

        respawnButton.canvasRenderer.SetAlpha(0.01f);
        respawnButton.CrossFadeAlpha(1f, .5f, false);

        endButton.canvasRenderer.SetAlpha(0.01f);
        endButton.CrossFadeAlpha(1f, .5f, false);
    }

    public void Respawn()
    {
        deathMenu.SetActive(false);

        canMove = true;
        transform.position = startPos;
        health = MAX_HEALTH;
        vSpeed = 0;
        ShowHealth();
    }

    private void ShowHealth()
    {
        for (int i = 0; i < MAX_HEALTH; i++)
        {
            healthImages[i].enabled = false;
        }

        for (int i = 0; i < health; i++)
        {
            healthImages[i].enabled = true;
        }
    }

    private bool CheckGround()
    {
        RaycastHit hit;
        bool grounded = false;
        LayerMask mask = (1 << 8);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, mask))
        {
            if (hit.distance < groundDistance)
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }

        return grounded;
    }

    public void SetSpawn(Vector3 position)
    {
        startPos = position;
    }
}
