# Skill 분류 및 목록 (Taxonomy & List)

> 관련 문서: [페르소나·시나리오](01-personas-and-scenarios.md) | [산출물 정리](07-output-summary.md)

본 문서는 Skill 4분류 체계를 기반으로 Skill 목록, 트리거 조건, 키워드·API 매핑을 정리합니다.

---

## 1. 분류 1: Viven 구체적 기능 명시 (키워드 기반)

**설명**: 사용자가 GrabbableModule, RPC, SyncView 등 Viven 특화 키워드를 직접 언급할 때 해당 Skill을 로드합니다.

### 트리거 조건

| 트리거 키워드 | 설명 |
|---------------|------|
| GrabbableModule, onGrab, onRelease, objectShortClickAction, objectLongClickAction | 물체 잡기·놓기·클릭 이벤트 |
| hold start/end, attach start/end | GrabbableModule 홀드·어태치 이벤트 |
| SittableModule, 앉기 | 의자·좌석 상호작용 |
| RPC, SendRPC, SendTargetRPC, RPCSendOption | 원격 프로시저 호출 |
| SyncView, CustomSyncView, TransformView, RigidbodyView | 동기화 뷰 |
| sendSyncUpdate, receiveSyncUpdate | 커스텀 동기화 데이터 송수신 |
| NetworkVariable | 네트워크 변수 |
| RoomProperty | 방 단위 상태 공유 |
| VivenLuaBehaviour, checkInject | Lua 스크립트·주입 |
| VObject | Viven 오브젝트 → V2 viven-sdk-vobject |
| VMap | Viven 맵 → V1 viven-sdk-vmap |

### 분류 1 Skill 목록

| Skill ID | 스킬명 | 트리거 | 참조 문서 |
|----------|--------|--------|-----------|
| S1 | viven-sdk-lua-behaviour | VivenLuaBehaviour, start, update, checkInject | [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) |
| S2 | viven-sdk-grabbable-module | GrabbableModule, onGrab, onRelease, objectShortClickAction, objectLongClickAction, hold start/end, attach start/end | [GrabbableModule](../02-content-creation/03-scripting/04-player-interaction-modules/01-grabbable-module.md) |
| S3 | viven-sdk-sittable-module | SittableModule, 앉기 | [SittableModule](../02-content-creation/03-scripting/04-player-interaction-modules/02-sittable-module.md) |
| S4 | viven-sdk-rpc | RPC, SendRPC, SendTargetRPC, RPCSendOption | [RPC](../02-content-creation/03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md) |
| S5 | viven-sdk-sync-view | SyncView, CustomSyncView, TransformView, RigidbodyView, sendSyncUpdate, receiveSyncUpdate | [SyncView](../02-content-creation/03-scripting/05-networking-and-synchronization/03-sync-view.md) |
| S6 | viven-sdk-network-variables | NetworkVariable | [Network Variables](../02-content-creation/03-scripting/05-networking-and-synchronization/02-network-variables.md) |
| S7 | viven-sdk-room-property | RoomProperty, 방 상태 | [Room Property](../02-content-creation/03-scripting/05-networking-and-synchronization/04-room-property.md) |

---

## 2. 분류 2: 일반적인 기능 요청

**설명**: 사용자가 "상호작용", "UI 제작", "동기화" 등 일반적인 표현으로 요청할 때, Agent가 해당 키워드를 Viven API·문서로 매핑합니다.

### 매핑 로직

Agent가 "상호작용" → GrabbableModule/onClick, "UI 제작" → World Space UI·Canvas, "동기화" → RPC/RoomProperty 패턴으로 해석합니다. 도메인(총, 점수판, 주사위)은 사용자 표현일 뿐, 키워드 기반으로 Skill을 선택합니다.

### 페르소나별 시나리오 기반 사용자 표현 → Skill 매핑

[01-personas-and-scenarios](01-personas-and-scenarios.md)의 시나리오 중 **분류 2 (일반 기능)**에 해당하는 시나리오를 기반으로, 사용자 표현에서 키워드를 추출하고 Skill을 매핑합니다.

