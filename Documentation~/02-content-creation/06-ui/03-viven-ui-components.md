# Viven 전용 UI 컴포넌트

## 개요

Viven SDK에서 제공하는 UI 전용 컴포넌트들의 기능과 사용법을 설명합니다. 이 컴포넌트들은 PC와 VR 환경 간의 차이를 자동으로 처리하고, Viven의 핵심 시스템과 UI를 연결하는 역할을 합니다.

## 주요 컴포넌트

### 1. VivenGraphicRaycaster

Unity의 기본 `Graphic Raycaster`를 대체하는 컴포넌트입니다. PC의 마우스 입력과 VR의 레이캐스트 입력을 모두 지원합니다.

- **Always Front**: 이 옵션을 활성화하면 UI가 월드의 다른 오브젝트에 가려지지 않고 항상 최상단에 렌더링되도록 레이어를 자동으로 조정합니다.
- **3D Occlusion**: `Always Front`가 꺼져 있을 때, 월드 내 실제 사물에 의해 UI가 가려지도록 설정할 수 있습니다.

### 2. UIModeChanger

현재 실행 중인 플랫폼 모드(PC, XR, Mobile)에 따라 활성화할 UI 오브젝트를 자동으로 전환해줍니다.

- **XR UI**: VR 모드에서 활성화될 UI 오브젝트를 할당합니다.
- **PC UI**: PC 또는 모바일 모드에서 활성화될 UI 오브젝트를 할당합니다.
- **동작 방식**: Viven의 `PlayModeManager` 이벤트를 구독하여 모드 변경 시 즉시 UI를 교체합니다.

## 사용 예시

### PC/VR 공용 캔버스 구성

1. Canvas 오브젝트를 생성하고 `World Space`로 설정합니다.
2. `VivenGraphicRaycaster`를 추가합니다.
3. 자식 오브젝트로 `PC_Layout`과 `VR_Layout`을 만듭니다.
4. 부모 Canvas에 `UIModeChanger`를 추가하고 각 레이아웃을 할당합니다.

## 확인 방법

- **인스펙터 설정**: 각 컴포넌트의 필드에 올바른 오브젝트가 할당되었는지 확인합니다.
- **런타임 동작**: PC와 VR 환경에서 각각 의도한 UI가 활성화되고 클릭이 정상적으로 작동하는지 확인합니다.

## 자주 일어나는 실수

- **기존 Raycaster 유지**: Unity 기본 `Graphic Raycaster`가 남아 있으면 `VivenGraphicRaycaster`와 충돌하거나 VR에서 작동하지 않을 수 있습니다. 반드시 제거하세요.
- **레이어 충돌**: `Always Front` 옵션 사용 시 레이어가 `UI`로 변경되므로, 카메라의 `Culling Mask`에 `UI` 레이어가 포함되어 있는지 확인해야 합니다.

## 관련 문서

- [체험 내 UI (World Space UI)](./02-world-space-ui.md)
- [VivenLuaBehaviour 활용](../03-scripting/01-viven-lua-behaviour.md)
