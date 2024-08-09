using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptTest : MonoBehaviour
{
    public float speed;
    private bool chooseDir = false;

    public EnemyState currState = EnemyState.Wander;

    private Vector2 randomDir;
    private SpriteRenderer sprite;
    public Stanza room;

    public Rect rect;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        getBounds();
        StartCoroutine(Wander());

              

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

    private bool IsWithinBounds(Vector3 position)
    {
        return rect.Contains(new Vector2(position.x, position.y));
    }

    void Update()
{
    if (currState == EnemyState.Wander && chooseDir)
    {
        Vector3 newPos = transform.position + (Vector3)randomDir * speed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, rect.xMin, rect.xMax);
        newPos.y = Mathf.Clamp(newPos.y, rect.yMin, rect.yMax);
        Debug.Log($"Current Position: {transform.position}, New Position: {newPos}, Within Bounds: {IsWithinBounds(newPos)}, CurrentRect : {rect}");

        if (IsWithinBounds(newPos))
        {
            transform.position = newPos;
        }
        else
        {
            chooseDir = false;  // Stop movement if outside bounds
            Debug.Log("Out of bounds, stopping movement.");
        }
    }
}

private void getBounds(){

     rect = this.room.GetRoomBounds();

}


private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(rect.center, rect.size);
}

}