| 시나리오 | 페르소나 | 예상 프롬프트 (사용자 표현) | 추출 키워드 | Skill |
|----------|----------|---------------------------|-------------|-------|
| **S1-1** | P1 비개발자 | "Viven에서 버튼 누르면 텍스트가 바뀌는 간단한 UI 만들어줘" | 버튼, 텍스트, UI, 바뀌게 | viven-sdk-ui-creation |
| **S1-2** | P1 비개발자 | "오브젝트를 잡으면 이펙트 나오게 해줘" | 잡기, 오브젝트, 이펙트 | viven-sdk-interaction |
| **S1-8** | P1 비개발자 | "버튼 누르면 소리 나오게 해줘" | 버튼, 소리, 클릭 | viven-sdk-ui-creation, viven-sdk-audio |
| **S1-9** | P1 비개발자 | "의자에 앉으면 화면이 바뀌게 해줘" | 앉기, 의자, 화면 바뀌게 | viven-sdk-interaction, viven-sdk-ui-creation |
| **S1-11** | P1 비개발자 | "10초 뒤에 텍스트 나오게 해줘" | 시간, 대기, 텍스트 | viven-sdk-ui-creation |
| **S1-13** | P1 비개발자 | "채팅 입력하면 화면에 표시되는 거 만들어줘" | 채팅, 화면 표시, UI | viven-sdk-ui-creation, viven-sdk-chat |
| **S2-1** | P2 개발자 | "총을 잡아서 버튼 누르면 총알이 발사되게 만들어줘" | 잡기, 버튼, 발사 | viven-sdk-interaction, viven-sdk-ui-creation |
| **S2-2** | P2 개발자 | "UI에 점수 현황판이 뜨도록 만들어줘. 플레이어별로 갱신되게" | 점수판, UI, 플레이어별, 갱신 | viven-sdk-ui-creation, viven-sdk-sync-state |
| **S2-3** | P2 개발자 | "주사위를 던졌을 때 결과가 모든 플레이어에게 보이게 해줘" | 모두에게 보이게, 결과, 동기화 | viven-sdk-sync-state |
| **S2-11** | P2 개발자 | "PC VR 버튼 입력 받는 거 Lua로 어떻게 해?" | 입력, 버튼, PC/VR | viven-sdk-input |
| **S2-13** | P2 개발자 | "아바타 애니메이션 오버라이드 하는 방법 알려줘" | 아바타, 애니메이션, 오버라이드 | viven-sdk-avatar |
| **S2-8** | P2 개발자 | "방에 들어온 사람 수를 UI에 표시하고 싶어" | 방 상태, UI, 표시 | viven-sdk-sync-state, viven-sdk-ui-creation |
| **S2-9** | P2 개발자 | "에셋 로드가 끝난 후에 UI를 띄우는 방식을 Lua로 구현하고 싶어" | 비동기, 에셋, UI | viven-sdk-async, viven-sdk-ui-creation |
| **S2-12** | P2 개발자 | "채팅 메시지를 보내고 받는 API 사용법을 알고 싶어" | 채팅, API | viven-sdk-chat |
| **S2-15** | P2 개발자 | "플레이어 텔레포트 API 사용법을 알고 싶어" | 텔레포트, 플레이어 | viven-sdk-player |
| **S3-2** | P3 제작자 | "RPC로 주사위 결과를 전달해 UI를 모두에게 갱신하려는 상황" | RPC, UI, 동기화 | viven-sdk-rpc, viven-sdk-ui-creation, viven-sdk-sync-state |

**통합 원칙**: 도메인 단어(총, 주사위, 점수판 등)는 무시하고 **기능 키워드**(잡기, 버튼, UI, 갱신, 동기화 등)로 Skill을 선택합니다. 복수 Skill이 필요한 시나리오는 Agent가 순차적으로 관련 Skill을 로드합니다.

### 일반 기능 키워드·API 매핑

