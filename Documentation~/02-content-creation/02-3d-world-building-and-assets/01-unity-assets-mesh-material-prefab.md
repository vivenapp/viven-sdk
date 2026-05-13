# Unity 에셋: 메시, 머티리얼, 프리팹

## 개요

Unity의 메시(Mesh), 머티리얼(Material), 프리팹(Prefab)을 사용해 Viven 월드에 3D 오브젝트를 배치하고 재사용할 수 있습니다. 이 문서는 기본 사용법과 Viven에서 유의해야 할 설정을 설명합니다.

## 언제 사용하나요?

- 3D 모델을 월드에 배치할 때
- 오브젝트에 색상·텍스처를 적용할 때
- 동일한 오브젝트를 여러 곳에 재사용할 때
- 상호작용 가능한 오브젝트(그랩, 클릭 등)를 만들 때

## 준비사항

- Unity 프로젝트에 3D 모델(FBX, OBJ 등) 또는 프리미티브가 있어야 합니다.
- Viven SDK가 프로젝트에 포함되어 있어야 합니다.

## 진행 순서

### 1. 메시(Mesh) 가져오기

1. 3D 모델 파일을 `Assets` 폴더로 드래그하거나 `Import`로 가져옵니다.
2. Project 창에서 모델을 선택한 뒤 Inspector에서 **Model** 탭을 확인합니다.
3. **Read/Write Enabled**를 체크합니다. (Viven Outline 사용 시 필수)

### 2. 머티리얼(Material) 설정

1. `Assets`에서 우클릭 → **Create** → **Material**로 머티리얼을 만듭니다.
2. 머티리얼에 사용하는 텍스처가 있다면, 해당 텍스처를 선택하고 Inspector에서 **Read/Write Enabled**를 체크합니다.
3. 머티리얼을 메시가 있는 오브젝트의 `MeshRenderer` 또는 `SkinnedMeshRenderer`에 드래그해 적용합니다.

### 3. 프리팹(Prefab) 만들기

1. Hierarchy에서 설정이 완료된 오브젝트를 Project 창으로 드래그해 프리팹으로 저장합니다.
2. 이후 Hierarchy에 드래그해 인스턴스를 배치하거나, Lua 스크립트로 동적 생성할 수 있습니다.

## 확인 방법

- **메시**: Scene 뷰에서 오브젝트가 올바르게 표시되는지 확인합니다.
- **머티리얼**: Game 뷰에서 색상·텍스처가 의도대로 보이는지 확인합니다.
- **프리팹**: 프리팹 인스턴스를 여러 개 배치해 동일하게 동작하는지 확인합니다.

## Viven에서 유의해야 할 점

### 1. Outline과 Read/Write 설정

**메시 또는 머티리얼이 사용하는 텍스처의 Read/Write Enabled가 비활성화되어 있으면 오브젝트의 Outline이 동작하지 않습니다.**

Viven의 상호작용 오브젝트(그랩, 클릭 등)는 Outline으로 시선/커서 시 인식 가능 상태를 표시합니다. Outline은 메시의 정점·법선 데이터를 사용하므로 다음을 확인하세요.

- **메시**: 모델 선택 → Inspector → Model 탭 → **Read/Write Enabled** 체크
- **텍스처**: 머티리얼에 사용하는 텍스처 선택 → Inspector → **Read/Write Enabled** 체크

### 2. Static 설정과 상호작용

**오브젝트가 Static으로 설정되면 상호작용이 정상적으로 이루어지지 않습니다.**

그랩, 클릭, 물리 등 상호작용이 필요한 오브젝트는 Static을 해제해야 합니다.

- Inspector 상단 **Static** 체크박스를 해제합니다.
- 배경·환경처럼 움직이지 않는 장식용 오브젝트만 Static으로 두는 것을 권장합니다.

### 3. VR 환경을 고려한 성능 최적화

Viven은 PC VR을 지원하므로, 90Hz 이상 유지를 위해 다음을 권장합니다.

- **드로우 콜**: 머티리얼 인스턴스 수를 줄이고, 가능하면 텍스처 아틀라스로 배칭합니다.
- **폴리곤 수**: 시선에 가까운 오브젝트는 적절한 LOD(Level of Detail)를 사용합니다.
- **라이트맵**: 정적인 조명은 베이크하여 실시간 라이트 수를 줄입니다.
- **오클루전**: 큰 맵에서는 Occlusion Culling을 검토합니다.

## 자주 일어나는 실수

- Outline이 보이지 않음 → 메시·텍스처의 Read/Write Enabled 미체크
- 그랩/클릭이 안 됨 → 오브젝트가 Static으로 설정됨
- VR에서 프레임 드랍 → 과도한 폴리곤, 실시간 라이트, 머티리얼 인스턴스 과다

## 관련 문서

- [3D 월드 빌딩 개요](00-overview.md)
- [VMap 빌드 과정](../../01-project-management/02-build-and-deployment-guide/01-vmap-build-process.md)
