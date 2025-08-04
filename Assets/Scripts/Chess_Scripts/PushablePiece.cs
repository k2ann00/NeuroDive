using UnityEngine;
using System.Collections;

public class PushablePiece : MonoBehaviour
{
    [Header("Süre Ayarları")]
    public float swordSpawnDelay;
    public float swordHitDelay;

    public FloatingChessPieces floatingPiecesController;
    public string allowedTag = "Player";
    [Tooltip("Hareket yönü (örnek: sadece sağa için 1,0,0)")]
    public Vector3 allowedDirection = Vector3.right;
    public Transform targetZone;
    public float moveSpeed = 2f;

    [Header("Karakter Animator Referansı")]
    public Animator playerAnimator;  // Inspector'dan atayacağın Animator

    private bool isBeingPushed = false;
    private Transform player;
    private Vector3 startPosition;

    [Header("Kılıç Pozisyon ve Rotasyon Ayarları")]
    public Vector3 swordSpawnPosition;
    public Vector3 swordSpawnRotation;

    public Vector3 swordHitPosition;
    public Vector3 swordHitRotation;

    [Header("Kılıç Aksiyon Ayarları")]
    public GameObject swordPrefab;
    public ParticleSystem hitVFX;

    public DoubleDoorOpener doorOpener;
    
    private GameObject pressEText;

    private void Start()
    {
        startPosition = transform.position;
        
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform pressETransform = canvas.transform.Find("Press E (TMP)");
            if (pressETransform != null)
            {
                pressEText = pressETransform.gameObject;
                pressEText.SetActive(false); // Hide at start
            }
            else
            {
                Debug.LogWarning("'Press E (TMP)' not found under Canvas.");
            }
        }
        else
        {
            Debug.LogWarning("'Canvas' not found in the scene.");
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(allowedTag))
        {
            player = other.transform;
            
            if (pressEText != null)
            {
                pressEText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(allowedTag))
        {
            if (isBeingPushed) StopPushing();
            player = null;
            
            if (pressEText != null)
            {
                pressEText.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isBeingPushed)
                StartPushing();
            else
                StopPushing();
        }

        if (isBeingPushed)
        {
            // Kamera yönüne göre world-space input vektörü
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            // Y ekseni etkisini temizle
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Girişleri al
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 desiredDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

            // Kameraya göre girilen yönü allowedDirection doğrultusuna projekte et
            Vector3 move = Vector3.Project(desiredDirection, allowedDirection.normalized);

            // Hareket
            transform.position += move * moveSpeed * Time.deltaTime;

            // Karakteri taşıma yönünün tersine sabitle
            player.position = transform.position - allowedDirection.normalized;
        }
    }

    private void StartPushing()
    {
        isBeingPushed = true;
        // Debug.Log("Taş itilmeye başlandı.");

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isConnected", true);
        }
    }

    private void StopPushing()
    {
        isBeingPushed = false;
        // Debug.Log("Taş bırakıldı.");

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isConnected", false);
        }

        if (targetZone != null && IsInsideTargetZone())
        {
            FinalAction();
        }
        else
        {
            ResetPosition();
        }
    }

    private bool IsInsideTargetZone()
    {
        if (targetZone == null) return false;

        Collider targetCollider = targetZone.GetComponent<Collider>();
        if (targetCollider == null) return false;

        return targetCollider.bounds.Contains(transform.position);
    }

    private void FinalAction()
    {
        // Debug.Log("Taş doğru alana yerleştirildi! Final Action tetiklendi.");

        Vector3 targetCenter = targetZone.GetComponent<Collider>().bounds.center;
        Vector3 newPosition = new Vector3(targetCenter.x, transform.position.y, targetCenter.z);
        transform.position = newPosition;

        StartCoroutine(SwordSpawnSequence());
    }

    private IEnumerator SwordSpawnSequence()
    {
        yield return new WaitForSeconds(swordSpawnDelay);

        if (swordPrefab != null)
        {
            Vector3 spawnPos = swordSpawnPosition;
            Quaternion spawnRot = Quaternion.Euler(swordSpawnRotation);

            GameObject sword = Instantiate(swordPrefab, spawnPos, spawnRot);

            yield return new WaitForSeconds(swordHitDelay);

            Vector3 targetPos = swordHitPosition;
            Quaternion targetRot = Quaternion.Euler(swordHitRotation);

            float duration = 0.15f;
            float elapsed = 0f;
            Vector3 startPos = sword.transform.position;

            while (elapsed < duration)
            {
                sword.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            sword.transform.position = targetPos;
            sword.transform.rotation = targetRot;

            if (floatingPiecesController != null)
            {
                floatingPiecesController.BeginFloating();
            }

            PlayHitEffect();
            CameraShake();
            doorOpener.OpenDoor();
        }
    }

    public void PlayHitEffect()
    {
        hitVFX.Play();
    }

    private void CameraShake()
    {
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            StartCoroutine(DoCameraShake(mainCam.transform, 0.1f, 0.2f));
        }
    }

    private IEnumerator DoCameraShake(Transform cam, float duration, float magnitude)
    {
        Vector3 originalPos = cam.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cam.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.localPosition = originalPos;
    }

    private void ResetPosition()
    {
        // Debug.Log("Taş yanlış pozisyona bırakıldı başlangıç konumuna dönülüyor.");
        transform.position = startPosition;
    }
}