| 키워드 (사용자 표현) | 매핑되는 Viven 기능·API | Skill | 참조 문서 |
|---------------------|-------------------------|-------|-----------|
| **상호작용** (잡기, 클릭, 트리거, 터치, 홀드, 어태치) | GrabbableModule, SittableModule, objectShortClickAction, objectLongClickAction, hold/attach, onClick, onGrab, onRelease | viven-sdk-interaction | [GrabbableModule](../02-content-creation/03-scripting/04-player-interaction-modules/01-grabbable-module.md), [상호작용 이벤트](../02-content-creation/03-scripting/04-player-interaction-modules/03-interaction-event-handling.md) |
| **UI 제작** (화면 표시, 텍스트, 버튼, HUD) | World Space Canvas, VivenGraphicRaycaster, TextMeshPro, UIModeChanger, checkInject, onClick.AddListener | viven-sdk-ui-creation | [World Space UI](../02-content-creation/06-ui/02-world-space-ui.md), [UGUI 가이드](../02-content-creation/06-ui/01-unity-ugui-guide.md), [Viven UI 컴포넌트](../02-content-creation/06-ui/03-viven-ui-components.md) |
| **동기화** (모두에게 보이게, 상태 공유, 여러 사람) | RPC, RoomProperty, SyncView, Global 테이블, SendRPC, receiveSyncUpdate | viven-sdk-sync-state | [RPC](../02-content-creation/03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md), [Room Property](../02-content-creation/03-scripting/05-networking-and-synchronization/04-room-property.md), [SyncView](../02-content-creation/03-scripting/05-networking-and-synchronization/03-sync-view.md), [동기화 시스템](../02-content-creation/01-project-management/06-viven-architecture/02-synchronization-system.md) |

### SDK 문서·가이드 기반 상세 매핑

#### 상호작용 API 매핑 (viven-sdk-interaction)

| 사용자 표현 | Viven API·컴포넌트 | Lua 이벤트·함수 | 문서 경로 |
|-------------|-------------------|-----------------|-----------|
| 물체 잡기, 들기 | SDKGrabbableModule, VivenGrabbableModule | onGrab, onRelease | 02-content-creation/03-scripting/04-player-interaction-modules/01-grabbable-module.md |
| 의자 앉기 | SittableModule | onSit, onStand | 02-content-creation/03-scripting/04-player-interaction-modules/02-sittable-module.md |
| 짧게/길게 클릭 | objectShortClickAction, objectLongClickAction | objectShortClickAction, objectLongClickAction | 02-content-creation/03-scripting/04-player-interaction-modules/01-grabbable-module.md |
| 홀드·어태치 | hold start/end, attach start/end | holdStart, holdEnd, attachStart, attachEnd | 02-content-creation/03-scripting/04-player-interaction-modules/01-grabbable-module.md |
| 버튼 클릭 | UI Button, onClick | onClick.AddListener | 02-content-creation/06-ui/01-unity-ugui-guide.md |
| 물리, 충돌 | trigger, collision | OnTriggerEnter, OnCollisionEnter | 02-content-creation/03-scripting/03-viven-services-and-api/01-unity-lifecycle-callbacks.md |

#### UI 제작 API 매핑 (viven-sdk-ui-creation)

| 사용자 표현 | Viven API·컴포넌트 | Lua·Unity 설정 | 문서 경로 |
|-------------|-------------------|-----------------|-----------|
| World Space UI | Canvas (World Space), VivenGraphicRaycaster | Render Mode: World Space | 02-content-creation/06-ui/02-world-space-ui.md |
| PC/VR 모드별 UI | UIModeChanger | pcUI, xrUI 할당 | 02-content-creation/06-ui/02-world-space-ui.md |
| 버튼·텍스트 갱신 | checkInject | Player, Room, UI, Chat 등 | 02-content-creation/03-scripting/01-viven-lua-behaviour.md |
| HUD (머리 따라다니는 UI) | Player.Mine.CharacterHead | update에서 position/rotation 동기화 | 02-content-creation/06-ui/02-world-space-ui.md |
| SDK UI API | UIAPI | Fader, 마우스 커서, Dock | sdkdoc.viven.app (UIAPI) |

#### 동기화 API 매핑 (viven-sdk-sync-state)

| 사용자 표현 | Viven API·컴포넌트 | Lua 함수·패턴 | 문서 경로 |
|-------------|-------------------|---------------|-----------|
| 일회성 이벤트 전달 | RPC | SyncView:SendRPC, SendTargetRPC | 02-content-creation/03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md |
| 방 단위 상태 | RoomProperty | Room.Property, Room:SetProperty | 02-content-creation/03-scripting/05-networking-and-synchronization/04-room-property.md |
| Transform·Rigidbody 동기화 | TransformView, RigidbodyView | 자동 동기화 | 02-content-creation/03-scripting/05-networking-and-synchronization/03-sync-view.md |
| 커스텀 데이터 동기화 | CustomSyncView | sendSyncUpdate, receiveSyncUpdate | 02-content-creation/03-scripting/05-networking-and-synchronization/03-sync-view.md |
| 네트워크 변수 | NetworkVariable | NetworkVariable.Value | 02-content-creation/03-scripting/05-networking-and-synchronization/02-network-variables.md |

