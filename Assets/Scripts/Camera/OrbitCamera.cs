using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SkeletonOrbitCamera : MonoBehaviour
{
    [Header("Цель")]
    [SerializeField] private Transform skeletonCenter;

    [Header("Чувствительность")]
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float zoomSpeed = 5f;

    [Header("Ограничения")]
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    [Header("Фокус по курсору")]
    [SerializeField] private LayerMask focusLayers = ~0;
    [SerializeField] private float maxRayDistance = 100f;

    private Camera cam;

    // Текущее состояние орбиты
    private Vector3 focusPoint;
    private float yaw;
    private float pitch;
    private float currentDistance;

    // Сохранённое начальное состояние
    private Vector3 startFocusPoint;
    private float startYaw;
    private float startPitch;
    private float startDistance;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (skeletonCenter == null)
        {
            Debug.LogError("SkeletonOrbitCamera: skeletonCenter не задан");
            enabled = false;
            return;
        }

        // Инициализируем фокус в центр скелета
        focusPoint = skeletonCenter.position;

        // Вычисляем yaw/pitch/distance из текущего положения камеры в сцене
        Vector3 offset = transform.position - focusPoint;
        if (offset.sqrMagnitude < 0.0001f)
        {
            offset = new Vector3(0f, 0f, -5f);
        }

        currentDistance = offset.magnitude;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        yaw = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        pitch = Mathf.Asin(offset.y / Mathf.Max(currentDistance, 0.0001f)) * Mathf.Rad2Deg;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Сохраняем всё это как "старт"
        startFocusPoint = focusPoint;
        startYaw = yaw;
        startPitch = pitch;
        startDistance = currentDistance;

        UpdateCameraTransform();

        // Подписка на старт уровня
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelStart += HandleLevelStart;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelStart -= HandleLevelStart;
    }

    private void HandleLevelStart(BoneData _, int __)
    {
        ResetView();
    }

    private void LateUpdate()
    {
        HandleInput();
        UpdateCameraTransform();
    }

    private void HandleInput()
    {
        bool orbitHeld = Input.GetMouseButton(1); // ПКМ
        bool setFocusPressed = Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0); // Alt + ЛКМ

        if (orbitHeld)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            yaw += mx * rotationSpeed * Time.deltaTime;
            pitch -= my * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (setFocusPressed)
        {
            TrySetFocusToMouse();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            currentDistance -= scroll * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }
    }

    private void TrySetFocusToMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, focusLayers))
        {
            focusPoint = hit.point; // ноги, голова — куда навёл [web:231][web:131]
        }
    }

    private void UpdateCameraTransform()
    {
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 dir = rot * Vector3.forward;
        transform.position = focusPoint + dir * currentDistance;
        transform.LookAt(focusPoint);
    }

    public void ResetView()
    {
        // Жёстко возвращаем сохранённое стартовое состояние
        focusPoint = startFocusPoint;
        yaw = startYaw;
        pitch = startPitch;
        currentDistance = startDistance;
        UpdateCameraTransform();
    }
}
