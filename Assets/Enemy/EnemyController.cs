using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public enum EnemyState
{
    Wander,
    Follow,
    Die,
    Attack
};

public class NewBehaviourScript : MonoBehaviour
{

    GameObject player;
    public EnemyState currState = EnemyState.Wander;
    public float range;
    public float speed;
    public float attackRange;
    public float coolDown;
    private float prevX;
    private bool chooseDir = false;
    //private bool dead = false;
    private bool coolDownAttack = false;
    private Vector3 randomDir;
    private SpriteRenderer sprite;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        prevX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        prevX = transform.position.x;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
        switch (currState)
        {
            case EnemyState.Wander:
                Wander();
                break;
            case EnemyState.Follow:
                Follow();
                break;
            case EnemyState.Die:
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
        if (IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Follow;
        }
        else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Wander;
        }
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            currState = EnemyState.Attack;
        }

        UpdateAnimationUpdate();
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }

    void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }
        StartCoroutine(WaitAndMove());
    }

    IEnumerator WaitAndMove()
    {
        // Aspetta un periodo di tempo casuale prima del prossimo movimento
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        transform.position += (Vector3)randomDirection * speed * Time.deltaTime;

        if (IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
            yield break;
        }

        // Richiama ricorsivamente la coroutine per un nuovo movimento casuale dopo il ritardo
        StartCoroutine(WaitAndMove());
    }



    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        if (!coolDownAttack && player.GetComponent<BoxCollider2D>().enabled == true)
        {
            GameController.DamagePlayer(1);
            _ = StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    public void Death()
    {

    }

    private void UpdateAnimationUpdate()
    {
        

        if(transform.position.x >= prevX)
        {
            sprite.flipX = false;
        } 
        else
        {
            sprite.flipX = true;
        }
    }

}