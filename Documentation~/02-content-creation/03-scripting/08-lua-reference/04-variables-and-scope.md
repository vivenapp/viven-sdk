# 변수 및 범위 (Variables and Scope)

## 개요

VivenScript에서 변수를 선언하는 방식에 따라 해당 변수가 외부(Inspector 등)에 노출되는지, 아니면 스크립트 내부에서만 사용되는지 결정됩니다. 또한 전역 공간을 활용하는 방법을 설명합니다.

## 스크립트 내 변수 범위

VivenScript의 모든 변수 선언은 해당 스크립트 파일 내로 국한됩니다.

### 전역 변수 (Global Variable) -> Public
스크립트의 최상위 레벨에서 `local` 키워드 없이 선언된 변수는 Viven 시스템에 의해 **Public** 변수로 취급됩니다.
- Unity Inspector에서 값을 확인하거나 수정할 수 있습니다.
- 다른 시스템이나 스크립트에서 접근할 수 있는 후보가 됩니다.

```lua
-- Public 변수 (전역 선언)
speed = 10.5
targetName = "Player1"
```

### 지역 변수 (Local Variable) -> Private
`local` 키워드를 사용하여 선언된 변수는 **Private** 변수로 취급됩니다.
- 해당 스크립트(또는 블록) 내부에서만 접근 가능합니다.
- 외부(Inspector 등)에 노출되지 않습니다.

```lua
-- Private 변수 (지역 선언)
local internalCounter = 0
local tempValue = "hidden"
```

## 전역 공간 활용 (self.Global)

VivenScript는 각 스크립트가 독립된 환경을 가집니다. 만약 여러 스크립트가 공유해야 하는 실제 전역 변수를 선언하고 싶다면 `self.Global`을 사용해야 합니다.

### 전역 변수 등록 및 사용
```lua
-- 스크립트 A에서 전역 변수 설정
self.Global.sharedScore = 100

-- 스크립트 B에서 전역 변수 읽기
local score = self.Global.sharedScore
print("공유된 점수: " .. score)
```

## 변수 명명 규칙

- **PascalCase**: Public 변수(전역 선언)에 권장합니다.
- **camelCase**: Private 변수(지역 선언)나 함수 인자에 권장합니다.
- **_underscore**: 사용하지 않는 인자나 내부 루프 변수에 사용합니다.

## 자주 일어나는 실수

- **실수로 local 생략**: 내부에서만 쓸 변수인데 `local`을 생략하면 의도치 않게 Public 변수가 되어 Inspector를 어지럽히거나 성능에 영향을 줄 수 있습니다.
- **스크립트 간 직접 접근 시도**: Lua의 표준 `_G`를 통한 전역 접근은 VivenScript 환경에서 제한될 수 있으므로, 반드시 `Global`을 사용하세요.
- **Global에 self 등록**: `Global.MyManager = self`는 VivenScript가 아닌 VivenLuaBehaviour(C# 객체)를 등록하게 됩니다. 스크립트 자신을 등록하려면 `Global.MyManager = __script`를 사용하세요.

## 관련 문서

- [형식 (Types)](01-types.md)
- [함수 및 이벤트 (Functions and Events)](06-functions-and-events.md)
