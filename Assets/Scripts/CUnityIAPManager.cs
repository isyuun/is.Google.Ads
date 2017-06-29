using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

// 구매 관련 기능 사용
using UnityEngine.Purchasing;
using System;

// 유니티 IAP 매니저는 구매 관련 처리 이벤트 관련 메소드를 구현하고 있어야 함
public class CUnityIAPManager : MonoBehaviour, IStoreListener
{
    // 인앱 결제 구매 시스템 참조
    private IStoreController _storeController;

    // 제품 정보 제공 시스템 참조
    private IExtensionProvider _extensionProvider;

    public Text _msgText;

    private void Start()
    {
        // 인앱 시스템 초기화 수행
        InitializePurchasing();
    }

    // 인앱 시스템 초기화 여부 체크
    public bool IsInitialized()
    {
        return _storeController != null && _extensionProvider != null;
    }

    public void InitializePurchasing()
    {
        // 결제 관련 모듈들이 이미 초기화 되어 있다면
        if (IsInitialized())
        {
            return;
        }

        // 인앱 시스템 빌더를 생성
        ConfigurationBuilder builder =
ConfigurationBuilder.Instance(
StandardPurchasingModule.Instance());

        // 인앱 시스템 빌더에 상품을 등록함
        builder.AddProduct("jewelry.100", ProductType.Consumable);
        builder.AddProduct("jewelry.300", ProductType.Consumable);
        builder.AddProduct("jewelry.1000", ProductType.Consumable);

        // 인앱 시스템을 초기화함
        UnityPurchasing.Initialize(this, builder);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // 인앱 시스템 초기화 성공 이벤트(콜백) 메소드
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _msgText.text = "인앱 시스템 초기화 성공";

        // 인앱 시스템 관련 객체들을 참조함
        _storeController = controller;
        _extensionProvider = extensions;
    }

    // 인앱 시스템 초기화 실패 이벤트(콜백) 메소드
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        _msgText.text = "인앱 시스템 초기화 실패 => " + error;
    }

    // 구매 성공 이벤트 메소드
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        string purchaseProducId = e.purchasedProduct.definition.id;

        _msgText.text = "보석 구매 성공 : " + purchaseProducId;

        // 1번 보석 구매 완료
        if (purchaseProducId.Equals("jewelry.100"))
        {
            _msgText.text = "보석100 지급 완료";
        }
        // 1번 보석 구매 완료
        else if (purchaseProducId.Equals("jewelry.300"))
        {
            _msgText.text = "보석300 지급 완료";
        }
        // 1번 보석 구매 완료
        else if (purchaseProducId.Equals("jewelry.1000"))
        {
            _msgText.text = "보석1000 지급 완료";
        }
        else
        {
            _msgText.text = "보석 구매에 문제가 발생함";
            return PurchaseProcessingResult.Pending;
        }

        // 구매 관련 정보 출력 (json <- 요안에 영수증 번호 있음)
        _msgText.text = _msgText.text + " (" + e.purchasedProduct.receipt + ")";

        return PurchaseProcessingResult.Complete;
    }

    // 구매 실패 이벤트 메소드
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        _msgText.text = "보석 구매에 실패함 (" + i.definition.storeSpecificId + ", " + p + ")";
    }

    // 구매 버튼 클릭
    public void OnPurchaseButtonClick(string productId)
    {
        // 인앱 시스템이 초기화 되어 있다면
        if (IsInitialized())
        {
            // 지정한 아이디를 가진 상품 객체를 참조함
            Product product = _storeController.products.WithID(productId);

            // 지정한 아이디의 제품이 존재하고
            // 구매가 가능한 제품 이라면
            if (product != null && product.availableToPurchase)
            {
                _msgText.text = "제품 구매를 수행함";
                // 제품 구매를 시도함
                _storeController.InitiatePurchase(product);
            }
            else
            {
                _msgText.text = "제품 구매를 수행할 수 없음";
            }
        }
        else
        {
            _msgText.text = "인앱 시스템 초기화가 되어 있지 않음";
        }
    }
}