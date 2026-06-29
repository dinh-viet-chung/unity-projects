using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Tooltip("Kéo thả GameObject cánh cửa (Door) vào đây")]
    public Transform doorVisual;

    [Tooltip("Chiều cao cửa sẽ nhấc lên khi mở")]
    public float openHeight = 3.5f;

    [Tooltip("Tốc độ mở cửa")]
    public float openSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 targetPosition;
    private bool isPlayerNear = false;

    void Start()
    {
        if (doorVisual != null)
        {
            // Lưu lại vị trí ban đầu của cửa khi đang đóng
            closedPosition = doorVisual.position;
            // Tính toán vị trí khi cửa mở (nhấc lên theo trục Y)
            targetPosition = closedPosition + new Vector3(0, openHeight, 0);
        }
    }

    void Update()
    {
        if (doorVisual == null) return;

        // Nếu người chơi ở gần thì di chuyển cửa về vị trí mở, ngược lại thì về vị trí đóng
        Vector3 currentTarget = isPlayerNear ? targetPosition : closedPosition;
        doorVisual.position = Vector3.MoveTowards(doorVisual.position, currentTarget, openSpeed * Time.deltaTime);
    }

    // Khi có vật thể bước vào vùng cảm ứng của cửa
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "Player")
        {
            isPlayerNear = true;
        }
    }

    // Khi vật thể rời khỏi vùng cảm ứng
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "Player")
        {
            isPlayerNear = false;
        }
    }
}