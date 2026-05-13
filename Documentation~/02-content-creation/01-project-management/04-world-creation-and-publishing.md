# 월드 생성 및 게시

## 개요

이 문서는 Viven에서 플레이 가능한 맵(월드)을 처음 만들고 테스트하는 절차를 설명합니다. Scene 생성부터 필수 컴포넌트 배치, Viven Script 추가, 맵 테스트 빌드까지 단계별로 진행할 수 있습니다.

## 언제 사용하나요?

- Viven 맵 제작을 처음 시작할 때
- 빈 Scene에서 기본 플레이 가능한 월드를 구성하려 할 때
- 맵 빌드 전에 VIVEN에서 직접 테스트해 보려 할 때

## 준비사항

- Viven 개발 환경 설정 완료(SDK 설치, 프로젝트 설정 등)
- Unity 에디터 실행

## 진행 순서

### 1. Scene 생성하기

Project 창에서 마우스 오른쪽 버튼을 클릭 → **Create** → **Scene** → **Scene**을 선택합니다.

### 2. Main Camera 제거

Viven은 내부적으로 자체 카메라를 사용하므로 추가 Camera 컴포넌트를 사용할 수 없습니다. 기본 Scene에 포함된 **Main Camera**를 제거합니다.

### 3. 필수 컴포넌트 추가

#### Viven Map Environment

- 빈 게임 오브젝트를 생성한 뒤 **Add Component** → **Viven Map Environment**를 추가합니다.
- `VivenMapEnvironment`가 추가된 GameObject는 **반드시 루트 레벨**에 배치되어야 합니다.

#### Viven Player Start Point

- 맵에 입장했을 때 아바타가 시작하는 위치를 지정하는 컴포넌트입니다.
- 게임 오브젝트를 원하는 위치로 이동·회전시킨 뒤 **Add Component** → **Viven Player Start Point**를 추가합니다.

### 4. 바닥 게임 오브젝트 생성

Viven 아바타는 기본적으로 중력의 영향을 받으므로, 바닥 역할을 하는 Collider가 포함된 오브젝트가 필요합니다.

- **3D Object** → **Plane**을 생성합니다. Plane은 예시이며, 충분한 크기의 Collider가 있다면 다른 오브젝트를 사용해도 됩니다.
- **Viven Player Start Point**가 포함된 게임 오브젝트의 위치가 바닥 오브젝트 범위를 벗어나면, VIVEN 실행 시 아바타가 아래로 추락할 수 있으므로 주의합니다.

### 5. Viven Script 추가

Viven Script는 Scene 내 게임 오브젝트에 추가하여 사용합니다.

1. 빈 게임 오브젝트를 생성한 뒤 **Add Component** → **Viven Lua Behaviour**를 추가합니다.
2. **VIVEN Script 만들기** 버튼을 클릭해 새 스크립트를 생성하거나, 기존 Viven Script 파일을 드래그 앤 드롭해 해당 컴포넌트에 적용합니다.
3. Viven Script가 설정되지 않은 Viven Lua Behaviour가 Scene에 있으면 맵 빌드 시 에러가 발생합니다. 사용하지 않을 경우 해당 컴포넌트를 반드시 제거합니다.

스크립트 사용 방법은 [VivenLuaBehaviour 활용](../03-scripting/01-viven-lua-behaviour.md) 문서를 참고하세요.

### 6. 맵 테스트 빌드하기

1. 상단 메뉴에서 **Test on VIVEN** 버튼을 클릭합니다.
2. 처음 맵 빌드를 하는 경우 알림 창이 표시될 수 있습니다. **Yes**를 클릭합니다.
3. VIVEN이 실행되면 테스트 맵에 입장할 수 있습니다.

## 확인 방법

- **Test on VIVEN** 실행 후 VIVEN 클라이언트에서 해당 맵에 입장할 수 있는지 확인합니다.
- 아바타가 Viven Player Start Point 위치에서 시작하는지, 바닥 위에 정상적으로 서 있는지 확인합니다.

## 자주 일어나는 실수

- **Main Camera 미제거**: Viven은 자체 카메라를 사용하므로 Main Camera를 제거하지 않으면 충돌이 발생할 수 있습니다.
- **Viven Map Environment 위치**: 루트 레벨이 아닌 자식 오브젝트에 배치하면 동작하지 않을 수 있습니다.
- **Player Start Point와 바닥 분리**: 시작 지점이 바닥 Collider 범위 밖에 있으면 아바타가 추락합니다.
- **빈 Viven Lua Behaviour**: Script가 설정되지 않은 Viven Lua Behaviour를 Scene에 남겨두면 맵 빌드 시 에러가 발생합니다.

## 관련 문서

- [Viven 컨텐츠 유형 (VObject, VMap, VAvatar)](01-viven-content-types-vobject-vmap-vavatar.md)
- [VMap 빌드 방법](02-build-and-deployment-guide/01-vmap-build-process.md)
- [VivenLuaBehaviour 활용](../03-scripting/01-viven-lua-behaviour.md)
- [스크립팅 개요](../03-scripting/00-overview.md)
