using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class MouseClickController : MonoBehaviour
{
    public Vector3 clickPosition;
    public Camera cam;
    public Ray mouseRay;
    private float rayLength;

    public UnityEvent<Vector3> OnMouseClick;

    void Update()
    {
        // Get the mouse click position in world space 
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast from camera to mouse click position
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo))
            {
                Vector3 clickWorldPosition = hitInfo.point;
                Debug.Log(clickWorldPosition);

                // Store the click position here
                rayLength = hitInfo.distance;
                clickPosition = clickWorldPosition;


                // Trigger an unity event to notify other scripts about the click here
                OnMouseClick.Invoke(clickWorldPosition);
            }
        }

        // Visual debugging here
        Debug.DrawRay(mouseRay.origin, mouseRay.direction * rayLength, Color.cyan);
        DebugExtension.DebugWireSphere(clickPosition);
    }

}
