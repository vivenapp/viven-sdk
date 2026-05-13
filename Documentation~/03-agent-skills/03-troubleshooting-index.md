# 트러블슈팅 인덱스

> 관련 문서: [Skill 분류](02-skill-taxonomy.md) | [페르소나·시나리오](01-personas-and-scenarios.md) | [산출물 정리](07-output-summary.md)

본 문서는 Viven 콘텐츠 제작 시 **자주 발생하는 에러·증상**을 정리한 인덱스입니다. 에러 메시지나 증상으로 원인을 빠르게 찾고, 해당 Skill 또는 문서로 연결할 수 있습니다.

---

## 1. 개요

### 사용 방법

1. **에러 메시지** 또는 **증상**을 아래 표에서 검색
2. 해당 행의 **연결 Skill** 또는 **참조 문서**를 확인
3. Agent에 프롬프트할 때: "~~가 안 돼", "~~라고 뜨는데", "에러", "nil", "찾을 수 없음" 등으로 요청하면 [Skill 분류 4 (트러블슈팅)](02-skill-taxonomy.md) Skill이 선택됩니다

### Skill 분류 4 (트러블슈팅) 개요

| Skill ID | Skill명 | 주요 대응 |
|----------|---------|-----------|
| T1 | viven-sdk-common-errors | nil, RPC 함수명 오타, SyncView receiveSyncUpdate 미호출 |
| T2 | viven-sdk-injection-troubleshooting | checkInject, local 금지, 주입 실패 |
| T3 | viven-sdk-lua-syntax | C# vs Lua 차이, 콜론/점, 1-based 인덱스 |
| T4 | viven-sdk-error-log | VivenLog 패턴, 스택 트레이스 해석 |
| T5 | viven-sdk-performance | FPS 저하, Lua 성능 최적화 |

---

## 2. 에러·증상 인덱스

### 2-1. Lua 런타임 에러

| 에러 메시지 / 증상 | 원인 후보 | 연결 Skill | 참조 문서 |
|--------------------|-----------|------------|-----------|
| `attempt to call nil value` | 함수가 nil (정의 누락, 오타, local로 선언된 주입 변수) | T1, T2 | [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) |
| `attempt to index nil` | nil 객체에 필드 접근 (주입 실패, 초기화 전 접근) | T1, T2 | [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) |
| `attempt to call a nil value (global 'xxx')` | 전역 함수/변수 미정의 또는 오타 | T1, T3 | [함수 및 이벤트](../02-content-creation/03-scripting/08-lua-reference/06-functions-and-events.md) |
| `bad argument #1 to 'xxx' (expected xxx, got nil)` | 인자로 nil 전달 (주입 실패, 조건부 초기화 누락) | T1, T2 | [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) |
| `'=' expected near 'xxx'` | Lua 문법 오류 (괄호, 키워드 누락) | T3 | [Lua 레퍼런스](../02-content-creation/03-scripting/08-lua-reference/00-overview.md) |
| `unexpected symbol near 'xxx'` | Lua 문법 오류 | T3 | [Lua 레퍼런스](../02-content-creation/03-scripting/08-lua-reference/00-overview.md) |

---

### 2-2. 주입(Injection) 관련

| 에러 메시지 / 증상 | 원인 후보 | 연결 Skill | 참조 문서 |
|--------------------|-----------|------------|-----------|
| Inspector에 변수 필드가 안 보임 | `checkInject` 미사용 또는 `local` 사용 | T2 | [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) |
| 주입한 변수가 항상 nil | `local`로 선언하여 주입 불가 | T2 | [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) |
| checkInject 경고 로그 출력 | Inspector에서 해당 필드 미할당 | T2 | [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) |
| 지원하지 않는 타입 주입 시도 | GameObject, Vector3, float 등 지원 타입 외 사용 | T2 | [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) |

**지원 타입**: `GameObject`, `UnityEngine.Object`, `Vector2`, `Vector3`, `float`, `int`, `bool`, `string`, `Color`, `VivenScript`

---

### 2-3. RPC 관련

| 에러 메시지 / 증상 | 원인 후보 | 연결 Skill | 참조 문서 |
|--------------------|-----------|------------|-----------|
| RPC 함수를 찾을 수 없음 / function not found | RPC 함수명 오타, Host에만 정의, `[RPC]` 속성 누락 | T1 | [RPC](../02-content-creation/03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md) |
| RPC 호출 시 직렬화 에러 | Lua 테이블 구조가 직렬화 불가 (함수, userdata 등) | T1, T3 | [RPC](../02-content-creation/03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md) |
| RPC가 호출되지 않음 | Host/Client 구분 미확인, 대상 지정 오류 | T1 | [RPC](../02-content-creation/03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md) |

---

### 2-4. SyncView / 동기화 관련

| 에러 메시지 / 증상 | 원인 후보 | 연결 Skill | 참조 문서 |
|--------------------|-----------|------------|-----------|
| SyncView가 다른 클라이언트에 반영 안 됨 | 오너(Owner)에서 `receiveSyncUpdate` 미호출 | T1 | [SyncView](../02-content-creation/03-scripting/05-networking-and-synchronization/03-sync-view.md) |
| CustomSyncView 직렬화 에러 | `sendSyncUpdate`/`receiveSyncUpdate` 직렬화 형식 불일치 | T1 | [SyncView](../02-content-creation/03-scripting/05-networking-and-synchronization/03-sync-view.md) |
| 오브젝트가 한쪽에서만 움직임 | Network Ownership, Host/Client 모델 미이해 | T1 | [네트워크 소유권](../02-content-creation/01-project-management/06-viven-architecture/03-network-ownership.md) |

