# 핸드 트래킹(Hand Tracking) API

:::warning 실험적 기능 (Experimental)
이 기능은 현재 실험적 단계이며, 향후 플랫폼 업데이트에 따라 사전 공지 없이 API 구조나 동작 방식이 변경될 수 있습니다.
:::

## 개요

핸드 트래킹 API를 사용하면 VR 컨트롤러 대신 사용자의 실제 손 모양과 움직임을 감지하여 물체와 상호작용할 수 있습니다. 특정 손 포즈(Pose)를 취하거나 제스처(Gesture)를 수행했을 때 이벤트를 발생시켜 다양한 콘텐츠 로직을 구현할 수 있습니다.

## 언제 사용하나요?

- 컨트롤러 없이 맨손으로 물체를 잡거나 조작하는 경험을 제공하고 싶을 때
- 특정 손 모양(예: OK 사인, 주먹 쥐기)을 통해 기능을 실행하고 싶을 때
- 실제 손의 관절 위치 데이터를 활용하여 정교한 인터랙션을 구현하고 싶을 때

## 준비사항

- **Viven SDK**: 실험적 기능이 포함된 최신 SDK가 필요합니다.
- **VivenPoseOrGestureInteraction 컴포넌트**: 손 모양을 감지할 오브젝트에 부착되어 있어야 합니다.
- **Hand Pose/Gesture 데이터**: 감지하려는 손 모양이 정의된 `ScriptableObject` 에셋이 필요합니다.

## 진행 순서

1. **컴포넌트 설정**: 상호작용을 원하는 오브젝트에 `VivenPoseOrGestureInteraction` 컴포넌트를 추가하고, 미리 정의된 `Hand Pose` 또는 `Gesture` 에셋을 할당합니다.
2. **대상 손 설정**: `Detect Hand Type`에서 왼쪽(Left), 오른쪽(Right) 중 감지할 손을 선택합니다.
3. **Lua 스크립트 연결**: Lua에서 해당 컴포넌트의 이벤트를 구독하여 동작을 정의합니다.

### 코드 예시

다음은 특정 손 포즈가 감지되었을 때 물체를 잡는 로직을 구현한 예시입니다.

```lua
-- 핸드 트래킹 API 참조
local XRHandAPI = CS.TwentyOz.VivenSDK.ExperimentExtension.Scripts.API.Experiment.XRHandAPI

local grabPoseDetector = nil

function awake()
    -- 컴포넌트 가져오기
    grabPoseDetector = self.gameObject:GetComponent("VivenPoseOrGestureInteraction")
end

function onEnable()
    -- 손 포즈가 감지되었을 때 실행될 리스너 등록
    grabPoseDetector.onPoseOrGesturePerformed:AddListener(onGrabPoseDetected)
end

function onDisable()
    -- 리스너 해제
    grabPoseDetector.onPoseOrGesturePerformed:RemoveListener(onGrabPoseDetected)
end

--- 손 포즈가 감지되었을 때 호출되는 함수
function onGrabPoseDetected()
    -- 현재 핸드 트래킹 모드인지 확인
    if XRHandAPI.GetHandTrackingMode() ~= "None" then
        -- 물체를 강제로 손에 잡게 함 (오른손 기준 예시)
        local grabbable = self.gameObject:GetComponent("VivenGrabbableModule")
        if grabbable then
            -- 세 번째 인자는 IsLeft 여부
            XRHandAPI.ForceGrabHandTracking(grabbable, false, false)
            print("핸드 트래킹으로 물체를 잡았습니다.")
        end
    end
end
```

## 확인 방법

1. 핸드 트래킹을 지원하는 VR 기기(예: Meta Quest 등)를 연결합니다.
2. Viven 앱 내에서 핸드 트래킹 모드를 활성화합니다.
3. 설정한 손 모양을 취했을 때 물체가 잡히거나 지정된 로그가 출력되는지 확인합니다.

## 자주 일어나는 실수

- **이벤트 리스너 미해제**: `onDisable`에서 `RemoveListener`를 호출하지 않으면 메모리 누수나 의도치 않은 동작이 발생할 수 있습니다.
- **잘못된 손 설정**: `Detect Hand Type` 설정이 실제 사용하려는 손과 일치하는지 확인하십시오.
- **유지 시간 부족**: `Hand Pose` 에셋에 설정된 `Minimum Hold Time`보다 짧게 포즈를 취하면 감지되지 않을 수 있습니다.

## 관련 문서

- [입력 시스템 개요](../00-overview.md)
- [VivenLuaBehaviour 활용](../../03-scripting/01-viven-lua-behaviour.md)
- [GrabbableModule 활용](../../03-scripting/04-player-interaction-modules/01-grabbable-module.md)
