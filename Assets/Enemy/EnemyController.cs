using System.Collections;
using UnityEngine;

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
    private bool chooseDir = false;
    private bool coolDownAttack = false;
    private Vector2 randomDir;
    private SpriteRenderer sprite;
    public Stanza room;

    private float normalSpeed;  // Store the normal speed
    public Rect rect;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        getBounds();

        // Store the normal speed at the start
        normalSpeed = speed;

        StartCoroutine(Wander());
    }

    void Update()
    {
        if (!GameController.morto)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
            
            switch (currState)
            {
                case EnemyState.Wander:
                    if (chooseDir)
                    {
                        WanderUpdate();
                    }
                    break;
                case EnemyState.Follow:
                    Follow();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
            }

            if (IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Follow;
                StopCoroutine(Wander());
                // Increase speed when following
                speed = normalSpeed * 2f;
            }
            else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                if (currState != EnemyState.Wander)
                {
                    currState = EnemyState.Wander;
                    StartCoroutine(Wander());
                    // Revert speed to normal when wandering
                    speed = normalSpeed;
                }
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currState = EnemyState.Attack;
            }

            UpdateAnimationUpdate();
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator Wander()
    {
        while (currState == EnemyState.Wander)
        {
            chooseDir = true;
            randomDir = Random.insideUnitCircle.normalized;
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            chooseDir = false;
            yield return new WaitForSeconds(Random.Range(0f, 1f));  // Pause before choosing a new direction
        }
    }

    void WanderUpdate()
    {
        Vector3 newPos = transform.position + (Vector3)randomDir * speed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, rect.xMin, rect.xMax);
        newPos.y = Mathf.Clamp(newPos.y, rect.yMin, rect.yMax);

        if (IsWithinBounds(newPos))
        {
            transform.position = newPos;
        }
        else
        {
            chooseDir = false;  // Stop movement if outside bounds
        }
    }

    void Follow()
    {
        Vector3 targetPos = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (IsWithinBounds(targetPos))
        {
            transform.position = targetPos;
        }
    }

    void Attack()
    {
        if (!coolDownAttack && player.GetComponent<BoxCollider2D>().enabled)
        {
            GameController.DamagePlayer(1);
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    private bool IsWithinBounds(Vector3 position)
    {
        return rect.Contains(new Vector2(position.x, position.y));
    }

    private void UpdateAnimationUpdate()
    {
        sprite.flipX = transform.position.x < player.transform.position.x;
    }

    private void getBounds()
    {
        rect = this.room.GetRoomBounds();
    }
}
