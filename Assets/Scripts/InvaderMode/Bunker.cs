using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Bunker : MonoBehaviour
{
    // Constants
    private const string INVADER_LAYER_NAME = "Invader";
    
    // Variables
    public Texture2D splat;
    public Texture2D originalTexture { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public new BoxCollider2D collider { get; private set; }

    private void Awake()
    {
        try
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<BoxCollider2D>();
            originalTexture = spriteRenderer.sprite.texture;

            ResetBunker();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in Awake(): {ex.Message}");
        }
    }

    // bunkers will be set to original look once round ends
    public void ResetBunker()
    {
        CopyTexture(originalTexture);

        gameObject.SetActive(true);
    }

    // Designing how bunker will look 
    private void CopyTexture(Texture2D source)
    {
        Texture2D copy = new Texture2D(source.width, source.height, source.format, false);
        copy.anisoLevel = source.anisoLevel;
        copy.filterMode = source.filterMode;
        copy.SetPixels(source.GetPixels());
        copy.wrapMode = source.wrapMode;
        copy.Apply();

        Sprite sprite = Sprite.Create(copy, spriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), spriteRenderer.sprite.pixelsPerUnit);
        spriteRenderer.sprite = sprite;
    }

    // Checking whether the colliding objects meets bunker
    public bool CheckCollision(BoxCollider2D other, Vector3 hitPoint)
    {
        Vector2 offset = other.size / 2;

        return Splat(hitPoint) ||
               Splat(hitPoint + (Vector3.down * offset.y)) ||
               Splat(hitPoint + (Vector3.up * offset.y)) ||
               Splat(hitPoint + (Vector3.left * offset.x)) ||
               Splat(hitPoint + (Vector3.right * offset.x));
    }

    // Creating the splat effect of the bunker once an object collides with it
    private bool Splat(Vector3 hitPoint)
    {
        int px;
        int py;

        if (!CheckPoint(hitPoint, out px, out py)) 
        {
            return false;
        }

        Texture2D texture = spriteRenderer.sprite.texture;

        px -= splat.width / 2;
        py -= splat.height / 2;

        int startX = px;

        for (int y = 0; y < splat.height; y++)
        {
            px = startX;

            for (int x = 0; x < splat.width; x++)
            {
                Color pixel = texture.GetPixel(px, py);
                pixel.a *= splat.GetPixel(x, y).a;
                texture.SetPixel(px, py, pixel);
                px++;
            }

            py++;
        }
        texture.Apply();

        return true;
    }

    // Checking whether a point is a non-empty pixel
    private bool CheckPoint(Vector3 hitPoint, out int px, out int py)
    {
        Vector3 localPoint = transform.InverseTransformPoint(hitPoint);

        localPoint.x += collider.size.x / 2;
        localPoint.y += collider.size.y / 2;

        Texture2D texture = spriteRenderer.sprite.texture;

        px = (int)((localPoint.x / collider.size.x) * texture.width);
        py = (int)((localPoint.y / collider.size.y) * texture.height);

        return texture.GetPixel(px, py).a != 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(INVADER_LAYER_NAME))
            {
                gameObject.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in OnTriggerEnter2D(): {ex.Message}");
        }
    }

}
