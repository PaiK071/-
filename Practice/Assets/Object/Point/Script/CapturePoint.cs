using System.Diagnostics;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{

    /*
     * OnTriggerEnter�� OnTriggerExit ��������Ƿ� IsTriggerüũ�� �ʼ���!!!!!!!!!
     */

    public float captureTime = 10.0f; // ���ɿ� �ʿ��� �ð�
    private float currentCaptureProgress = 0.0f;//���� ���� ����
    private bool isBeingCaptured = false; //���� �Ǿ����� Ȯ�� 
    public PlayerController.Team currentTeam = PlayerController.Team.None; // ���� ������ �� ù ���� None���� ����

    // �÷��̾ ������ ���� �� ȣ��
    void OnTriggerEnter(Collider other)
    {
        // �÷��̾� ��ũ��Ʈ���� �� ���� ������ ���߿� DB�� ���� �ʿ�
        PlayerController player = other.GetComponent<PlayerController>(); 
       
        if (player != null)
        {
            // �÷��̾��� ���� ������
            PlayerController.Team playerTeam = player.GetTeam();

            // ���� ������ ���ɵ��� �ʾҰų�, �÷��̾��� ���� ���� ���� ������ ���� ����
            if (currentTeam == PlayerController.Team.None || currentTeam == playerTeam)
            {
                isBeingCaptured = true;
            }
        }
    }

    // �÷��̾ �������� ���� �� ȣ��
    void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && isBeingCaptured)
        {
            isBeingCaptured = false;
        }
    }

    // �� �����Ӹ��� ���� ���¸� ������Ʈ
    void Update()
    {

        UnityEngine.Debug.Log("Per:" + currentCaptureProgress);
        if (isBeingCaptured)
        {
            // ���� ����
            currentCaptureProgress += Time.deltaTime;
            
            // ������ �Ϸ�Ǹ�
            if (currentCaptureProgress >= captureTime)
            {
                CaptureComplete();
            }
        }
        else
        {
            // ���� ���� �ƴϸ� ���� ���൵ ���� (���� ȿ��)
            currentCaptureProgress = Mathf.Max(0, currentCaptureProgress - Time.deltaTime);
        }
    }

    // ������ �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
    void CaptureComplete()
    {
        UnityEngine.Debug.Log("Point Captured!");

        // ���� ������ ���� ���� (������ ���� ���� ���� �������� �÷��̾��� ���� ����)
        currentTeam = isBeingCaptured ? PlayerController.Team.TeamA : PlayerController.Team.TeamB;

        // ���� �¸� �����̳� UI ������Ʈ ���� �ļ� ó�� ����
    }
}
