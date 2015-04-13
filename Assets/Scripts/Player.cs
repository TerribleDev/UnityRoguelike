using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;
    public Text foodText;
    private Animator _animator;
    private int _food;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;
	// Use this for initialization
	protected override void Start ()
	{
	    foodText.text = "Food: " + _food;
	    _animator = GetComponent<Animator>();
	    _food = GameManager.Instance.PlayerFoodPoints;
        base.Start();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (string.Equals(other.tag, "Exit", StringComparison.CurrentCultureIgnoreCase))
        {
            Invoke("Restart", RestartLevelDelay);
            enabled = false;
        }
        else if (string.Equals(other.tag, "Food", StringComparison.CurrentCultureIgnoreCase))
        {
            _food += PointsPerFood;
            foodText.text = "+" + PointsPerFood + " Food: " + _food;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if (string.Equals(other.tag, "Soda", StringComparison.CurrentCultureIgnoreCase))
        {
            foodText.text = "+" + PointsPerFood + " Food: " + _food;
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            _food += PointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //If it's not the player's turn, exit the function.
        if (!GameManager.Instance.PlayersTurn) return;

        int horizontal = 0;  	//Used to store the horizontal move direction.
        int vertical = 0;		//Used to store the vertical move direction.

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Check if moving horizontally, if so set vertical to zero.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove<Wall>(horizontal, vertical);
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.PlayerFoodPoints = _food;
    }

    private void CheckIfGameOver()
    {
        if (_food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.Instance.GameOver();
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        _food--;
        foodText.text = "Food: " + _food;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        if (Move(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }
        CheckIfGameOver();
        GameManager.Instance.PlayersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        var hitWall = component as Wall;
        hitWall.DamageWall(WallDamage);
        _animator.SetTrigger("PlayerChop");
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood(int loss)
    {
        _animator.SetTrigger("playerhit");
        _food -= loss;
        foodText.text = "-" + loss + " Food: " + _food;
        CheckIfGameOver();
    }
}
