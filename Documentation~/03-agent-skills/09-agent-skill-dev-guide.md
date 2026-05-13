# Agent Skill 개발 가이드

> 관련 문서: [Skill 분류](02-skill-taxonomy.md) | [산출물 정리](07-output-summary.md)

**대상**: Agent Skill을 *개발*하는 개발자 (Skill 추가·수정·확장)

본 문서는 Viven Agent Skill을 **개발**하는 방법을 안내합니다. Skill *사용* 방법은 [08-agent-skill-user-guide.md](08-agent-skill-user-guide.md)를 참조하세요.

---

## 1. Skill 개요

### 1-1. Cursor Skill 구조

**Agent Skill**은 AI 에이전트가 특정 작업을 수행할 수 있도록 도메인 지식과 워크플로우를 담은 패키지입니다. [Agent Skills](https://agentskills.io/) 오픈 표준을 따르며, Cursor, Antigravity, Claude Code 등에서 공통 사용됩니다.

**디렉터리 레이아웃**

```
.cursor/skills/
├── viven-sdk-grabbable-module/
│   └── SKILL.md              # 필수 - 메인 지침
├── viven-sdk-common-errors/
│   └── SKILL.md
└── viven-sdk-beginner-workflow/
    └── SKILL.md
```

| 항목 | 설명 |
|------|------|
| **프로젝트 Skill** | `.cursor/skills/` — 해당 프로젝트에서만 사용 |
| **전역 Skill** | `~/.cursor/skills/` — 모든 프로젝트에서 사용 |

**주의**: `~/.cursor/skills-cursor/`는 Cursor 내장 Skill 전용이므로 사용하지 않습니다.

### 1-2. SKILL.md 파일 형식

모든 Skill은 **YAML frontmatter**와 **Markdown 본문**으로 구성됩니다.

```markdown
---
name: viven-sdk-example
description: >-
  Brief description of what this skill does and when to use it.
---

# Skill 제목

## 본문 섹션
...
```

| 구성 요소 | 역할 |
|-----------|------|
| **YAML frontmatter** | `name`, `description` — Agent가 Skill 선택 시 참조 |
| **Markdown 본문** | Agent가 해당 Skill을 적용할 때 읽는 지침 |

---

## 2. Skill 작성 방법

### 2-1. name 필드

| 요구사항 | 설명 |
|----------|------|
| **형식** | 소문자, 숫자, 하이픈만 사용 |
| **길이** | 최대 64자 |
| **고유성** | 프로젝트 내 다른 Skill과 중복 불가 |

**Viven Skill 네이밍 규칙**

- `viven-sdk-{기능명}` 또는 `viven-{기능명}`
- 예: `viven-sdk-grabbable-module`, `viven-sdk-beginner-workflow`

### 2-2. description 필드

**description은 Skill 발견의 핵심**입니다. Agent가 프롬프트를 분석해 이 필드와 매칭하여 Skill을 로드합니다.

| 요구사항 | 설명 |
|----------|------|
| **길이** | 최대 1024자, 비어 있으면 안 됨 |
| **인칭** | 3인칭으로 작성 (시스템 프롬프트에 주입됨) |
| **내용** | WHAT(무엇을 하는지) + WHEN(언제 사용하는지) |

**작성 원칙**

1. **구체적 키워드 포함**: 트리거로 사용할 용어를 description에 명시
2. **Use when ~ 패턴**: "Use when the user mentions X, Y, Z" 형태로 트리거 조건 명시

**예시**

```yaml
# 좋은 예
description: >-
  Guides GrabbableModule usage with onGrab, onRelease, objectShortClickAction
  and related Lua events. Use when the user mentions GrabbableModule, onGrab,
  onRelease, objectShortClickAction, 잡기, 클릭, 물체 상호작용.

# 나쁜 예 (모호함)
description: Helps with Viven scripting.
```

### 2-3. 본문 작성 규칙

| 원칙 | 설명 |
|------|------|
| **간결성** | Agent는 이미 똑똑함. Agent가 모르는 정보만 추가 |
| **500줄 이하** | SKILL.md 본문은 500줄 이내 권장 |
| **점진적 공개** | 상세 내용은 `reference.md` 등 별도 파일로 분리, 필요 시 링크 |
| **용어 일관성** | 한 Skill 내에서 동일 용어 사용 (예: "주입" vs "injection" 혼용 금지) |

**본문 구조 권장**

```markdown
# Skill 제목

## 트리거 조건
(이 Skill이 선택되는 키워드·상황)

## 1. 핵심 지침
(필수 절차, 코드 예시)

## 2. 추가 정보
(선택 사항, 주의점)

## 3. 참조 문서
(VivenGuide 기반 문서 경로)
```

---

## 3. 트리거 설계

### 3-1. 키워드 정의

Agent는 프롬프트에 포함된 **키워드**를 기반으로 Skill을 선택합니다. description과 본문의 "트리거 조건" 섹션에 키워드를 명시합니다.

**키워드 유형**

| 유형 | 예시 | Skill 분류 |
|------|------|------------|
| **Viven 구체적 키워드** | GrabbableModule, RPC, SyncView, RoomProperty | 분류 1 |
| **일반 기능 키워드** | 상호작용, UI 제작, 동기화, 버튼 | 분류 2 |
| **추상적 표현** | "~만들고 싶어", 설계, 아키텍처 | 분류 3 |
| **에러·증상** | "안 돼", "에러", "nil", "찾을 수 없음" | 분류 4 |

### 3-2. 매핑 로직

분류별 매핑 로직:

| 분류 | 매핑 방식 | 예시 |
|------|-----------|------|
| **분류 1** | 프롬프트에 Viven 키워드 포함 시 해당 Skill 로드 | "onGrab 예제" → grabbable-module |
| **분류 2** | 일반 표현 → Viven 기능 매핑 | "상호작용" → GrabbableModule, onClick |
| **분류 3** | 추상적 요청 → content-design, implementation-roadmap | "주사위 만들고 싶어" → content-design |
| **분류 4** | 에러 패턴 → 트러블슈팅 Skill | "nil 뜨는데" → common-errors |

**의도 추론 예시**

- "오브젝트 잡으면 이펙트 나오게 해줘" → **interaction** + **grabbable-module**
- "버튼 누르면 텍스트 바뀌게 해줘" → **ui-creation**
- "모두에게 보이게 해줘" → **sync-state** (RPC/RoomProperty/SyncView)

### 3-3. 분류별 트리거 패턴

[Skill 분류](02-skill-taxonomy.md) 기반 트리거 패턴:

**분류 1: Viven 구체적 기능**

| 트리거 키워드 | Skill |
|---------------|-------|
| GrabbableModule, onGrab, onRelease, objectShortClickAction | grabbable-module |
| RPC, SendRPC, SendTargetRPC | rpc |
| SyncView, CustomSyncView, TransformView, RigidbodyView | sync-view |
| RoomProperty, 방 상태 | room-property |
| VivenLuaBehaviour, checkInject, namespace | lua-behaviour |
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
| "~만들고 싶어", 설계, 아키텍처 | content-design |
| "다음 단계", 순서, 구현 | implementation-roadmap |
| "처음", "시작", "뭘 해야 해" | beginner-workflow |

**분류 4: 트러블슈팅**

| 트리거 키워드 | Skill |
|---------------|-------|
| "안 돼", "에러", "nil", "찾을 수 없음" | common-errors |
| checkInject, local, 주입 | injection-troubleshooting |
| Lua, C# 차이, 콜론, 점 | lua-syntax |

### 3-4. 트리거 설계 체크리스트

새 Skill 작성 시 확인:

- [ ] description에 트리거 키워드 3개 이상 포함
- [ ] "Use when ~" 패턴으로 사용 시점 명시
- [ ] 본문에 "트리거 조건" 섹션 포함
- [ ] 기존 Skill과 트리거 중복·충돌 여부 검토

---

## 4. 참조 문서 연동

### 4-1. VivenGuide 기반 경로

Skill 내부에서 Viven 가이드 문서를 참조할 때는 [VivenGuide 목차](../00-index.md) 기반 경로를 사용합니다.

**경로 규칙**

- 상대 경로: `VivenGuide/02-content-creation/03-scripting/...`
- 또는 프로젝트 루트 기준: `VivenGuide/...`

**예시 (SKILL.md 본문)**

```markdown
## 참조 문서

- [GrabbableModule](VivenGuide/02-content-creation/03-scripting/04-player-interaction-modules/01-grabbable-module.md)
- [VivenLuaBehaviour](VivenGuide/02-content-creation/03-scripting/01-viven-lua-behaviour.md)
- [RPC](VivenGuide/02-content-creation/03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md)
```

### 4-2. 컨텐츠 유형별 Skill

[Skill 분류](02-skill-taxonomy.md)에 따라 구체적 기능 분류를 반영한 Skill:

| 분류 | Skill | 참조 |
|--------------|-------|------|
| 프로젝트 설정 확인 | viven-sdk-project-config | Addressable, OpenXR |
| VivenScript 공통 | viven-sdk-viven-script | namespace, 이벤트, PLO, DoTween |
| VMAP 제작 | viven-sdk-vmap | startPoint, ModuleScript |
| VOBJECT 제작 | viven-sdk-vobject | Grabbable, Sittable, 텔레포트 |
| VAvatar 제작 | viven-sdk-vavatar | Override Animation |
| SDK 문서 | viven-sdk-api | sdkdoc.viven.app |

### 4-3. ToC 섹션 → Skill 매핑

[Skill 분류](02-skill-taxonomy.md) 7절 참조. 새 Skill 추가 시 해당 ToC 섹션과 매핑 관계를 [02-skill-taxonomy.md](02-skill-taxonomy.md)에 반영합니다.

---

## 5. Skill 테스트

### 5-1. 작성 검증

Skill 작성 후 다음을 확인합니다:

| 항목 | 확인 방법 |
|------|-----------|
| **YAML 문법** | frontmatter `---` 구분, 들여쓰기, `description` 이어쓰기(`>-`) |
| **name/description** | 64자/1024자 이내, 필수 필드 누락 없음 |
| **본문** | 500줄 이내 권장, 링크 깨짐 없음 |
| **트리거** | description에 키워드 포함, "Use when" 패턴 |

### 5-2. 트리거 동작 확인

1. **프롬프트 테스트**: 트리거 키워드를 포함한 프롬프트로 채팅
2. **Skill 로드 확인**: Cursor Settings → Rules → Agent Decides에서 로드된 Skill 확인
3. **응답 품질**: Agent가 해당 Skill의 지침을 따르는지 검증

**테스트 프롬프트 예시**

| Skill | 테스트 프롬프트 |
|-------|-----------------|
| grabbable-module | "onGrab에서 이펙트 재생하는 예제 보여줘" |
| common-errors | "nil 이라고 뜨는데 뭐가 문제야?" |
| content-design | "주사위 게임 만들고 싶어" |

---

## 6. 기존 Skill 확장

### 6-1. 새 Skill 추가 절차

1. **분류 결정**: [Skill 분류](02-skill-taxonomy.md) 4분류 중 해당 분류 선택
2. **트리거 정의**: 기존 Skill과 중복되지 않는 키워드 설계
3. **디렉터리 생성**: `.cursor/skills/viven-sdk-{이름}/`
4. **SKILL.md 작성**: name, description, 본문 (트리거 조건, 지침, 참조 문서)
5. **02-skill-taxonomy.md 반영**: Skill 목록·트리거 조건 추가
6. **테스트**: 트리거 동작 및 응답 품질 확인

### 6-2. 기존 Skill 수정 절차

1. **변경 범위 확인**: description만 수정 vs 본문 대폭 수정
2. **트리거 영향 검토**: 키워드 추가/삭제 시 기존 사용자 프롬프트 매칭 영향
3. **수정 적용**: SKILL.md 편집
4. **02-skill-taxonomy.md 동기화**: 트리거·설명 변경 시 taxonomy 문서 업데이트
5. **회귀 테스트**: 기존 테스트 프롬프트로 동작 확인

### 6-3. Skill 간 참조

한 Skill에서 다른 Skill을 참조할 때:

```markdown
## 에러 패턴 → Skill 매핑

| 에러/증상 | 이 Skill | 추가 참조 |
|-----------|----------|-----------|
| nil, attempt to call nil | ✓ | viven-sdk-injection-troubleshooting |
| RPC function not found | ✓ | - |
```

트러블슈팅 Skill은 에러 패턴에 따라 적절한 Skill로 연결하는 매핑표를 포함하는 것이 좋습니다.

---

## 7. 관련 문서

| 문서 | 내용 |
|------|------|
| [Skill 분류](02-skill-taxonomy.md) | Skill 4분류, 전체 Skill 목록, 트리거 조건, 시나리오-Skill 매핑 |
| [산출물 정리](07-output-summary.md) | 산출물 경로, 역할별 분류 |
| [Agent Skill 사용 가이드](08-agent-skill-user-guide.md) | Skill *사용* 방법 |
| [VivenGuide 목차](../00-index.md) | Viven 문서 목차 |
| [create-skill (Cursor)](https://cursor.com/docs/skills) | Cursor Skill 작성 일반 가이드 |
