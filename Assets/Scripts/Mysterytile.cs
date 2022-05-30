using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Mysterytile : MonoBehaviour
{
    public System.Action<Mysterytile> destroyed;
    static bool isRight = true;
    private bool currState = true;
    public new BoxCollider2D collider { get; private set; }

    private void Awake()
    {
        this.collider = GetComponent<BoxCollider2D>();
        if(isRight)
        {
            isRight = false;
        }
        else
        {
            isRight = true;
        }
        currState = isRight;
    }

    private void OnDestroy()
    {
        this.destroyed?.Invoke(this);
    }

    private void Update()
    {
        Vector3 direct = new Vector3();
        if (currState)
        {
            direct.x = 0.3f;
        }
        else
        {
            direct.x = -0.3f;
        }
        direct.y = -1.0f;
        this.transform.position += direct * 30.0f * Time.deltaTime;
    }
    
    private void CheckCollision(Collider2D other)
    {
        Bunker bunker = other.gameObject.GetComponent<Bunker>();
        if (bunker == null || bunker.CheckCollision(this.collider, this.transform.position))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

}
