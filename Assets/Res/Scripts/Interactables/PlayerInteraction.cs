using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    #region Variables

    public float playerReach = 3f;
    Interactable currentInteractable;

    #endregion

    #region Methods

    #region Unity Callbacks

    void Update()
    {
        CheckInteraction();
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    #endregion

    #region Custom Methods

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if(Physics.Raycast(ray, out hit,playerReach))
        {
            if(hit.collider.tag == "Interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();


                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    private void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        EventManager.Broadcast(GameEvent.OnEnableItemInteraction,-1);
    }
    void DisableCurrentInteractable()
    {
        EventManager.Broadcast(GameEvent.OnDisableItemInteraction, -1);
        if (currentInteractable)
        {
            currentInteractable = null;
        }
    }

    #endregion

    #endregion
}
