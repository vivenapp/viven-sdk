# 개발 환경 설정

## 개요

Viven SDK를 설치한 뒤, 스크립트 편집과 AI 지원 도구를 추가로 설정하면 콘텐츠 제작 효율을 높일 수 있습니다.

이 문서는 SDK 설치 이후 선택적으로 구성할 수 있는 개발 도구를 안내합니다.

## 구성 요소

| 도구 | 설명 | 필수 여부 |
|------|------|-----------|
| [Viven SDK](../01-sdk-installation-and-setup.md) | Unity 패키지, 문서 포함 | 필수 |
| [AI Toolkit](01-ai-toolkit.md) | AI 코딩 에이전트에 Viven SDK 지식을 제공하는 스킬 모음 | 선택 |
| [Lua Language Server](02-lua-language-server.md) | VS Code 계열 IDE에서 Lua 자동완성, 타입 체크, 린트 제공 | 선택 |
| [MCP Server](03-mcp-server.md) | AI가 Unity Editor를 직접 조작할 수 있는 브릿지 | 선택 |

## 설치 방법

선택 도구는 두 가지 방법으로 설치할 수 있습니다.

### 방법 A: Unity Editor

SDK 설치 시 표시되는 환경설정 창(`VIVEN SDK > Settings`)에서 각 도구의 설치 버튼을 클릭합니다.

### 방법 B: CLI (Setup Wizard)

터미널에서 Setup Wizard를 실행하면 인터랙티브하게 각 도구를 설치할 수 있습니다.

**Bash (macOS/Linux/Git Bash):**
```bash
bash Setup~/setup.sh
```

**PowerShell (Windows):**
```powershell
.\Setup~\setup.ps1
```

Setup Wizard는 다음 순서로 진행됩니다.

1. Unity 환경 확인
2. AI Toolkit 설치
3. Lua Language Server 설치
4. MCP Server 안내
5. 환경 검증 (Doctor)

설치 후 언제든 `doctor` 명령으로 환경 상태를 확인할 수 있습니다.

```bash
bash Setup~/setup.sh doctor          # 상태 확인
bash Setup~/setup.sh doctor --fix    # 자동 수정 가능한 항목 수정
```

## 권장 설치 조합

### AI 코딩 에이전트를 사용하는 경우

AI Toolkit을 설치하면 에이전트가 Viven SDK의 API, 컴포넌트, 네트워크 동기화 패턴을 이해하고 정확한 코드를 생성할 수 있습니다.

MCP Server를 추가하면 에이전트가 Unity Editor에서 직접 씬을 편집하고, GameObject를 생성하고, 컴포넌트를 추가할 수 있습니다.

→ **AI Toolkit** + **MCP Server** 설치를 권장합니다.

### VS Code 계열 IDE로 Lua 스크립트를 편집하는 경우

Lua Language Server를 설치하면 자동완성, 타입 추론, 실시간 문법 검사를 사용할 수 있습니다.

→ **Lua Language Server** 설치를 권장합니다.

### 둘 다 해당되는 경우

→ 세 가지 모두 설치하면 가장 효과적입니다.

## 문서 읽는 순서

1. 사용하는 도구에 해당하는 문서를 읽고 설치합니다.
2. 설치 후 `doctor`로 환경을 검증합니다.
3. [컨텐츠 제작 개요](../../02-content-creation/00-overview.md)로 이동하여 콘텐츠 제작을 시작합니다.

## 관련 문서

- [SDK 설치 및 설정](../01-sdk-installation-and-setup.md)
- [컨텐츠 제작 개요](../../02-content-creation/00-overview.md)
