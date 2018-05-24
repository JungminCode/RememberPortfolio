using UnityEngine;
using System.Collections;

public class PortalMove : MonoBehaviour // 부딪힌 캐릭터를 다른 포탈로 이동 시키는 스크립트 클래스
{
    public GameObject otherPortal;  // 다른 포탈 게임오브젝트를 넣는 변수
    public bool portalFlag = false; // portalFlag를 false로 바꾼다.
    public GameObject portalMoveParticle;   // 포탈을 이동했을 때, 포탈 쪽에 쓰일 파티클 변수

    private PortalGunScript portalGun;   // 포탈건, 스크립트를 쓰기위한 변수

    void Start()
    {
        portalGun = GameObject.Find("PortalGun").GetComponent<PortalGunScript>();   // 이름이 PortalGun인 객체를 찾아, 그 객체의 PortalGun 클래스 스크립트를 사용
    }

    void OnTriggerStay(Collider other) // 부딪혔을 때,
    {
        if (other.CompareTag("Player") && otherPortal.GetComponent<PortalMove>().portalFlag == true)  // tag가 Player고, 서로의 포탈의 PortalFlag가 true라면
        {
            SoundManager.instance.PlayPortalMoveSound();
            Rigidbody mOtherRigidBody = other.transform.GetComponent<Rigidbody>();   // 리지드 바디를 가져와 대입
            Vector3 mExitVelocity = otherPortal.transform.forward * mOtherRigidBody.velocity.magnitude;  // 포탈의 앞부분과 플레이어의 속도의 크기를 곱해서, 대입
            mOtherRigidBody.velocity = mExitVelocity; // 속도를 대입시킨다. mOtherRigidBody에
            other.transform.position = otherPortal.transform.position + otherPortal.transform.forward * 2f;    // 플레이어의 포지션을, 부딪힌 포탈의 다른 포탈의 앞부분의 * 2한것과 다른 포탈의 위치를 더한다.
            GameObject tempObj; // 임시 게임 오브젝트 변수
            tempObj = Instantiate(portalMoveParticle, otherPortal.transform.position + otherPortal.transform.forward , Quaternion.identity) as GameObject;  // 임시 게임 오브젝트에, 이동 시 파티클을 복제시킨다.
            Destroy(tempObj, 1.2f); // 사용된 이동 시 파티클을 1.2초 뒤에 없앤다.
            //
            portalGun.leftPortal.SetActive(false);
            portalGun.rightPortal.SetActive(false);
            portalGun.leftPortal.GetComponent<PortalMove>().portalFlag = false;
            portalGun.rightPortal.GetComponent<PortalMove>().portalFlag = false;
            MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 0;
            MouseCursorManager.mouseCursorInstance.mouseRightNumber = 0;
            MouseCursorManager.mouseCursorInstance.mouseTwoNumber = 0;

            // 이동이 되었기 때문에, 양쪽 포탈을 숨기고, 양쪽 포탈의 이동가능 변수를 false로 바꿔주고, 마우스 커서를 원래대로 초기화한다.
        }
        if (other.CompareTag("Key") && otherPortal.GetComponent<PortalMove>().portalFlag == true)  // tag가 Key고, 서로의 포탈의 PortalFlag가 true라면, 위와 같다.
        {
            SoundManager.instance.PlayPortalMoveSound();
            Rigidbody mOtherRigidBody = other.transform.GetComponent<Rigidbody>();   // 리지드 바디를 가져와 대입
            Vector3 mExitVelocity = otherPortal.transform.forward * mOtherRigidBody.velocity.magnitude;  // 포탈의 앞부분과 플레이어의 속도의 크기를 곱해서, 대입
            mOtherRigidBody.velocity = mExitVelocity; // 속도를 대입시킨다. mOtherRigidBody에
            other.transform.position = otherPortal.transform.position + otherPortal.transform.forward * 2f;    // 플레이어의 포지션을, 부딪힌 포탈의 다른 포탈의 앞부분의 * 2한것과 다른 포탈의 위치를 더한다.
            GameObject tempObj;
            tempObj = Instantiate(portalMoveParticle, other.transform.position - other.transform.forward, Quaternion.identity) as GameObject;
            Destroy(tempObj, 1.2f);
            portalGun.leftPortal.SetActive(false);
            portalGun.rightPortal.SetActive(false);
            portalGun.leftPortal.GetComponent<PortalMove>().portalFlag = false;
            portalGun.rightPortal.GetComponent<PortalMove>().portalFlag = false;
            MouseCursorManager.mouseCursorInstance.mouseLeftNumber = 0;
            MouseCursorManager.mouseCursorInstance.mouseRightNumber = 0;
            MouseCursorManager.mouseCursorInstance.mouseTwoNumber = 0;
        }
    }
}
