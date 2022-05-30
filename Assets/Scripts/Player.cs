using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IObserver
{
    public static Player Instance { get; private set; }

    public float speed = 5.0f;
    public Projectile laserPrefab;
    public System.Action killed;
    public bool laserActive { get; private set; }
    private bool isTwoLazers = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Vector3 position = this.transform.position;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= this.speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += this.speed * Time.deltaTime;
        }
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.75f, rightEdge.x - 0.75f);
        this.transform.position = position;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!this.laserActive)
        {
            this.laserActive = true;
            List<Projectile> lasers = new List<Projectile>();
            if(isTwoLazers)
            {
                Vector3 transformVector = this.transform.position;
                transformVector.x -= 0.5f;
                lasers.Add(Instantiate(this.laserPrefab, transformVector, Quaternion.identity));
                transformVector.x += 1.0f;
                lasers.Add(Instantiate(this.laserPrefab, transformVector, Quaternion.identity));
            }
            else
            {
                lasers.Add(Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity));
            }
            foreach(var laser in lasers)
                laser.destroyed += OnLaserDestroyed;
        }
    }

    private void OnLaserDestroyed(Projectile laser)
    {
        this.laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            this.killed?.Invoke();
        }
    }

    void IObserver.Update()
    {
        this.speed = 10.0f;
        isTwoLazers = true;
    }

    void IObserver.UndoUpdate()
    {
        this.speed = 5.0f;
        isTwoLazers = false;
    }
}
