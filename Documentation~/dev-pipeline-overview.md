# Viven SDK 개발 환경 — 관리 포인트 및 파이프라인

> 이 문서는 SDK와 개발 도구를 **관리하는 개발자** 관점의 내부 문서입니다.
> 사용자(콘텐츠 제작자) 대상 문서는 [개발 환경 설정](01-overview/02-development-environment/00-overview.md)을 참고하세요.

---

## 1. 전체 시스템 구성도

```
┌─────────────────────────────────────────────────────────────────────────┐
│                        관리 저장소 (Source of Truth)                     │
│                                                                         │
│  ┌──────────────────────────────────────────────────────────────────┐   │
│  │   viven-client-sdk (이 프로젝트)                                 │   │
│  │   (Content Creator 템플릿 + SDK 배포 원점)                       │   │
│  │                                                                  │   │
│  │  Assets/TwentyOz/VivenSDK/  ← SDK 소스 코드                     │   │
│  │  .claude/skills/             ← 스킬 원본                         │   │
│  │  VivenGuide/                 ← 문서 원본                         │   │
│  │  Setup~/                     ← 셋업 위저드                       │   │
│  │  .gitlab-ci.yml              ← 파이프라인                        │   │
│  └──────────────────────────────┬───────────────────────────────────┘   │
│                                 │                                       │
└─────────────────────────────────────────┼───────────────────────────────┘
                                          │
                                          ▼
┌─────────────────────────┐   ┌───────────────────────────────────────────┐
│  배포 저장소 (Git UPM)   │   │  배포 저장소 (GitHub)                     │
│                         │   │                                           │
│  viven-public/viven-sdk │   │  vivenapp/viven-ai-toolkit                │
│  (Public SDK 패키지)     │   │  (AI Toolkit 배포)                        │
│                         │   │                                           │
│  viven-sdk.developer    │   │  vivenapp/lls-viven-plugin                │
│  (개발자 전용 패키지)     │   │  (LLS Plugin 배포)                       │
└────────────┬────────────┘   └──────────────┬────────────────────────────┘
             │                               │
             ▼                               ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                         사용자 (콘텐츠 제작자)                           │
│                                                                         │
│  Unity Package Manager  ←── SDK 설치                                    │
│  VIVEN SDK > Settings   ←── Editor Window (AI Toolkit, LLS, MCP 설치)   │
│  Setup~/setup.sh        ←── CLI (동일 기능)                              │
│  Claude Marketplace     ←── Claude Code 전용 스킬 설치                   │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## 2. 관리 포인트 상세

### A. 스킬 콘텐츠 (40개 스킬)

```
Source of Truth                          외부 배포
──────────────────────────────────────────────────────────────

.claude/skills/                          ci/ai-toolkit/sync.sh
  ├── viven-sdk-rpc/                      ┌────────────────┐
  │   └── SKILL.md  ─────────────────────▶│  GitHub         │
  ├── viven-sdk-avatar/                   │  vivenapp/      │
  ├── viven-sdk-async/                    │  viven-ai-      │
  ├── ... (39 skills)                     │  toolkit        │
  │                                       │                 │
  │   ci/ai-toolkit/sync-manifest.yml     │  skills/        │
  │   viven-sdk-* 만 sync:               │   viven-sdk-*/  │
  │                                       │                 │
  │   .claude/skills/ → skills/           │  templates/     │
  │                                       │  bootstrap.sh   │
  └── 편집은 여기서만!                      │  docs/          │
                                          └────────────────┘
```

**관리 규칙:**
- 스킬 편집은 반드시 `.claude/skills/` 에서만 수행
- 프로젝트 내 `.agent/skills/`, `.cursor/skills/` 복사본 없음 — `.claude/skills/`가 유일한 원본
- `ci/ai-toolkit/sync.sh`가 `sync-manifest.yml`에 따라 GitHub `viven-ai-toolkit`에 배포 (viven-sdk-* 스킬만)
- `bootstrap.sh`는 로컬에서 `.claude/skills/` 직접 참조, toolkit 다운로드 시 `skills/` fallback

---

### B. 문서 (VivenGuide)

```
VivenGuide/
  ├── 01-overview/
  │   ├── 01-sdk-installation-and-setup.md     ← 사용자 대상 SDK 설치 가이드
  │   └── 02-development-environment/          ← 사용자 대상 개발 도구 가이드
  │       ├── 00-overview.md
  │       ├── 01-ai-toolkit.md
  │       ├── 02-lua-language-server.md
  │       └── 03-mcp-server.md
  ├── 02-content-creation/                     ← 기능별 사용 가이드
  └── 03-agent-skills/                         ← sync.sh로 GitHub에 배포
       └── (스킬 보조 문서)