### sdkdoc.viven.app API 참조

| API | 용도 | Skill 연동 |
|-----|------|------------|
| [PlayerAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.PlayerAPI.html) | Move Lock, Rotate Lock, 텔레포트, 캐릭터 컨트롤러 | viven-sdk-player |
| [UIAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.UIAPI.html) | Fader, 마우스 커서, Dock | viven-sdk-ui-creation |
| [ChatAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.ChatAPI.html) | 텍스트·음성 채팅 | viven-sdk-chat |
| [RoomAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.RoomAPI.html) | 방 정보, 입장/퇴장 | viven-sdk-sync-state |
| [SystemAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.SystemAPI.html) | 시스템 제어 | viven-sdk-api |
| [VivenUtilAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.VivenUtilAPI.html) | 유틸리티 | viven-sdk-api |
| [XRAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.XRAPI.html) | VR·XR 디바이스 | viven-sdk-input |

---

## 3. 분류 3: 추상적 요청 (아키텍처·설계)

**설명**: 사용자가 "\~컨텐츠 제작해줘", "총/주사위 만들고 싶어", "미니게임 설계" 등 추상적으로 요청할 때 적용됩니다.

### 트리거 조건

| 트리거 패턴 | 예시 |
|-------------|------|
| "\~만들고 싶어", "\~제작해줘" | "주사위 게임 만들고 싶어", "가위바위보 만들어줘" |
| "설계", "아키텍처" | "4인 협동 미니게임 아키텍처 설계해줘" |
| "다음 단계", "순서", "구현" | "1단계 끝났어. 다음에 뭐 해?" |
| "멀티플레이", "협동", "Host" | "Host 검증, RPC 흐름 포함해서 설계해줘" |

### 분류 3 Skill 목록

| Skill ID | 스킬명 | 트리거 | 설명 |
|----------|--------|--------|------|
| W2 | viven-sdk-content-design | "\~만들고 싶어", "설계" | (1) 범위 파악 질문 (2) 포함/제외·GameObject·구현 순서 설계 (3) 단계별 구현 안내 |
| W4 | viven-sdk-minigame-architecture | "멀티플레이", "협동", "Host" | Host/Client 모델, RPC vs SyncView vs RoomProperty 선택 가이드 |
| W3 | viven-sdk-implementation-roadmap | "다음 단계", "순서", "구현" | RPS 구현 가이드 스타일 단계 분해 |

**매핑 로직**: Agent가 "\~만들고 싶어" 감지 시 → `viven-sdk-content-design`로 설계(범위 파악 → 설계 제시 → 구현 단계 분해) 후 단계별 안내합니다.

---

## 4. 분류 4: 트러블슈팅

**설명**: 사용자가 "~~가 안 돼", "에러", "nil" 등 문제 해결을 요청할 때 적용됩니다.

### 트리거 조건

| 트리거 패턴 | 예시 |
|-------------|------|
| "안 돼", "안 되는데" | "스크립트 실행이 안 돼" |
| "에러", "라고 뜨는데" | "nil 이라고 뜨는데", "함수를 찾을 수 없다고 나오는데" |
| "nil", "찾을 수 없음" | "attempt to call nil", "function not found" |
| "주입", "checkInject", "local" | "checkInject가 안 돼", "local 쓰면 왜 안 돼?" |

### 분류 4 Skill 목록

