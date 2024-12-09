using UnityEngine;

public abstract class NPC : MonoBehaviour, InteractableIF
{
    [SerializeField] private SpriteRenderer interactCheckSprite;
    protected PlayerController player;
    private const float INTERACT_DISTANCE = 2f;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        _Update();
        if (Input.GetKeyDown(KeyCode.E) && IsInteract())
            Interact();

        if (interactCheckSprite.gameObject.activeSelf && !IsInteract())
            interactCheckSprite.gameObject.SetActive(false);
        else if (!interactCheckSprite.gameObject.activeSelf && IsInteract())
            interactCheckSprite.gameObject.SetActive(true);
    }

    public abstract void Interact();
    protected abstract void _Update();
    protected bool IsInteract()
    {
        return (Vector2.Distance(player.transform.position, transform.position) < INTERACT_DISTANCE) ? true : false;
    }
}
