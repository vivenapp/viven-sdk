# Unity Input System 활용

## 개요

Unity의 기본 Input System을 사용하여 키보드 입력을 직접 감지할 수 있습니다. 이 기능은 주로 개발 단계에서 기능을 빠르게 테스트하거나 디버깅용 단축키를 만들 때 유용합니다.

## 언제 사용하나요?

- 특정 기능을 실행하기 위한 임시 단축키가 필요할 때
- 로컬 환경에서 스크립트 동작을 빠르게 검증하고 싶을 때
- Viven의 표준 입력 시스템 외에 추가적인 키보드 조작이 필요할 때

## 준비사항

- **Viven SDK**: 최신 버전의 SDK가 프로젝트에 포함되어 있어야 합니다.
- **PC 플랫폼**: 이 기능은 PC(Windows/Mac) 환경에서만 동작합니다.

## 진행 순서

1. **Lua 스크립트 작성**: `VivenLuaBehaviour`를 사용하는 Lua 파일에서 `Keyboard.current`를 사용하여 입력을 체크합니다.
2. **키 상태 확인**: `wasPressedThisFrame` (이번 프레임에 눌림), `isPressed` (누르고 있음) 등의 속성을 활용합니다.
3. **로직 연결**: 입력 조건이 만족되었을 때 실행할 함수나 로직을 작성합니다.

### 코드 예시

다음은 키보드의 '1'번 키를 눌렀을 때 특정 아이템을 생성하여 접시에 담는 예시입니다.

```lua
function Update()
    -- 숫자 1 키가 이번 프레임에 눌렸는지 확인
    if Keyboard.current.digit1Key.wasPressedThisFrame then
        -- 테스트용 아이템 생성 및 배치 로직
        local dishData = DishFactory.NewCookedTteokbokki()
        plate.PutDish(dishData)
        
        print("테스트: 떡볶이가 접시에 담겼습니다.")
    end
end
```

## 확인 방법

1. Unity 에디터에서 Play 모드를 실행합니다.
2. 지정한 키(예: 숫자 1)를 누릅니다.
3. 콘솔 로그나 월드 내 오브젝트 변화를 통해 기능이 실행되는지 확인합니다.

## 자주 일어나는 실수

- **키 매핑 충돌**: Viven의 기본 조작키(이동, 상호작용 등)와 겹치는 키를 지정할 경우, 의도치 않은 동작이 발생하거나 조작이 방해받을 수 있습니다.
- **플랫폼 제한**: 모바일(Android/iOS)이나 VR 환경에서는 `Keyboard.current`가 `nil`이거나 동작하지 않을 수 있으므로, 반드시 PC 플랫폼 전용 테스트 코드로만 사용해야 합니다.
- **Update 함수 부하**: 매 프레임 입력 상태를 체크하므로, 복잡한 로직을 직접 넣기보다는 상태 변화만 감지하여 별도의 함수를 호출하는 것이 좋습니다.

## 관련 문서

- [입력 시스템 개요](../04-input/00-overview.md)
- [VivenLuaBehaviour 활용](../03-scripting/01-viven-lua-behaviour.md)
