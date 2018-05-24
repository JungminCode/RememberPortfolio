using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentFollowCam : MonoBehaviour
{
    public GameObject student;  // 2스테이지의 학생 오브젝트 변수
    Transform studentTransform; // 학생 오브젝트의 Transform 컴포넌트를 넣을 변수

    private void Awake()
    {
        studentTransform = student.GetComponent<Transform>();   // 학생 오브젝트의 Transform컴포넌트를 대입
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(studentTransform.position.x, studentTransform.position.y, -15);    // 학생을 따라다닐 카메라의 위치를 학생의 위치에서 z값으로는 -15만큼 떨어진 위치에서 볼 수 있게 함.
    }
}
