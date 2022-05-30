using Assets.Scripts;
using UnityEditor;
using UnityEngine;


public class MysteryShip : PlayingElement, IObserver
{
    public float speed = 5.0f;
    public float cycleTime = 10.0f;
    public float attackTime = 0.25f;
    public int score;
    public System.Action<MysteryShip> killed;
    public Vector3 leftDestination { get; private set; }
    public Vector3 rightDestination { get; private set; }
    public int direction { get; private set; } = -1;
    public bool spawned { get; private set; }

    private IWeapon weapon;

    public GameObject _invadersFactory;
    private ILevelFactory _factory;

    private void Start()
    {
        _invadersFactory = LevelController.Level;
        _factory = _invadersFactory.GetComponent<ILevelFactory>();
        this.gameObject.GetComponent<SpriteRenderer>().sprite = _factory.GetMysteryShipView().MysteryShip;

        ShipScore shipScore = new SimpleShipScore();
        if (LevelController.TextLevel == "Hard")
        {
            attackTime = 0.1f;
            shipScore = new HardScore(shipScore);
        }
        else
        {
            attackTime = 0.25f;
            shipScore = new EasyScore(shipScore);
        }
        this.score = shipScore.CalculateTotalScore();

        SetPosition();
        Despawn();
    }

    public void SetWeapon(IWeapon weapon)
    {
        this.weapon = weapon;
    }

    private void Spawn()
    {
        this.direction *= -1;
        if (this.direction == 1)
        {
            this.transform.position = this.leftDestination;
        }
        else
        {
            this.transform.position = this.rightDestination;
        }
        this.spawned = true;
        InvokeRepeating(nameof(Attack), 0f, attackTime);
    }

    protected override void ChangeDirection()
    {
        this.direction = -1;
    }

    protected override void SetPosition()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        Vector3 left = this.transform.position;
        left.x = leftEdge.x - 1.0f;
        this.leftDestination = left;

        Vector3 right = this.transform.position;
        right.x = rightEdge.x + 1.0f;
        this.rightDestination = right;

        this.transform.position = this.leftDestination;
    }

    protected override void Despawn()
    {
        CancelInvoke();
        this.spawned = false;
        if (this.direction == 1)
        {
            this.transform.position = this.rightDestination;
        }
        else
        {
            this.transform.position = this.leftDestination;
        }
        Invoke(nameof(Spawn), this.cycleTime);
    }

    private void Update()
    {
        if (!this.spawned)
        {
            return;
        }

        if (this.direction == 1)
        {
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;

            if (this.transform.position.x >= this.rightDestination.x)
            {
                Despawn();
            }
        }
        else
        {
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;
            if (this.transform.position.x <= this.leftDestination.x)
            {
                Despawn();
            }
        }
    }

    private void Attack()
    {
        weapon.Shoot(this.transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Despawn();
            this.killed?.Invoke(this);
        }
    }

    public void StopAttacking()
    {
        CancelInvoke();
    }

    void IObserver.Update()
    {
        this.cycleTime = 5.0f;
        this.speed = 9.0f;
    }

    void IObserver.UndoUpdate()
    {
        this.cycleTime = 10.0f;
        this.speed = 5.0f;
    }
}