```

**관리 규칙:**
- `VivenGuide/03-agent-skills/` 는 `ci/ai-toolkit/sync-manifest.yml`에 의해 GitHub `docs/`로 배포
- SDK 릴리즈 시 `documentation` CI job이 DocFX로 API 레퍼런스 생성
- 사용자 가이드(01, 02)와 Agent 스킬 문서(03)는 독립적으로 유지

---

### C. 셋업 인프라

```
                    ┌─────────────────────────────┐
                    │    Setup~/                   │
                    │    ├── setup.sh  (Bash)      │
                    │    ├── setup.ps1 (PowerShell) │
                    │    └── modules/              │
                    │        ├── check-unity.sh    │
                    │        ├── install-ai-toolkit│
                    │        ├── install-lls-plugin│
                    │        ├── setup-mcp-guide   │
                    │        ├── doctor.sh         │
                    │        └── download-utils.sh │
                    └──────────┬──────────────────┘
                               │
              ┌────────────────┼────────────────┐
              ▼                ▼                ▼
     ┌────────────┐   ┌──────────────┐   ┌──────────┐
     │ AI Toolkit │   │ LLS Plugin   │   │ MCP 안내 │
     │            │   │              │   │          │
     │ GitHub     │   │ GitHub       │   │ 가이드   │
     │ Release    │   │ Release      │   │ 파일     │
     │ .zip       │   │ .zip         │   │ 출력     │
     │    ↓       │   │    ↓         │   │          │
     │ .agent/    │   │ .lls-plugins/│   │          │
     │   skills/  │   │   lls-viven- │   │          │
     │   bootstrap│   │   plugin/    │   │          │
     │   templates│   │              │   │          │
     │    ↓       │   │ .vscode/     │   │          │
     │ bootstrap  │   │   settings   │   │          │
     │   .sh      │   │   .json      │   │          │
     └────────────┘   └──────────────┘   └──────────┘
