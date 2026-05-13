# Lua Language Server

## 개요

Lua Language Server(LuaLS)와 Viven LLS Plugin을 설치하면 VS Code 계열 IDE에서 VivenScript 작성 시 다음 기능을 사용할 수 있습니다.

- Viven API 자동완성 (`Player.Mine`, `Room.SetRoomProp` 등)
- C# 타입 추론 및 메서드 시그니처 표시
- 실시간 문법 검사 및 경고
- EmmyLua 스타일 Annotation 지원

## 지원 IDE

다음 IDE에서 LuaLS 확장을 설치할 수 있습니다.

- Visual Studio Code
- Cursor
- Windsurf
- Antigravity

> JetBrains Rider, IntelliJ 사용자는 EmmyLua 플러그인을 대신 사용하세요.

## 구성 요소

| 구성 요소 | 설명 | 설치 위치 |
|-----------|------|-----------|
| LuaLS Extension | VS Code 확장, Lua 언어 지원 엔진 | IDE 확장 마켓 |
| Viven LLS Plugin | Viven SDK 전용 타입 정의 및 진단 규칙 | `.lls-plugins/lls-viven-plugin/` |

두 가지가 모두 설치되어야 정상 동작합니다.

## 설치 방법

### 방법 A: Unity Editor

1. `VIVEN SDK > Settings` 창을 엽니다.
2. `Lua Language Server` 섹션에서 `Install` 버튼을 클릭합니다.
3. LuaLS Extension과 Viven LLS Plugin이 순서대로 설치됩니다.
4. `.vscode/settings.json`이 자동으로 설정됩니다.

### 방법 B: CLI (Setup Wizard)

Setup Wizard의 3단계에서 자동으로 안내됩니다.

```bash
bash Setup~/setup.sh
```

수동으로 각 단계를 진행할 수도 있습니다.

**1단계: LuaLS Extension 설치**

```bash
code --install-extension sumneko.lua
```

**2단계: Viven LLS Plugin 다운로드**

Setup Wizard가 GitHub Release에서 최신 버전을 다운로드하여 `.lls-plugins/lls-viven-plugin/`에 배치합니다.

**3단계: VS Code 설정**

`.vscode/settings.json`에 다음 설정이 추가됩니다.

```json
{
    "Lua.workspace.userThirdParty": [".lls-plugins"],
    "Lua.runtime.version": "Lua 5.4",
    "Lua.diagnostics.globals": [
        "CS",
        "VivenAPI"
    ]
}
```

이미 `.vscode/settings.json`이 있는 경우 `Lua.workspace.userThirdParty` 항목만 수동으로 추가하세요.

## 설치 후 확인

### Doctor로 확인

```bash
bash Setup~/setup.sh doctor
```

정상 설치 시 다음과 같이 표시됩니다.

```
✅ LuaLS Extension
✅ Viven LuaLS Plugin
```

### IDE에서 확인

Lua 파일을 열고 `CS.UnityEngine.`까지 입력했을 때 자동완성 목록이 표시되면 정상입니다.

## 알려진 제한 사항

### 전역 변수 오인식

VivenScript에서 선언한 변수는 격리된 환경에서 실행되어 전역 변수로 공유되지 않지만, LuaLS는 이를 전역 변수로 인식하여 경고를 표시할 수 있습니다.

- 가급적 `local` 키워드를 사용하세요.
- `checkInject`로 주입받는 변수는 **`local`로 재선언하면 안 됩니다.** 주입 메커니즘이 동작하지 않습니다.
- 주입 변수의 경고는 `.luarc.json`의 `diagnostics.globals`에 추가하여 무시할 수 있습니다.

### C# 타입 추론 제약

Lua에서 호출하는 C# 객체나 메서드에 대한 타입 추론이 완벽하지 않을 수 있습니다. Viven LLS Plugin이 주요 API에 대한 타입 정의를 제공하지만 모든 C# 타입을 다루지는 않습니다.

### xLua 런타임 차이

Viven의 Lua 런타임(xLua 기반)과 표준 Lua 간에 미세한 문법 차이가 있을 수 있습니다. LuaLS 경고가 실제 실행 결과와 다를 수 있습니다.

## Annotation 활용

EmmyLua 스타일 Annotation을 사용하면 타입을 명시하여 자동완성과 타입 체크 정확도를 높일 수 있습니다.

```lua
---@type CS.UnityEngine.GameObject
local target

---@param newState string | number 새 방 상태
---@return boolean success 상태 변경 성공 여부
function SetRoomState(newState)
    -- ...
end
```

## 업데이트

Viven LLS Plugin을 업데이트하려면 기존 플러그인을 삭제하고 다시 설치합니다.

```bash
rm -rf .lls-plugins/lls-viven-plugin
bash Setup~/setup.sh   # 3단계에서 재설치
```

## 환경 변수

| 변수 | 기본값 | 설명 |
|------|--------|------|
| `VIVEN_LLS_PLUGIN_REPO` | `vivenapp/lls-viven-plugin` | GitHub 저장소 |
| `VIVEN_LLS_PLUGIN_VERSION` | `latest` | 버전 (`latest` 또는 태그) |

## 관련 문서

- [개발 환경 설정 개요](00-overview.md)
- [VivenLuaBehaviour 활용](../../02-content-creation/03-scripting/01-viven-lua-behaviour.md)
