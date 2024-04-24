using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteraction;

    public void Interact()
    {
        OnInteraction.Invoke();
    }
}
