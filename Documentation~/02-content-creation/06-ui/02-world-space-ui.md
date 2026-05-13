# 체험 내 UI (World Space UI)

## 개요

Viven은 PC와 VR 환경을 모두 지원하므로, 모든 환경에서 상호작용 가능한 UI를 제작하는 것이 중요합니다. VR 환경에서는 일반적인 `Screen Space - Overlay` 방식의 Canvas를 조작할 수 없으므로, 모든 UI는 `World Space` Canvas로 제작하는 것을 권장합니다.

## 언제 사용하나요?

- VR 환경에서 조작 가능한 UI를 만들 때
- 월드 내 특정 사물(키오스크, 표지판 등)에 부착된 UI를 만들 때
- 플레이어의 시야를 따라다니는 HUD(Heads-Up Display) 형태의 UI를 만들 때
- PC와 VR에서 각각 최적화된 UI 레이아웃을 제공해야 할 때

## 준비사항

- **World Space Canvas**: Canvas의 `Render Mode`를 `World Space`로 설정
- **VivenGraphicRaycaster**: PC와 VR 모두에서 UI 클릭/입력을 처리하기 위한 컴포넌트
- **UIModeChanger**: PC/VR 모드에 따라 UI를 자동으로 전환해주는 컴포넌트

## 진행 순서

### 1. Canvas 설정

1. Canvas를 생성하고 `Render Mode`를 `World Space`로 변경합니다.
2. 기존의 `Graphic Raycaster` 컴포넌트를 제거합니다.
3. **`VivenGraphicRaycaster`** 컴포넌트를 추가합니다. 이 컴포넌트는 PC의 마우스 클릭과 VR의 레이캐스트 입력을 모두 처리할 수 있게 해줍니다.

### 2. PC/VR 모드별 UI 대응

PC와 VR은 입력 방식과 시야가 다르므로 각각에 맞는 UI를 배치해야 합니다.

1. Canvas 하위에 `PC_UI`와 `VR_UI`라는 이름의 빈 게임 오브젝트를 만듭니다.
2. 부모 오브젝트에 **`UIModeChanger`** 컴포넌트를 추가합니다.
3. `UIModeChanger`의 `pcUI` 필드에는 `PC_UI`를, `xrUI` 필드에는 `VR_UI`를 할당합니다.
4. 이제 Viven 실행 시 현재 모드(PC 또는 VR)에 맞는 UI만 활성화됩니다.

### 3. VR Overlay 효과 구현

VR에서는 화면 전체를 덮는 Overlay 기능이 없으므로, Overlay 효과를 내려면 UI가 플레이어의 머리(카메라)를 따라다니도록 설정해야 합니다.

1. **머리 추적 스크립트 작성**: `Player.Mine.CharacterHead`를 사용하여 UI가 플레이어의 시야를 따라오게 할 수 있습니다.

```lua
-- UI가 플레이어의 머리를 따라다니게 하는 예시
local uiTransform

function awake()
    uiTransform = self.gameObject.transform
end

function update()
    local head = Player.Mine.CharacterHead
    if head then
        -- 위치 동기화 (약간 앞쪽에 배치)
        uiTransform.position = head.position + head.forward * 1.5
        -- 회전 동기화 (플레이어를 바라보게 하거나 머리 회전과 일치)
        uiTransform.rotation = head.rotation
    end
end
```

2. **Always Front 설정**: `VivenGraphicRaycaster`의 `alwaysFront` 옵션을 켜면 UI가 다른 월드 오브젝트에 가려지지 않고 항상 앞에 보이게 할 수 있습니다. 이는 VR에서 HUD와 같은 효과를 줄 때 유용합니다.

## 확인 방법

- **PC 모드**: 마우스 커서로 UI 버튼이 클릭되는지 확인합니다.
- **VR 모드**: 컨트롤러에서 나가는 레이(Ray)로 UI 버튼이 하이라이트되고 클릭되는지 확인합니다.
- **모드 전환**: PC와 VR 모드를 전환했을 때 `UIModeChanger`에 의해 올바른 UI가 표시되는지 확인합니다.

## 자주 일어나는 실수

- **Overlay Canvas 사용**: VR 모드에서는 `Screen Space - Overlay` Canvas가 렌더링되지 않거나 조작이 불가능합니다. 반드시 `World Space`를 사용하세요.
- **Raycaster 누락**: 일반 `Graphic Raycaster`는 VR 레이캐스트 입력을 인식하지 못합니다. 반드시 `VivenGraphicRaycaster`로 교체해야 합니다.
- **UI 레이어 설정**: `VivenGraphicRaycaster`의 `alwaysFront` 옵션에 따라 레이어가 `UI` 또는 `WorldspaceUI`로 자동 변경되므로, 카메라의 `Culling Mask` 설정을 확인해야 합니다.

## 관련 문서

- [Unity UGUI 활용 가이드](./01-unity-ugui-guide.md)
- [PC/VR 플랫폼 개발](../01-project-management/05-pc-vr-platform-development.md)
