# 코드 재사용과 모듈화

## 개요

Viven Script에서는 `require`를 통해 Lua 모듈을 불러와 여러 스크립트에서 공통 로직을 재사용할 수 있습니다. 맵 환경(`VivenMapEnvironment`)에 등록한 ModuleScript는 맵 로드 시점에 미리 준비되며, 씬 내 모든 `VivenLuaBehaviour`에서 `require(모듈이름)`으로 가져와 사용할 수 있습니다. [Viven Script 가이드](01-viven-lua-behaviour.md)에서 안내하는 Lua 기반 스크립팅과 함께 코드를 모듈화하는 데 유용합니다.

## 언제 사용하나요?

- 여러 오브젝트에서 같은 유틸 함수(거리 계산, 포맷 변환 등)를 쓰고 싶을 때
- 게임 로직을 기능별로 나누어 관리하고 싶을 때
- 한 번 작성한 Lua 코드를 여러 스크립트에서 재사용하고 싶을 때
- `ImportLuaScript`처럼 Injection 없이, 맵 전체에서 공통 모듈을 쓰고 싶을 때

## 준비사항

- Viven SDK가 포함된 Unity 프로젝트
- Viven 맵 환경이 있는 씬 (`VivenMapEnvironment` 컴포넌트가 씬에 존재)
- 재사용할 Lua 코드를 담은 `.lua` 파일 또는 VivenScript 에셋

## 진행 순서

### 1. 모듈용 Lua 스크립트 작성

모듈로 쓸 Lua 스크립트는 **테이블을 반환**하는 형태로 작성합니다. 반환한 테이블이 `require`의 결과로 전달됩니다.

```lua
-- utils.lua: 유틸리티 함수 모듈
local M = {}

function M.distance(a, b)
    local dx = a.x - b.x
    local dy = a.y - b.y
    local dz = a.z - b.z
    return math.sqrt(dx * dx + dy * dy + dz * dz)
end

function M.formatTime(seconds)
    local m = math.floor(seconds / 60)
    local s = math.floor(seconds % 60)
    return string.format("%02d:%02d", m, s)
end

return M
```

### 2. VivenScript 에셋으로 준비

1. Project 창에서 `.lua` 파일을 만들거나, 기존 `.lua` 파일을 프로젝트에 추가합니다.
2. `VivenLuaImporter`가 자동으로 VivenScript 에셋으로 변환합니다.
3. Inspector에서 `scriptString` 내용을 확인·수정합니다.
4. **에셋 이름**을 기억합니다. `require`에 넣을 모듈 이름은 이 에셋 이름과 같아야 합니다.

> **참고**: `utils.lua` 파일이면 에셋 이름은 보통 `utils`입니다. Project 창에서 에셋을 선택했을 때 표시되는 이름을 사용하세요.

### 3. VivenMapEnvironment에 ModuleScript 등록

1. 맵 씬에서 `VivenMapEnvironment` 컴포넌트가 붙은 GameObject를 선택합니다.
2. Inspector에서 **Module Scripts** (또는 `moduleScripts`) 목록을 펼칩니다.
3. **+** 버튼으로 항목을 추가하고, 위에서 만든 VivenScript 에셋을 드래그하여 할당합니다.
4. 맵 로드 시 이 목록에 있는 스크립트가 `require`로 불러 쓸 수 있도록 등록됩니다.

### 4. 다른 스크립트에서 require로 사용

`VivenLuaBehaviour`가 붙은 오브젝트의 Lua 스크립트에서 `require`로 모듈을 가져옵니다.

```lua
function start()
    local utils = require("utils")
    if utils then
        local pos1 = { x = 0, y = 0, z = 0 }
        local pos2 = { x = 3, y = 4, z = 0 }
        Debug.Log("거리: " .. utils.distance(pos1, pos2))
        Debug.Log("시간: " .. utils.formatTime(125))
    end
end
```

`require`에 넣는 문자열은 VivenMapEnvironment에 등록한 VivenScript 에셋의 **이름**과 정확히 일치해야 합니다.

## require vs ImportLuaScript

| 방식 | 등록 방법 | 사용 시점 |
|------|-----------|-----------|
| `require(이름)` | VivenMapEnvironment의 Module Scripts에 VivenScript 추가 | 맵 로드 시 자동 등록, 맵 내 모든 스크립트에서 사용 |
| `ImportLuaScript(스크립트)` | VivenLuaBehaviour의 Injection에 VivenScript 주입 | 해당 오브젝트 스크립트에서만 사용, 스크립트별로 주입 필요 |

맵 전체에서 공통으로 쓰는 유틸·헬퍼는 `require`와 ModuleScript를, 특정 오브젝트에만 필요한 스크립트는 `ImportLuaScript`와 Injection을 사용하는 것이 좋습니다.

## 확인 방법

1. **Play** 모드로 맵 씬 실행
2. Console에 `Debug.Log` 출력이 예상대로 나오는지 확인
3. `require` 결과가 `nil`이 아닌지, 모듈 함수가 호출되는지 확인
4. 여러 오브젝트의 스크립트에서 같은 모듈을 `require`해 동작하는지 확인

## 자주 일어나는 실수

- **모듈 이름 불일치**: `require("utils")`에서 `"utils"`는 VivenScript 에셋 이름과 같아야 합니다. 대소문자와 띄어쓰기도 정확히 맞춰야 합니다.
- **ModuleScript 미등록**: VivenMapEnvironment의 Module Scripts 목록에 추가하지 않으면 `require` 시 모듈을 찾지 못해 에러가 납니다.
- **return 누락**: 모듈 스크립트 끝에 `return M`(또는 반환할 테이블)을 넣지 않으면 `require` 결과가 비어 있을 수 있습니다.
- **VivenMapEnvironment 없음**: 맵 씬에 `VivenMapEnvironment`가 없거나 비활성화되어 있으면 ModuleScript가 등록되지 않습니다.
- **방 전환 시 초기화**: 맵을 바꾸면 이전 맵의 ModuleScript는 해제됩니다. 새 맵의 VivenMapEnvironment에 필요한 모듈을 다시 등록해야 합니다.

## 관련 문서

- [VivenScript와 LuaBehaviour 사용하기](01-viven-lua-behaviour.md) — 기본 스크립팅, Injection, ImportLuaScript
