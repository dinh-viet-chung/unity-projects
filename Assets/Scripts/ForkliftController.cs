using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class ForkliftController : MonoBehaviour
{
    [Header("Forklift Movement")]
    public float speed = 8.0f; // Xe nâng chạy nhanh hơn chạy bộ một chút
    public float rotationSpeed = 150.0f;
    private Rigidbody rb;

    [Header("Gameplay System")]
    public float timeRemaining = 60.0f; // 60 giây ca làm việc
    public TMP_Text gameplayUIText; // Kéo thả Text hiển thị TIME và MONEY vào đây

    private int sessionMoney = 0; // Tiền kiếm được trong ca này
    private bool isGameActive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogWarning("Forklift_Player cần có Rigidbody!");

        UpdateGameplayUI();
    }

    void FixedUpdate()
    {
        if (!isGameActive) return;
        if (Keyboard.current == null) return;

        // Lái xe nâng bằng phím WASD / Mũi tên
        Vector2 moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y = -1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x = 1f;

        // Tiến - Lùi theo hướng đầu xe
        Vector3 movement = transform.forward * moveInput.y * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Xoay hướng xe nâng (Đảo hướng xoay khi lùi xe cho thực tế)
        float turnDirection = moveInput.x;
        if (moveInput.y < 0) turnDirection = -turnDirection;

        float turn = turnDirection * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void Update()
    {
        if (!isGameActive) return;

        // Hệ thống đếm ngược thời gian
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateGameplayUI();
        }
        else
        {
            // Hết giờ! Kết thúc ca làm việc
            timeRemaining = 0;
            EndWorkShift();
        }
    }

    void UpdateGameplayUI()
    {
        if (gameplayUIText != null)
        {
            gameplayUIText.text = "TIME: " + Mathf.CeilToInt(timeRemaining) + "s\nMONEY: $" + sessionMoney;
        }
    }

    // Hàm cộng tiền khi thả hàng vào Drop Zone thành công (Chúng ta sẽ gọi hàm này ở bước sau)
    public void EarnMoney(int amount)
    {
        sessionMoney += amount;
        UpdateGameplayUI();
    }

    // Xử lý khi hết giờ làm việc
    void EndWorkShift()
    {
        isGameActive = false;

        // 1. Cộng tổng số tiền kiếm được trong ca vào bộ nhớ lưu trữ DataManager
        DataManager.AddMoney(sessionMoney);

        // 2. Tăng số ngày làm việc lên ngày tiếp theo
        DataManager.AdvanceDay();

        // 3. Tự động chuyển quay trở lại Scene 1 (Căn phòng) để bắt đầu ngày mới
        SceneManager.LoadScene("Scene1_Home");
    }
}