| Skill ID | 스킬명 | 트리거 | 참조 문서 |
|----------|--------|--------|-----------|
| T1 | viven-sdk-common-errors | "안 돼", "에러", "nil", "찾을 수 없음" | [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md), [함수 및 이벤트](../02-content-creation/03-scripting/08-lua-reference/06-functions-and-events.md) |
| T2 | viven-sdk-injection-troubleshooting | checkInject, local, 주입 | [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) |
| T3 | viven-sdk-lua-syntax | Lua, C# 차이, 콜론, 점, 1-based 인덱스, 테이블 직렬화, 직렬화 | [함수 및 이벤트](../02-content-creation/03-scripting/08-lua-reference/06-functions-and-events.md) |
| T4 | viven-sdk-error-log | VivenLog, 로그, 스택 | [Viven SDK 로그](../02-content-creation/11-advanced/02-emmylua-debugger-connection/02-viven-sdk-log-review-and-analysis.md) |
| T5 | viven-sdk-performance | 성능, FPS, 최적화 | [Lua 성능 최적화](../02-content-creation/11-advanced/01-lua-performance-optimization-guide/00-overview.md) |

### 주요 에러 패턴 → Skill 매핑

| 에러·증상 | 원인 후보 | Skill |
|-----------|-----------|-------|
| nil, attempt to call nil | local 사용으로 주입 실패, checkInject 미호출 | viven-sdk-injection-troubleshooting, viven-sdk-common-errors |
| function not found, 함수를 찾을 수 없음 | RPC 함수명 오타, Lua 함수명 규칙 | viven-sdk-common-errors, viven-sdk-lua-syntax |
| receiveSyncUpdate 호출 안 됨 | SyncView.IsMine, 오너 설정 | viven-sdk-common-errors |
| 콜론 vs 점 혼동 | self 전달 여부 (obj:method vs obj.method) | viven-sdk-lua-syntax |
| 테이블 직렬화 오류 | RPC/SyncView 매개변수 타입 제한 | viven-sdk-common-errors, viven-sdk-rpc |

---

## 5. 전체 스킬 목록

### 5-1. 워크플로우·설계

| ID | 스킬명 | ToC 참조 | 대상 페르소나 | 트리거 키워드 |
|----|--------|----------|---------------|---------------|
| W1 | viven-sdk-beginner-workflow | 01-overview, 02-content-creation 개요 | P1 | "처음", "시작", "뭘 해야 해" |
| W2 | viven-sdk-content-design | 01-project-management (VObject/VMap), 06-architecture | P1, P2, P3 | "\~만들고 싶어", "설계", "아키텍처" |
| W3 | viven-sdk-implementation-roadmap | 03-scripting, 06-ui, 99-test-plan | P1, P2 | "다음 단계", "순서", "구현" |
| W4 | viven-sdk-minigame-architecture | 06-architecture (Host/Client, 동기화, 소유권) | P3 | "멀티플레이", "협동", "Host" |

### 5-2. 프로젝트·빌드

| ID | 스킬명 | ToC 참조 | 대상 페르소나 | 트리거 키워드 |
|----|--------|----------|---------------|---------------|
| P1 | viven-sdk-project-setup | [01-sdk-installation](../01-overview/01-sdk-installation-and-setup.md), [01-viven-content-types](../02-content-creation/01-project-management/01-viven-content-types-vobject-vmap-vavatar.md) | P1, P2 | "설치", "VObject", "VMap", "시작" |
| P1b | viven-sdk-project-config | Addressable, OpenXR | P1, P2 | "Addressable", "OpenXR", "설정 확인" |
| P2 | viven-sdk-build-deploy | [02-build-and-deployment-guide](../02-content-creation/01-project-management/02-build-and-deployment-guide/00-overview.md) | P2, P3 | "빌드", "배포", "업로드" |

### 5-3. 스크립팅 (Viven 특화)

