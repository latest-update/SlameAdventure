using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 5f;

    public float health = 100f;

    private float hitPower = 30f;


    public bool isGrounded;

    public Rigidbody rb;
    private Animator anim;

    private int score = 0;


    public Text hpDisp;

    public Text scoreDisp;

    public Text gameOver;

    private bool giveDamage = false;

    private bool isLive = true;

    private Collider enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();  
        hpDisp.text = "HP: " + health; 
    }

    // Update is called once per frame
    void Update()
    {
        checkHp();
        scoreUpdate();
        AnimationControl(); 
    }

    private void checkHp(){
        if(health <= 0){
            anim.SetBool("isAttack", false);
            anim.SetBool("isDie", true);
            isLive = false;
            gameOver.text = "Game Over";
            FindObjectOfType<AudioManager>().Play("PlayerDie");
        }
    }

    float timePassed = 0f;
    void AnimationControl(){
        if(!isLive){
            return;
        }
        if(Input.GetKey(KeyCode.W)){
            anim.SetBool("isWalking", true);
        } else {
            anim.SetBool("isWalking", false);
        }

        if(Input.GetKey(KeyCode.S)){
            anim.SetBool("isBackwardWalking", true);
        } else {
            anim.SetBool("isBackwardWalking", false);
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            FindObjectOfType<AudioManager>().Play("PlayerJump");
            anim.SetBool("isJumping", true);
            isGrounded = false;
            rb.velocity = new Vector3(0.0f, jump, 0.0f);
        } else {
            anim.SetBool("isJumping", false);
        }

        if(Input.GetKey(KeyCode.E)){
            anim.SetBool("isAttack", true);
            if(giveDamage){
                transform.LookAt(enemy.gameObject.transform);
                timePassed += Time.deltaTime;
                if(timePassed > 1f){
                    enemy.gameObject.GetComponent<EnemyTurtle>().getHit(hitPower);
                    timePassed = 0f;
                }
            }
        } else {
            anim.SetBool("isAttack", false);
            transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }
        
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    public void getHit(float power){
        health -= power;
        hpDisp.text = "HP: " + health;
        FindObjectOfType<AudioManager>().Play("PlayerDamage");
    }

    public float getHealth(){
        return health;
    }

    void FixedUpdate() {
        if(!isLive){
            return;
        }
        Vector3 m_Input = new Vector3(Input.GetAxis("Vertical"), 0, 0);
        rb.MovePosition(transform.position + m_Input * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag=="EnemyTurtle"){
            giveDamage = true;
            enemy = other;
        } else if(other.gameObject.tag=="Help"){
            FindObjectOfType<AudioManager>().Play("PlayerHelp");
            Destroy(other.gameObject);
            health = 100f;
            hpDisp.text = "HP: " + health;
        } else if(other.gameObject.tag=="Trophy"){
            if(score > 50){
                FindObjectOfType<AudioManager>().Play("GetTrophy");
                Destroy(other.gameObject);
                isLive = false;
                gameOver.text = "You Win!!!";
                gameOver.color = Color.black;
            }
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag=="EnemyTurtle"){
            giveDamage = false;
            enemy = null;
        } 
    }

    public void increaseScore(int scoreCost){
        score += scoreCost;
    }

    private void scoreUpdate(){
        scoreDisp.text = "Score: " + score;
    }

}
