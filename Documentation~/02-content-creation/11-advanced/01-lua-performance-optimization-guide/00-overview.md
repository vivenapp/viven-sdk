# Lua 성능 최적화 가이드

## 개요

Viven 플랫폼에서 Lua 스크립트를 작성할 때 성능 저하를 방지하고 최적의 사용자 경험을 제공하기 위한 가이드입니다.

## 언제 사용하나요?

- 복잡한 로직을 Lua로 구현할 때
- 다수의 오브젝트를 제어하는 스크립트를 작성할 때
- 모바일 및 VR 기기에서 프레임 드랍이 발생할 때

## 준비사항

- Viven SDK가 설치된 Unity 프로젝트
- 성능 측정을 위한 `Advanced FPS Counter` (단축키 **F9** 키)

## 최적화 방법

### 1. 매 프레임 할당 지양

`Update` 함수 내에서 테이블을 생성하거나 문자열을 연결하는 행위는 가비지 컬렉션(GC) 부하를 유발합니다.

```lua
-- ❌ 나쁜 예: 매 프레임 테이블 생성
function Update()
    local pos = { x = 1, y = 2, z = 3 }
    -- 로직 수행
end

-- ✅ 좋은 예: 테이블 재사용
local pos = { x = 0, y = 0, z = 0 }
function Update()
    pos.x = 1
    pos.y = 2
    pos.z = 3
    -- 로직 수행
end
```

### 2. 로컬 변수 활용

전역 변수(`Global`)보다 로컬 변수(`local`) 접근 속도가 훨씬 빠릅니다. 자주 사용하는 외부 함수나 모듈은 로컬 변수에 캐싱하여 사용하세요.

```lua
-- ✅ 좋은 예: 자주 쓰는 함수 캐싱
local sin = math.sin

function Update()
    local val = sin(os.clock())
    -- ...
end
```

> **Lua 로그 사용법**: Lua에서는 `Debug` 전역 객체를 사용하여 로그를 출력합니다.
> 사용 가능한 함수: `Debug.Log(msg)`, `Debug.LogInfo(msg)`, `Debug.LogWarning(msg)`, `Debug.LogError(msg)`
> **주의: `VivenLog`은 C# 전용 API입니다.** Lua에서 직접 호출하면 nil 에러가 발생합니다. 자세한 내용은 [로그 확인 및 분석](../02-emmylua-debugger-connection/02-viven-sdk-log-review-and-analysis.md)을 참조하세요.

### 3. 빈번한 C# Bridge 호출 최소화

Lua에서 C# API를 호출하는 것은 비용이 발생합니다. 가능한 Lua 내부에서 계산을 처리하고, 결과값만 C#으로 전달하는 것이 좋습니다.

### 4. 문자열 연결 최적화

다수의 문자열을 연결할 때는 `..` 연산자 대신 `table.concat`을 활용하세요.

```lua
-- ✅ 좋은 예: 많은 문자열 연결 시
local parts = {}
for i = 1, 100 do
    table.insert(parts, "Item " .. i)
end
local result = table.concat(parts, ", ")
```

## 성능 측정 방법

### 1. 실행 시간 측정

`os.clock()`을 사용하여 특정 로직의 실행 시간을 초 단위로 측정할 수 있습니다.

```lua
local startTime = os.clock()

-- 측정할 로직 시작
for i = 1, 10000 do
    -- 복잡한 계산
end
-- 측정할 로직 끝

local endTime = os.clock()
print(string.format("Execution time: %.4f seconds", endTime - startTime))
```

### 2. 메모리 사용량 확인

`collectgarbage("count")`를 통해 현재 Lua 가상 머신이 사용하는 메모리 양(KB)을 확인할 수 있습니다.

```lua
local memBefore = collectgarbage("count")
-- 로직 수행
local memAfter = collectgarbage("count")
print("Memory delta: " .. (memAfter - memBefore) .. " KB")
```

## 자주 일어나는 실수

- **무분별한 Update 사용**: 모든 로직을 `Update`에 넣지 말고, 이벤트 기반(`OnTriggerEnter` 등)이나 코루틴을 활용하세요.
- **클로저 남발**: 함수 내부에서 익명 함수를 생성하면 매번 새로운 함수 객체가 생성되어 메모리를 소모합니다.

## 관련 문서

- [FPS 및 성능 지표 확인 (단축키)](01-fps-and-performance-metrics-shortcuts.md)
- [Viven SDK 로그 확인 및 분석](../02-emmylua-debugger-connection/02-viven-sdk-log-review-and-analysis.md)
