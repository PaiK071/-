using System.Diagnostics;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{

    /*
     * OnTriggerEnter와 OnTriggerExit 사용했으므로 IsTrigger체크가 필수임!!!!!!!!!
     */

    public float captureTime = 10.0f; // 점령에 필요한 시간
    private float currentCaptureProgress = 0.0f;//현재 점령 정도
    private bool isBeingCaptured = false; //점령 되었는지 확인 
    public PlayerController.Team currentTeam = PlayerController.Team.None; // 현재 점령한 팀 첫 값은 None으로 시작

    // 플레이어가 거점에 들어올 때 호출
    void OnTriggerEnter(Collider other)
    {
        // 플레이어 스크립트에서 팀 정보 가져옴 나중에 DB로 변경 필요
        PlayerController player = other.GetComponent<PlayerController>(); 
       
        if (player != null)
        {
            // 플레이어의 팀을 가져옴
            PlayerController.Team playerTeam = player.GetTeam();

            // 현재 거점이 점령되지 않았거나, 플레이어의 팀이 현재 팀과 같으면 점령 시작
            if (currentTeam == PlayerController.Team.None || currentTeam == playerTeam)
            {
                isBeingCaptured = true;
            }
        }
    }

    // 플레이어가 거점에서 나갈 때 호출
    void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && isBeingCaptured)
        {
            isBeingCaptured = false;
        }
    }

    // 매 프레임마다 점령 상태를 업데이트
    void Update()
    {

        UnityEngine.Debug.Log("Per:" + currentCaptureProgress);
        if (isBeingCaptured)
        {
            // 점령 진행
            currentCaptureProgress += Time.deltaTime;
            
            // 점령이 완료되면
            if (currentCaptureProgress >= captureTime)
            {
                CaptureComplete();
            }
        }
        else
        {
            // 점령 중이 아니면 점령 진행도 감소 (리셋 효과)
            currentCaptureProgress = Mathf.Max(0, currentCaptureProgress - Time.deltaTime);
        }
    }

    // 점령이 완료되었을 때 호출되는 함수
    void CaptureComplete()
    {
        UnityEngine.Debug.Log("Point Captured!");

        // 현재 점령한 팀을 설정 (이전에 점령 중인 팀이 없었으면 플레이어의 팀이 점령)
        currentTeam = isBeingCaptured ? PlayerController.Team.TeamA : PlayerController.Team.TeamB;

        // 이후 승리 조건이나 UI 업데이트 등의 후속 처리 가능
    }
}
