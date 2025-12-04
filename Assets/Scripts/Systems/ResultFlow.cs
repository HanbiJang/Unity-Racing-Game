using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResultFlow
{
    public static void GoToResult()
    {
        // 오버레이 패널을 쓰는 경우:
        var panel = GameObject.FindWithTag("ResultPanel");
        if (panel != null) { panel.SetActive(true); return; }
    }

    public static void BackToLobby()
    {
        // 결과 패널 닫고 로비로 이동
        GameModeManager.instance?.ResetForLobby();

        // 로비 씬으로
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);

        // 네트워크 정책(5) 참고: 유지/재연결 처리
        // NetworkPolicy.BackToLobbyConnectionPolicy();
    }
}
