# AI Toolkit

## 개요

AI Toolkit은 AI 코딩 에이전트에게 Viven SDK 전용 지식을 제공하는 스킬 모음입니다.

설치하면 에이전트가 다음을 이해하고 활용할 수 있습니다.

- Viven SDK 컴포넌트 (`GrabbableModule`, `SittableModule`, `SyncView` 등)
- VivenScript(Lua) 작성 패턴과 라이프사이클
- 네트워크 동기화 (RPC, RoomProperty, NetworkVariable)
- 빌드 및 배포 절차
- 플랫폼별 입력/UI/오디오 처리

## 지원 IDE

| IDE | 스킬 형식 | 비고 |
|-----|-----------|------|
| Claude Code | `.claude/skills/*/SKILL.md` | Marketplace 설치도 가능 |
| Cursor | `.cursor/rules/*.mdc` | |
| Windsurf | `.windsurf/rules/*.md` | |
| GitHub Copilot | `AGENTS.md` (통합) | |
| Cline | `.clinerules/*.md` | |
| Roo Code | `.roo/rules/*.md` | |
| Antigravity (Gemini) | `GEMINI.md` (통합) | |

## 설치 방법

### 방법 A: Unity Editor

1. `VIVEN SDK > Settings` 창을 엽니다.
2. `AI Toolkit` 섹션에서 `Install` 버튼을 클릭합니다.
3. 사용 중인 IDE를 선택합니다.
4. 설치가 완료되면 초록색 체크 표시가 나타납니다.

### 방법 B: CLI (Setup Wizard)

Setup Wizard의 2단계에서 자동으로 안내됩니다.

```bash
bash Setup~/setup.sh
```

또는 수동으로 설치할 수도 있습니다.

```bash
# 1. AI Toolkit 다운로드 (.agent/ 디렉토리에 배치)
#    Setup Wizard가 GitHub Release에서 최신 버전을 다운로드합니다.

# 2. IDE별 스킬 배포
bash ai-toolkit/bootstrap.sh
```

`bootstrap.sh`를 실행하면 IDE 선택 메뉴가 표시됩니다.

```
사용 중인 AI IDE를 선택하세요 (쉼표로 복수 선택):
  1. Claude Code
  2. Cursor
  3. Windsurf
  4. GitHub Copilot
  5. Cline
  6. Roo Code
  7. Antigravity (Gemini)
  a. 전체
```

선택한 IDE에 맞는 형식으로 스킬이 자동 배포됩니다.

### 방법 C: Claude Code Marketplace

Claude Code 사용자는 Marketplace에서 직접 설치할 수 있습니다.

```bash
claude install viven-sdk-skills
```

> 이 방법은 Claude Code 전용입니다. 다른 IDE는 방법 A 또는 B를 사용하세요.

## 설치 후 확인

### Doctor로 확인

```bash
bash Setup~/setup.sh doctor
```

정상 설치 시 다음과 같이 표시됩니다.

```
✅ AI Toolkit (39 skills)
```

### IDE에서 확인

AI 에이전트에게 다음과 같이 질문하여 스킬이 활성화되었는지 확인할 수 있습니다.

```
"GrabbableModule로 물체를 잡고 던지는 스크립트를 만들어줘"
```

에이전트가 `onGrab`, `onRelease` 이벤트와 올바른 Inspector 설정을 포함한 코드를 생성하면 정상적으로 동작하고 있는 것입니다.

## 포함된 스킬 목록

설치되는 스킬은 약 40개이며, 다음 영역을 다룹니다.

| 영역 | 주요 스킬 |
|------|-----------|
| 프로젝트 설정 | `viven-sdk-project-setup`, `viven-sdk-project-config` |
| 스크립팅 기초 | `viven-sdk-viven-script`, `viven-sdk-lua-behaviour`, `viven-sdk-lua-syntax` |
| 컨텐츠 유형 | `viven-sdk-vobject`, `viven-sdk-vmap`, `viven-sdk-vavatar` |
| 상호작용 | `viven-sdk-grabbable-module`, `viven-sdk-sittable-module`, `viven-sdk-interaction` |
| 네트워크 | `viven-sdk-rpc`, `viven-sdk-network-variables`, `viven-sdk-sync-view`, `viven-sdk-room-property` |
| 플랫폼 | `viven-sdk-input`, `viven-sdk-physics`, `viven-sdk-spatial`, `viven-sdk-player` |
| UI/오디오 | `viven-sdk-ui-creation`, `viven-sdk-audio`, `viven-sdk-chat` |
| 고급 | `viven-sdk-performance`, `viven-sdk-security`, `viven-sdk-async` |
| 설계 | `viven-sdk-content-design`, `viven-sdk-minigame-architecture`, `viven-sdk-implementation-roadmap` |
| 에러 | `viven-sdk-common-errors`, `viven-sdk-error-log`, `viven-sdk-injection-troubleshooting` |

## 업데이트

AI Toolkit을 업데이트하려면 동일한 설치 과정을 다시 실행하면 됩니다. 기존 스킬이 최신 버전으로 덮어씌워집니다.

```bash
bash ai-toolkit/bootstrap.sh --force
```

## 환경 변수

Setup Wizard에서 다운로드 소스를 변경할 수 있습니다.

| 변수 | 기본값 | 설명 |
|------|--------|------|
| `VIVEN_TOOLKIT_REPO` | `vivenapp/viven-ai-toolkit` | GitHub 저장소 |
| `VIVEN_TOOLKIT_VERSION` | `latest` | 버전 (`latest` 또는 태그) |

## 관련 문서

- [개발 환경 설정 개요](00-overview.md)
- [MCP Server](03-mcp-server.md) — AI가 Unity Editor를 직접 조작하도록 설정
