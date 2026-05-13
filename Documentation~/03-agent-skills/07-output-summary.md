# 산출물 정리

> 관련 문서: [페르소나·시나리오](01-personas-and-scenarios.md) | [Skill 분류](02-skill-taxonomy.md)

본 문서는 Agent Skills 관련 **전체 산출물 목록**, **경로**, **역할별 분류**를 정리합니다.

---

## 1. 문서 목록 (VivenGuide/03-agent-skills/)

| 문서 | 경로 | 설명 |
|------|------|------|
| 개요 | `00-overview.md` | Agent Skills 개요, 관련 문서 링크 |
| 페르소나·시나리오 | `01-personas-and-scenarios.md` | 페르소나 정의, 시나리오, 예상 프롬프트 |
| Skill 분류·목록 | `02-skill-taxonomy.md` | Skill 4분류, 목록, 트리거 조건 |
| 트러블슈팅 인덱스 | `03-troubleshooting-index.md` | 자주 발생하는 에러·증상 인덱스 |
| 비개발자 가이드 | `04-beginner-prompt-guide.md` | 비개발자 작업 순서, 프롬프트 작성 방법 |
| 컨텐츠 설계 | `05-content-design-for-one-shot-requests.md` | 한 번에 요청 시 설계 방법, 범위 파악, 구현 단계 분해 |
| **산출물 정리** | `07-output-summary.md` | 본 문서. 전체 산출물 목록, 경로, 역할별 분류 |
| Agent Skill (사용자) | `08-agent-skill-user-guide.md` | Skill *사용* 가이드. Cursor/Antigravity/Claude Code 기준 |
| Agent Skill (개발자) | `09-agent-skill-dev-guide.md` | Skill *개발* 가이드. SKILL.md 구조, 트리거 설계 |

---

## 2. Skill 목록 (.cursor/skills/)

| 카테고리 | Skill | 경로 | 설명 |
|----------|-------|------|------|
| **워크플로우** | viven-sdk-beginner-workflow | `viven-sdk-beginner-workflow/SKILL.md` | 비개발자 작업 순서, 프롬프트 템플릿 |
| | viven-sdk-content-design | `viven-sdk-content-design/SKILL.md` | 범위 파악 → 설계 제시 → 구현 단계 분해 |
| | viven-sdk-implementation-roadmap | `viven-sdk-implementation-roadmap/SKILL.md` | RPS 구현 가이드 스타일 단계 분해 |
| **프로젝트 설정** | viven-sdk-project-setup | `viven-sdk-project-setup/SKILL.md` | SDK 설치, VObject/VMap/VAvatar 구분 |
| | viven-sdk-project-config | `viven-sdk-project-config/SKILL.md` | Addressable, OpenXR 설정 확인 |
| **SDK 구체적 기능** | viven-sdk-lua-behaviour | `viven-sdk-lua-behaviour/SKILL.md` | VivenLuaBehaviour, start, update, checkInject |
| | viven-sdk-grabbable-module | `viven-sdk-grabbable-module/SKILL.md` | onGrab, onRelease, objectShortClickAction |
| | viven-sdk-rpc | `viven-sdk-rpc/SKILL.md` | SendRPC, SendTargetRPC, 직렬화 제한 |
| | viven-sdk-sync-view | `viven-sdk-sync-view/SKILL.md` | TransformView, CustomSyncView, sendSyncUpdate/receiveSyncUpdate |
| | viven-sdk-room-property | `viven-sdk-room-property/SKILL.md` | RoomProperty, 방 단위 상태 공유 |
| **일반 기능** | viven-sdk-interaction | `viven-sdk-interaction/SKILL.md` | 상호작용, 잡기, 클릭, 트리거 |
| | viven-sdk-ui-creation | `viven-sdk-ui-creation/SKILL.md` | UI 제작, 버튼, 텍스트, 화면 표시 |
| | viven-sdk-sync-state | `viven-sdk-sync-state/SKILL.md` | 동기화, 모두에게 보이게, 상태 공유 |
| **컨텐츠 유형별** | viven-sdk-viven-script | `viven-sdk-viven-script/SKILL.md` | namespace, Life Cycle, PLO, DoTween, 전역 변수 |
| | viven-sdk-vmap | `viven-sdk-vmap/SKILL.md` | VMap, startPoint, ModuleScript |
| | viven-sdk-vobject | `viven-sdk-vobject/SKILL.md` | VObject, Grabbable/Sittable, 텔레포트 |
| | viven-sdk-vavatar | `viven-sdk-vavatar/SKILL.md` | VAvatar, Override Animation, 아바타 설정 |
| | viven-sdk-api | `viven-sdk-api/SKILL.md` | sdkdoc.viven.app, Player/UI/Room/XR API |
| **트러블슈팅** | viven-sdk-common-errors | `viven-sdk-common-errors/SKILL.md` | nil, RPC 함수명 오타, SyncView receiveSyncUpdate |
| | viven-sdk-injection-troubleshooting | `viven-sdk-injection-troubleshooting/SKILL.md` | checkInject, local 금지, 지원 타입 |
| | viven-sdk-lua-syntax | `viven-sdk-lua-syntax/SKILL.md` | C# vs Lua, 콜론 vs 점, 1-based 인덱스 |
| **빌드·배포** | viven-sdk-build-deploy | `viven-sdk-build-deploy/SKILL.md` | VMap/VObject 빌드, 콘텐츠 업로드, 심사 과정 |
| **아키텍처** | viven-sdk-minigame-architecture | `viven-sdk-minigame-architecture/SKILL.md` | 멀티플레이 미니게임, Host/Client, RPC vs SyncView 선택 |
| **기타** | vivenguide-docs-setup | `vivenguide-docs-setup/SKILL.md` | VivenGuide 문서 체계 설계 |

