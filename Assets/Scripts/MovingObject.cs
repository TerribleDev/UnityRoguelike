using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{
    public float MoveTime = 0.1f;
    public LayerMask BlockingLayer;

    private BoxCollider2D BoxCollider;
    private Rigidbody2D Rb2d;
    private float InverseMoveTime;

    protected virtual void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        Rb2d = GetComponent<Rigidbody2D>();
        InverseMoveTime = 1f/MoveTime;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        var sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(Rb2d.position, end, InverseMoveTime*Time.deltaTime);
            Rb2d.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        BoxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, BlockingLayer);
        BoxCollider.enabled = true;
        if (hit.transform != null) return false;
        StartCoroutine(SmoothMovement(end));
        return true;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        if (hit.transform == null)
            return;

        var hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null) { OnCantMove(hitComponent);}
    }
    protected abstract void OnCantMove<T>(T component)
        where T : Component;

}
