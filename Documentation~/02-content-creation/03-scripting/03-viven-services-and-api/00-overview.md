# Viven 전용 서비스 및 API 개요

## 개요

Viven API는 Lua 스크립트를 통해 Viven 플랫폼의 핵심 기능(플레이어 제어, 방 정보 관리, 시스템 UI 조작 등)을 제어할 수 있게 해주는 인터페이스입니다. Unity의 표준 라이프사이클과 Viven 전용 이벤트를 결합하여 안정적이고 풍부한 사용자 경험을 제공할 수 있습니다.

## 주요 특징

- **풍부한 API 모듈**: 플레이어(`Player`), 방(`Room`), 텍스트 채팅(`TextChat`), 시스템 UI(`UI`) 등 다양한 기능을 모듈화하여 제공합니다.
- **Unity 라이프사이클 통합**: `awake`, `start`, `update` 등 친숙한 Unity 이벤트 함수를 Lua에서 그대로 사용할 수 있습니다.
- **Viven 전용 콜백**: 방 입장(`onRoomJoined`), 사용자 입장(`onRoomUserJoined`) 등 네트워크 환경에 특화된 이벤트를 처리합니다.
- **동적 환경 제어**: 맵의 시간(`SkyDomeTime`), 안개(`Fog`) 농도 등 환경 설정을 실시간으로 변경할 수 있습니다.

## 학습 순서

1. **[Unity 라이프사이클 및 실행 시점](./01-unity-lifecycle-callbacks.md)**: Lua 스크립트의 초기화 단계와 각 콜백 함수의 호출 시점을 이해합니다.
2. **[Viven API 레퍼런스](./02-viven-api.md)**: 플레이어 제어, 방 관리, 시스템 UI 조작 등 주요 API 모듈의 상세 기능을 확인합니다.
3. **[플레이어 데이터 및 상태 관리](../../09-players/01-player-data-and-state-management.md)**: 플레이어 닉네임, ID, 접속 모드 등 정보를 조회하고 활용하는 방법을 배웁니다.
4. **[메시지 송수신](../../10-chat/01-text-chat-api/01-message-send-and-receive.md)**: 전체 채팅과 귓속말 전송, 채팅창 UI 제어 방법을 익힙니다.

## 준비사항

- `VivenLuaBehaviour` 컴포넌트가 부착된 Unity 오브젝트
- Viven SDK가 포함된 Unity 프로젝트
- Lua 스크립트 작성을 위한 에디터 환경

## 관련 문서

- [스크립팅 개요](../00-overview.md)
- [VivenLuaBehaviour 활용](../01-viven-lua-behaviour.md)
- [네트워크 및 동기화 개요](../05-networking-and-synchronization/00-overview.md)