```

**관리 규칙:**
- `Setup~/` 폴더는 Unity의 `~` 컨벤션으로 빌드에서 제외됨
- `setup.sh`와 `setup.ps1`은 기능 대칭 유지 필요
- 환경 변수로 다운로드 소스 오버라이드 가능

---

### D. Bootstrap (IDE별 스킬 배포)

```
ai-toolkit/bootstrap.sh
        │
        │  SKILL.md frontmatter 파싱
        │
        ├─────────────────────────────────────────────────────┐
        │                                                     │
        ▼                                                     ▼
  ┌─── 파일 복사 방식 ───┐                          ┌─── 변환/통합 방식 ───┐
  │                      │                          │                      │
  │ Claude Code          │                          │ Cursor               │
  │  .claude/skills/     │                          │  .cursor/rules/*.mdc │
  │  {name}/SKILL.md     │                          │  (YAML→MDC 변환)     │
  │                      │                          │                      │
  │ Cline                │                          │ GitHub Copilot       │
  │  .clinerules/*.md    │                          │  AGENTS.md (통합)    │
  │                      │                          │                      │
  │ Roo Code             │                          │ Antigravity          │
  │  .roo/rules/*.md     │                          │  GEMINI.md (통합)    │
  │                      │                          │                      │
  │ Windsurf             │                          │                      │
  │  .windsurf/rules/*.md│                          │                      │
  └──────────────────────┘                          └──────────────────────┘

  + templates/ 에서 지침 파일 복사:
    CLAUDE.md, .cursorrules, AGENTS.md, GEMINI.md,
    .claudeignore, .cursorignore, .windsurfrules, ...
```

**관리 규칙:**
- 새 IDE 지원 시 `bootstrap.sh` + `templates/` + `setup.ps1`/`setup.sh` 모두 업데이트
- `--force` 플래그로 기존 파일 덮어쓰기 (업데이트 시)
- 통합 방식(Copilot, Antigravity)은 모든 스킬을 단일 파일로 머지

---

## 3. CI/CD 파이프라인

```
                         ┌──────────────────────────┐
                         │     트리거 조건            │
                         └──────────┬───────────────┘
                                    │
                 ┌──────────────────┼──────────────────┐
                 │                  │                   │
                 ▼                  ▼                   ▼
        Release/vX.X.X       feature 브랜치        웹 UI 수동
        태그 push             push                 실행
                 │                  │                   │
                 ▼                  ▼                   ▼
            TYPE=TAG          TYPE=SYNC_ONLY       TYPE 선택
                 │                  │                   │
   ┌─────────────┼──────┐          │         ┌─────────┼─────────┐
   │             │      │          │         │         │         │
   ▼             ▼      ▼          │         ▼         ▼         ▼
┌───────┐ ┌──────┐ ┌────┐         │    FORCE_RUN  DEPLOY_DEV  SYNC_ONLY
│prepare│ │deploy│ │doc │         │      (전체)    (dev pkg)    (sync)
│       │ │      │ │    │         │
│Unity  │ │SDK   │ │CHNG│         │
│version│ │Git   │ │LOG │         │
│extract│ │UPM   │ │    │         │
│       │ │push  │ │API │         │
│       │ │      │ │docs│         │
└───┬───┘ └──┬───┘ └─┬──┘         │
    │        │       │            │
    ▼        ▼       ▼            ▼
┌────────────────────────────────────────┐
│              Stage: sync               │
│                                        │
│  ┌────────────────────────────────┐    │
│  │ sync-ai-toolkit               │    │
│  │                                │    │
│  │ .claude/skills/                │    │
│  │   → GitHub vivenapp/           │    │
│  │     viven-ai-toolkit           │    │
│  │     (skills/agent,claude,cursor)│    │
│  └────────────────────────────────┘    │
│                                        │
└────────────────────────────────────────┘
```

### 파이프라인 트리거 정리

| 트리거 | TYPE | 실행 Job | 용도 |
|--------|------|----------|------|
| `Release/vX.X.X` 태그 push | TAG | prepare → deploy → doc → sync | 정식 릴리즈 |
| `feature/*` 브랜치 push | SYNC_ONLY | sync-ai-toolkit | 스킬 개발 중 동기화 |
| 웹 UI `RUN_TARGET=sync` | SYNC_ONLY | sync만 | 수동 동기화 |
| 웹 UI `RUN_TARGET=deploy-dev` | DEPLOY_DEV | deploy-sdk-dev | 개발자 패키지만 배포 |
| 웹 UI (기본) | FORCE_RUN | 전체 | 전체 수동 실행 |

### CI 변수 요약

| 변수 | 용도 | 사용 Job |
|------|------|----------|
| `GITLAB_TOKEN` | GitLab PAT (write_repository) | deploy, doc, sync-skills |
| `GITLAB_USERNAME` | GitLab 사용자명 | doc |
| `GITHUB_TOKEN` | GitHub PAT (Contents: RW) | sync-ai-toolkit |
| `UNITY_LICENSE` | Unity 라이센스 | test (비활성) |
| `SDK_DEPLOY_REPO` | Public SDK 배포 repo URL | deploy-sdk |
| `SDK_DEV_DEPLOY_REPO` | Developer 패키지 배포 repo URL | deploy-sdk-dev |

---

## 4. 사용자에게 도달하는 경로

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│   관리자가 편집                   파이프라인               사용자     │
│                                                                     │
│                                                                     │
│   .claude/skills/SKILL.md                                           │
│          │                                                          │
│          └── CI sync-ai-toolkit ──▶ GitHub viven-ai-toolkit         │
│                (ci/ai-toolkit/sync-manifest.yml로 3경로에 동일 내용 배포)           │
│                                          │                          │
│                                          ├──▶ Setup Wizard 다운로드  │
│                                          │     → .agent/            │
│                                          │     → bootstrap.sh 실행  │
│                                          │     → IDE별 스킬 배포     │
│                                          │                          │
│                                          ├──▶ Editor Window 다운로드 │
│                                          │     (동일 경로)           │
│                                          │                          │
│                                          └──▶ Claude Marketplace    │
│                                                (별도 게시)           │
│                                                                     │
│   SDK 소스 (viven-client-sdk)                                       │
│          │                                                          │
│          └── CI deploy-sdk ──▶ Git UPM repo                         │
│                                    │                                │
│                                    └──▶ Unity Package Manager       │
│                                          (사용자가 URL 입력)        │
│                                                                     │
│   lls-viven-plugin 소스                                             │
│          │                                                          │
│          └── GitHub Release ──▶ GitHub vivenapp/lls-viven-plugin    │
│                                       │                             │
│                                       ├──▶ Setup Wizard 다운로드    │
│                                       │     → .lls-plugins/         │
│                                       │                             │
│                                       └──▶ Editor Window 다운로드   │
│                                             (동일 경로)             │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 5. 관리 체크리스트

### 스킬 추가/수정 시

1. `.claude/skills/{name}/SKILL.md` 편집
2. push → CI `sync-ai-toolkit`이 GitHub `viven-ai-toolkit`에 자동 배포
3. Claude Marketplace 게시가 필요하면 별도 수동 작업

### SDK 릴리즈 시

1. `Release/vX.X.X` 태그 push
2. CI: deploy-sdk → Git UPM repo에 패키지 push
3. CI: changelog → CHANGELOG.md 갱신
4. CI: documentation → DocFX API 문서 생성
5. CI: sync → 스킬 및 AI Toolkit 최신화

### LLS Plugin 업데이트 시

1. `lls-viven-plugin` 저장소에서 작업
2. GitHub Release 생성 (태그)
3. 사용자는 Setup Wizard 또는 Editor Window에서 재설치

### 새 IDE 지원 추가 시

1. `ai-toolkit/bootstrap.sh` — 새 IDE 분기 추가
2. `ai-toolkit/templates/` — 지침/ignore 파일 추가
3. `Setup~/setup.sh` — IDE 선택 메뉴 추가
4. `Setup~/setup.ps1` — PowerShell 버전 동일 추가
5. VivenGuide 문서 업데이트 (AI Toolkit, MCP Server)

### Setup 인프라 변경 시

1. `setup.sh`와 `setup.ps1` 기능 대칭 확인
2. `doctor` 검증 항목 동기화
3. Editor Window UI 업데이트 (있는 경우)
4. VivenGuide 문서 반영

---

## 6. 저장소 관계도

```
┌─────────────────────────────────────────────────────────────────────┐
│  viven-client-sdk (이 프로젝트, GitLab)                              │
│  SDK 소스 + 문서 + 스킬 + 셋업 — 모든 배포의 원점                     │
│                                                                     │
│  ┌──────────────┐  ┌─────────────┐  ┌─────────────┐  ┌───────────┐ │
│  │ SDK 소스     │  │ .claude/    │  │ VivenGuide/ │  │ Setup~/   │ │
│  │ Assets/      │  │ skills/     │  │ (문서 원본)  │  │ (위저드)  │ │
│  │ TwentyOz/    │  │ (스킬 원본)  │  │             │  │           │ │
│  └──────┬───────┘  └──────┬──────┘  └──────┬──────┘  └───────────┘ │
│         │                 │                │                        │
│         │  CI pipeline    │  CI pipeline   │                        │
│         ▼                 ▼                ▼                        │
│  ┌────────────┐  ┌──────────────────────────────┐                  │
│  │ deploy-sdk │  │ sync-skills + sync-ai-toolkit│                  │
│  └──────┬─────┘  └──────────────┬───────────────┘                  │
│         │                       │                                   │
└─────────┼───────────────────────┼───────────────────────────────────┘
          │                       │
          ▼                       ▼
┌──────────────────┐   ┌──────────────────────┐
│ Git UPM repos    │   │ viven-ai-toolkit     │
│ (GitLab, public) │   │ (GitHub, public)     │
│                  │   │                      │
│ com.viven.sdk    │   │ skills/agent/        │
│ com.viven.sdk    │   │ skills/claude/       │
│   .developer     │   │ skills/cursor/       │
│                  │   │ templates/           │
│                  │   │ bootstrap.sh         │
│                  │   │ docs/                │
└────────┬─────────┘   └──────────┬───────────┘
         │                        │
         ▼                        ▼
    Unity Package            Setup Wizard /
    Manager                  Editor Window /
                             Marketplace

                    ┌─────────────────────────┐
                    │  lls-viven-plugin        │
                    │  (GitHub, public)        │
                    │                          │
                    │  LuaLS Plugin 소스       │
                    │  → GitHub Release        │
                    │  → Setup Wizard 다운로드  │
                    └─────────────────────────┘

┌───────────────────────┐
│  viven_clone          │
│  (GitLab, private)    │
│                       │
│  클라이언트 메인       │
│  (배포에 관여하지 않음) │
└───────────────────────┘
```

---

## 7. 요약: 무엇을 어디서 관리하는가

| 관리 대상 | 편집 위치 | 배포 경로 | 사용자 접근 |
|-----------|-----------|-----------|-------------|
| SDK 소스 코드 | `Assets/TwentyOz/VivenSDK/` | CI deploy-sdk → Git UPM repo | Unity Package Manager |
| 스킬 콘텐츠 (40개) | `.claude/skills/` | CI → .agent/ → GitHub toolkit | Setup Wizard / Editor / Marketplace |
| 사용자 문서 (VivenGuide) | `VivenGuide/` | SDK에 포함 + CI로 DocFX 배포 | 프로젝트 내 직접 참조 |
| LLS Plugin | `lls-viven-plugin` repo | GitHub Release | Setup Wizard / Editor |
| 셋업 인프라 | `Setup~/` | SDK에 포함 | 사용자가 직접 실행 |
| Bootstrap | `ai-toolkit/bootstrap.sh` | CI → GitHub toolkit | Setup Wizard가 다운로드 후 실행 |
| IDE 템플릿 | `ai-toolkit/templates/` | CI → GitHub toolkit | bootstrap.sh가 복사 |
| MCP 가이드 | `.claude/skills/unity-editor-mcp/` | 스킬과 함께 배포 | AI Toolkit 설치 시 포함 |
| CI 파이프라인 | `.gitlab-ci.yml` | (자체 실행) | — |
