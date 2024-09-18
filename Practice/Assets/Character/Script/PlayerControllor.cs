using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Team team; // 플레이어 팀 정보 추가

    /*
     * 프라이빗 필드를 유지하면서도 인스펙터에서 해당 필드를 수정 가능하게 제작
     * 변수 선언 부분
     */
    private Rigidbody rb;
    private Animator animator;
    private float moveSpeed = 0f;
    private float jumpHeight = 5f;
    private float dash = 5f;
    private float rotSpeed = 50f;
    private Vector3 dir = Vector3.zero;

    // 점프를 위해 레이어를 사용하므로, 레이어를 생성한 후 땅이 되는 오브젝트의 레이어를 변경해줘야 함.
    private bool ground = false;
    public LayerMask layer;

    /* 
     * 기본 이동 방식은 Input.GetAxis로 이동할 방향의 벡터(dir)를 구한 후
     * Normalize 하여 FixedUpdate 내부에서 MovePosition을 이용해 현재 위치 +
     * (moveSpeed * Time.deltaTime)만큼 더 이동시키는 방식임.
     */

    /*
     * 애니메이션은 Jump는 Trigger
     * 기본 이동은 float Speed를 활용하여 실행함
     * Jump의 경우 Has Exit Time이 필수임 (애니메이션이 끝까지 실행되지 않으면 어색함)
     * / 점프 후 속도 줄이기는 추가 고려 필요
     */

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // 팀 정보는 초기화 시 할당, 또는 외부에서 할당 가능
        // 예를 들어, 팀 A로 시작
        team = Team.TeamA;
    }

    private void Update()
    {
        // 방향 벡터 설정
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        dir.Normalize();

        // 매 업데이트마다 땅에 닿아 있는지 확인하는 코드 (점프용)
        CheckGround();

        if (Input.GetButtonDown("Jump") && ground)
        {
            // 점프 애니메이션 실행
            animator.SetTrigger("Jump");
            Vector3 jumpPower = Vector3.up * jumpHeight;
            rb.AddForce(jumpPower, ForceMode.VelocityChange);
        }

        if (Input.GetButtonDown("Dash")) // 대시 키 추가 필수 (Project Setting -> Input Manager)
        {
            moveSpeed = 15f;
            animator.SetFloat("Speed", moveSpeed);

            // 대쉬 애니메이션이 없음 ㅠ;
            Vector3 dashPower = this.transform.forward * dash * -Mathf.Log(1 / rb.drag);
            rb.AddForce(dashPower, ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        // 방향이 설정되어 있을 때만 이동
        if (dir != Vector3.zero)
        {
            // 캐릭터의 이동 방향이 반대일 때 돌아서는 속도가 다른 오류를 해결하기 위해 작성
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                transform.Rotate(0, 1, 0);
            }
            moveSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
        }
        else
        {
            moveSpeed = 0f;
        }

        // 애니메이션 속도 업데이트
        animator.SetFloat("Speed", moveSpeed);

        // 이동 처리
        rb.MovePosition(this.gameObject.transform.position + dir * moveSpeed * Time.deltaTime);
    }

    void CheckGround() // 땅에 닿았는지 확인하기 위한 코드
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, layer))
        {
            // Raycast: 레이저 쏘는 것 (현재 위치에서 위로 0.2f 이동한 후, 아래 방향으로 0.4만큼 레이어가 검출되면 hit에 정보 저장)
            ground = true;
        }
        else
        {
            ground = false;
        }
    }

    // 팀 정보를 반환하는 메서드 (CapturePoint에서 사용)
    public Team GetTeam()
    {
        return team;
    }

    // 팀 열거형 정의
    public enum Team
    {
        None,
        TeamA,
        TeamB
    }
}
