# Viven 컨텐츠 UI 제작 가이드

## 개요

Viven 컨텐츠 내에서 사용자에게 정보를 제공하고 상호작용하기 위한 UI(User Interface)를 제작하는 방법을 설명합니다. Unity의 UGUI 시스템을 기반으로 하며, Lua 스크립트를 통해 동적으로 UI를 제어할 수 있습니다.

> [!CAUTION]
> **UI Toolkit은 Viven SDK에서 지원하지 않습니다.** 모든 UI 제작에는 반드시 **UGUI**를 사용해야 합니다.
> 텍스트 UI에는 Unity 기본 `Text` 대신 **TextMeshPro** (`TMP_Text`, `TextMeshProUGUI`)를 사용하세요.

## 언제 사용하나요?

- 게임 진행 상황(점수, 시간, 아이템 등)을 표시할 때
- 메인 메뉴, 상점, 결과창 등 화면 전환이 필요할 때
- 버튼 클릭, 슬라이더 조절 등 사용자 입력을 처리할 때
- 월드 내 특정 위치에 떠 있는 UI(World Space UI)를 구현할 때

## 준비사항

- **Unity Canvas**: UI 요소들이 배치될 기본 캔버스
- **UGUI 컴포넌트**: `Button`, `TMP_Text`, `Image`, `Slider`, `Toggle` 등
- **VivenLuaBehaviour**: UI 로직을 제어할 Lua 스크립트가 연결된 컴포넌트

## 진행 순서

### 1. UI 컴포넌트 참조 (Injection)

스크립트에서 UI 요소를 제어하기 위해 `Injection list`를 사용하여 오브젝트를 참조합니다.

```lua
--#region Injection list

-- Unity Inspector에서 할당할 오브젝트들

---@type GameObject
StartButtonObject = checkInject(StartButtonObject)
---@type GameObject
ScoreTextObject = checkInject(ScoreTextObject)

-- ... 또는

---@type Button
startButton = checkInject(startButton)
---@type TMP_Text
scoreText = checkInject(scoreText)

--#endregion
```

### 2. 컴포넌트 초기화 (Awake/Start)

`awake` 또는 `start` 함수에서 실제 UGUI 컴포넌트를 가져오고 초기 설정을 수행합니다.

```lua
local startButton
local scoreText

function awake()
    -- 컴포넌트 가져오기
    startButton = startButtonObject:GetComponent(typeof(Button))
    scoreText = scoreTextObject:GetComponent(typeof(TMP_Text))
    
    -- 버튼 클릭 이벤트 리스너 등록
    startButton.onClick:AddListener(OnStartButtonClicked)
end

function OnStartButtonClicked()
    -- 버튼 클릭 시 효과음 재생 (권장)
    Global_SoundManager.PlayOneShotSFX_Client(SOUND_LIST.BUTTON_CLICK)
    -- 게임 시작 로직 실행
    Debug.Log("Game Started!")
end
```

### 3. UI 동적 업데이트

데이터 변화에 따라 UI를 실시간으로 갱신합니다. `Util.EventBus`를 활용하면 데이터와 UI의 결합도를 낮출 수 있습니다.

```lua
function start()
    -- 전역 이벤트 구독
    Util.EventBus:registerEvent("OnScoreChanged", UpdateScoreUI)
end

function UpdateScoreUI(newScore)
    if scoreText then
        scoreText.text = "Score: " .. tostring(newScore)
    end
end

function onDestroy()
    -- 이벤트 구독 해제 (중요)
    Util.EventBus:unregisterEvent("OnScoreChanged", UpdateScoreUI)
end
```

### 4. 프리팹 동적 생성 (Dynamic UI)

상점 아이템 목록처럼 데이터에 따라 UI 요소가 늘어나는 경우 `GameObject.Instantiate`를 사용합니다.

```lua
function InitializeItemList(items)
    for _, itemData in ipairs(items) do
        -- 프리팹 생성
        local itemObject = GameObject.Instantiate(ItemPrefab, ItemParent.transform)
        -- 생성된 오브젝트의 Lua 컴포넌트 가져오기
        local itemUI = itemObject:GetLuaComponent("ItemUI")
        if itemUI then
            itemUI.Initialize(itemData)
        end
    end
end
```

## 확인 방법

- **버튼 반응**: 버튼을 클릭했을 때 등록한 함수가 실행되는지 확인합니다.
- **텍스트 업데이트**: 데이터 값이 변경될 때 화면의 텍스트가 즉시 갱신되는지 확인합니다.
- **레이아웃**: 다양한 해상도나 화면 비율에서도 UI 요소가 올바른 위치에 있는지 확인합니다.

## 자주 일어나는 실수

- **이벤트 구독 해제 누락**: `onDestroy`에서 `unregisterEvent`를 호출하지 않으면 메모리 누수나 예기치 않은 동작이 발생할 수 있습니다.
- **Null 참조**: `GetComponent`로 가져온 컴포넌트가 `nil`인지 확인하지 않고 접근하면 에러가 발생합니다.
- **효과음 누락**: 버튼 클릭 시 사용자 피드백을 위해 효과음을 재생하는 것을 잊지 마세요.
- **World Space UI 레이어**: 월드 공간 UI가 다른 오브젝트에 가려지거나 카메라 방향을 제대로 보지 않는 경우가 있으므로 설정을 확인해야 합니다.

## 관련 문서

- [VivenLuaBehaviour 활용](../03-scripting/01-viven-lua-behaviour.md)
- [Viven 전용 서비스 및 API](../03-scripting/03-viven-services-and-api/00-overview.md)
- [오디오 재생 및 제어](../05-audio/01-viven-audio-event-instance-fmod.md)
