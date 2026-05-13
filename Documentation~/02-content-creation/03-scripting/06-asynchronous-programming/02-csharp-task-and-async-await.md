# C# Task와 async/await

## 개요

Viven API 중 일부는 작업 완료에 시간이 걸리는 `Task` 타입을 반환합니다. Lua(VivenScript)에서는 C#의 `await` 키워드를 직접 사용할 수 없으므로, 코루틴 내에서 **Awaiter** 패턴을 사용하여 비동기 작업의 완료를 대기해야 합니다.

## 언제 사용하나요?

- `Player.Mine.TryGrab` 처럼 결과가 즉시 나오지 않고 대기가 필요한 API를 호출할 때
- 네트워크 요청이나 리소스 로딩 등 비동기 작업의 완료를 기다려야 할 때
- 비동기 작업의 결과값(Success 여부 등)을 받아와야 할 때

## 준비사항

- `VivenLuaBehaviour`를 사용하는 Lua 스크립트
- `util.cs_generator` (코루틴 실행용)

## 진행 순서

1. **모듈 가져오기**: 스크립트 상단에서 `local util = require 'xlua.util'`을 호출하여 유틸리티 모듈을 가져옵니다.
2. **비동기 메서드 호출**: `Task`를 반환하는 메서드를 호출하여 변수에 저장합니다.
3. **Awaiter 획득**: Task 객체에서 `GetAwaiter()`를 호출합니다.
4. **완료 대기**: 코루틴 내부에서 `while not awaiter.IsCompleted` 루프를 사용하여 작업이 끝날 때까지 프레임을 양보합니다.
5. **결과 수집**: `awaiter:GetResult()`를 호출하여 최종 결과값을 가져옵니다.

### 코드 예시

다음은 플레이어가 오브젝트를 잡는 시도를 하고, 그 성공 여부를 기다린 후 서버에 알리는 예시입니다.

```lua
local util = require 'xlua.util'

function start()
    self:StartCoroutine(util.cs_generator(function()
        -- 1. 비동기 메서드 호출 (Task 반환)
        local grabTask = Player.Mine.TryGrab(grabbable, isLeft, false, CS.Twoz.Viven.Interactions.Interactor.GrabInterpolation.None)
        if not grabTask then
            logError("Task를 생성할 수 없습니다.")
            return
        end

        -- 2. Awaiter 획득
        local awaiter = grabTask:GetAwaiter()
        if not awaiter then
            logError("Awaiter를 가져올 수 없습니다.")
            return
        end

        -- 3. 작업 완료까지 대기 (IsCompleted 체크)
        while not awaiter.IsCompleted do
            coroutine.yield(WaitForEndOfFrame())
        end

        -- 4. 결과값 가져오기 (GetResult)
        local isSuccess = awaiter:GetResult()
        
        if isSuccess then
            print("잡기 성공!")
        else
            print("잡기 실패.")
        end
        
        -- 결과에 따른 후속 처리 (예: RPC 호출)
        RPC_Server("OnAttemptGrab_Result", isSuccess)
    end))
end
```

## 확인 방법

1. 스크립트 실행 시 로그를 통해 "잡기 성공" 또는 "실패" 메시지가 비동기적으로 출력되는지 확인합니다.
2. `while` 루프 대기 중에 게임이 멈추지 않고 다른 로직이 정상적으로 동작하는지 확인합니다.

## 자주 일어나는 실수

- **코루틴 외부에서 대기 시도**: `while` 루프를 통한 대기는 반드시 `util.cs_generator`로 실행되는 코루틴 내부에서 이루어져야 합니다. 메인 스레드에서 직접 루프를 돌리면 게임이 멈춥니다(Freezing).
- **GetResult 중복 호출**: `GetResult()`는 작업이 완료된 후 한 번만 호출하는 것이 안전합니다.
- **IsCompleted 체크 누락**: 완료되지 않은 상태에서 `GetResult()`를 호출하면 오류가 발생하거나 잘못된 값을 얻을 수 있습니다.

## 관련 문서

- [코루틴 (Coroutine)](01-unity-coroutines.md)
- [그랩 모듈 (Grabbable Module)](../04-player-interaction-modules/01-grabbable-module.md)
