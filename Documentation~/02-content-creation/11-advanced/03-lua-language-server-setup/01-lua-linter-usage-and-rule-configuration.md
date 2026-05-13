# Lua Linter 사용 및 규칙 설정

Lua Language Server(LuaLS)를 활용하면 Viven 스크립트 작성 시 실시간 문법 검사, 타입 추론, 자동 완성을 통해 개발 효율을 획기적으로 높일 수 있습니다. Viven SDK 샘플 프로젝트에서도 이러한 Annotation을 적극적으로 활용하여 코드의 안정성을 높이고 있습니다.

## Lua Language Server 설정 방법

1. **확장 프로그램 설치**: VS Code에서 [Lua Language Server](https://marketplace.visualstudio.com/items?itemName=sumneko.lua) 확장을 설치합니다.
2. **프로젝트 설정**: 프로젝트 루트에 `.luarc.json` 파일을 생성하여 Viven 환경에 맞는 규칙을 설정합니다.
3. **진단(Diagnostics) 활성화**: 에디터 하단의 진단 도구를 통해 실시간으로 코드의 잠재적 오류를 확인합니다.

## 알려진 문제 및 제약 사항

현재 Viven 환경에서 LuaLS를 사용할 때 다음과 같은 특이 사항이 발생할 수 있습니다.

### 1. 전역 변수 오인식 문제
Viven 스크립트에서 선언한 변수는 실제 실행 시 격리된 환경에서 동작하여 전역 변수로 공유되지 않지만, Linter는 이를 전역 변수(`Global variable`)로 인식하여 경고를 표시할 수 있습니다.
- **해결책**: 가급적 `local` 키워드를 명시하여 범위를 제한하거나, `.luarc.json`의 `diagnostics.globals` 설정에 해당 변수를 추가하여 경고를 무시할 수 있습니다.
- **주의 사항**: `checkInject` 함수를 통해 주입받는 변수(예: `SpectatorManagerObject` 등)는 **절대로 `local`로 재선언해서는 안 됩니다.** 이 변수들은 외부에서 주입되는 전역 심볼이므로, `local`로 선언할 경우 주입 메커니즘이 정상적으로 동작하지 않을 수 있습니다. Linter 경고가 발생하더라도 전역 설정을 통해 해결해야 합니다.

### 2. C# 타입 추론 제약
Lua에서 호출하는 C# 객체나 메서드에 대한 자동 타입 추론이 완벽하지 않을 수 있습니다. 
- **참고**: 향후 Viven SDK 전용 Addon을 통해 C# API에 대한 정밀한 타입 정의 파일이 제공될 예정입니다.

### 3. VivenScript 문법 차이
Viven의 Lua 런타임(xLua 기반)과 표준 Lua 간에 미세한 문법 해석 차이가 있을 수 있으므로, Linter의 경고가 실제 실행 결과와 다를 수 있음을 유의해야 합니다.

## Annotation 활용의 이점

이러한 제약 사항에도 불구하고, **EmmyLua 스타일의 Annotation**을 사용하여 타입을 명시하는 것은 매우 권장됩니다. 샘플 프로젝트(`SystemManager.lua` 등)에서도 다음과 같이 사용되고 있습니다.

```lua
---@type EventBus
local event

---@type SystemManager
Global_SystemManager = self:GetLuaComponent("SystemManager")

---@param newState string | number 새 방 상태
---@return boolean success 상태 변경 성공 여부
function SetRoomState(newState)
    -- ...
end
```

- **개발 효율**: 복잡한 API 구조에서도 자동 완성을 통해 오타를 방지하고 메서드 목록을 즉시 확인합니다.
- **디버깅**: 잘못된 타입의 인자가 전달되는 것을 에디터 상에서 사전에 감지하여 런타임 에러를 획기적으로 줄여줍니다.
- **가독성**: 코드 자체가 문서 역할을 하여 협업 및 유지보수 시 의도를 명확히 전달합니다.
