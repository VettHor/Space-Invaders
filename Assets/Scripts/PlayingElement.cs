using UnityEngine;

public abstract class PlayingElement : MonoBehaviour
{
    public void ResetAll()
    {
        SetKilled();
        ChangeDirection();
        SetPosition();
        Despawn();
    }
    protected virtual void SetKilled() { }
    protected abstract void ChangeDirection();
    protected abstract void SetPosition();
    protected virtual void Despawn() { }

}