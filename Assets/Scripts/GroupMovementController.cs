using UnityEngine;

public class GroupMovementController : MonoBehaviour
{
    public bool canMove = false;
    [SerializeField] private float interval = 5f;

    private void Start()
    {
        InvokeRepeating(nameof(ToggleCanMove), interval, interval);
    }

    void ToggleCanMove()
    {
        canMove = !canMove;
    }

}
