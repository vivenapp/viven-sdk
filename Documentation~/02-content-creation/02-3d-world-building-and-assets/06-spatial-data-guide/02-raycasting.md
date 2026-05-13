# Raycasting

## 개요

**Raycast**는 특정 방향으로 가상의 광선을 쏘아, 맞은 오브젝트를 찾는 방법입니다. Viven에서는 `VivenUtil.Physics.RayCast`와 `VivenUtil.Physics.RayCastAll`을 사용해 월드 공간에서 오브젝트를 검출할 수 있습니다. Raycast로 맞은 오브젝트에서 다른 Viven Lua 스크립트를 찾을 때는 `GetLuaComponent`를 사용합니다.

## 언제 사용하나요?

- 손이나 컨트롤러 방향으로 쏜 광선에 맞은 오브젝트를 찾고 싶을 때
- 특정 위치·방향에서 가장 가까운 오브젝트를 검출하고 싶을 때
- UI 터치·포인팅이 아닌 3D 공간 상의 오브젝트 선택이 필요할 때

## 준비사항

- Raycast를 실행할 Lua 스크립트가 `VivenLuaBehaviour`에 연결되어 있어야 합니다.
- Raycast 대상 오브젝트에는 **Collider**가 있어야 합니다.
- Viven의 Raycast는 **Default** 또는 **Grabbable** 레이어에 있는 오브젝트만 검출합니다.

## 진행 순서

### 1. Raycast API 사용

Viven에서 제공하는 Raycast API는 [VivenUtil.Physics](https://wiki.viven.app/48e55347-0331-43ed-9db2-ed3938cfeb8c)를 참조하세요.

**`VivenUtil.Physics.RayCast(ray, distance)`**

- 단일 Raycast를 실행합니다.
- Lua에서는 C#의 `out` 파라미터가 반환값으로 전달됩니다.
- 반환: `(isHit, hitInfo)` — `isHit`는 적중 여부, `hitInfo`는 `RaycastHit`입니다.

**`VivenUtil.Physics.RayCastAll(ray, distance)`**

- 광선 경로상의 모든 오브젝트를 검출합니다.
- 반환: `RaycastHit[]` 배열

### 2. 기본 사용 예시

```lua
-- 광선 생성: (시작점, 방향)
local ray = Ray(RightHandBone.position, RightHandBone.forward)

-- Lua에서는 C#의 Out을 return으로 받습니다.
local isHit, hitRayCast = VivenUtil.Physics.RayCast(ray, 100.0)

if isHit then
    if hitRayCast.transform.name == "Cube" then
        Debug.Log("Cube Hit")
    end
end
```

### 3. Raycast로 맞은 오브젝트에서 다른 VivenScript 찾기

Raycast로 맞은 오브젝트(`hitRayCast.transform.gameObject`)에 다른 Viven Lua 스크립트가 붙어 있다면, `GetLuaComponent`를 사용해 해당 스크립트의 Lua 테이블을 가져올 수 있습니다. 자세한 내용은 [Viven Lua Behaviour](../../03-scripting/01-viven-lua-behaviour.md)를 참조하세요.

```lua
local ray = Ray(RightHandBone.position, RightHandBone.forward)
local isHit, hitRayCast = VivenUtil.Physics.RayCast(ray, 100.0)

if isHit then
    local hitGameObject = hitRayCast.transform.gameObject
    -- 다른 VivenScript(예: "InteractableScript")의 Lua 테이블 가져오기
    local otherScript = hitGameObject:GetLuaComponent("InteractableScript")
    if otherScript and otherScript.OnRaycastHit then
        otherScript:OnRaycastHit()
    end
end
```

### 4. RayCastAll로 여러 오브젝트 검출

```lua
local ray = Ray(origin.position, origin.forward)
local hits = VivenUtil.Physics.RayCastAll(ray, 100.0)

-- C# 배열은 0부터 시작 (xLua)
for i = 0, hits.Length - 1 do
    local hit = hits[i]
    local hitObject = hit.transform.gameObject
    local script = hitObject:GetLuaComponent("MyScript")
    if script then
        -- 처리
    end
end
```

## 확인 방법

- Play 모드에서 Raycast를 쏘고 `Debug.Log`로 `isHit`, `hitRayCast.transform.name` 등을 출력해 적중 여부를 확인합니다.
- Raycast가 맞지 않으면 대상 오브젝트의 **Layer**가 Default 또는 Grabbable인지 확인합니다.
- `GetLuaComponent`가 `nil`을 반환하면 해당 GameObject에 지정한 이름의 Viven Lua 스크립트가 붙어 있는지 확인합니다.

## 자주 일어나는 실수

- `UnityEngine.Physics.Raycast`를 직접 사용함 → Viven 레이어 필터가 적용되지 않음. **VivenUtil.Physics.RayCast**를 사용하세요.
- Raycast 대상 오브젝트에 **Collider**가 없음 → 적중되지 않음
- 대상 오브젝트가 **Default**·**Grabbable** 레이어가 아님 → Viven Raycast에서 무시됨
- 다른 VivenScript를 찾을 때 `GetComponent` 대신 `GetLuaComponent`를 사용하지 않음 → [Viven Lua Behaviour](../../03-scripting/01-viven-lua-behaviour.md) 참조

## 관련 문서

- [VivenUtil.Physics (Raycast API)](https://wiki.viven.app/48e55347-0331-43ed-9db2-ed3938cfeb8c)
- [Collisions](01-collisions.md)
- [Viven Lua Behaviour](../../03-scripting/01-viven-lua-behaviour.md)
