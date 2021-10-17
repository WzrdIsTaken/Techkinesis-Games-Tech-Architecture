using UnityEngine;

// Base class for anything that can move

public class MovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField, RequireInterface(typeof(IInputProvider))] ScriptableObject inputProvider;
    protected IInputProvider InputProvider => inputProvider as IInputProvider;

    [SerializeField] protected Optional<Camera> cam;

    [Header("Attributes")]
    [SerializeField] protected float mass = -12f;
    [SerializeField, Range(0, 1)] protected float airControlPercent = 0.5f;  // 1 = more control, 0 = less control

    [Header("Movement")]
    [SerializeField] float turnSmoothTime = 0.2f;

    protected InputState inputState;
    protected CharacterController controller;
    protected Animator animator;

    protected Vector3 velocity;
    protected float velocityY;

    float turnSmoothVelocity;
    
    public virtual void Start()
    {
        InputProvider.OnJump += Jump;
        InputProvider.OnMindControlStart += MindControlStart;
        InputProvider.OnMindControlEnd += MindControlEnd;

        controller = GetComponent<CharacterController>();
        TryGetComponent(out animator);
    }

    public virtual void Update()
    {
        inputState = InputProvider.GetState();

        // Rotation 
        if (inputState.movementDirection != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputState.movementDirection.x, inputState.movementDirection.y) * Mathf.Rad2Deg;
            targetRotation += cam.Enabled ? cam.Value.transform.eulerAngles.y : 0;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        // Gravity
        velocityY += mass * Time.deltaTime;
        velocity = Vector3.up * velocityY;

        // if (controller.isGrounded) velocityY = 0f; Interferes with jumping, ngl idk if we even want this because of the levitation mechanic. Will have to think of another solution anyway..

        // Note to self: Using character controller may pose issues. Not 100% sure yet, but you might have to change it.
    }

    public virtual void Jump()
    {
    }

    public virtual void MindControlStart()
    {
    }

    public virtual void MindControlEnd()
    {
    }

    protected float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded) return smoothTime;
        if (airControlPercent == 0) return float.MaxValue;

        return smoothTime / airControlPercent;
    }
}