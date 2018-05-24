using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorManager : MonoBehaviour     // 포탈을 쏠 때마다 마우스 커서가 변경되는 마우스커서 매니저의 클래스
{
    public static MouseCursorManager mouseCursorInstance = null;    // 싱글턴 패턴을 사용하기 위해, static으로 변수를 만들고 null로 초기화
    public Texture2D idleCursor , leftCursor , rightCursor , twoAttachCursor;       // 아무것도 부착되지 않았을 때, 왼쪽만 부착 되었을 때, 오른쪽만 부착 되었을 때, 두 포탈 다 부착되었을 때의 커서 텍스처들을 담을 변수
    public int mouseLeftNumber , mouseRightNumber , mouseTwoNumber = 0;     // 왼쪽 부착, 오른쪽 부착, 두 포탈 부착에 관한 것을 int형으로 해서, 비교

    private void Awake()
    {
        if (mouseCursorInstance == null)                           //게임매니저 중복생성 방지 및 싱글톤 객체화
        {
            mouseCursorInstance = this;
        }
        else if (mouseCursorInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        mouseTwoNumber = mouseLeftNumber + mouseRightNumber;    // mouseTwoNumber를 mouseLeftNumber와 mouseRightNumber를 더한 값을 대입시킨다. 매 프레임마다.

        if (mouseTwoNumber == 1)    // 왼쪽 포탈만 부착 되었을 때
        {
            Cursor.SetCursor(leftCursor, Vector2.zero, CursorMode.Auto);    // leftCursor의 텍스처를 마우스 커서로 사용
        }
        if (mouseTwoNumber == 2)    // 오른쪽 포탈만 부착 되었을 때
        {
            Cursor.SetCursor(rightCursor, Vector2.zero, CursorMode.Auto);   // rightCursor의 텍스처를 마우스 커서로 사용
        }
        if (mouseTwoNumber == 3)    // 두 포탈 모두 부착되었을 때
        {
            Cursor.SetCursor(twoAttachCursor, Vector2.zero, CursorMode.Auto);   // twoAttachCursor의 텍스처를 마우스 커서로 사용
        }
        if (mouseTwoNumber == 0)    // 아무것도 부착이 되지 않았을 때
        {
            Cursor.SetCursor(idleCursor, Vector2.zero, CursorMode.Auto);    // idleCursor의 텍스처를 마우스 커서로 사용
        }
    }
}
