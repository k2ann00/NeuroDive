using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class IsometricCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Tooltip("Dönüş hızı (daha düşük = daha yavaş, daha yumuşak dönüş)")]
    [Range(1f, 20f)]
    public float rotationSmoothness = 10f;

    [Header("Input")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    [Header("Gravity Settings")]
    public float gravity = -9.81f;

    [Header("References")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private float verticalVelocity = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (cameraTransform == null) return;

        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0)
        {
            // Yere temas edince düşme hızını sıfırla (küçük negatif değer karakterin yere sıkışmasını önler)
            verticalVelocity = -2f;
        }

        float inputX = Input.GetAxisRaw(horizontalAxis);
        float inputZ = Input.GetAxisRaw(verticalAxis);
        Vector3 inputDir = new Vector3(inputX, 0, inputZ).normalized;

        Vector3 moveDir = Vector3.zero;

        if (inputDir.magnitude >= 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDir = camForward * inputZ + camRight * inputX;
            moveDir.Normalize();

            Vector3 horizontalMove = moveDir * moveSpeed;
            horizontalMove.y = verticalVelocity;

            controller.Move(horizontalMove * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);

            animator.SetFloat("speed", horizontalMove.magnitude);
        }
        else
        {
            // Yalnızca yerçekimi uygula
            Vector3 gravityMove = new Vector3(0, verticalVelocity, 0);
            controller.Move(gravityMove * Time.deltaTime);

            animator.SetFloat("speed", 0f);
        }

        // Yerçekimini uygula
        verticalVelocity += gravity * Time.deltaTime;
    }
}