---

### 2-5. C# vs Lua 문법 차이

| 에러 메시지 / 증상 | 원인 후보 | 연결 Skill | 참조 문서 |
|--------------------|-----------|------------|-----------|
| `obj.Method()` 호출 시 에러 (self 누락) | C# 인스턴스 메서드는 콜론(`:`) 사용 필요 | T3 | [함수 및 이벤트](../02-content-creation/03-scripting/08-lua-reference/06-functions-and-events.md) |
| 인덱스가 1씩 어긋남 | Lua는 1-based 인덱스 사용 | T3 | [데이터 구조](../02-content-creation/03-scripting/08-lua-reference/02-data-structures.md) |
| 테이블을 RPC로 보냈는데 깨짐 | 직렬화 불가 필드 포함 (함수, userdata) | T1, T3 | [RPC](../02-content-creation/03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md) |

---

### 2-6. 로그·디버깅

| 에러 메시지 / 증상 | 원인 후보 | 연결 Skill | 참조 문서 |
|--------------------|-----------|------------|-----------|
| VivenLog 메시지 해석이 안 됨 | 로그 패턴, 스택 트레이스 읽는 법 미숙지 | T4 | [Viven SDK 로그 확인 및 분석](../02-content-creation/11-advanced/02-emmylua-debugger-connection/02-viven-sdk-log-review-and-analysis.md) |
| 스택 트레이스에서 파일/라인 찾기 어려움 | Lua 파일 경로, 에러 위치 매핑 | T4 | [EmmyLua 디버거 연결](../02-content-creation/11-advanced/02-emmylua-debugger-connection/00-overview.md) |

---

### 2-7. 성능

| 에러 메시지 / 증상 | 원인 후보 | 연결 Skill | 참조 문서 |
|--------------------|-----------|------------|-----------|
| FPS 저하, 버벅임 | update에서 과도한 연산, LINQ/할당 | T5 | [Lua 성능 최적화](../02-content-creation/11-advanced/01-lua-performance-optimization-guide/00-overview.md) |
| 메모리 사용량 증가 | 이벤트 리스너 미해제, 객체 풀링 미사용 | T5 | [Lua 성능 최적화](../02-content-creation/11-advanced/01-lua-performance-optimization-guide/00-overview.md) |

---

## 3. 시나리오별 트러블슈팅 매핑

[페르소나·시나리오](01-personas-and-scenarios.md)에서 트러블슈팅과 연결된 시나리오입니다.

| 시나리오 ID | 상황 | 연결 Skill |
|-------------|------|------------|
| S1-4 | nil 에러 발생, 원인 모름 | T1, T2 |
| S1-14 | "attempt to call nil" 에러 | T1, T2, T3 |
| S2-5 | RPC 호출 시 함수를 찾을 수 없음 | T1 |
| S2-10 | VObject 빌드 시 에러 | T4, [빌드 가이드](../02-content-creation/01-project-management/02-build-and-deployment-guide/00-overview.md) |
| S3-5 | SyncView 트러블슈팅 | T1 |

---

## 4. 프롬프트 예시 (Agent Skill 트리거)

다음과 같은 표현으로 요청하면 트러블슈팅 Skill이 선택됩니다.

| 사용자 표현 | 연결 Skill |
|-------------|------------|
| "~~가 안 돼" | T1 |
| "~~라고 뜨는데" | T1, T4 |
| "에러", "에러가 났어" | T1 |
| "nil", "nil이에요" | T1, T2 |
| "찾을 수 없음", "function not found" | T1 |
| "checkInject", "주입", "local" | T2 |
| "Lua", "C# 차이", "콜론", "점" | T3 |
| "VivenLog", "로그", "스택" | T4 |
| "성능", "FPS", "최적화" | T5 |

---

## 5. 참조 문서 요약

| 주제 | 문서 경로 |
|------|-----------|
| VivenLuaBehaviour, 주입, local 금지 | [01-viven-lua-behaviour.md](../02-content-creation/03-scripting/01-viven-lua-behaviour.md) |
| RPC | [01-remote-procedure-calls.md](../02-content-creation/03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md) |
| SyncView | [03-sync-view.md](../02-content-creation/03-scripting/05-networking-and-synchronization/03-sync-view.md) |
| 함수 및 이벤트 (콜론/점) | [06-functions-and-events.md](../02-content-creation/03-scripting/08-lua-reference/06-functions-and-events.md) |
| Viven SDK 로그 | [02-viven-sdk-log-review-and-analysis.md](../02-content-creation/11-advanced/02-emmylua-debugger-connection/02-viven-sdk-log-review-and-analysis.md) |
| Lua 성능 최적화 | [01-lua-performance-optimization-guide](../02-content-creation/11-advanced/01-lua-performance-optimization-guide/00-overview.md) |
