# EmmyLua Debugger 연결 가이드

Viven SDK에서 Lua 스크립트를 디버깅하기 위해 [EmmyLua Debugger](https://github.com/EmmyLua/EmmyLuaDebugger)를 연결하는 방법을 설명합니다. EmmyLua는 고성능 크로스 플랫폼 Lua 디버거로, 중단점(Breakpoint), 변수 조사(Variable Watch), 콜 스택(Stack Trace) 등의 기능을 제공합니다.

## 1. 디버거 연결 원리

EmmyLua Debugger는 **TCP 통신**을 기반으로 작동합니다.
1. **IDE (VSCode, Rider 등)**: 디버그 서버를 실행하고 연결을 기다립니다.
2. **Viven SDK (Lua)**: `emmy_core` 모듈을 로드하고 IDE의 디버그 서버에 접속합니다.

## 2. Lua 스크립트 설정

디버거를 연결하려면 Lua 스크립트 상단에 다음과 같은 설정 코드가 필요합니다.

```lua
-- 1. 디버거 바이너리 경로 설정 (IDE에서 제공하는 경로 사용)
-- VSCode의 경우 'EmmyLua: Insert Emmy Debugger Code' 명령어로 확인 가능합니다.
local debuggerInstallPathString = "C:/Users/YourName/.vscode/extensions/tangzx.emmylua-xxxx/debugger/emmy/windows/x64/?.dll"

-- 2. 패키지 경로 추가 및 모듈 로드
package.cpath = package.cpath .. ";" .. debuggerInstallPathString
local dbg = require("emmy_core")

-- 3. 디버그 서버 연결 (기본 포트: 9966)
dbg.tcpListen("localhost", 9966)

-- 4. (선택 사항) IDE가 연결될 때까지 대기
-- 스크립트 시작 시점부터 디버깅이 필요한 경우 사용합니다.
local waitForIDE = true
if waitForIDE then
    dbg.waitIDE()
end
```

## 3. IDE별 설정 방법

### Visual Studio Code (VSCode)
1. **확장 설치**: `EmmyLua` 확장을 설치합니다.
2. **디버그 구성 추가**: `launch.json`에 `EmmyLua New Debug` 구성을 추가합니다.
   - `ideConnectMode`: `false` (디버거가 IDE를 기다리는 모드)
   - `host`: `localhost`
   - `port`: 9966
3. **디버깅 시작**: `F5`를 눌러 디버그 서버를 실행합니다.
4. **Viven 실행**: Viven에서 해당 Lua 스크립트가 포함된 컨텐츠를 실행하면 디버거가 연결됩니다.

### JetBrains Rider / IntelliJ IDEA
1. **플러그인 설치**: `EmmyLua` 플러그인을 설치합니다.
2. **Run/Debug Configuration**: `EmmyDebugger(New)` 구성을 생성합니다.
   - `Connection 기반`: `TCP Listen` 선택
   - `Port`: 9966
3. **디버깅 시작**: 디버그 아이콘을 클릭하여 서버를 실행합니다.

## 4. 주요 디버깅 기능
- **중단점 (Breakpoints)**: 코드의 특정 라인에서 실행을 멈추고 상태를 확인합니다.
- **변수 조사 (Variable Watch)**: 로컬 및 전역 변수의 값을 실시간으로 확인합니다.
- **스텝 실행 (Step Over/Into/Out)**: 코드를 한 줄씩 실행하며 흐름을 추적합니다.
- **로그 분석**: `Debug.Log`와 함께 사용하여 런타임 오류를 더 정밀하게 진단할 수 있습니다. 자세한 내용은 [로그 확인 및 분석](02-viven-sdk-log-review-and-analysis.md)을 참조하세요.

> **주의**: 디버거 연결 코드는 개발 단계에서만 사용하고, 실제 배포 시에는 제거하거나 주석 처리하는 것이 좋습니다. `dbg.waitIDE()`를 사용하면 IDE가 연결될 때까지 게임이 멈춰 있을 수 있습니다.