| ID | 스킬명 | ToC 참조 | 대상 페르소나 | 트리거 키워드 |
|----|--------|----------|---------------|---------------|
| S0 | viven-sdk-viven-script | namespace, 이벤트, PLO, DoTween, Instantiate/Destroy, 전역 변수, 비동기 | P1, P2, P3 | namespace, Life Cycle, 충돌 이벤트, Room 이벤트, PLO, DoTween, Instantiate, Destroy |
| S1 | viven-sdk-lua-behaviour | [01-viven-lua-behaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md), [03-unity-lifecycle](../02-content-creation/03-scripting/03-viven-services-and-api/01-unity-lifecycle-callbacks.md) | P1, P2, P3 | VivenLuaBehaviour, start, update, checkInject |
| S2 | viven-sdk-grabbable-module | [04-player-interaction-modules/01-grabbable-module](../02-content-creation/03-scripting/04-player-interaction-modules/01-grabbable-module.md) | P1, P2, P3 | GrabbableModule, onGrab, onRelease, objectShortClickAction, objectLongClickAction, hold/attach |
| S3 | viven-sdk-sittable-module | [04-player-interaction-modules/02-sittable-module](../02-content-creation/03-scripting/04-player-interaction-modules/02-sittable-module.md) | P2, P3 | SittableModule, 앉기 |
| S4 | viven-sdk-rpc | [05-networking/01-remote-procedure-calls](../02-content-creation/03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md) | P2, P3 | RPC, SendRPC, SendTargetRPC |
| S5 | viven-sdk-sync-view | [05-networking/03-sync-view](../02-content-creation/03-scripting/05-networking-and-synchronization/03-sync-view.md) | P3 | SyncView, CustomSyncView, TransformView, RigidbodyView |
| S6 | viven-sdk-network-variables | [05-networking/02-network-variables](../02-content-creation/03-scripting/05-networking-and-synchronization/02-network-variables.md) | P3 | NetworkVariable |
| S7 | viven-sdk-room-property | [05-networking/04-room-property](../02-content-creation/03-scripting/05-networking-and-synchronization/04-room-property.md) | P2, P3 | RoomProperty, 방 상태 |
| S8 | viven-sdk-async | [06-asynchronous-programming](../02-content-creation/03-scripting/06-asynchronous-programming/00-overview.md) | P2, P3 | Coroutine, await, 비동기 |
| S9 | viven-sdk-security | [07-security](../02-content-creation/03-scripting/07-security/00-overview.md) | P3 | Host 검증, 변조 방지 |

### 5-4. 컨텐츠 유형별

| ID | 스킬명 | ToC 참조 | 대상 페르소나 | 트리거 키워드 |
|----|--------|----------|---------------|---------------|
| V1 | viven-sdk-vmap | startPoint, Viven Env, ModuleScript | P2, P3 | "VMap", "맵", "ModuleScript", "startPoint" |
| V2 | viven-sdk-vobject | Grabbable/Sittable, 텔레포트 이동 | P2, P3 | "VObject", "오브젝트 이동", "텔레포트" |
| V3 | viven-sdk-vavatar | Override Animation, 아바타 설정 | P2, P3 | "VAvatar", "Override Animation", "아바타 설정" |

### 5-5. SDK·문서

| ID | 스킬명 | ToC 참조 | 대상 페르소나 | 트리거 키워드 |
|----|--------|----------|---------------|---------------|
| D1 | viven-sdk-api | sdkdoc.viven.app, Player/UI/Chat/Room/System/XR API | P2, P3 | "SDK 문서", "API", "만들 수 있는 것 알려줘" |

### 5-6. 일반 기능 (키워드 기반)

| ID | 스킬명 | ToC 참조 | 대상 페르소나 | 트리거 키워드 |
|----|--------|----------|---------------|---------------|
| F1 | viven-sdk-interaction | [04-player-interaction-modules](../02-content-creation/03-scripting/04-player-interaction-modules/00-overview.md), [03-interaction-event-handling](../02-content-creation/03-scripting/04-player-interaction-modules/03-interaction-event-handling.md) | P1, P2 | 상호작용, 잡기, 클릭, 트리거 |
| F2 | viven-sdk-ui-creation | [06-ui](../02-content-creation/06-ui/00-overview.md) (UGUI, World Space UI, Viven UI) | P1, P2, P3 | UI 제작, 버튼, 텍스트, 화면 표시 |
| F3 | viven-sdk-sync-state | [05-networking](../02-content-creation/03-scripting/05-networking-and-synchronization/00-overview.md) 전체, [06-architecture/02-synchronization](../02-content-creation/01-project-management/06-viven-architecture/02-synchronization-system.md) | P2, P3 | 동기화, 모두에게 보이게, 상태 공유 |

### 5-7. 3D·물리·환경

