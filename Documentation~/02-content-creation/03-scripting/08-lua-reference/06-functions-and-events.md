# 함수 및 이벤트 (Functions and Events)

## 개요

Lua의 함수 정의 방법과 xLua를 통한 C# **Delegate**, **Event** 연동 방법을 설명합니다. VivenScript에서의 이벤트 리스너 등록 방식도 포함합니다.

## Lua 함수 정의

```lua
-- 일반 함수
function add(a, b)
    return a + b
end

-- 지역 함수 (스크립트 내부용)
local function internalLog(msg)
    Debug.Log("[Internal] " .. msg)
end
```

## C# 델리게이트 (Delegate) 연동

C#의 델리게이트 프로퍼티에 Lua 함수를 직접 할당할 수 있습니다.

```lua
-- C# 객체의 델리게이트 프로퍼티에 할당
myObject.onCallback = function(n)
    print("Callback received: " .. n)
end
```

## C# 이벤트 (Event) 구독

C#의 `event`는 Lua에서 직접 `+=` 연산자를 쓸 수 없으므로, xLua가 제공하는 방식이나 Viven SDK의 래퍼를 사용합니다.

### xLua 기본 방식
```lua
-- 이벤트 추가 (+)
myObject:TestEvent("+", luaCallback)

-- 이벤트 제거 (-)
myObject:TestEvent("-", luaCallback)
```

### UnityEvent 사용 방식
Unity의 `UnityEvent`(예: Button의 `onClick`)는 `AddListener`를 통해 Lua 함수를 직접 등록할 수 있습니다.

```lua
function start()
    -- completeButton: injection으로 주입된 Button 컴포넌트
    -- CompleteOrder: 호출될 Lua 함수
    completeButton.onClick:AddListener(CompleteOrder)
end

function CompleteOrder()
    Debug.Log("주문이 완료되었습니다.")
end

function onDestroy()
    -- 스크립트 종료 시 리스너를 제거하는 것이 좋습니다.
    completeButton.onClick:RemoveListener(CompleteOrder)
end
```

## 자주 일어나는 실수

- **이벤트 해제 누락**: `AddListener` 등으로 등록한 이벤트는 `onDestroy` 등에서 반드시 제거해야 메모리 누수를 방지할 수 있습니다.
- **콜론(:)과 점(.)의 구분**: C# 멤버 메서드를 호출할 때는 반드시 콜론(`:`)을 사용하여 `self` 인자가 전달되도록 하세요. (예: `obj:Method()`는 `obj.Method(obj)`와 같음)

## 관련 문서

- [VivenLuaBehaviour 활용](../01-viven-lua-behaviour.md)
- [변수 및 범위 (Variables and Scope)](04-variables-and-scope.md)