---

## 3. 역할별 분류

### 3-1. 페르소나별 추천 문서·Skill

| 역할 | 페르소나 | 우선 문서 | 우선 Skill |
|------|----------|-----------|------------|
| **P1: 비개발자** | Unity·Viven·Lua 처음 | 04-beginner-prompt-guide, 01-personas-and-scenarios | viven-sdk-beginner-workflow, viven-sdk-content-design, viven-sdk-implementation-roadmap, viven-sdk-project-setup, viven-sdk-interaction, viven-sdk-ui-creation, viven-sdk-common-errors, viven-sdk-injection-troubleshooting |
| **P2: 개발자** | Unity·C# 경험 있음, Viven·Lua 처음 | 02-skill-taxonomy, 08-agent-skill-user-guide | viven-sdk-content-design, viven-sdk-project-setup, viven-sdk-lua-behaviour, viven-sdk-grabbable-module, viven-sdk-rpc, viven-sdk-interaction, viven-sdk-ui-creation, viven-sdk-sync-state, viven-sdk-common-errors, viven-sdk-injection-troubleshooting, viven-sdk-lua-syntax |
| **P3: 제작자** | Unity·Viven·Lua 경험 있음 | 02-skill-taxonomy, 08-agent-skill-user-guide, 09-agent-skill-dev-guide | viven-sdk-grabbable-module, viven-sdk-rpc, viven-sdk-sync-view, viven-sdk-room-property, viven-sdk-vmap, viven-sdk-vobject, viven-sdk-vavatar, viven-sdk-api, viven-sdk-common-errors, viven-sdk-lua-syntax |

### 3-2. 사용 목적별 문서·Skill

| 목적 | 대상 | 문서 | Skill |
|------|------|------|-------|
| **Skill 사용** | 컨텐츠 제작자, 비개발자·개발자 학생 | 08-agent-skill-user-guide | 전체 Skill (플랫폼별 호출 방법 참조) |
| **Skill 개발** | Skill 추가·수정·확장 담당자 | 09-agent-skill-dev-guide | - |
| **비개발자 시작** | 처음 Viven 컨텐츠 제작 | 04-beginner-prompt-guide, 01-personas-and-scenarios | viven-sdk-beginner-workflow |
| **한 번에 요청 설계** | "~만들고 싶어" 추상적 요청 | 05-content-design-for-one-shot-requests | viven-sdk-content-design |
| **에러 해결** | nil, RPC 오류, SyncView 문제 | 03-troubleshooting-index | viven-sdk-common-errors, viven-sdk-injection-troubleshooting, viven-sdk-lua-syntax |
| **Skill 전체 파악** | Skill 목록·트리거·분류 확인 | 02-skill-taxonomy | - |

### 3-3. 문서·Skill 참조 관계

```
00-overview ──┬── 01-personas-and-scenarios
              ├── 02-skill-taxonomy
              ├── 04-beginner-prompt-guide
              ├── 05-content-design-for-one-shot-requests
              ├── 07-output-summary (본 문서)
              ├── 08-agent-skill-user-guide
              └── 09-agent-skill-dev-guide

01 ──→ 04, 05 (페르소나별 가이드)
02 ──→ 08/09 가이드 (Skill 분류 참조)
```

---

## 4. 빠른 참조

| 하고 싶은 일 | 참조 |
|--------------|------|
| Skill 사용 방법 알기 | [08-agent-skill-user-guide](08-agent-skill-user-guide.md) |
| Skill 개발·추가 방법 | [09-agent-skill-dev-guide](09-agent-skill-dev-guide.md) |
| 비개발자로 처음 시작 | [04-beginner-prompt-guide](04-beginner-prompt-guide.md) |
| Skill 전체 목록·트리거 | [02-skill-taxonomy](02-skill-taxonomy.md) |
| 에러·증상 인덱스 | [03-troubleshooting-index](03-troubleshooting-index.md) |
| 한 번에 요청 설계 | [05-content-design-for-one-shot-requests](05-content-design-for-one-shot-requests.md) |
