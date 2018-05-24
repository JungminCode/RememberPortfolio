using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;   // UI의 패널을 집어넣을 변수
    public PortalGunScript portalGun;   // PortalGunScript를 사용할 변수
    private PlayerScirpt playerScript;  // PlayerScirpt를 사용할 변수
    private bool pause = false; // 일시정지 bool형 변수
    private bool mainMenu = false;  // 메인메뉴 bool형 변수

    private void Awake()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScirpt>();   // PlayerScript를 가져와 대입시킨다.
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))    // Esc키를 누르면,
        {
            pause = !pause; // pause를 pause의 반대값으로 변경 true - false
            portalGun.portalShootFlag = !portalGun.portalShootFlag; // 일시정지가 되면, 포탈을 사용하지 못하게 만들어준다. 일시정지가 풀리면, 포탈 사용이 가능
        }
        if(pause == false && mainMenu == true)  // 메인메뉴 버튼이 클릭되면,
        {
            mainMenu = false;   // mainMenu를 false로 바꾸고
            SceneManager.LoadScene("Stage_MainTitle");  // 메인타이틀의 씬으로 넘어감.
        }
        if(pause == false)  // pause가 false면,
        {
            Time.timeScale = 1f;    // 일시정지를 풀고
            pausePanel.SetActive(false);    // 패널을 감추고
            playerScript.moveFlag = true;   // 플레이어를 움직일 수 있게 하고
            portalGun.portalShootFlag = true;   // 포탈을 사용할 수 있게 한다.
        }
        if(pause == true)   // pause가 true면
        {
            Time.timeScale = 0f;    // 일시정지 시키고,
            pausePanel.SetActive(true); // 패널을 보이게 하고
            playerScript.moveFlag = false;  // 이동이 불가능하게 하고
            portalGun.portalShootFlag = false;  // 포탈을 사용할 수 없게 한다.
        }
    }

    public void RetryButton()   // 계속 시작 버튼
    {
        pause = false;  // pause를 false로 바꾼다.
    }

    public void MainMenuButton()    // 메인메뉴 버튼
    {
        pause = false;  // pause가 false로 바뀌고
        mainMenu = true;    // mainMenu는 true로 바뀐다.
    }

    public void ExitButton()    // 종료 버튼
    {
        Application.Quit(); // 빌드파일을 종료시킨다.
    }
}
