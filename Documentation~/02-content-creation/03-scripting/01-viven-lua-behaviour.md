# VivenScript와 LuaBehaviour 사용하기

## 개요

VivenScript는 Lua 기반 스크립트로, Viven 콘텐츠의 게임 로직을 구현하는 핵심 수단입니다. `VivenLuaBehaviour` 컴포넌트에 VivenScript를 연결하면, Unity의 이벤트(Start, Update 등)와 Viven 전용 이벤트(방 입장, 오브젝트 잡기 등)를 Lua로 처리할 수 있습니다. Unity 인스펙터에서 변수를 주입하여 Lua 스크립트에서 바로 사용할 수 있습니다.

## 언제 사용하나요?

- 오브젝트에 상호작용 로직을 넣고 싶을 때 (버튼 클릭, 트리거 진입, 잡기/놓기 등)
- 방 입장·퇴장, 플레이어 입장·퇴장 같은 네트워크 이벤트에 반응하고 싶을 때
- Lua로 간단히 로직을 작성하고 싶을 때 (C# 컴파일 없이 수정 가능)
- 여러 오브젝트 간 데이터를 공유하고 싶을 때 (`Global` 테이블 사용)

## 준비사항

- Viven SDK가 포함된 Unity 프로젝트
- Viven 맵 환경이 있는 씬 (MapEnvironmentManager가 초기화된 상태)
- `.lua` 파일 또는 VivenScript 에셋

## 진행 순서

### 1. VivenScript 에셋 만들기

`.lua` 파일을 프로젝트에 추가하면 자동으로 VivenScript 에셋으로 임포트됩니다.

1. Project 창에서 우클릭 → **Create** → **VivenScriptable** 선택 (빈 `.lua` 파일 생성)
2. 또는 기존 `.lua` 파일을 프로젝트 폴더에 복사
3. Lua 코드 작성 또는 수정

### 2. VivenLuaBehaviour 컴포넌트 붙이기

1. Lua 로직을 적용할 GameObject 선택
2. **Add Component** → `VivenLuaBehaviour` 검색 후 추가
3. Inspector에서 **Lua Script** 필드에 위에서 만든 VivenScript 에셋을 드래그

### 3. 변수 주입(Injection) 설정

Lua 스크립트에서 `---@type` 주석과 `checkInject(변수명)` 형태로 주입할 변수를 선언하면, Inspector에 해당 필드가 표시됩니다. `checkInject`는 VivenScript에 기본으로 주입되는 함수로, 주입된 객체가 `nil`일 경우 경고 로그를 출력합니다.

> **중요**: 주입받을 변수를 선언할 때 **절대로 `local` 키워드를 사용하지 마세요.** `local`로 선언하면 외부(Unity Inspector)에서 값을 주입할 수 없으며, 해당 변수는 항상 `nil`이 됩니다.

```lua
---@type GameObject
box = checkInject(box) -- 올바름: 전역 심볼로 선언하여 주입 허용

---@type float
local speed = checkInject(speed) -- 잘못됨: local 선언 시 주입되지 않음
```

지원 타입: `GameObject`, `UnityEngine.Object`, `Vector2`, `Vector3`, `float`, `int`, `bool`, `string`, `Color`, `VivenScript`

### 인스펙터에서 InjectedField 편집하기

Lua 스크립트에서 `checkInject()` 선언을 추가하거나 수정한 후, Unity Inspector에 새 필드가 즉시 반영되지 않을 수 있습니다. 이 경우 다음 순서로 진행하세요:

1. Lua 스크립트(`.lua` 파일)를 저장합니다.
2. Inspector에서 VivenLuaBehaviour 컴포넌트의 **Refresh** 버튼을 클릭합니다.
3. Injection 목록이 갱신되면, 각 InjectedField에 원하는 오브젝트·값을 할당합니다.

> [!TIP]
> **Refresh**는 Lua 스크립트를 다시 파싱하여 `checkInject()` 선언을 Inspector에 반영합니다. 스크립트를 수정할 때마다 Refresh를 수행하는 습관을 들이면 주입 필드 누락을 방지할 수 있습니다.

> [!WARNING]
> Refresh 없이 Inspector에서 필드를 편집하면, 스크립트에서 새로 추가한 `checkInject()` 변수가 표시되지 않거나 기존 필드와 매핑이 어긋날 수 있습니다.

### 4. Lua 스크립트 작성

VivenScript에 Lua 코드를 작성합니다. Unity 이벤트 함수는 **소문자**로 정의합니다.

```lua
---@type GameObject
targetObject = checkInject(targetObject)

function start()
    Debug.Log("스크립트 시작!")
    if targetObject then
        Debug.Log("대상 오브젝트: " .. targetObject.name)
    end
end

function update()
    -- 매 프레임 실행 (필요할 때만 정의)
end

function onDestroy()
    Debug.Log("스크립트 종료")
end
```

## 기본 제공 변수와 함수

Lua 스크립트에서 별도 선언 없이 사용할 수 있는 항목입니다.

| 이름 | 설명 |
|------|------|
| `self` | 현재 VivenLuaBehaviour 인스턴스 (C# MonoBehaviour 객체). **VivenScript가 아님에 주의** |
| `__script` | 현재 VivenScript 인스턴스. Global에 스크립트를 등록할 때는 `self` 대신 `__script`를 사용해야 합니다 |
| `behaviour` | `self`와 동일 |
| `gameObject` | 이 스크립트가 붙은 GameObject |
| `transform` | 이 스크립트가 붙은 Transform |
| `Global` | 현재 방 전체에서 공유하는 Lua 테이블 |
| `ImportLuaScript(script)` | 다른 VivenScript 파일을 모듈처럼 가져오기 |
| `ImportLuaString(code)` | Lua 코드 문자열을 실행하고 결과 반환 |
| `startCoroutine(routine)` | 코루틴 시작 |
| `stopCoroutine(coroutine)` | 코루틴 중지 |
| `checkInject(obj)` | 주입 변수 선언 시 사용. Inspector에 필드 표시되며, nil 주입 시 경고 로그 출력 |

### Global 테이블 사용

`Global`은 같은 방에 있는 모든 VivenLuaBehaviour 스크립트가 공유하는 테이블입니다.

```lua
function start()
    Global.score = 0
    Global.playerCount = 0
end

function onRoomUserJoined(userData)
    Global.playerCount = Global.playerCount + 1
    Debug.Log("현재 인원: " .. Global.playerCount)
end
```

> **주의**: Global에 스크립트 자신을 등록할 때는 `self`가 아닌 `__script`를 사용하세요. `self`는 VivenLuaBehaviour(C# MonoBehaviour)를 참조하고, `__script`는 VivenScript 인스턴스를 참조합니다.
> ```lua
> -- 올바른 사용
> Global.MyManager = __script
>
> -- 잘못된 사용 (VivenScript가 아닌 VivenLuaBehaviour가 등록됨)
> Global.MyManager = self
> ```

## Unity 라이프사이클 콜백

다음 함수를 정의하면 해당 Unity 이벤트 시점에 호출됩니다.

| Lua 함수 | 호출 시점 |
|----------|-----------|
| `awake()` | 컴포넌트가 로드될 때 (가장 먼저) |
| `start()` | 첫 프레임 전 |
| `update()` | 매 프레임 |
| `fixedUpdate()` | 물리 업데이트마다 |
| `onEnable()` | 컴포넌트 활성화 시 |
| `onDisable()` | 컴포넌트 비활성화 시 |
| `onDestroy()` | 오브젝트 파괴 직전 |
| `onApplicationFocus(focus)` | 포커스 획득/상실 시 |
| `onApplicationPause(pause)` | 일시정지/재개 시 |
| `onApplicationQuit()` | 앱 종료 시 |

## 충돌·트리거 콜백

| Lua 함수 | 호출 시점 |
|----------|-----------|
| `onCollisionEnter(collision)` | 3D 충돌 시작 |
| `onCollisionStay(collision)` | 3D 충돌 유지 |
| `onCollisionExit(collision)` | 3D 충돌 종료 |
| `onTriggerEnter(collider)` | 트리거 진입 |
| `onTriggerStay(collider)` | 트리거 내부 유지 |
| `onTriggerExit(collider)` | 트리거 이탈 |
| `onPlayerEnter(userID)` | 플레이어가 트리거에 진입 (Tag "Player") |
| `onPlayerStay(userID)` | 플레이어가 트리거 내부에 있음 |
| `onPlayerExit(userID)` | 플레이어가 트리거에서 나감 |

## DTS(방·맵) 콜백

| Lua 함수 | 호출 시점 |
|----------|-----------|
| `onRoomJoined(roomData)` | 방 입장 시 |
| `onRoomUserJoined(userData)` | 다른 사용자 입장 시 |
| `onUserLeaveRoom(userData)` | 다른 사용자 퇴장 시 |
| `onRoomLeave()` | 자신이 방을 나가기 직전 |
| `onMapChanged(mapName)` | 맵 변경 시 |
| `onRoomMbrAdded(roomAuthMbrs)` | 방 멤버 추가 시 |
| `onRoomMbrUpdated(roomAuthMbrs)` | 방 멤버 정보 갱신 시 |
| `onRoomMbrRemoved(userId)` | 방 멤버 제거 시 |
| `onRoomPropChanged(propId, propVal)` | 방 속성 변경 시 |
| `onMapPropChanged(mapId, propId, propVal)` | 맵 속성 변경 시 |

## Viven 컴포넌트 이벤트

같은 GameObject에 `VivenGrabbableModule` 또는 `VivenAttachPoint`가 있을 때 사용할 수 있습니다.

### GrabbableModule (잡기 가능 오브젝트)

| Lua 함수 | 호출 시점 |
|----------|-----------|
| `onGrab()` | 오브젝트를 잡았을 때 |
| `onRelease()` | 오브젝트를 놓았을 때 |
| `objectShortClickAction()` | 짧게 클릭 (Button 1) |
| `objectLongClickAction()` | 길게 클릭 (Button 1) |
| `objectHoldActionStart()` | 누르기 시작 (Button 1) |
| `objectHoldActionEnd()` | 누르기 종료 (Button 1) |
| `ShortClickAction1()` ~ `HoldActionEnd3()` | 버튼 1~3별 짧은/긴 클릭, 홀드 |

### AttachPoint (부착점)

| Lua 함수 | 호출 시점 |
|----------|-----------|
| `onAttach()` | 오브젝트가 부착점에 부착됐을 때 |
| `onDetach()` | 오브젝트가 부착점에서 분리됐을 때 |

## 외부 스크립트 가져오기

`require`를 이용한 코드 재사용과 모듈화는 [코드 재사용과 모듈화](02-code-reuse-and-modularity.md)를 참조하세요.

## 다른 Lua 스크립트 참조하기

같은 GameObject나 자식/부모에 있는 다른 VivenLuaBehaviour의 Lua 환경(LuaTable)을 가져올 수 있습니다. `GetLuaComponent`는 VivenScript 에셋의 **이름**을 인자로 받습니다.

> **유의**: 가져온 다른 Lua 스크립트의 함수를 호출할 때는 반드시 `.`(점)을 사용하세요. `:`(콜론)을 사용하면 `self`가 잘못 전달되어 오동작할 수 있습니다.

```lua
function start()
    -- 같은 GameObject의 "OtherScript"라는 이름의 VivenScript 환경 가져오기
    local otherEnv = gameObject:GetLuaComponent("OtherScript")
    if otherEnv and otherEnv.someFunction then
        otherEnv.someFunction()  -- 올바름: . 사용
        -- otherEnv:someFunction()  -- 잘못됨: : 사용 시 오동작
    end
end
```

- `gameObject:GetLuaComponent(scriptName)` — 같은 GameObject
- `gameObject:GetLuaComponentInChildren(scriptName)` — 자식 중 첫 번째
- `gameObject:GetLuaComponentsInChildren(scriptName)` — 자식 전부 (배열)
- `gameObject:GetLuaComponentInParent(scriptName)` — 부모 중 첫 번째

## 유틸리티 메서드

`self`(또는 `behaviour`)를 통해 다음 메서드를 호출할 수 있습니다.

### 코루틴 (Coroutine)

Lua에서 비동기 처리를 위해 코루틴을 시작하거나 중지할 수 있습니다. 자세한 사용법은 [Unity 코루틴 사용하기](./06-asynchronous-programming/01-unity-coroutines.md)를 참조하세요.

- `startCoroutine(routine)`: 코루틴 시작
- `stopCoroutine(coroutine)`: 코루틴 중지

### Global 변수 공유

`Global` 테이블을 사용하여 같은 방에 있는 모든 `VivenLuaBehaviour` 간에 데이터를 공유할 수 있습니다. 변수 범위와 `Global` 사용에 대한 상세 내용은 [변수와 스코프](./08-lua-reference/04-variables-and-scope.md)를 참조하세요.

```lua
function start()
    Global.score = 0 -- 모든 스크립트에서 접근 가능
end
```

## 확인 방법

1. **Play** 모드로 씬 실행
2. Console에 `Debug.Log` 출력이 나오는지 확인
3. Injection으로 지정한 오브젝트가 Lua에서 `nil`이 아닌지 확인 (`checkInject` 활용)
4. 방 입장 후 `onRoomJoined` 등 DTS 콜백이 호출되는지 확인

## 자주 일어나는 실수

- **Lua Script가 비어 있음**: VivenLuaBehaviour에 VivenScript를 할당하지 않으면 `Lua Script가 없습니다` 에러가 나고 컴포넌트가 비활성화됩니다.
- **MapEnvironmentManager 미초기화**: 맵 환경이 준비되기 전에 씬을 로드하면 Lua 스크립트가 실행되지 않습니다. Viven 맵 씬에서 테스트하세요.
- **함수 이름 대소문자**: Unity 콜백은 `start`, `update`처럼 **소문자**로 정의해야 합니다. `Start`, `Update`는 호출되지 않습니다.
- **Injection 선언 누락**: `---@type`과 `변수 = checkInject(변수)`로 선언하지 않으면 Inspector에 필드가 표시되지 않습니다. Lua에서 사용할 변수명과 선언 시 변수명이 일치해야 합니다.
- **다른 스크립트 함수 호출 시 `:` 사용**: `GetLuaComponent`로 가져온 Lua 환경의 함수를 호출할 때 `:`(콜론)을 쓰면 `self`가 잘못 전달됩니다. 반드시 `.`(점)을 사용하세요.
- **Global 남용**: `Global`에 큰 테이블이나 자주 바뀌는 데이터를 넣으면 모든 스크립트에 영향을 주므로, 필요한 경우에만 사용하세요.
- **레이어/태그 임의 변경 금지**: 게임오브젝트의 Layer와 Tag를 사용자 지정 값으로 설정하지 마세요. Viven은 `Default`, `Grabbable`, `Player`, `NotRender` 등 시스템 레이어를 런타임에 자동 관리하며, 임의 변경 시 잡기(Grab), 1인칭 Culling, 레이캐스트 등 핵심 기능이 오작동합니다. 스크립트에서 `gameObject.layer`나 `gameObject.tag`를 직접 변경하거나 `CompareTag()`로 사용자 지정 태그를 비교하지 마세요. 플레이어 감지는 `onPlayerEnter(userID)` 이벤트를 사용하세요.

## 관련 문서

- [코드 재사용과 모듈화](02-code-reuse-and-modularity.md) — require로 Lua 모듈 공유
- [Grabbable 모듈](../04-player-interaction-modules/01-grabbable-module.md) — 잡기 가능 오브젝트 설정
- [Sittable 모듈](../04-player-interaction-modules/02-sittable-module.md) — 앉기 가능 오브젝트
