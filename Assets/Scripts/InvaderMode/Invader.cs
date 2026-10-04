using UnityEditor.Timeline.Actions;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Invader : MonoBehaviour
{
    // Variables
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] animationSprites = new Sprite[0];
    public float animationTime = 1f;
    public int animationFrame { get; private set; }
    public int score = 10;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animationSprites[0];
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    // Creating visuals for the invaders 
    private void AnimateSprite()
    {
        animationFrame++;

        if (animationFrame >= animationSprites.Length) 
        {
            animationFrame = 0;
        }

        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
    {
        if (GameManager.Instance != null)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
            {
                if (GameManager.Instance.scoreText != null)
                {
                    GameManager.Instance.OnInvaderKilled(this);
                }
                else
                {
                    Debug.LogError("scoreText in GameManager is null.");
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
            {
                GameManager.Instance.OnBoundaryReached();
            }
        }
        else
        {
            Debug.LogError("GameManager instance is null.");
        }
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Error in OnTriggerEnter2D(): {ex.Message}");
    }
    }

}
