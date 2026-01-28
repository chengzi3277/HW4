using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public delegate void BirdEvent();
    public event BirdEvent OnDeath; 

    [Header("Movement")]
    public float flapForce = 6f;

    [Header("Audio")]
    public AudioSource flapSound;
    public AudioSource hitSound;

    private Rigidbody2D rb;
    private bool isAlive = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isAlive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flap();
        }
    }

    void Flap()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);

        if (flapSound != null)
            flapSound.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) return;

        if (collision.gameObject.CompareTag("Pipe"))
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAlive) return;

        if (other.CompareTag("ScoreZone"))
        {
            OnScore?.Invoke();
        }
    }

    void Die()
    {
        isAlive = false;

        if (hitSound != null)
            hitSound.Play();

        rb.velocity = Vector2.zero;
        Time.timeScale = 0f; // stop the game

        OnDeath?.Invoke();
    }
}
