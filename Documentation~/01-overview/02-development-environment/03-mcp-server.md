# MCP Server

## 개요

MCP(Model Context Protocol) Server를 설정하면 AI 코딩 에이전트가 Unity Editor를 직접 조작할 수 있습니다.

에이전트가 할 수 있는 작업 예시:

- 씬에 GameObject 추가/삭제
- 컴포넌트 추가 및 Inspector 값 설정
- Hierarchy 탐색 및 씬 편집
- Material, Prefab, UI 요소 조작

MCP Server가 없으면 에이전트는 C# 스크립트를 생성하거나 수동 단계를 안내하는 방식으로 대체합니다.

## 지원하는 MCP Server

두 가지 MCP Server를 사용할 수 있습니다.

| 서버 | 특징 | 사전 요구 |
|------|------|-----------|
| **coplay-mcp** (권장) | 자연어 기반 Unity Editor 조작 | Python 3.11+, `uvx`, Coplay 계정 |
| **unity-mcp** | 개별 핸들러 기반 Editor 제어 | Node.js 18+ |

## 설치 방법

### 방법 A: Unity Editor

1. `VIVEN SDK > Settings` 창을 엽니다.
2. `MCP Server` 섹션에서 안내에 따라 설치합니다.

### 방법 B: CLI (Setup Wizard)

Setup Wizard의 4단계에서 안내됩니다.

```bash
bash Setup~/setup.sh
```

### 수동 설치

#### coplay-mcp

**1단계: 사전 요구 사항 설치**

```bash
# uvx 설치 (아직 없는 경우)
pip install uv

# 정상 설치 확인
uvx --version
```

**2단계: Unity 패키지 설치**

1. Unity Editor에서 `Window > Package Manager`를 엽니다.
2. `+` → `Add package from git URL`을 선택합니다.
3. 다음 URL을 입력합니다.
   ```
   https://github.com/CoplayDev/coplay-unity-plugin.git#beta
   ```
4. `Coplay > Toggle Window` (Ctrl+G)에서 Coplay 계정으로 로그인합니다.

**3단계: IDE에 MCP Server 등록**

사용 중인 IDE에 따라 등록 방법이 다릅니다.

**Claude Code:**
```bash
claude mcp add --scope project --transport stdio coplay-mcp \
  --env MCP_TOOL_TIMEOUT=720000 \
  -- uvx --python ">=3.11" coplay-mcp-server@latest
```

**Cursor:** `.cursor/mcp.json`에 추가합니다.
```json
{
  "mcpServers": {
    "coplay-mcp": {
      "command": "uvx",
      "args": ["--python", ">=3.11", "coplay-mcp-server@latest"],
      "env": { "MCP_TOOL_TIMEOUT": "720000" }
    }
  }
}
```

**Windsurf:** `~/.codeium/windsurf/mcp_config.json`에 추가합니다.
```json
{
  "mcpServers": {
    "coplay-mcp": {
      "command": "uvx",
      "args": ["--python", ">=3.11", "coplay-mcp-server@latest"],
      "disabled": false,
      "env": { "MCP_TOOL_TIMEOUT": "720000" }
    }
  }
}
```

**GitHub Copilot:** `.vscode/mcp.json`에 추가합니다.
```json
{
  "mcpServers": {
    "coplay-mcp": {
      "type": "stdio",
      "command": "uvx",
      "args": ["--python", ">=3.11", "coplay-mcp-server@latest"],
      "env": { "MCP_TOOL_TIMEOUT": "720000" }
    }
  }
}
```

**Antigravity:** `~/.gemini/antigravity/mcp_config.json`에 추가합니다. 현재 글로벌 설정만 지원됩니다.

**Cline / Roo Code:** 각 IDE의 MCP 설정 파일에 동일한 JSON 형식으로 추가합니다.

> 더 자세한 IDE별 등록 방법은 AI Toolkit 설치 후 `.claude/skills/unity-editor-mcp/MCP-SETUP-GUIDE.md`에서 확인할 수 있습니다.

#### unity-mcp

**1단계: Unity 패키지 설치**

1. `Window > Package Manager`에서 `+` → `Add package from git URL`을 선택합니다.
2. 다음 URL을 입력합니다.
   ```
   https://github.com/isuzu-shiranui/UnityMCP.git?path=jp.shiranui-isuzu.unity-mcp
   ```

**2단계: TypeScript 클라이언트 설치**

1. `Edit > Preferences > Unity MCP`에서 `Open Installer Window`를 클릭합니다.
2. 설치 디렉터리를 선택하고 다운로드합니다.
3. 설치 완료 후 클라이언트 경로를 기록합니다.

**3단계: IDE에 등록**

```bash
# Claude Code 예시
claude mcp add --scope project --transport stdio unity-mcp \
  --env MCP_HOST=127.0.0.1 --env MCP_PORT=27182 \
  -- node /path/to/unity-mcp-client/build/index.js
```

## 설치 후 확인

### Doctor로 확인

```bash
bash Setup~/setup.sh doctor
```

정상 설치 시 다음과 같이 표시됩니다.

```
✅ MCP 서버 (uvx 사용 가능)
```

### 동작 확인

AI 에이전트에게 다음과 같이 요청합니다.

```
"씬에 Cube를 추가하고 Rigidbody 컴포넌트를 붙여줘"
```

에이전트가 MCP 도구를 호출하여 Unity Editor에서 직접 실행하면 정상입니다.

MCP Server가 감지되지 않으면 에이전트는 C# 스크립트를 생성하거나 수동 단계를 안내합니다.

## 트러블슈팅

### MCP Server가 연결되지 않음

**coplay-mcp:**
- Python 3.11 이상이 설치되어 있는지 확인합니다: `python3 --version`
- `uvx`가 설치되어 있는지 확인합니다: `uvx --version`
- Unity Editor에서 Coplay 창이 열려 있고 로그인되어 있는지 확인합니다.
- 방화벽이 로컬 통신을 차단하고 있지 않은지 확인합니다.

**unity-mcp:**
- Node.js 18 이상이 설치되어 있는지 확인합니다: `node --version`
- `Edit > Preferences > Unity MCP`에서 MCP Server가 시작되었는지 확인합니다.
- TCP 포트 27182가 사용 가능한지 확인합니다.

### AI가 MCP 도구를 사용하지 않음

- IDE에서 MCP Server가 활성 상태인지 확인합니다.
- Cursor의 경우 Agent 모드에서 사용 중인지 확인합니다 (일반 Chat에서는 MCP 불가).
- AI Toolkit이 설치되어 있으면 조건부 규칙이 자동으로 적용됩니다.

### Windows 환경 특이사항

- `uvx` 명령이 인식되지 않으면 `pip install uv` 후 터미널을 재시작합니다.
- unity-mcp의 `node` 경로는 절대 경로를 사용하는 것이 안전합니다.

## 관련 문서

- [개발 환경 설정 개요](00-overview.md)
- [AI Toolkit](01-ai-toolkit.md) — MCP와 함께 사용하면 가장 효과적
