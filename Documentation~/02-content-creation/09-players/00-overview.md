# 플레이어 (Players) 개요

## 개요

Viven 플랫폼에서 현재 접속 중인 플레이어(나 또는 타인)의 데이터를 조회하고, 상태를 관리하며, 월드 내에서의 이동과 텔레포트를 제어하는 방법을 설명합니다. 플레이어 정보를 기반으로 한 개인화된 경험을 제공할 수 있습니다.

## 주요 특징

- **플레이어 데이터 조회**: 닉네임, 고유 ID, 프로필 정보 등 플레이어의 상세 데이터를 Lua로 확인합니다.
- **접속 모드 확인**: PC, XR, Mobile 중 어떤 환경으로 접속했는지에 따라 연출을 다르게 구성할 수 있습니다.
- **텔레포트 및 이동 제어**: 플레이어를 특정 위치로 순간이동시키거나 이동 잠금 기능을 통해 체험의 흐름을 제어합니다.
- **방장 권한 활용**: 방장(Host) 권한을 가진 플레이어가 다른 플레이어를 이동시키거나 강제 퇴장시키는 등 관리 기능을 수행합니다.

## 학습 순서

1. **[플레이어 데이터 및 상태 관리](./01-player-data-and-state-management.md)**: `Player.Mine`과 `Player.Other`를 통해 플레이어 정보를 조회하고 활용하는 방법을 배웁니다.
2. **[텔레포트 및 월드 이동](./02-teleport-and-world-travel.md)**: 플레이어를 특정 위치로 이동시키거나 다른 월드로 이동시키는 기능을 익힙니다.

## 준비사항

- `VivenLuaBehaviour`가 부착된 게임 오브젝트
- Viven SDK가 포함된 Unity 프로젝트
- `Player` API (VivenScript 기본 제공)

## 관련 문서

- [컨텐츠 제작 개요](../00-overview.md)
- [아바타 시스템](../08-characters/01-viven-avatar-system.md)
- [Viven API 레퍼런스 (Player)](../03-scripting/03-viven-services-and-api/02-viven-api.md)
