# 데이터 구조 (Data Structures)

## 개요

Lua의 유일하고 강력한 데이터 구조인 **Table**과, xLua를 통해 연동되는 C#의 **Struct**, **Array**, **List** 사용 방법을 설명합니다.

## Lua Table

Table은 배열(Array), 리스트(List), 딕셔너리(Dictionary), 객체(Object)의 역할을 모두 수행합니다.

### 배열로 사용하기
Lua의 배열 인덱스는 **1부터 시작**합니다.

```lua
local colors = {"Red", "Green", "Blue"}
print(colors[1]) -- "Red"
table.insert(colors, "Yellow") -- 요소 추가
```

### 딕셔너리/객체로 사용하기
키-값 쌍을 저장합니다.

```lua
local player = {
    name = "Alice",
    level = 10,
    hp = 100
}
print(player.name) -- "Alice"
player.hp = 90
```

## C# 구조체 (Struct) 연동

C#의 `Vector3`, `Quaternion` 등 구조체는 Lua에서 테이블처럼 생성하거나 직접 호출할 수 있습니다.

### 생성 및 사용
```lua
-- 생성자 호출
local pos = CS.UnityEngine.Vector3(1, 2, 3)
local rot = CS.UnityEngine.Quaternion.identity

-- 필드 접근
pos.x = 10
print(pos.y)
```

### 테이블을 통한 자동 변환
xLua는 특정 조건에서 Lua 테이블을 C# 구조체나 클래스로 자동 변환해줍니다.

```lua
-- C# 함수가 Vector3를 인자로 받을 때 테이블로 전달 가능
self.transform.localPosition = { x = 0, y = 5, z = 0 }
```

## C# 배열 및 리스트 (Array & List)

### C# 배열 접근
C# 배열은 Lua 테이블과 달리 **0번 인덱스**부터 시작할 수 있음에 유의하세요 (xLua 설정에 따라 다를 수 있으나 기본적으로 C# 규칙을 따름).

```lua
-- C#에서 넘어온 배열 items
print(items.Length)
print(items[0]) -- 첫 번째 요소
```

### C# 리스트 사용
```lua
local list = CS.System.Collections.Generic.List(CS.System.String)()
list:Add("Item 1")
print(list[0])
```

## 자주 일어나는 실수

- **인덱스 혼동**: Lua 테이블은 1부터 시작하지만, C# 객체(Array, List)는 0부터 시작합니다.
- **값 복사 vs 참조**: Lua 테이블을 C# 객체에 대입할 때, xLua는 값을 복사하여 새 객체를 생성하는 경우가 많습니다. 대량의 데이터를 매 프레임 변환하는 것은 성능에 좋지 않습니다.

## 관련 문서

- [형식 (Types)](01-types.md)
- [기능 (Features)](03-features.md)
