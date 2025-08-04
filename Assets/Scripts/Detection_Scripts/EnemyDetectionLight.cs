using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EnemyDetectionLight : MonoBehaviour
{
    [Header("Zones (three concentric triggers)")]
    public SphereCollider zone1; // bigger
    public SphereCollider zone2; // middle
    public SphereCollider zone3; // smallest (closest to enemy)

    [Header("Visual Rings")]
    public GameObject light1Obj;      // Zone1 only
    public GameObject light2Obj;      // Zone2
    public GameObject light3Obj;      // Zone3 (new)
    [Tooltip("Renderer whose material has the opacity property (usually on Light2 object).")]
    public Renderer light2Renderer;
    [Tooltip("Renderer of Light3 for blinking effect.")]
    public Renderer light3Renderer;

    [Header("Opacity Control (Light2)")]
    public string opacityProperty = "_Opacity";
    public float minOpacity = 0.2f;
    public float maxOpacity = 1.0f;
    [Tooltip("Seconds to smooth opacity changes.")]
    public float opacitySmoothTime = 0.15f;

    [Header("Light3 Blink Settings")]
    public float light3BlinkMin = 0.2f;
    public float light3BlinkMax = 0.7f;
    public float light3BlinkSpeed = 2f; // cycles per second

    [Header("Zone3 Hold Settings")]
    [Tooltip("Seconds the player must stay in Zone3 to trigger FinalAction().")]
    public float zone3HoldSeconds = 3f;
    public TMP_Text countdownText; // shows remaining time while in Zone3

    [Header("Zone2 Interaction Hold")]
    [Tooltip("E tuşunu bu kadar süre basılı tutarsa Interaction() tetiklenir.")]
    public float interactionHoldSeconds = 2f;

    [Header("Zone1 Interaction Hold")]
    [Tooltip("Zone1'de E tuşunu bu kadar süre basılı tutarsa Interaction() tetiklenir.")]
    public float zone1InteractionHoldSeconds = 5f;

    [Header("UI Signal Indicators")]
    public GameObject noSignal;
    public GameObject lowSignal;
    public GameObject goodSignal;
    public GameObject alertSignal;

    [Header("Player")]
    public string playerTag = "Player";
    public Transform player; // optional; auto-found if null

    [Header("Fail Aksiyonunda Tetiklenecek Elemanlar")]
    public GameObject panelToActivate;
    public GameObject tmpTextToActivate;

    [Header("UI Canvas ve Matrix Paneli")]
    // public GameObject canvas;
    public GameObject matrixPanel;
    
    float _currentOpacity;
    float _targetOpacity;
    float _opacityVel;
    MaterialPropertyBlock _mpb2;
    MaterialPropertyBlock _mpb3;

    float _zone3Timer;
    bool _finalInvoked;
    bool _isLight3Active;

    float _interactionHoldTimer;
    bool _interactionTriggeredThisHold;

    float _zone1HoldTimer;
    bool _zone1InteractionTriggered;

    void Start()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag(playerTag);
            if (p != null) player = p.transform;
        }

        if (light2Renderer == null && light2Obj != null)
            light2Renderer = light2Obj.GetComponentInChildren<Renderer>();

        if (light3Renderer == null && light3Obj != null)
            light3Renderer = light3Obj.GetComponentInChildren<Renderer>();

        _mpb2 = new MaterialPropertyBlock();
        _mpb3 = new MaterialPropertyBlock();

        SetActiveSafe(light1Obj, false);
        SetActiveSafe(light2Obj, false);
        SetActiveSafe(light3Obj, false);
        SetTMPActive(false);
        ApplyOpacityLight2(minOpacity);

        _zone3Timer = zone3HoldSeconds;
        _isLight3Active = false;
        ResetInteractionHold();
        ResetZone1Hold();

        ValidateWarnings();
    }

    void Update()
    {
        if (player == null || zone1 == null || zone2 == null)
            return;

        float dist = Vector3.Distance(GetZoneCenter(zone1), player.position);
        float r1 = zone1.radius * GetLargestAxisScale(zone1.transform.lossyScale);
        float r2 = zone2.radius * GetLargestAxisScale(zone2.transform.lossyScale);

        bool inZone1 = dist <= r1;
        bool inZone2 = dist <= r2;

        float r3 = 0f;
        bool inZone3 = false;
        if (zone3 != null)
        {
            r3 = zone3.radius * GetLargestAxisScale(zone3.transform.lossyScale);
            inZone3 = dist <= r3;
        }

        if (!inZone1)
        {
            SetActiveSafe(light1Obj, false);
            SetActiveSafe(light2Obj, false);
            SetActiveSafe(light3Obj, false);
            _isLight3Active = false;
            SetTMPActive(false);
            _targetOpacity = minOpacity;
            ResetZone3State();
            ResetInteractionHold();
            ResetZone1Hold();

            SetActiveSafe(noSignal, true);
            SetActiveSafe(lowSignal, false);
            SetActiveSafe(goodSignal, false);
            SetActiveSafe(alertSignal, false);
        }
        else if (inZone3)
        {
            SetActiveSafe(light1Obj, false);
            SetActiveSafe(light2Obj, false);
            SetActiveSafe(light3Obj, true);
            _isLight3Active = true;
            _targetOpacity = minOpacity;
            HandleZone3Timer();
            ResetInteractionHold();
            ResetZone1Hold();

            SetActiveSafe(noSignal, false);
            SetActiveSafe(lowSignal, false);
            SetActiveSafe(goodSignal, false);
            SetActiveSafe(alertSignal, true);
        }
        else if (inZone2)
        {
            SetActiveSafe(light1Obj, false);
            SetActiveSafe(light2Obj, true);
            SetActiveSafe(light3Obj, false);
            _isLight3Active = false;
            SetTMPActive(false);
            ResetZone3State();

            float t = 1f - Mathf.InverseLerp(0f, r2, dist);
            _targetOpacity = Mathf.Lerp(minOpacity, maxOpacity, t);

            if (!_interactionTriggeredThisHold)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    _interactionHoldTimer += Time.deltaTime;
                    if (_interactionHoldTimer >= interactionHoldSeconds)
                    {
                        _interactionTriggeredThisHold = true;
                        _interactionHoldTimer = 0f;
                        Interaction();
                    }
                }

                if (Input.GetKeyUp(KeyCode.E))
                    ResetInteractionHold();
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.E))
                    ResetInteractionHold();
            }

            ResetZone1Hold();

            SetActiveSafe(noSignal, false);
            SetActiveSafe(lowSignal, false);
            SetActiveSafe(goodSignal, true);
            SetActiveSafe(alertSignal, false);
        }
        else
        {
            SetActiveSafe(light1Obj, true);
            SetActiveSafe(light2Obj, false);
            SetActiveSafe(light3Obj, false);
            _isLight3Active = false;
            SetTMPActive(false);
            _targetOpacity = minOpacity;
            ResetZone3State();
            ResetInteractionHold();

            if (!_zone1InteractionTriggered)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    _zone1HoldTimer += Time.deltaTime;
                    if (_zone1HoldTimer >= zone1InteractionHoldSeconds)
                    {
                        _zone1InteractionTriggered = true;
                        _zone1HoldTimer = 0f;
                        Interaction();
                        
                    }
                }

                if (Input.GetKeyUp(KeyCode.E))
                    ResetZone1Hold();
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.E))
                    ResetZone1Hold();
            }

            SetActiveSafe(noSignal, false);
            SetActiveSafe(lowSignal, true);
            SetActiveSafe(goodSignal, false);
            SetActiveSafe(alertSignal, false);
        }

        _currentOpacity = Mathf.SmoothDamp(_currentOpacity, _targetOpacity, ref _opacityVel, opacitySmoothTime);
        ApplyOpacityLight2(_currentOpacity);

        if (_isLight3Active && light3Renderer != null)
        {
            float blinkT = Mathf.PingPong(Time.time * light3BlinkSpeed, 1f);
            float blinkValue = Mathf.Lerp(light3BlinkMin, light3BlinkMax, blinkT);
            ApplyOpacityLight3(blinkValue);
        }
    }

    void HandleZone3Timer()
    {
        if (_finalInvoked)
        {
            UpdateCountdownTMP(0f);
            return;
        }

        _zone3Timer -= Time.deltaTime;
        UpdateCountdownTMP(Mathf.Max(_zone3Timer, 0f));

        if (_zone3Timer <= 0f)
        {
            _finalInvoked = true;
            FinalAction();
        }
    }

    void ResetZone3State()
    {
        _zone3Timer = zone3HoldSeconds;
        _finalInvoked = false;
    }

    void UpdateCountdownTMP(float timeLeft)
    {
        if (countdownText == null) return;
        if (!countdownText.gameObject.activeSelf)
            countdownText.gameObject.SetActive(true);

        countdownText.text = timeLeft.ToString("0.00");
    }

    void SetTMPActive(bool state)
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(state);
    }

    void ResetInteractionHold()
    {
        _interactionHoldTimer = 0f;
        _interactionTriggeredThisHold = false;
    }

    void ResetZone1Hold()
    {
        _zone1HoldTimer = 0f;
        _zone1InteractionTriggered = false;
    }

    void ApplyOpacityLight2(float value)
    {
        if (light2Renderer == null) return;
        light2Renderer.GetPropertyBlock(_mpb2);
        _mpb2.SetFloat(opacityProperty, value);
        light2Renderer.SetPropertyBlock(_mpb2);
    }

    void ApplyOpacityLight3(float value)
    {
        if (light3Renderer == null) return;
        light3Renderer.GetPropertyBlock(_mpb3);
        _mpb3.SetFloat(opacityProperty, value);
        light3Renderer.SetPropertyBlock(_mpb3);
    }

    static float GetLargestAxisScale(Vector3 lossyScale)
    {
        return Mathf.Max(lossyScale.x, Mathf.Max(lossyScale.y, lossyScale.z));
    }

    static Vector3 GetZoneCenter(SphereCollider sphere)
    {
        return sphere.transform.TransformPoint(sphere.center);
    }

    static void SetActiveSafe(GameObject go, bool state)
    {
        if (go != null && go.activeSelf != state)
            go.SetActive(state);
    }

    void ValidateWarnings()
    {
        if (zone1 != null && !zone1.isTrigger)
            Debug.LogWarning($"{name}: zone1 should be isTrigger=true.");
        if (zone2 != null && !zone2.isTrigger)
            Debug.LogWarning($"{name}: zone2 should be isTrigger=true.");
        if (zone3 != null && !zone3.isTrigger)
            Debug.LogWarning($"{name}: zone3 should be isTrigger=true.");
        if (light2Renderer == null)
            Debug.LogWarning($"{name}: light2Renderer is null. Assign it to drive opacity.");
        if (light3Renderer == null)
            Debug.LogWarning($"{name}: light3Renderer is null. Assign it to blink opacity.");
        if (countdownText == null)
            Debug.LogWarning($"{name}: countdownText is null. Assign a TMP_Text to show the Zone3 timer.");
    }

    void OnDrawGizmosSelected()
    {
        if (zone1 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetZoneCenter(zone1),
                zone1.radius * GetLargestAxisScale(zone1.transform.lossyScale));
        }

        if (zone2 != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetZoneCenter(zone2),
                zone2.radius * GetLargestAxisScale(zone2.transform.lossyScale));
        }

        if (zone3 != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(GetZoneCenter(zone3),
                zone3.radius * GetLargestAxisScale(zone3.transform.lossyScale));
        }
    }

    void FinalAction()
    {
        // Debug.Log($"{name}: FinalAction triggered!");
        if (panelToActivate != null)
            panelToActivate.SetActive(true);

        if (tmpTextToActivate != null)
            tmpTextToActivate.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }

    void Interaction()
    {
        // Debug.Log($"{name}: Interaction triggered!");
        StartCoroutine(LoadSceneWithPanelDelay());
        
    }
    
    IEnumerator LoadSceneWithPanelDelay()
    {
        if (matrixPanel != null)
        {
            //DontDestroyOnLoad(canvas); // Sahne geçerken silinmesin
            // DontDestroyOnLoad(matrixPanel); // Sahne geçerken silinmesin
            matrixPanel.SetActive(true);    // Paneli göster
        }
        
        // Asenkron sahne yüklemesi
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Chess");
        asyncLoad.allowSceneActivation = false; // Sahne hemen aktif olmasın

        // Panelin gösterimde kalma süresi
        float displayDuration = 7f; // saniye
        float timer = 0f;

        while (timer < displayDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        asyncLoad.allowSceneActivation = true; // Artık sahneye geç
    }

}
