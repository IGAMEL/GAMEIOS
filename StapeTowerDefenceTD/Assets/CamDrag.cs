using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class CameraDrag : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    private float dragSpeed = 15f;
    [SerializeField] private float inertiaDecay = 3f; // Замедление инерции
    [SerializeField] private float maxInertiaSpeed = 10f; // Макс. скорость инерции

    private Camera cam;
    private Bounds tilemapBounds;
    private Vector3 dragOrigin;
    private Vector3 inertia; // Вектор инерции
    private bool isDragging;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        CalculateTilemapBounds();
        CenterCameraIfTooBig();
    }

    private void Update()
    {
        HandleTouchInput();
        ApplyInertia();
        ClampCamera();
    }

    private void HandleTouchInput()
    {
        // Работает и на ПК (мышь), и на мобильных (тач)
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            ContinueDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }

        // Для мультитача (если нужно)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                StartDrag(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                ContinueDrag(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                EndDrag();
            }
        }
    }

    private void StartDrag(Vector2 inputPosition)
    {
        dragOrigin = cam.ScreenToWorldPoint(inputPosition);
        isDragging = true;
        inertia = Vector3.zero; // Сброс инерции при новом касании
    }

    private void ContinueDrag(Vector2 inputPosition)
    {
        if (!isDragging) return;

        Vector3 currentPos = cam.ScreenToWorldPoint(inputPosition);
        Vector3 difference = dragOrigin - currentPos;
        difference.z = 0; // Игнорируем Z

        // Запоминаем разницу для инерции
        inertia = difference * dragSpeed;
        inertia = Vector3.ClampMagnitude(inertia, maxInertiaSpeed);

        // Плавное перемещение
        transform.position += difference * dragSpeed * Time.deltaTime;
        dragOrigin = currentPos;
    }

    private void EndDrag()
    {
        isDragging = false;
    }

    private void ApplyInertia()
    {
        if (isDragging) return; // Инерция работает только после отпускания

        if (inertia.magnitude > 0.01f)
        {
            transform.position += inertia * Time.deltaTime;
            inertia = Vector3.Lerp(inertia, Vector3.zero, inertiaDecay * Time.deltaTime);
        }
        else
        {
            inertia = Vector3.zero;
        }
    }

    private void ClampCamera()
    {
        if (tilemap == null) return;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        bool isCameraWider = (camWidth * 2 >= tilemapBounds.size.x);
        bool isCameraTaller = (camHeight * 2 >= tilemapBounds.size.y);

        float clampedX = isCameraWider ? tilemapBounds.center.x : Mathf.Clamp(
            transform.position.x,
            tilemapBounds.min.x + camWidth,
            tilemapBounds.max.x - camWidth
        );

        float clampedY = isCameraTaller ? tilemapBounds.center.y : Mathf.Clamp(
            transform.position.y,
            tilemapBounds.min.y + camHeight,
            tilemapBounds.max.y - camHeight
        );

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void CalculateTilemapBounds()
    {
        tilemapBounds = new Bounds(tilemap.transform.position, new Vector2(22, 11));
    }

    private void CenterCameraIfTooBig()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        if (camWidth * 2 >= tilemapBounds.size.x || camHeight * 2 >= tilemapBounds.size.y)
        {
            transform.position = new Vector3(
                tilemapBounds.center.x,
                tilemapBounds.center.y,
                transform.position.z
            );
        }
    }
}