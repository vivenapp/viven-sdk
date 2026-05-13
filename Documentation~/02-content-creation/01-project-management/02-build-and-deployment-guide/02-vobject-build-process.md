# VObject 빌드 방법

## 개요

VObject는 Viven에서 상호작용이 가능한 물체입니다. 잡기, 배치, 탑승, 착석 같은 상호작용을 담을 수 있으며, VMap에 포함하거나 독립적으로 빌드하여 여러 맵에서 재사용할 수 있습니다. 이 문서는 Unity에서 VObject를 제작하고 빌드하는 절차를 설명합니다.

## 언제 사용하나요?

- 월드 안에 배치할 개별 소품, 도구, 탑승물, 의자 등을 제작할 때
- VObject를 별도로 빌드하여 여러 맵에서 꺼내 쓸 수 있게 하려 할 때
- VMap에 포함되지 않은 독립 오브젝트를 배포하려 할 때

## 준비사항

- Viven SDK가 설치된 Unity 프로젝트
- 빌드할 프리팹 또는 씬(메시, Collider 포함)
- 필요한 상호작용 모듈(Grabbable, Sittable, Ridable 등) 설정 완료

## 진행 순서

### 1. 컴포넌트 추가

플레이어가 오브젝트와 상호작용할 수 있도록 필요한 모듈을 추가합니다.

- **VivenGrabbableModule**: 물체를 잡을 수 있게 합니다. PC/VR 환경에서 상호작용 가능하며, 물리적 상태는 네트워크를 통해 동기화됩니다.
- **VivenRigidBodyControlModule**: 물리와 관련된 설정을 변경합니다. Unity의 Rigidbody를 대체하므로, 별도의 Rigidbody 컴포넌트가 있다면 삭제해야 합니다.
- **Collider**: VivenGrabbableModule은 Collider를 통해 상호작용합니다. 오브젝트에 적절한 Collider를 추가해야 합니다.

### 2. 메시 설정

오브젝트의 Mesh(fbx 등) Import Setting에서 **Read/Write Enabled**를 활성화합니다.

### 3. Viven VObject 설정

프리팹을 선택한 뒤 Inspector에서 다음을 설정합니다.

- **ContentType**:
  - **Prepared**: VMap과 함께 빌드되는 오브젝트. VMap에 배치된 VObject는 이 타입으로 설정합니다.
  - **V Object**: 독립적으로 빌드되는 오브젝트. 맵과 상관없이 불러올 수 있습니다.
- **Object Id**: 생성 과정에서 자동으로 변경됩니다. 수동 수정이 필요하지 않습니다.
- **Display Name**: 오브젝트를 지칭할 이름을 작성합니다. 플레이어가 오브젝트와 상호작용할 때 화면에 표시됩니다.

### 4. VObject 빌드

1. Project 창에서 빌드할 프리팹을 **우클릭**합니다.
2. **Viven → BuildVivenObject**를 클릭합니다.
3. 빌드가 완료되면 `.vobject` 파일을 저장할 경로를 선택합니다.
4. 성공 창이 표시되면 빌드가 정상적으로 완료된 것입니다.

## 확인 방법

- 빌드가 성공하면 선택한 경로에 `.vobject` 파일이 생성됩니다.
- 성공 팝업 창이 표시됩니다.
- [Viven Content Upload](03-viven-content-upload.md) 문서에 따라 VIVEN에 업로드할 수 있는 형태인지 확인합니다.
- 업로드 후 여러 맵에서 해당 VObject를 꺼내 쓸 수 있는지 테스트합니다.

## 자주 일어나는 실수

- **Addressable 설정 누락**: 빌드에 실패한다면 Addressable 설정을 확인하세요. 에셋이 올바르게 Addressable로 등록되어 있는지 점검합니다.
- **ContentType 혼동**: VMap에 포함할 오브젝트는 Prepared로, 독립 배포할 오브젝트는 V Object로 설정해야 합니다. 잘못 설정하면 의도와 다른 형태로 배포됩니다.
- **Rigidbody 중복**: VivenRigidBodyControlModule을 사용할 때 별도의 Rigidbody 컴포넌트가 남아 있으면 충돌이 발생합니다. 기존 Rigidbody를 삭제해야 합니다.
- **Collider 누락**: VivenGrabbableModule은 Collider를 통해 상호작용하므로, Collider가 없으면 잡기 등이 동작하지 않습니다.
- **Read/Write 미활성화**: Mesh Import Setting에서 Read/Write가 비활성화되어 있으면 문제가 발생할 수 있습니다.

## 관련 문서

- [Viven 컨텐츠 유형 (VObject, VMap, VAvatar)](../01-viven-content-types-vobject-vmap-vavatar.md)
- [컨텐츠 빌드 및 배포 가이드 개요](00-overview.md)
- [VMap 빌드 방법](01-vmap-build-process.md)
- [Viven Content Upload](03-viven-content-upload.md)
- [GrabbableModule (물체 잡기)](../../03-scripting/04-player-interaction-modules/01-grabbable-module.md)
- [SittableModule (의자 앉기)](../../03-scripting/04-player-interaction-modules/02-sittable-module.md)
