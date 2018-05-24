using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPortal : MonoBehaviour
{
    public GameObject otherPortal;  // 다른 포탈의 게임 오브젝트

    void OnTriggerStay(Collider other) // 부딪혔을 때,
    {
        if (other.CompareTag("Key"))    // 키와 부딪혔다면
        {
            SoundManager.instance.PlayPortalMoveSound();    // 포탈 이동 사운드를 실행하고
            Rigidbody mOtherRigidBody = other.transform.GetComponent<Rigidbody>();  // 리지드 바디를 가져와 대입
            Vector3 mExitVelocity = otherPortal.transform.forward;  // 포탈의 앞 부분의 위치를, mExitVelocity에 대입시켜준다.
            mOtherRigidBody.velocity = mExitVelocity;   // mExitVelocity를 아까 대입한 리지드 바디의 velocity에 대입
            other.transform.position = otherPortal.transform.position;    // 키의 위치를 다른 포탈의 위치로 옮긴다.
            Destroy(this.gameObject);   // 현재 포탈과
            Destroy(otherPortal);       // 다른 포탈을 종료한다.
        }
    }
}
