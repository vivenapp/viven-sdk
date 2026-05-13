# 코루틴 (Coroutine)

## 개요

코루틴은 실행을 일시 중단하고 Unity에 제어권을 돌려주었다가, 특정 조건(시간 경과 등)이 충족되면 중단된 지점부터 다시 실행할 수 있는 기능입니다. VivenScript에서 시간 지연이 필요한 로직이나 순차적인 비동기 작업을 구현할 때 필수적으로 사용됩니다.

## 언제 사용하나요?

- 특정 시간 동안 기다린 후 다음 로직을 실행해야 할 때 (예: 스킬 재사용 대기시간)
- 여러 프레임에 걸쳐 작업을 나누어 처리해야 할 때
- 특정 조건이 만족될 때까지 대기해야 할 때 (예: 애니메이션 재생 종료 대기)

## 준비사항

- `VivenLuaBehaviour`를 사용하는 Lua 스크립트
- `xlua.util` 모듈 (XLua 기본 제공 유틸리티)

## 진행 순서

1. **모듈 가져오기**: 스크립트 상단에서 `local util = require('xlua.util')`을 호출합니다.
2. **코루틴 함수 정의**: 비동기로 동작할 로직을 함수 내부에 작성합니다.
3. **Yield 사용**: 대기가 필요한 지점에서 `coroutine.yield`를 호출합니다.
4. **코루틴 시작**: `startCoroutine`과 `util.cs_generator`를 사용하여 코루틴을 실행합니다.

### 코드 예시

다음은 공격 후 일정 시간 동안 재공격이 불가능하도록 대기시간을 처리하는 예시입니다.

```lua
local util = require('xlua.util')

local attackCooldown = 2.0
local canAttack = true

function OnInteract()
    if not canAttack then return end

    canAttack = false
    Debug.Log("공격 수행!")

    -- 코루틴 시작
    startCoroutine(util.cs_generator(function()
        coroutine.yield(WaitForSeconds(attackCooldown))
        canAttack = true
        Debug.Log("다시 공격 가능!")
    end))
end
```

### 코루틴 중지

핸들을 저장하면 나중에 중단할 수 있습니다.

```lua
local util = require('xlua.util')
local handle = nil

function start()
    handle = startCoroutine(util.cs_generator(function()
        while true do
            Debug.Log("반복 실행")
            coroutine.yield(WaitForSeconds(1))
        end
    end))
end

function onDestroy()
    if handle then
        stopCoroutine(handle)
    end
end
```

### Yield 명령어

VivenScript에서는 아래 Yield 명령어를 `CS.` 없이 바로 사용할 수 있습니다.

| 명령어 | 설명 |
|--------|------|
| `WaitForSeconds(초)` | 지정된 시간 동안 대기 |
| `WaitForSecondsRealtime(초)` | Time.timeScale 영향 없이 대기 |
| `WaitForEndOfFrame()` | 프레임 렌더링 완료까지 대기 |
| `WaitForFixedUpdate()` | 다음 FixedUpdate까지 대기 |
| `WaitUntil(function)` | 조건이 true가 될 때까지 대기 |
| `WaitWhile(function)` | 조건이 true인 동안 대기 |

## 확인 방법

1. 스크립트를 저장하고 Viven 월드에서 해당 오브젝트와 상호작용합니다.
2. 로그 메시지가 즉시 출력되는지, 그리고 지정한 시간이 지난 후에 다음 메시지가 출력되는지 확인합니다.

## 자주 일어나는 실수

### util.cs_generator 누락 (가장 흔한 실수)

> [!CAUTION]
> Lua 함수를 `startCoroutine`에 직접 전달하면 **반드시 오류가 발생합니다.**
> `xlua.util`의 `cs_generator`가 Lua 함수를 C# `IEnumerator`로 변환하는 필수 과정입니다.

```lua
local util = require('xlua.util')

-- ✅ 올바른 사용법
startCoroutine(util.cs_generator(MyCoroutine))

-- ✅ 인라인 함수도 가능
startCoroutine(util.cs_generator(function()
    coroutine.yield(WaitForSeconds(1))
    Debug.Log("완료")
end))

-- ❌ 잘못된 사용법 1: 함수를 직접 호출하여 전달
startCoroutine(MyCoroutine())

-- ❌ 잘못된 사용법 2: cs_generator 없이 전달
startCoroutine(MyCoroutine)
```

### 기타 주의사항

- **무한 루프 주의**: 코루틴 내부에서 `while true` 루프를 사용할 경우, 반드시 내부에 `coroutine.yield`를 포함하여 Unity가 멈추지 않도록 해야 합니다.
- **오브젝트 비활성화**: 코루틴이 실행 중인 GameObject가 비활성화(`Active = false`)되면 코루틴도 함께 중단됩니다.
- **수명 관리**: 코루틴 핸들을 저장하고, `onDestroy`에서 `stopCoroutine(handle)`로 정리하세요.

---

## vivenCoroutine (Experimental)

> [!NOTE]
> `vivenCoroutine`은 실험적 기능입니다. `util.cs_generator` 래핑을 자동 처리하여 코루틴 사용을 간소화합니다.

```lua
-- vivenCoroutine은 Lua 함수를 직접 전달
vivenCoroutine.start(function()
    Debug.Log("공격!")
    coroutine.yield(WaitForSeconds(2))
    Debug.Log("2초 후 재공격 가능")
end)

-- 인자 전달
vivenCoroutine.start(attack, "적", 50)

-- 중단
local handle = vivenCoroutine.start(myRoutine)
vivenCoroutine.stop(handle)
```

### startCoroutine과의 비교

| | `startCoroutine` | `vivenCoroutine.start` |
|---|---|---|
| 인자 | `IEnumerator` (C# 객체) | Lua 함수 |
| `util.cs_generator` | 직접 호출 필요 | 내부 자동 처리 |
| 상태 | **안정 (권장)** | **Experimental** |

> `vivenCoroutine`은 각 `VivenLuaBehaviour`마다 독립 동작하며, GameObject 파괴 시 자동 정리됩니다.

## 관련 문서

- [비동기 프로그래밍 개요](00-overview.md)
- [C# Task와 async/await](02-csharp-task-and-async-await.md)
