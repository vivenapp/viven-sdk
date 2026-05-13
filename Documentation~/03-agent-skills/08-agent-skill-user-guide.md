# Agent Skill 사용 가이드

> 관련 문서: [Skill 분류](02-skill-taxonomy.md) | [페르소나·시나리오](01-personas-and-scenarios.md)

**대상**: 컨텐츠 제작자, 비개발자·개발자 학생 등 Agent Skill을 *사용*하는 사용자

**기준 플랫폼**: Cursor, Antigravity, Claude Code

본 문서는 Viven Agent Skill을 **사용**하는 방법을 안내합니다. Skill *개발* 방법은 [09-agent-skill-dev-guide.md](09-agent-skill-dev-guide.md)를 참조하세요.

---

## 1. Skill 개요

### 1-1. Viven Agent Skill이란?

**Agent Skill**은 AI 에이전트가 Viven 콘텐츠 제작 시 특정 작업을 수행할 수 있도록 도메인 지식과 워크플로우를 담은 패키지입니다. [Agent Skills](https://agentskills.io/) 오픈 표준을 따르며, Cursor, Antigravity, Claude Code 등 여러 AI 코딩 도구에서 공통으로 사용할 수 있습니다.

- **프로젝트 Skill**: `.cursor/skills/`, `.claude/skills/`, `.agent/skills/` 등 프로젝트 폴더에 두면 해당 프로젝트에서만 사용
- **전역 Skill**: 사용자 홈 디렉터리(`~/.cursor/skills/`, `~/.claude/skills/` 등)에 두면 모든 프로젝트에서 사용

Viven 프로젝트에는 `.cursor/skills/` 아래에 `viven-sdk-beginner-workflow`, `viven-sdk-content-design`, `viven-sdk-project-setup` 등 Viven 전용 Skill이 포함되어 있습니다.

### 1-2. Skill 4분류 구조

Viven Skill은 다음 4가지로 분류됩니다.

| 분류 | 설명 | 예시 Skill |
|------|------|------------|
| **분류 1: Viven 구체적 기능** | GrabbableModule, RPC, SyncView 등 Viven 키워드를 직접 언급할 때 | grabbable-module, rpc, sync-view, room-property |
| **분류 2: 일반 기능** | "상호작용", "UI 제작", "동기화" 등 일반 표현으로 요청할 때 | interaction, ui-creation, sync-state |
| **분류 3: 추상적 요청** | "~만들고 싶어", "설계", "아키텍처" 등 추상적으로 요청할 때 | content-design, implementation-roadmap |
| **분류 4: 트러블슈팅** | "안 돼", "에러", "nil" 등 문제 해결을 요청할 때 | common-errors, injection-troubleshooting, lua-syntax |

자세한 Skill 목록과 트리거 조건은 [02-skill-taxonomy.md](02-skill-taxonomy.md)를 참조하세요.

---

## 2. 플랫폼별 사용 방법

### 2-1. Cursor

**Skill 위치**

- 프로젝트: `.cursor/skills/` 또는 `.agents/skills/`
- 전역: `~/.cursor/skills/`

**Skill 호출 방법**

| 방법 | 설명 |
|------|------|
| **자동 호출** | 채팅 시 Agent가 프롬프트를 분석해 관련 Skill을 자동으로 적용 |
| **슬래시 명령** | Agent 채팅에서 `/` 입력 후 Skill 이름 검색 (예: `/viven-sdk-beginner-workflow`) |
| **명시적 호출** | `/skill-name` 입력으로 특정 Skill만 실행 |
| **컨텍스트 첨부** | `@` 입력 후 Skill을 선택해 대화에 첨부 |

**확인 방법**

- Cursor Settings (Ctrl+Shift+J / Cmd+Shift+J) → Rules → Agent Decides 섹션에서 로드된 Skill 확인

**참고**: [Cursor Skills 문서](https://cursor.com/docs/skills)

---

### 2-2. Antigravity (Google Antigravity)

**Skill 위치**

- 프로젝트: `.agent/skills/`
- 전역: `~/.gemini/antigravity/skills/`

**Skill 호출 방법**

| 방법 | 설명 |
|------|------|
| **자동 호출** | Agent가 사용자 의도를 분석해 `description` 필드와 시맨틱 매칭 후 관련 Skill 로드 |
| **슬래시 명령** | `/skill-name` 형태로 명시적 호출 (플랫폼 지원 시) |

**특징**

- **Progressive Disclosure**: Agent는 처음에 Skill 메타데이터만 로드하고, 관련 작업이 감지되면 전체 지침을 로드
- **스크립트 실행**: Skill 내 `scripts/` 폴더의 Python, Bash, Node 스크립트를 Agent가 실행 가능

**참고**: [Antigravity Skills Codelab](https://codelabs.developers.google.com/getting-started-with-antigravity-skills)

---

### 2-3. Claude Code

**Skill 위치**

- 프로젝트: `.claude/skills/`
- 전역: `~/.claude/skills/`
- 호환: `.cursor/skills/`, `.codex/skills/` (Cursor/Codex 디렉터리도 인식)

**Skill 호출 방법**

| 방법 | 설명 |
|------|------|
| **자동 호출** | Claude가 작업과 관련 있다고 판단하면 Skill을 자동 로드 |
| **슬래시 명령** | `/skill-name` 입력으로 직접 호출 (예: `/viven-sdk-content-design`) |

**특징**

- 중첩 디렉터리 지원: `packages/frontend/.claude/skills/` 등 모노레포 내 패키지별 Skill 로드
- `.claude/commands/`의 기존 명령 파일도 Skill과 동일하게 동작

**참고**: [Claude Code Skills 문서](https://docs.claude.com/en/docs/claude-code/skills)

---

### 2-4. 기타 AI IDE 지원

위 3개 플랫폼 외에도, 심볼릭 링크를 통해 다수의 AI IDE에서 동일한 Viven Skill을 사용할 수 있습니다.

**Skills/Rules 디렉터리 호환 IDE** (심볼릭 링크로 `.cursor/skills/` 공유 가능):

| IDE | 프로젝트 경로 | 비고 |
|-----|---------------|------|
| Codex (OpenAI) | `.agents/skills/` | SKILL.md 형식 동일 |
| Windsurf (Codeium) | `.windsurf/rules/` | YAML frontmatter 구조 유사 |
| Continue | `.continue/rules/` | alwaysApply/globs 구조 동일 |
| Cline | `.clinerules/` | 선택적 frontmatter 지원 |
| Augment Code | `.augment/rules/` | 타 도구 rules 자동 임포트 지원 |

**AGENTS.md 기반 IDE** (별도 `AGENTS.md` 파일 필요):

| IDE | 프로젝트 경로 | 비고 |
|-----|---------------|------|
| GitHub Copilot | `.github/copilot-instructions.md` | `AGENTS.md` 지원 |
| JetBrains Junie | `.junie/guidelines.md` | `AGENTS.md` 우선 탐색 |
| Roo Code | `.roo/rules/` | `AGENTS.md` 지원, 모드별 분리 |
| Amazon Q Developer | `.amazonq/rules/` | 순수 Markdown |
| Trae (ByteDance) | `.trae/rules/` | 순수 Markdown |
| Aider | `CONVENTIONS.md` | 별도 설정 필요 |

심볼릭 링크 설정 방법은 [IDE별 Agent Skill 폴더 구성 가이드](10-ide-skill-setup-guide.md)를 참조하세요.

### 2-5. 플랫폼 공통 요약

| 항목 | Cursor | Antigravity | Claude Code | Codex | Windsurf | Continue |
|------|--------|-------------|-------------|-------|----------|----------|
| 프로젝트 Skill 경로 | `.cursor/skills/` | `.agent/skills/` | `.claude/skills/` | `.agents/skills/` | `.windsurf/rules/` | `.continue/rules/` |
| 자동 호출 | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| `/skill-name` 호출 | ✅ | ✅ | ✅ | ✅ | ❌ | ❌ |
| `@` 컨텍스트 첨부 | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |

---

## 3. 트리거·매핑

Agent는 프롬프트에 포함된 **키워드**를 기반으로 어떤 Skill을 로드할지 결정합니다. 아래 패턴을 참고해 요청하면 원하는 Skill이 더 잘 선택됩니다.

### 3-1. 분류별 트리거 키워드

**분류 1: Viven 구체적 기능**

| 트리거 키워드 | Skill |
|---------------|-------|
| GrabbableModule, onGrab, onRelease, objectShortClickAction | grabbable-module |
| RPC, SendRPC, SendTargetRPC | rpc |
| SyncView, CustomSyncView, TransformView, RigidbodyView | sync-view |
| RoomProperty, 방 상태 | room-property |
| VivenLuaBehaviour, checkInject | lua-behaviour |
| VObject, VMap, VAvatar | vobject, vmap, vavatar |

**분류 2: 일반 기능**

| 트리거 키워드 | Skill |
|---------------|-------|
| 상호작용, 잡기, 클릭, 트리거 | interaction |
| UI 제작, 버튼, 텍스트, 화면 표시 | ui-creation |
| 동기화, 모두에게 보이게, 상태 공유 | sync-state |

**분류 3: 추상적 요청**

| 트리거 키워드 | Skill |
|---------------|-------|
| "~만들고 싶어", "설계", "아키텍처" | content-design |
| "다음 단계", "순서", "구현" | implementation-roadmap |
| "멀티플레이", "협동", "Host" | minigame-architecture |

**분류 4: 트러블슈팅**

| 트리거 키워드 | Skill |
|---------------|-------|
| "안 돼", "에러", "nil", "찾을 수 없음" | common-errors |
| checkInject, local, 주입 | injection-troubleshooting |
| Lua, C# 차이, 콜론, 점 | lua-syntax |

상세 트리거 조건은 [02-skill-taxonomy.md](02-skill-taxonomy.md)를 참조하세요.

### 3-2. 효과적인 프롬프트 작성 팁

- **키워드 포함**: 원하는 기능에 맞는 키워드를 프롬프트에 넣으면 해당 Skill이 선택될 가능성이 높아집니다.
- **구체적으로 요청**: "버튼 누르면 텍스트 바뀌게 해줘"처럼 [조건] → [결과]를 명시하면 interaction·ui-creation Skill이 잘 매칭됩니다.
- **에러 시 메시지 복사**: "nil 이라고 뜨는데"처럼 증상만 쓰기보다, 에러 메시지 전체를 복사해 붙이면 common-errors·injection-troubleshooting Skill이 더 정확히 동작합니다.

---

## 4. 페르소나별 추천 Skill

[Skill 분류](02-skill-taxonomy.md) 6절 페르소나별 매트릭스를 기반으로, 역할별로 우선 사용할 Skill을 정리했습니다.

### 4-1. P1: 비개발자 학생 (초급)

**필수 Skill**

| Skill | 용도 |
|-------|------|
| viven-sdk-beginner-workflow | 작업 순서, 프롬프트 템플릿, 단계별 안내 |
| viven-sdk-content-design | "~만들고 싶어" 요청 시 범위 파악 → 설계 → 구현 단계 분해 |
| viven-sdk-project-setup | 설치, VObject/VMap 구분, 프로젝트 시작 |
| viven-lua-behaviour | VivenLuaBehaviour, start, update, checkInject |
| viven-sdk-interaction | 잡기, 클릭 등 상호작용 |
| viven-sdk-ui-creation | 버튼, 텍스트 등 UI 제작 |
| viven-common-errors | nil, 에러 해결 |
| viven-injection-troubleshooting | checkInject, local 사용 문제 |

**권장 Skill**

- viven-sdk-implementation-roadmap, viven-grabbable-module, viven-sdk-vobject, viven-sdk-sync-state, viven-input, viven-audio, viven-lua-syntax

---

### 4-2. P2: 개발자 학생 (중급)

**필수 Skill**

| Skill | 용도 |
|-------|------|
| viven-sdk-project-setup | 프로젝트 설정 |
| viven-lua-behaviour | Lua 스크립트 기본 |
| viven-grabbable-module | 물체 잡기·놓기 |
| viven-rpc | RPC 호출 |
| viven-sdk-interaction | 상호작용 |
| viven-sdk-ui-creation | UI 제작 |
| viven-sdk-sync-state | 동기화 |
| viven-common-errors | 에러 해결 |
| viven-injection-troubleshooting | 주입 문제 |
| viven-lua-syntax | Lua vs C# 문법 |

**권장 Skill**

- viven-sdk-content-design, viven-sdk-implementation-roadmap, viven-sdk-project-config, viven-sdk-vmap, viven-sdk-vobject, viven-sdk-vavatar, viven-sdk-api, viven-sittable-module, viven-sync-view, viven-room-property, viven-async, viven-physics, viven-world-building, viven-input, viven-audio, viven-avatar, viven-player, viven-error-log

---

### 4-3. P3: 컨텐츠 제작자 (고급)

**필수 Skill**

| Skill | 용도 |
|-------|------|
| viven-minigame-architecture | Host/Client, 동기화 아키텍처 |
| viven-sdk-project-config | Addressable, OpenXR 등 설정 |
| viven-build-deploy | 빌드·배포 |
| viven-sdk-vmap, viven-sdk-vobject, viven-sdk-vavatar | VMap/VObject/VAvatar 제작 |
| viven-sdk-api | SDK API 참조 |
| viven-lua-behaviour, viven-grabbable-module, viven-sittable-module | 스크립팅·상호작용 |
| viven-rpc, viven-sync-view, viven-room-property | 네트워킹 |
| viven-async, viven-security | 비동기·보안 |
| viven-sdk-interaction, viven-sdk-ui-creation, viven-sdk-sync-state | 일반 기능 |
| viven-physics, viven-world-building | 3D·물리 |
| viven-input, viven-audio, viven-avatar, viven-player | 입력·오디오·캐릭터 |
| viven-common-errors, viven-lua-syntax, viven-error-log, viven-performance | 트러블슈팅·성능 |

**권장 Skill**

- viven-sdk-content-design, viven-network-variables, viven-spatial, viven-chat, viven-injection-troubleshooting

---

## 5. 프롬프트 예시

[페르소나·시나리오](01-personas-and-scenarios.md)의 프롬프트 템플릿을 기반으로 한 예시입니다.

### 5-1. 템플릿 A: "~하고 싶어" (추상적·한 번에 요청)

```
[만들고 싶은 것] 만들고 싶어.
```

**예시**

- "주사위 게임 만들고 싶어"
- "가위바위보 게임 만들고 싶어"

→ **content-design** Skill이 로드됩니다. AI가 범위 파악 질문 → 설계 제시 → 구현 단계 분해 후 단계별로 안내합니다.

---

### 5-2. 템플릿 B: "~하면 ~되게 해줘" (동작 명시)

```
[조건] 하면 [결과] 되게 해줘.
```

**예시**

- "버튼 누르면 텍스트가 바뀌게 해줘"
- "오브젝트를 잡으면 이펙트 나오게 해줘"
- "의자에 앉으면 화면이 바뀌게 해줘"

→ **interaction**, **ui-creation** Skill이 로드됩니다. AI가 Lua 코드와 Inspector 설정 방법을 제시합니다.

---

### 5-3. 템플릿 C: "~가 안 돼" (에러 대응)

```
[증상] 인데 뭐가 문제야?
[에러 메시지] 라고 뜨는데 고쳐줘.
```

**예시**

- "스크립트 실행이 안 돼, nil 이라고 뜨는데 뭐가 문제야?"
- "attempt to call nil 이라고 뜨는데 고쳐줘"

→ **common-errors**, **injection-troubleshooting** Skill이 로드됩니다. 에러 메시지를 그대로 복사해 붙이면 더 정확한 해결안을 받을 수 있습니다.

---

### 5-4. 템플릿 D: "다음 단계 뭐야?" (순서 진행)

```
1단계 끝났어. 다음에 뭐 해?
```

**예시**

- "버튼 뼈대 만들었어. 다음에 뭐 해?"
- "1단계 끝났어. 다음"

→ **implementation-roadmap**, **beginner-workflow** Skill이 로드됩니다. 구현 순서에 따라 다음 단계를 안내합니다.

---

### 5-5. Viven 키워드 활용 (고급)

```
GrabbableModule의 onGrab에서 이펙트 재생하는 예제 보여줘
RPC로 주사위 결과를 모두에게 전달하는 방법 알려줘
CustomSyncView로 주사위 눈 값 동기화하는 구조로 바꿔줘
```

→ **grabbable-module**, **rpc**, **sync-view** 등 분류 1 Skill이 로드됩니다.

---

## 6. 관련 문서

| 문서 | 내용 |
|------|------|
| [Skill 분류](02-skill-taxonomy.md) | Skill 4분류, 전체 Skill 목록, 트리거 조건, 키워드 매핑 |
| [페르소나 및 시나리오](01-personas-and-scenarios.md) | 페르소나별 시나리오와 예상 프롬프트 |
| [비개발자 프롬프트 가이드](04-beginner-prompt-guide.md) | 초급·중급·고급 학습 경로, 프롬프트 패턴 |
| [컨텐츠 설계 (한 번에 요청)](05-content-design-for-one-shot-requests.md) | "~만들고 싶어" 요청 시 설계·범위 파악·구현 단계 |
| [트러블슈팅 인덱스](03-troubleshooting-index.md) | 자주 발생하는 에러·증상 인덱스 |
| [Agent Skill 개발 가이드](09-agent-skill-dev-guide.md) | Skill *개발* 방법 (SKILL.md 구조, 트리거 설계) |