| ID | 스킬명 | ToC 참조 | 대상 페르소나 | 트리거 키워드 |
|----|--------|----------|---------------|---------------|
| E1 | viven-sdk-physics | [02-unity-physics](../02-content-creation/02-3d-world-building-and-assets/02-unity-physics/00-overview.md) (Rigidbody, Collider, Joint) | P2, P3 | 물리, Rigidbody, Collider, 던지기 |
| E2 | viven-sdk-world-building | [02-3d-world-building](../02-content-creation/02-3d-world-building-and-assets/00-overview.md) (에셋, 환경, 시각 효과) | P2, P3 | 월드, 에셋, 라이팅, Skybox |
| E3 | viven-sdk-spatial | [06-spatial-data](../02-content-creation/02-3d-world-building-and-assets/06-spatial-data-guide/00-overview.md) (충돌, 레이캐스트) | P3 | 충돌, 레이캐스트, Raycast |

### 5-8. 입력·오디오

| ID | 스킬명 | ToC 참조 | 대상 페르소나 | 트리거 키워드 |
|----|--------|----------|---------------|---------------|
| I1 | viven-sdk-input | [04-input](../02-content-creation/04-input/00-overview.md) (Input System, PC/VR, 핸드트래킹) | P2, P3 | 입력, 버튼, PC/VR, 핸드트래킹 |
| I2 | viven-sdk-audio | [05-audio](../02-content-creation/05-audio/00-overview.md) (VivenAudioEventInstance, FMOD) | P2, P3 | 오디오, 소리, 이펙트 |

### 5-9. 캐릭터·플레이어·채팅

| ID | 스킬명 | ToC 참조 | 대상 페르소나 | 트리거 키워드 |
|----|--------|----------|---------------|---------------|
| C1 | viven-sdk-avatar | [08-characters](../02-content-creation/08-characters/00-overview.md) (아바타, 감정, 애니메이션) | P2, P3 | 아바타, 감정, 이모트 |
| C2 | viven-sdk-player | [09-players](../02-content-creation/09-players/00-overview.md) (데이터, 텔레포트, 월드 이동) | P2, P3 | 플레이어, 텔레포트, 월드 이동 |
| C3 | viven-sdk-chat | [10-chat](../02-content-creation/10-chat/00-overview.md) (텍스트, 음성) | P3 | 채팅, 메시지, 음성 |

### 5-10. 트러블슈팅

| ID | 스킬명 | ToC 참조 | 대상 페르소나 | 트리거 키워드 |
|----|--------|----------|---------------|---------------|
| T1 | viven-sdk-common-errors | 03-scripting, [08-lua-reference](../02-content-creation/03-scripting/08-lua-reference/00-overview.md) | P1, P2, P3 | "안 돼", "에러", "nil", "찾을 수 없음" |
| T2 | viven-sdk-injection-troubleshooting | [01-viven-lua-behaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) | P1, P2 | checkInject, local, 주입 |
| T3 | viven-sdk-lua-syntax | [08-lua-reference](../02-content-creation/03-scripting/08-lua-reference/00-overview.md) (함수, 변수, 연산자) | P2, P3 | Lua, C# 차이, 콜론, 점 |
| T4 | viven-sdk-error-log | [11-advanced/02-emmylua](../02-content-creation/11-advanced/02-emmylua-debugger-connection/02-viven-sdk-log-review-and-analysis.md) (Viven SDK 로그) | P3 | VivenLog, 로그, 스택 |
| T5 | viven-sdk-performance | [11-advanced/01-lua-performance](../02-content-creation/11-advanced/01-lua-performance-optimization-guide/00-overview.md) | P3 | 성능, FPS, 최적화 |

---

## 6. 페르소나별 필요 스킬 매트릭스

