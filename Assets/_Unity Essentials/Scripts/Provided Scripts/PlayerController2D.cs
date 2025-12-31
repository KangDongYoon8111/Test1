using System;
using UnityEngine;
using UnityEngine.InputSystem; // [중요] Input System을 쓰기 위해 꼭 추가!

// 2D 플레이어 움직임을 담당하는 클래스입니다.
public class PlayerController2D : MonoBehaviour
{
    // [공개 변수] - 유니티 인스펙터 창에서 조절 가능
    public float speed = 5f; // 플레이어가 얼마나 빨리 움직일지 속도 설정
    public bool canMoveDiagonally = true; // 대각선 이동을 허용할지 여부 (체크하면 대각선 이동 가능)

    // [비공개 변수] - 내부 로직용
    private Rigidbody2D rb; // 물리적인 움직임을 담당하는 2D 컴포넌트
    private Vector2 movement; // 플레이어가 가야 할 방향를 저장하는 벡터 (X, Y)
    private bool isMovingHorizontally = true; // "대각선 이동 금지"일 때, 지금 가로로 가는지 세로로 가는지 기억하는 변수
    
    private PlayerControls controls;// 우리가 만든 입력 설계도 클래스

    private void OnEnable()
    {
        controls = new PlayerControls(); // 설계도를 생성
        controls.Enable(); // 설계도를 발동(켜기)
    }

    private void OnDisable()
    {
        controls.Disable(); // 설계도를 끄기
    }

    // Start: 게임이 시작될 때 딱 한 번 실행됩니다.
    void Start()
    {
        // 1. 내 오브젝트에 붙어있는 Rigidbody2D 컴포넌트를 찾아옵니다.
        rb = GetComponent<Rigidbody2D>(); 
        
        // 2. 물리 충돌 시 캐릭터가 팽이처럼 뱅글뱅글 도는 것을 방지합니다. (Z축 회전 고정)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update: 매 프레임마다 실행됩니다. (입력 감지용)
    void Update()
    {
        // 1. 키보드 입력을 받습니다. (방향키 또는 WASD)
        // GetAxisRaw는 값을 -1, 0, 1 로 딱딱 끊어서 반환합니다. (즉각적인 반응)
        //float horizontalInput = Input.GetAxisRaw("Horizontal"); // 좌(-1), 우(1), 안누름(0)
        //float verticalInput = Input.GetAxisRaw("Vertical");     // 하(-1), 상(1), 안누름(0)
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();

        // 2. 대각선 이동이 허용된 경우
        if (canMoveDiagonally)
        {
            // 입력받은 X, Y 값을 그대로 이동 벡터에 넣습니다. (대각선 가능)
            movement = new Vector2(moveInput.x, moveInput.y);
            
            // 캐릭터를 이동 방향으로 회전시킵니다.
            RotatePlayer(moveInput.x, moveInput.y);
        }
        // 3. 대각선 이동이 금지된 경우 (4방향 이동만 가능)
        else
        {
            // 가로 입력을 했으면 -> "지금은 가로 이동 중이야"라고 표시
            if (moveInput.x != 0)
            {
                isMovingHorizontally = true;
            }
            // 세로 입력을 했으면 -> "지금은 세로 이동 중이야"라고 표시
            else if (moveInput.y != 0)
            {
                isMovingHorizontally = false;
            }

            // 우선순위에 따라 이동 방향 결정
            if (isMovingHorizontally)
            {
                // 가로 모드면 Y값은 무시하고 X값만 반영
                movement = new Vector2(moveInput.x, 0);
                RotatePlayer(moveInput.x, 0);
            }
            else
            {
                // 세로 모드면 X값은 무시하고 Y값만 반영
                movement = new Vector2(0, moveInput.y);
                RotatePlayer(0, moveInput.y);
            }
        }
    }

    // FixedUpdate: 물리 연산 주기(보통 0.02초)마다 실행됩니다. (이동 처리용)
    void FixedUpdate()
    {
        // 실제 물리적인 이동을 처리하는 곳입니다.
        // 방향(movement) * 속도(speed)를 리지드바디의 속도에 대입합니다.
        // 참고: Unity 6 부터는 velocity 대신 linearVelocity를 사용합니다.
        rb.linearVelocity = movement * speed;
    }

    // 캐릭터를 이동 방향으로 회전시키는 커스텀 함수
    void RotatePlayer(float x, float y)
    {
        // 입력이 없으면(0, 0) 회전하지 않고 함수 종료
        if (x == 0 && y == 0) return;

        // 1. 아크탄젠트(Atan2) 수학 함수를 이용해 좌표(y, x)를 각도(라디안)로 변환하고,
        // 2. 그 라디안 값을 우리가 아는 '도(Degree)' 단위로 바꿉니다.
        // 예: 오른쪽(1,0) -> 0도, 위쪽(0,1) -> 90도
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        
        // 3. 구한 각도(angle)만큼 Z축을 기준으로 회전시킵니다.
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}