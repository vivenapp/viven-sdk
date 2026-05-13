# 컨텐츠 제작 (Content Creation) 개요

## 개요

Viven 플랫폼에서 사용자 경험을 구성하는 다양한 컨텐츠(맵, 오브젝트, 아바타)를 제작하는 전반적인 과정과 핵심 시스템을 설명합니다. Unity 6와 Viven SDK를 활용하여 몰입감 있는 멀티플레이어 환경을 구축할 수 있습니다.

## 주요 특징

- **다양한 컨텐츠 유형**: 공간 전체를 구성하는 `VMap`, 상호작용 가능한 `VObject`, 사용자 외형인 `VAvatar`를 제작할 수 있습니다.
- **강력한 스크립팅 시스템**: xLua 기반의 Lua 스크립트를 통해 복잡한 게임 로직과 상호작용을 손쉽게 구현합니다.
- **멀티플레이어 최적화**: DTS 서버를 통한 자동 동기화 및 RPC 시스템으로 대규모 인원이 참여하는 환경을 지원합니다.
- **크로스 플랫폼 지원**: PC, VR, 모바일 등 다양한 기기 환경에서 동일한 경험을 제공할 수 있도록 설계되었습니다.

## 학습 순서

1. **[프로젝트 관리](./01-project-management/00-overview.md)**: 컨텐츠 유형을 이해하고 빌드 및 배포 프로세스를 익힙니다.
2. **[3D 월드 구성 및 에셋](./02-3d-world-building-and-assets/00-overview.md)**: Unity 에셋과 물리 시스템을 활용하여 월드를 시각화하고 구성합니다.
3. **[스크립팅 (Scripting)](./03-scripting/00-overview.md)**: Lua를 사용하여 상호작용, 네트워크 동기화, 비동기 로직 등을 구현합니다.
4. **[입력 및 인터랙션](./04-input/00-overview.md)**: PC와 VR 환경에 대응하는 입력 처리와 핸드 트래킹 활용법을 배웁니다.
5. **[캐릭터 및 플레이어](./08-characters/00-overview.md)**: 아바타 시스템과 플레이어 데이터 관리 방법을 익힙니다.
6. **[고급 기능](./11-advanced/00-overview.md)**: 성능 최적화와 디버깅 도구 활용법을 학습합니다.

## 준비사항

- Unity 6 (6000.0.43f1 권장) 설치
- Viven SDK 패키지 임포트 및 초기 설정 완료
- Lua 스크립트 작성을 위한 에디터(VSCode 등) 환경

## 관련 문서

- [SDK 설치 및 설정](../01-overview/01-sdk-installation-and-setup.md)
- [Viven API 레퍼런스](./03-scripting/03-viven-services-and-api/02-viven-api.md)
