# 얼굴 표현 (Facial Expression)

## 개요

Viven 아바타 시스템(VAvatar)은 블렌드쉐이프(BlendShape)를 사용하여 아바타의 다양한 얼굴 표정을 구현합니다. 현재 Viven은 **PerfectSync(52개 블렌드쉐이프)** 규격을 지원하며, 이를 통해 미세한 표정 변화를 아바타에 적용하고 Viven 내 UI에서 선택하여 실행할 수 있습니다.

## 언제 사용하나요?

- 아바타에 기쁨, 슬픔, 화남 등 특정한 표정을 프리셋으로 등록하고 싶을 때 사용합니다.
- Viven 내 아바타 메뉴 UI에서 선택하여 실행할 수 있는 표정 리스트를 구성할 때 사용합니다.

## 준비사항

- **PerfectSync 지원 아바타**: 얼굴 메쉬(SkinnedMeshRenderer)에 PerfectSync 표준 52개 블렌드쉐이프가 포함되어 있어야 합니다.
- **아이콘 스프라이트**: UI에 표시될 표정 아이콘(Sprite)이 필요합니다.
- **SDKFacialExpressionComponent**: 아바타 프리팹에 부착하여 얼굴 표현을 관리하는 컴포넌트입니다.

## 진행 순서

### 1. 컴포넌트 추가
1. 아바타 프리팹의 얼굴 메쉬가 포함된 오브젝트 또는 루트 오브젝트에 `SDKFacialExpressionComponent`를 추가합니다.
2. `Facial Blend Shape Parent` 필드에 얼굴 블렌드쉐이프가 포함된 `SkinnedMeshRenderer`를 연결합니다.

### 2. 표정 프리셋 제작 (에디터 활용)
블렌드쉐이프 값을 일일이 입력하는 대신, 에디터의 복사 기능을 활용하면 편리합니다.

1. 씬(Scene) 뷰에서 얼굴 메쉬의 `SkinnedMeshRenderer` 컴포넌트를 직접 조작하여 원하는 표정을 만듭니다.
2. `SDKFacialExpressionComponent`의 `Sdk Facial Expressions` 리스트에 새로운 항목을 추가합니다.
3. 해당 항목 아래의 **CopyCurrentBlendShape** 버튼을 클릭합니다.
4. 현재 메쉬에 설정된 모든 블렌드쉐이프 값이 해당 표정 항목에 자동으로 저장됩니다.

### 3. 표정 정보 설정
등록된 각 표정 항목에 대해 다음 정보를 입력합니다.

| 설정 항목 | 설명 |
| :--- | :--- |
| **Expression Name** | Viven 내 UI에 표시될 표정의 이름입니다. |
| **Sprite** | UI에 표시될 아이콘 이미지(Sprite)입니다. |
| **Blend Shape Values** | 52개의 블렌드쉐이프 값 배열입니다. (복사 기능을 사용하면 자동 입력됨) |

## 확인 방법

- **Viven 내 UI 확인**: Viven에 접속하여 아바타 메뉴의 표정 탭을 열었을 때, 설정한 이름과 아이콘이 올바르게 표시되는지 확인합니다.
- **표정 실행**: 리스트에서 표정을 선택했을 때 아바타의 얼굴이 설정한 대로 즉시 변경되는지 확인합니다.

## 자주 일어나는 실수

- **PerfectSync 규격 미준수**: 아바타의 블렌드쉐이프 이름이 PerfectSync 표준(예: `eyeBlinkLeft`, `jawOpen` 등)과 다르면 표정이 정상적으로 작동하지 않을 수 있습니다.
- **블렌드쉐이프 개수 불일치**: `Blend Shape Values`의 개수는 반드시 52개여야 합니다. 에디터의 복사 기능을 사용하지 않고 수동으로 입력할 때 주의하십시오.
- **부모 오브젝트 미연결**: `Facial Blend Shape Parent`가 올바르게 연결되지 않으면 런타임에 어떤 메쉬의 표정을 바꿔야 할지 알 수 없습니다.

## 관련 문서

- [Viven 아바타 시스템 개요](01-viven-avatar-system.md)
- [아바타 제작 및 커스터마이징](02-avatar-customization.md)
- [PerfectSync 블렌드쉐이프 목록 (외부 링크)](https://vrm.dev/en/univrm/blendshape/blendshape_setup.html)
