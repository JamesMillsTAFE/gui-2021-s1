using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonController
{
    public class CameraMotor : MonoBehaviour
    {
        [SerializeField, Range(0, 3)] private float sensitivity = .5f;
        [SerializeField] private Vector2 verticalLookBounds = new Vector2(-90f, 45f);
        [SerializeField] private InputActionReference look;
        [SerializeField] private Transform player;

        [Header("Collisions")]
        [SerializeField] private new Transform camera;
        // The distance from the player the camera will sit
        [SerializeField, Min(.1f)] private float cameraDistance = 3f;
        // How smooth the camera will be
        [SerializeField, Range(.05f, .2f)] private float damping = .1f;
        // The radius that the camera has a collider of and offsets it's position from hit things
        [SerializeField, Range(.1f, .5f)] private float colliderRadius = .25f;
        
        private Vector2 rotation = Vector2.zero;
        private Vector3 velocity = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
            look.action.performed += OnLookPerformed;

            // Start the camera at the maximum distance from the player
            camera.localPosition = new Vector3(0, 0, -cameraDistance);
        }

        // Update is called once per frame
        void Update()
        {
            transform.localRotation = Quaternion.AngleAxis(rotation.y, Vector3.left);
            player.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);

            // Calculate the smoothed out position based on the environment around
            // the camera
            Vector3 newPos = Vector3.SmoothDamp(
                camera.position, CalculatePosition(),
                ref velocity, damping);

            camera.position = newPos;
        }

        private Vector3 CalculatePosition()
        {
            // Calculate the default position of the camera using the target
            Vector3 newPos = transform.position - transform.forward * cameraDistance;

            // Use the inverse of the forward for the direction
            Vector3 direction = -transform.forward;

            if(Physics.Raycast(transform.position, direction, out RaycastHit hit, cameraDistance))
            {
                // Set the newPosition to slightly offset from the hit collider
                newPos = hit.point + transform.forward * colliderRadius;
            }

            return newPos;
        }

        private void OnLookPerformed(InputAction.CallbackContext _context)
        {
            // The actual value of the input (thumbstick pos/mouse delta)
            Vector2 value = _context.ReadValue<Vector2>();

            // Add the input values x sensitivity to the rotation and clamp the y rotation
            rotation += value * sensitivity;
            rotation.y = Mathf.Clamp(rotation.y, verticalLookBounds.x, verticalLookBounds.y);
        }

        // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position - transform.forward * cameraDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position - transform.forward * cameraDistance, colliderRadius);
        }
    }
}