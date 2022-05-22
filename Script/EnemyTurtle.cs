using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurtle : MonoBehaviour
{
    private Animator anim;

    public Transform player;
    public Transform enemy;
    private float distance = 1f; 

    public Rigidbody enemyrb;

    private float hitDist = 1.5f;
    private float lookDist = 3f;

    private float speed = 4f;
    private bool movingRight = true;
    public Transform groundDetection;

    private float angleForTurtle = 180f;

    private float health = 100f;

    private float hitPower = 10f;

    private bool playerLive = true;

    private bool isLive = true;

    private int achievment = 15;

    float timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        enemyrb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    private bool isAttacking = false;
    void Update()
    {
        if(!isLive){
            return;
        }

        RaycastHit hit;

        moveTurtle();

        if(health <= 0){
            FindObjectOfType<AudioManager>().Play("ObjectDie");
            isLive = false;
            anim.SetBool("isDie", true);
            player.GetComponent<PlayerMove>().increaseScore(achievment);
            return;
        }
        
        if(movingRight){
            
            if(!Physics.Raycast(groundDetection.position, -Vector3.up, out hit, distance)){
                movingRight = false;
                angleForTurtle = 360f;
                setRotation();
            }
        } else {
            
            if(!Physics.Raycast(groundDetection.position, -Vector3.up, out hit, distance)){
                movingRight = true;
                angleForTurtle = 180f;
                setRotation();
            }
        }

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if(distToPlayer < lookDist && playerLive){
            anim.SetBool("LookToPlayer", true);
            enemy.LookAt(player);
            isAttacking = true;
        } else {
            anim.SetBool("LookToPlayer", false);
            isAttacking = false;
            setRotation();
        }

        if(distToPlayer < hitDist && playerLive){ 
            anim.SetBool("Attacking", true);
            timePassed += Time.deltaTime;
            if(timePassed > 1f){
                player.GetComponent<PlayerMove>().getHit(hitPower);
                float playerHp = player.GetComponent<PlayerMove>().getHealth();
                if(playerHp <= 0){
                    playerLive = false;
                }
                timePassed = 0f;
            }
            enemy.LookAt(player);
        } else {
            anim.SetBool("Attacking", false);
        }

        
    }

    private void setRotation(){
        transform.eulerAngles = new Vector3(0f, angleForTurtle, 0f);
    }

    public void getHit(float power){
        health -= power;
        FindObjectOfType<AudioManager>().Play("PlayerDamage");
    }

    private void moveTurtle(){
        if(movingRight && !isAttacking){
            enemyrb.MovePosition(transform.position + new Vector3(0, 0, -1f) * Time.deltaTime * speed);
        } else if(!movingRight && !isAttacking) {
            enemyrb.MovePosition(transform.position + new Vector3(0, 0, 1f) * Time.deltaTime * speed);
        }
    }

    void FixedUpdate() {
        
        

    }

}
