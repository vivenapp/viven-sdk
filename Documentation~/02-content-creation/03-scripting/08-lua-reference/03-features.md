# 기능 (Features)

## 개요

VivenScript에서 xLua를 통해 C# 코드를 호출할 때의 특수한 기능들과 주의사항을 설명합니다. 특히 메서드 오버로딩, 연산자 지원 등 고급 기능을 다룹니다.

## C# 클래스 및 정적 멤버 접근

모든 C# 관련 기능은 `CS` 네임스페이스 아래에 위치합니다.

```lua
-- 정적 프로퍼티 읽기
local deltaTime = CS.UnityEngine.Time.deltaTime

-- 정적 메서드 호출
local obj = CS.UnityEngine.GameObject.Find("MyObject")
```

## 메서드 오버로딩 (Method Overloading)

C#에서 이름은 같지만 매개변수가 다른 여러 메서드가 있을 때, Lua는 전달된 인자의 타입에 따라 적절한 메서드를 호출하려고 시도합니다.

### 주의사항
- **타입 모호성**: Lua의 `number`는 C#의 `int`, `float`, `double` 모두에 대응될 수 있습니다. 인자가 숫자일 경우 C#에서 어떤 오버로딩이 호출될지 모호할 수 있습니다.
- **성능**: 오버로딩된 메서드를 호출할 때 xLua는 적절한 메서드를 찾기 위해 런타임에 타입을 체크하므로, 오버로딩이 없는 메서드보다 약간의 오버헤드가 발생합니다.

## 연산자 오버로딩

C#에서 정의된 연산자(+, -, *, /, == 등)를 Lua에서도 사용할 수 있습니다.

```lua
local v1 = CS.UnityEngine.Vector3(1, 1, 1)
local v2 = CS.UnityEngine.Vector3(2, 2, 2)
local v3 = v1 + v2 -- Vector3의 + 연산자 호출
```

## 확장 메서드 (Extension Methods)

C#에서 정의된 확장 메서드도 Lua에서 일반 멤버 메서드처럼 호출할 수 있습니다. 단, 해당 확장 메서드가 포함된 클래스가 xLua에 의해 생성 코드(Gen Code)에 포함되어 있어야 합니다.

## 제네릭 메서드 (Generic Methods)

Lua에서 제네릭 메서드를 직접 호출하는 것은 제한적입니다. 보통 C#에서 특정 타입을 지정한 래퍼 메서드를 만들거나, 확장 메서드 형태로 제공하여 사용합니다.

## LuaCallCSharp 유의 사항

- **성능**: Lua에서 C#을 호출하는 것은 경계를 넘나드는 작업이므로 비용이 발생합니다. 매 프레임 수천 번씩 호출하는 루프는 피해야 합니다.
- **GC(Garbage Collection)**: Lua에서 생성한 C# 객체는 Lua의 GC와 C#의 GC가 협력하여 관리합니다. 너무 많은 임시 C# 객체를 Lua에서 생성하면 메모리 압박이 발생할 수 있습니다.

## 관련 문서

- [함수 및 이벤트 (Functions and Events)](06-functions-and-events.md)
- [데이터 구조 (Data Structures)](02-data-structures.md)
