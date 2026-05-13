# Viven SDK 로그 확인 및 분석

## 개요

VivenScript(Lua)에서 로그를 출력하려면 `Debug` 전역 객체를 사용합니다. 이 객체는 내부적으로 `DebugBridge`를 통해 VivenLogger(NLog 기반)로 연결되므로, Unity 콘솔과 Viven 로그 시스템 양쪽에서 확인할 수 있습니다.

## Lua에서 사용 가능한 로그 API

| 함수 | 로그 레벨 | 용도 |
|------|----------|------|
| `Debug.Log(message)` | Debug | 일반 디버그 로그 |
| `Debug.LogInfo(message)` | Info | 정보성 로그 |
| `Debug.LogWarning(message)` | Warning | 경고 로그 |
| `Debug.LogError(message)` | Error | 에러 로그 |
| `Debug.LogException(message)` | Error | 예외 로그 |

### 기본 사용법

```lua
function start()
    Debug.Log("[MyScript] 초기화 완료")
end

function onGrab()
    Debug.Log("[MyScript] 오브젝트가 잡혔습니다")
end

function onRelease()
    Debug.Log("[MyScript] 오브젝트가 놓였습니다")
end
```

### 레벨별 사용 예시

```lua
-- 일반 디버그 정보
Debug.Log("[GameController] 상태 전이: Idle → Playing")

-- 정보성 로그 (시스템 이벤트)
Debug.LogInfo("[GameController] 게임 시작, 라운드 1")

-- 경고 (비정상적이지만 동작에 문제 없음)
Debug.LogWarning("[GameController] 예상치 못한 상태에서 호출됨: " .. tostring(currentState))

-- 에러 (동작 실패)
Debug.LogError("[GameController] 필수 오브젝트를 찾을 수 없습니다")
```

### 문자열 포매팅

```lua
-- 문자열 연결
Debug.Log("[RpsGame] 라운드 " .. tostring(round) .. " 시작")

-- string.format 사용 (권장)
Debug.Log(string.format("[RpsGame] 라운드 %d | 결과: %s", round, result))
```

## 자주 하는 실수

### 1. `VivenLog` 직접 호출 (잘못됨)

```lua
-- ❌ 잘못된 사용: VivenLog은 C# 전용 API이며 Lua에서 직접 접근할 수 없음
VivenLog.Debug("메시지")    -- attempt to index a nil value 에러 발생
VivenLog.Log("메시지")      -- 에러 발생
VivenLog.Info("메시지")     -- 에러 발생

-- ✅ 올바른 사용: Debug 전역 객체를 통해 호출
Debug.Log("메시지")
Debug.LogError("에러 메시지")
```

`VivenLog`은 C# 코드에서만 사용하는 정적 클래스입니다. Lua 스크립트에서는 반드시 `Debug.Log`, `Debug.LogError` 등을 사용하세요.

### 2. `print()` 사용 (비권장)

```lua
-- ❌ 비권장: Viven 로그 시스템을 우회하므로 로그 관리가 어려움
print("메시지")

-- ✅ 권장: Viven 로그 시스템을 통한 출력
Debug.Log("메시지")
```

## 로그 확인 방법

1. **Unity 콘솔**: 에디터 하단의 Console 창에서 실시간 확인
2. **Viven 로그 뷰어**: 런타임에서 F9 키로 FPS 카운터 및 로그 뷰어 토글
3. **로그 파일**: NLog 설정에 따라 파일로도 기록됨

## 내부 구조

Lua의 `Debug.Log("msg")`는 다음 경로로 처리됩니다:

```
Lua: Debug.Log("msg")
  → C#: DebugBridge.Log(message)
    → NLog: LogManager.GetLogger("Lua.Log")
      → Unity Console + 로그 파일
```

`DebugBridge`는 `[LuaCallCSharp]` 어트리뷰트가 적용된 정적 클래스로, xLua를 통해 Lua 전역 테이블에 자동 노출됩니다.