| 스킬 | P1 (비개발자) | P2 (개발자) | P3 (제작자) |
|------|:------------:|:-----------:|:-----------:|
| W1 beginner-workflow | 필수 | - | - |
| W2 content-design | 필수 | 권장 | 권장 |
| W3 implementation-roadmap | 필수 | 권장 | - |
| W4 minigame-architecture | - | - | 필수 |
| P1 project-setup | 필수 | 필수 | - |
| P1b project-config | 권장 | 권장 | 필수 |
| P2 build-deploy | - | 권장 | 필수 |
| S0 viven-script | 필수 | 필수 | 필수 |
| V1 vmap | - | 권장 | 필수 |
| V2 vobject | 권장 | 필수 | 필수 |
| V3 vavatar | - | 권장 | 필수 |
| D1 sdk-api | - | 권장 | 필수 |
| S1 lua-behaviour | 필수 | 필수 | 필수 |
| S2 grabbable-module | 권장 | 필수 | 필수 |
| S3 sittable-module | - | 권장 | 필수 |
| S4 rpc | - | 필수 | 필수 |
| S5 sync-view | - | 권장 | 필수 |
| S6 network-variables | - | - | 권장 |
| S7 room-property | - | 권장 | 필수 |
| S8 async | - | 권장 | 필수 |
| S9 security | - | - | 필수 |
| F1 interaction | 필수 | 필수 | 필수 |
| F2 ui-creation | 필수 | 필수 | 필수 |
| F3 sync-state | 권장 | 필수 | 필수 |
| E1 physics | - | 권장 | 필수 |
| E2 world-building | - | 권장 | 필수 |
| E3 spatial | - | - | 권장 |
| I1 input | 권장 | 필수 | 필수 |
| I2 audio | 권장 | 필수 | 필수 |
| C1 avatar | - | 권장 | 필수 |
| C2 player | - | 권장 | 필수 |
| C3 chat | - | - | 권장 |
| T1 common-errors | 필수 | 필수 | 필수 |
| T2 injection-troubleshooting | 필수 | 필수 | 권장 |
| T3 lua-syntax | 권장 | 필수 | 필수 |
| T4 error-log | - | 권장 | 필수 |
| T5 performance | - | - | 필수 |

---

## 7. ToC 섹션 → 스킬 매핑

| ToC 섹션 | 해당 스킬 |
|----------|-----------|
| 1. 개요 (시작, SDK 설치) | W1, P1 |
| 2. 프로젝트 관리 (VObject/VMap, 빌드, 아키텍처) | W2, W4, P1, P2 |
| 3. 3D 월드 구성 (에셋, 물리, 환경, 공간) | E1, E2, E3 |
| 4. 스크립팅 (Lua, 상호작용, 네트워크, 보안) | S0, S1\~S9, F1, F3 |
| 5. 입력 | I1 |
| 6. 오디오 | I2 |
| 7. UI | F2 |
| 8. 환경 | E2 |
| 9. 캐릭터 | C1 |
| 10. 플레이어 | C2 |
| 11. 채팅 | C3 |
| 12. 고급 (성능, 디버거, Linter) | T4, T5 |

---

## 8. 컨텐츠 유형별 Skill 보강

구체적 기능 분류를 반영한 스킬 매핑:

| 분류 | 세부 항목 | 스킬 매핑 |
|--------------|-----------|------------|
| **프로젝트 설정 확인** | Addressable, OpenXR | P1b viven-sdk-project-config |
| **VivenScript 공통 가이드** | namespace, Unity/Viven 이벤트, GameObject, Instantiate/Destroy(V-Object 제외), hold/attach 이벤트, SyncView/RPC, Life Cycle, PLO, DoTween, 전역 변수, 비동기 | S0 viven-sdk-viven-script |
| **VMAP 제작 시** | startPoint, Viven Env, 카메라/EventSystem, ModuleScript | V1 viven-sdk-vmap |
| **VOBJECT 제작 시** | GrabbableModule, SittableModule, 오브젝트 이동(텔레포트) | V2 viven-sdk-vobject |
| **VAvatar 제작 시** | Override Animation, 얼굴·키 아바타 설정 | V3 viven-sdk-vavatar |
| **SDK 기능·문서 확인** | Player/UI/Chat/Room/System/VivenUtil/XR API, DoTween, Webview | D1 viven-sdk-api |

---

## 9. 우선 구현 스킬 (필수 15개)

페르소나 커버리지와 사용 빈도를 기준으로 한 1차 구현 대상.

1. **W1** viven-sdk-beginner-workflow
2. **W2** viven-sdk-content-design
3. **P1** viven-sdk-project-setup
4. **S1** viven-sdk-lua-behaviour
5. **S2** viven-sdk-grabbable-module
6. **S4** viven-sdk-rpc
7. **F1** viven-sdk-interaction
8. **F2** viven-sdk-ui-creation
9. **F3** viven-sdk-sync-state
10. **T1** viven-sdk-common-errors
11. **T2** viven-sdk-injection-troubleshooting
12. **T3** viven-sdk-lua-syntax
13. **W3** viven-sdk-implementation-roadmap
14. **S5** viven-sdk-sync-view
15. **S7** viven-sdk-room-property
