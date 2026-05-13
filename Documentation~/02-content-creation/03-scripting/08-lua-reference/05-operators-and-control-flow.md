# 연산자 및 제어 구조 (Operators and Control Flow)

## 개요

Lua 5.5에서 제공하는 표준 연산자와 코드의 흐름을 제어하는 조건문, 반복문 사용 방법을 설명합니다.

## 연산자 (Operators)

### 산술 연산자
- `+`, `-`, `*`, `/` (부동 소수점 나눗셈)
- `//` (정수 나눗셈/Floor division)
- `%` (나머지)
- `^` (제곱)
- `-` (단항 마이너스)

### 비교 연산자
- `==`, `~=` (같지 않음)
- `<`, `>`, `<=`, `>=`

### 논리 연산자
- `and`, `or`, `not`
- **주의**: Lua에서 `false`와 `nil`만 거짓으로 취급하며, 숫자 `0`이나 빈 문자열 `""`은 **참(true)**입니다.

### 기타 연산자
- `..` (문자열 연결)
- `#` (길이 연산자 - 문자열이나 테이블의 크기)

## 제어 구조 (Control Flow)

### 조건문 (if)
```lua
if score > 90 then
    print("A")
elseif score > 80 then
    print("B")
else
    print("C")
end
```

### 반복문 (while, repeat, for)

#### while
```lua
local i = 1
while i <= 5 do
    print(i)
    i = i + 1
end
```

#### repeat-until (do-while과 유사)
```lua
local i = 1
repeat
    print(i)
    i = i + 1
until i > 5
```

#### 숫자형 for
```lua
-- for 변수 = 시작, 끝, [증감]
for i = 1, 10, 2 do
    print(i) -- 1, 3, 5, 7, 9
end
```

#### 범용 for (ipairs, pairs)
```lua
local list = {"a", "b", "c"}
for index, value in ipairs(list) do
    print(index, value)
end

local dict = { name = "Viven", type = "Platform" }
for key, value in pairs(dict) do
    print(key, value)
end
```

## break와 return

- `break`: 가장 가까운 루프를 탈출합니다.
- `return`: 함수 실행을 종료하고 값을 반환합니다.

## 관련 문서

- [함수 및 이벤트 (Functions and Events)](06-functions-and-events.md)
- [변수 및 범위 (Variables and Scope)](04-variables-and-scope.md)
