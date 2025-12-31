using System;
using UnityEngine;
using UnityEngine.InputSystem; // [중요] Input System을 쓰기 위해 꼭 추가!

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f; // 점프 힘
    private float speed = 3.0f; // Set player's movement speed.
    private float rotationSpeed = 60.0f; // Set player's rotation speed.
    
    private Rigidbody rb; // Reference to player's Rigidbody.

    private PlayerControls controls;// 우리가 만든 입력 설계도 클래스
    private Vector2 moveInput;      // 입력받은 움직임 값
    private bool isGrounded = true; // 바닥에 닿아있는지

    // OnEnable: 컴포넌트(스크립트)가 활성화될 때 한번 발동되는 유니티 이벤트 메서드
    private void OnEnable()
    {
        controls = new PlayerControls(); // 설계도를 생성
        controls.Enable(); // 설계도를 발동(켜기)
        
        // Jump 액션이 '수행(Performed)' 되었을 때, OnJump 메서드를 실행해라!
        // 라고 구독(연결)하는 것입니다.
        // += 해당 표시는 '이 기능도 추가해주세요'라는 뜻입니다.
        controls.Player.Jump.performed += OnJump;
    }

    // OnDisable: 컴포넌트(스크립트)가 비활성화될 때 한번 발동되는 유니티 이벤트 메서드
    private void OnDisable()
    {
        // 스크립트가 꺼질 때는 연결을 끊어줘야 메모리 에러가 안 납니다.
        controls.Player.Jump.performed -= OnJump;
        controls.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Access player's Rigidbody.
    }

    // Handle physics-based movement and rotation.
    private void FixedUpdate()
    {
        // (기존)Input.GetAxis 대신, (새로운)Input System에서 값을 읽어오기
        // WASD... 키 입력을 Vector2(x, y) 형태로 한 번에 가져옵니다.
        // x축은 좌우(A,D), y축은 상하(W,S)에 대응해서 값이 들어옵니다.
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        
        // Move player based on vertical input.
        // float moveVertical = Input.GetAxis("Vertical");
        // 위 기존 방식 대신 아래로 변경
        float moveVertical = moveInput.y;
        Vector3 movement = (transform.forward * moveVertical) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Rotate player based on horizontal input.
        // float turn = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
        // 위 기존 방식 대신 아래로 변경
        float turn = moveInput.x * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    // 실제로 점프를 수행하는 메서드
    // 매개변수 'InputAction.CallbackContext context'는 입력에 대한 정보를
    // 담고있는 매개변수입니다.
    private void OnJump(InputAction.CallbackContext context)
    {
        // 땅에 있을 때만 점프 가능하게
        if (isGrounded)
        {
            // 위쪽 방향(page.478) * 점프력
            // ForceMode.Impulse: 순간적인 힘을 가할 때 사용
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            // 점프를 했으니 공중에 떠있음
            isGrounded = false;
        }
    }

    // 충돌 감지: 플레이어가 바닥에 다시 닿았는지 확인
    private void OnCollisionEnter(Collision other)
    {
        // 바닥에 닿으면 다시 점프 가능 상태로 변경
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}