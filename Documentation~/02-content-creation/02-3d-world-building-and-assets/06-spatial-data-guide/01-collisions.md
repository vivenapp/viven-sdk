# Collisions

## 개요

Unity의 **Collision** 이벤트와 **Trigger** 이벤트를 사용해, 오브젝트가 서로 닿거나 통과할 때 Lua 스크립트로 반응할 수 있습니다. `VivenLuaBehaviour`에서 콜백 함수를 정의하면 충돌·트리거 시점에 맞춰 로직을 실행할 수 있습니다.

## 언제 사용하나요?

- 플레이어나 물체가 특정 영역에 들어왔을 때 이벤트를 발생시키고 싶을 때
- 오브젝트끼리 부딪힐 때 점수·효과·사운드를 트리거하고 싶을 때
- 통과 가능한 영역(트리거)과 물리적으로 부딪히는 영역(콜라이더)을 구분해 사용하고 싶을 때

## 준비사항

- `VivenLuaBehaviour`가 붙은 GameObject에 **Collider**가 있어야 합니다.
- **Collision** 이벤트를 받으려면 양쪽 모두 **Rigidbody**가 있어야 하며, Collider의 **Is Trigger**가 꺼져 있어야 합니다.
- **Trigger** 이벤트를 받으려면 한쪽에 Rigidbody가 있고, Collider의 **Is Trigger**가 켜져 있어야 합니다.

## 진행 순서

### 1. Collider 설정

1. `VivenLuaBehaviour`가 붙은 GameObject를 선택합니다.
2. **Add Component** → **Box Collider**, **Sphere Collider**, **Capsule Collider** 등 원하는 Collider를 추가합니다.
3. **Collision** 이벤트를 사용할 경우: **Is Trigger**를 끕니다.
4. **Trigger** 이벤트를 사용할 경우: **Is Trigger**를 켭니다.

### 2. Lua 스크립트에 콜백 정의

`VivenLuaBehaviour`에 연결된 Lua 스크립트에서 아래 콜백 함수를 정의합니다. 정의된 함수만 호출됩니다.

**Collision 이벤트 (물리 충돌, Is Trigger = false):**

| 콜백 | 설명 |
|------|------|
| `onCollisionEnter(collision)` | 처음 충돌했을 때 |
| `onCollisionStay(collision)` | 충돌 중 매 프레임 |
| `onCollisionExit(collision)` | 충돌이 끝났을 때 |
| `onCollisionEnter2D(collision)` | 2D 충돌 시작 |
| `onCollisionStay2D(collision)` | 2D 충돌 유지 |
| `onCollisionExit2D(collision)` | 2D 충돌 종료 |

**Trigger 이벤트 (통과 감지, Is Trigger = true):**

| 콜백 | 설명 |
|------|------|
| `onTriggerEnter(collider)` | 트리거 영역에 들어왔을 때 |
| `onTriggerStay(collider)` | 트리거 영역 안에 있을 때 매 프레임 |
| `onTriggerExit(collider)` | 트리거 영역을 벗어났을 때 |
| `onTriggerEnter2D(collider)` | 2D 트리거 진입 |
| `onTriggerStay2D(collider)` | 2D 트리거 유지 |
| `onTriggerExit2D(collider)` | 2D 트리거 이탈 |

### 3. 충돌한 오브젝트에서 다른 VivenScript 찾기

충돌·트리거로 전달된 `collision.gameObject` 또는 `collider.gameObject`에서 다른 Viven Lua 스크립트를 찾으려면 `GetLuaComponent`를 사용합니다. 자세한 내용은 [Viven Lua Behaviour](../../03-scripting/01-viven-lua-behaviour.md)를 참조하세요.

```lua
function onCollisionEnter(collision)
    local otherGameObject = collision.gameObject
    -- 다른 VivenScript(예: "MyOtherScript")의 Lua 테이블 가져오기
    local otherScript = otherGameObject:GetLuaComponent("MyOtherScript")
    if otherScript and otherScript.OnHit then
        otherScript:OnHit()
    end
end

function onTriggerEnter(collider)
    local otherGameObject = collider.gameObject
    local otherScript = otherGameObject:GetLuaComponent("MyOtherScript")
    if otherScript then
        -- 트리거 진입 시 처리
    end
end
```

## 확인 방법

- Play 모드에서 오브젝트를 충돌·트리거시키고, `Debug.Log` 등으로 콜백이 호출되는지 확인합니다.
- Collision 이벤트가 발생하지 않으면 두 오브젝트 모두 Rigidbody가 있는지, Is Trigger가 꺼져 있는지 확인합니다.
- Trigger 이벤트가 발생하지 않으면 한쪽에 Rigidbody가 있고, Is Trigger가 켜져 있는지 확인합니다.

## 자주 일어나는 실수

- Collision 이벤트를 기대하는데 **Is Trigger**가 켜져 있음 → Trigger 이벤트만 발생
- Collision 이벤트를 받으려는 오브젝트에 **Rigidbody**가 없음 → 충돌 이벤트 미발생
- 콜백 함수 이름을 잘못 작성함 (예: `OnCollisionEnter`) → Lua에서는 소문자 `onCollisionEnter` 사용
- 충돌한 오브젝트의 Lua 스크립트를 찾을 때 `GetComponent` 대신 `GetLuaComponent`를 사용하지 않음 → [Viven Lua Behaviour](../../03-scripting/01-viven-lua-behaviour.md) 참조

## 관련 문서

- [Rigidbody와 Collider](../02-unity-physics/01-rigidbody-and-collider.md)
- [Raycasting으로 오브젝트 찾기](02-raycasting.md)
- [Viven Lua Behaviour](../../03-scripting/01-viven-lua-behaviour.md)
