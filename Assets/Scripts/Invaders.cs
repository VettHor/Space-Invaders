using System.Collections.Generic;
using UnityEngine;

public class Invaders : PlayingElement, IObservable
{
    private List<IObserver> _observers = new List<IObserver>();
    public Invader[] prefabs = new Invader[5];
    public AnimationCurve speed = new AnimationCurve();
    public Vector3 direction { get; private set; } = Vector3.right;
    public Vector3 initialPosition { get; private set; }
    public System.Action<Invader> killed;
    public int AmountKilled { get; private set; }
    public int AmountAlive => this.TotalAmount - this.AmountKilled;
    public int TotalAmount => this.rows * this.columns;
    public float PercentKilled => (float)this.AmountKilled / (float)this.TotalAmount;
    private int rows = 5;
    private int columns = 11;
    public Projectile missilePrefab;
    public float missileSpawnRate = 1.0f;

    public GameObject _invadersFactory;
    private ILevelFactory _factory;
    private bool isOpenAchievements = false;

    private void Awake()
    {
        _invadersFactory = LevelController.Level;
        _factory = _invadersFactory.GetComponent<ILevelFactory>();
        var factVar = _factory.GetInvaderView(PercentKilled, speed);
        Invader invader = factVar.Invader;
        this.rows = factVar.Rows;
        this.columns = factVar.Columns;

        this.initialPosition = this.transform.position;
        for (int i = 0; i < this.rows; i++)
        {
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2.0f * i) + centerOffset.y, 0.0f);

            for (int j = 0; j < this.columns; j++)
            {
                Invader copiedInvader = (Invader)invader.Clone();
                copiedInvader.killed += OnInvaderKilled;
                Vector3 position = rowPosition;
                position.x += 2.0f * j;
                copiedInvader.transform.parent = transform;
                copiedInvader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileSpawnRate, this.missileSpawnRate);
    }

    private void MissileAttack()
    {
        int amountAlive = this.AmountAlive;
        if (amountAlive == 0)
        {
            return;
        }

        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (Random.value < (1.0f / (float)amountAlive))
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void Update()
    {
        float speed = _factory.GetInvaderView(PercentKilled, this.speed).Speed;
        this.transform.position += this.direction * speed * Time.deltaTime;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (this.direction == Vector3.right && invader.position.x >= (rightEdge.x - 0.75f))
            {
                AdvanceRow();
                break;
            }
            else if (this.direction == Vector3.left && invader.position.x <= (leftEdge.x + 0.75f))
            {
                AdvanceRow();
                break;
            }
        }
        if(PercentKilled >= 0.3f && !isOpenAchievements)
        {
            isOpenAchievements = true;
            Notify();
        }
    }

    private void AdvanceRow()
    {
        this.direction = new Vector3(-this.direction.x, 0.0f, 0.0f);
        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);

        this.AmountKilled++;
        this.killed(invader);
    }

    protected override void SetKilled()
    {
        this.AmountKilled = 0;
    }
    protected override void ChangeDirection()
    {
        this.direction = Vector3.right;
    }
    protected override void SetPosition()
    {
        this.transform.position = this.initialPosition;
        foreach (Transform invader in this.transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

    public void Attach(IObserver observer)
    {
        this._observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observer.UndoUpdate();
        this._observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (IObserver observer in _observers)
        {
            observer.Update();
        }
    }
}
