# 형식 (Types)

## 개요

Lua는 동적 타이핑 언어로, 변수 자체가 아닌 값(Value)이 타입을 가집니다. VivenScript에서 사용하는 주요 타입과 C#과의 매핑 관계를 설명합니다.

## Lua 기본 타입

Lua 5.5에서 지원하는 8가지 기본 타입입니다.

| 타입 | 설명 | 예시 |
| :--- | :--- | :--- |
| `nil` | 값이 없음을 나타냄 (C#의 null과 유사) | `local x = nil` |
| `boolean` | 논리값 | `true`, `false` |
| `number` | 정수 및 부동 소수점 숫자 (Lua 5.3+ 부터 내부적으로 구분) | `10`, `3.14` |
| `string` | 문자열 | `"Hello Viven"`, `'Lua Script'` |
| `table` | 유일한 데이터 구조 (배열, 딕셔너리, 객체 역할) | `{ key = "value" }` |
| `function` | 실행 가능한 함수 | `function() ... end` |
| `userdata` | C# 객체 등 외부 데이터를 담는 타입 | `CS.UnityEngine.GameObject` |
| `thread` | 코루틴(Coroutine) 실행을 위한 타입 | `coroutine.create(...)` |

## C# 데이터 타입 매핑

xLua를 통해 C# API를 호출할 때, 다음과 같이 타입이 자동 변환됩니다.

### 숫자형 (Number)
- C#의 `int`, `float`, `double`, `long` 등 모든 숫자 타입은 Lua의 `number`로 매핑됩니다.
- **주의**: Lua 5.5는 64비트 정수를 지원하지만, 부동 소수점 정밀도 문제에 유의해야 합니다.

### 문자열 (String)
- C#의 `string`은 Lua의 `string`으로 매핑됩니다.

### 불리언 (Boolean)
- C#의 `bool`은 Lua의 `boolean`으로 매핑됩니다.

### 객체 (Object/Userdata)
- C#의 클래스 인스턴스나 구조체는 Lua에서 `userdata`로 취급됩니다.
- Lua에서 `CS.UnityEngine.GameObject`와 같이 접근하여 사용할 수 있습니다.

## C# 열거형 (Enum) 사용

열거형은 정적 멤버처럼 접근하거나 문자열/숫자로부터 변환할 수 있습니다.

```lua
-- 직접 접근
myLight.type = CS.UnityEngine.LightType.Point

-- 문자열/숫자에서 변환 (__CastFrom)
local type = CS.UnityEngine.LightType.__CastFrom("Directional")
```

## 타입 확인 방법

`type()` 함수를 사용하여 값의 타입을 문자열로 얻을 수 있습니다.

```lua
print(type("Viven"))    -- "string"
print(type(100))        -- "number"
print(type({}))         -- "table"
print(type(CS.UnityEngine.Vector3(0, 0, 0))) -- "userdata"
```

## 관련 문서

- [데이터 구조 (Data Structures)](02-data-structures.md)
- [변수 및 범위 (Variables and Scope)](04-variables-and-scope.md)
