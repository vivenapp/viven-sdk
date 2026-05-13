# 채팅 (Chat) 개요

## 개요

Viven 플랫폼에서 플레이어 간의 원활한 소통을 위한 텍스트 채팅과 음성 채팅 시스템을 설명합니다. 스크립트를 통해 메시지를 자동으로 전송하거나, 특정 플레이어에게 귓속말을 보내고, 채팅창 UI를 제어하여 소통의 흐름을 관리할 수 있습니다.

## 주요 특징

- **텍스트 채팅 API**: 전체 채팅 채널이나 특정 유저(귓속말)에게 텍스트 메시지를 전송합니다.
- **채팅 UI 제어**: 특정 연출이나 상황에서 채팅창이 열리지 않도록 잠그거나 해제할 수 있습니다.
- **음성 채팅(Voice Chat) 지원**: 마이크 음소거, 스피커 볼륨 조절 등 음성 채팅 설정을 실시간으로 제어합니다.
- **공간 음향(Spatial Voice)**: 플레이어 간의 거리에 따라 소리가 입체적으로 들리도록 설정합니다.

## 학습 순서

1. **[텍스트 채팅 API](./01-text-chat-api/00-overview.md)**: 전체 채팅과 귓속말 전송, 채팅창 UI 잠금 및 해제 방법을 배웁니다.
2. **[음성 채팅 (Voice Chat)](./02-voice-chat/00-overview.md)**: 마이크/스피커 제어와 공간 음향 설정 방법을 익힙니다.

## 준비사항

- `VivenLuaBehaviour`가 부착된 게임 오브젝트
- `TextChat` 및 `Room.VoiceChat` API (VivenScript 기본 제공)
- 마이크 및 오디오 출력 장치 (음성 채팅 테스트 시)

## 관련 문서

- [컨텐츠 제작 개요](../00-overview.md)
- [플레이어 데이터 및 상태 관리](../09-players/01-player-data-and-state-management.md)
- [Viven API 레퍼런스 (TextChat/VoiceChat)](../03-scripting/03-viven-services-and-api/02-viven-api.md)
