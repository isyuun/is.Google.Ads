﻿using UnityEngine;
using System.Collections;

// 유니티 광고 사용
using UnityEngine.Advertisements;

using UnityEngine.UI;

public class CUnityAdsManager : MonoBehaviour
{

    [SerializeField]
    string _androidGameId = ""; // 안드로이드 게임 아이디

    [SerializeField]
    bool _testMode; // 테스트 모드 사용 여부

    public Button _adsButton; // 광고 보기 버튼

    public Text _messageText; // 로그 출력 텍스트

    // Use this for initialization
    void Start()
    {
        // 광고제공이 가능하고 아직 초기화가 안되어 있다면
        if (Advertisement.isSupported && !Advertisement.isInitialized)
        {
            // 유니티 광고 초기화
            Advertisement.Initialize(_androidGameId, _testMode);
            _messageText.text = "광고 시스템이 초기화 됨";
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 광고를 볼 수 있을때만 버튼을 활성화 함
        _adsButton.interactable = (
            Advertisement.isInitialized && Advertisement.IsReady(null)
        );
    }

    // 광고 실행 버튼 클릭
    public void OnUnityAdsButtonClick()
    {
        // 광고 보기 옵션
        ShowOptions option = new ShowOptions();
        option.resultCallback = UnityAdsShowCallback;

        // 옵션 설정 후 광고를 띄움
        Advertisement.Show(null, option);
    }

    // 광고 출력 결과 이벤트 메소드
    void UnityAdsShowCallback(ShowResult result)
    {
        switch (result)
        {
            // 광고 시청을 완료함
            case ShowResult.Finished:
                _messageText.text = "광고 시청을 완료 했음";
                // 보상 지급 처리 코드...
                break;
            // 광고를 스킵함
            case ShowResult.Skipped:
                _messageText.text = "광고 시청을 그냥 넘긴 놈임";
                break;
            // 광고 보기를 실패함
            case ShowResult.Failed:
                _messageText.text = "광고 시청이 실패하였음";
                break;
        }
        Debug.Log(_messageText.text);
    }
}
