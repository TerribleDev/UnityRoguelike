using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {



    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }

    public void MoveEnemy()
    {
        var xDir = 0;
        var yDir = 0;
        //in the same column?
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;

        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        var hitPlayer = component as Player;
        animator.SetTrigger("EnemyAttack");
        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
        hitPlayer.LoseFood(playerDamage);
    }

   
}
