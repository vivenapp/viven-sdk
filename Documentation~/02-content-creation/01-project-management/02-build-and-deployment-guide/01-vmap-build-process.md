# VMap 빌드 방법

## 개요

VMap은 플레이어가 접속할 수 있는 맵 컨텐츠입니다. Viven SDK를 사용해 Unity에서 제작한 씬을 빌드하면 `.vmap` 파일로 출력되며, VIVEN 플랫폼에 업로드해 배포할 수 있습니다. VMap에는 VObject를 포함할 수 있고, VivenBehaviour를 사용해 별도의 로직을 수행할 수도 있습니다.

## 언제 사용하나요?

- 플레이어가 접속할 월드(맵)를 제작하고 배포하려 할 때
- VObject와 VivenBehaviour를 포함한 맵을 하나의 파일로 빌드하려 할 때
- Windows, MacOS, Android, iOS 등 여러 플랫폼을 하나의 통합 빌드로 지원하려 할 때

## 준비사항

- Viven SDK가 설치된 Unity 프로젝트
- 빌드할 씬(Scene) 준비 완료
- SDK 설치, 프로젝트 설정 등 빌드 전 필수 사항 확인 완료
- VMap에 포함할 VObject가 있다면 `ContentType`을 **Prepared**로 설정

### VObject 함께 빌드하기

VMap에 VObject를 추가하는 과정은 [VObject 빌드 방법](02-vobject-build-process.md)과 대부분 동일합니다. 다만 다음 사항을 지켜야 합니다.

- **VMap에 배치된 VObject**는 `ContentType`을 **Prepared**로 설정해야 합니다.
- VMap에 포함된 VObject를 **Prepared 오브젝트**라고 부릅니다.
- Prepared 오브젝트는 사용자가 제거할 수 없으며, VMap이 로딩될 때 함께 로딩됩니다.
- 제거 불가라는 점을 제외하면 VObject와 동일하게 동작합니다.

## 진행 순서

1. **빌드할 씬 열기**: Unity 에디터에서 빌드할 Scene을 엽니다.
2. **Build V-map 실행**: 에디터 상단에 있는 **Build V-map** 버튼을 클릭합니다.
3. **플랫폼 선택**: 통합 빌드 창에서 원하는 플랫폼을 선택합니다.
   - Windows, MacOS, Android, iOS 중 **반드시 하나 이상**을 선택해야 합니다.
   - 여러 플랫폼을 선택하면 하나의 파일로 통합 빌드됩니다.
4. **빌드 시작**: **통합 빌드 시작**을 클릭하면 빌드가 시작됩니다.
5. **저장 위치 선택**: 빌드가 완료되면 `.vmap` 파일을 저장할 위치를 선택합니다.
6. **완료 확인**: 빌드가 정상적으로 완료되면 팝업 창이 표시됩니다.

## 확인 방법

- 빌드가 성공하면 선택한 경로에 `.vmap` 파일이 생성됩니다.
- 팝업 창에 빌드 완료 메시지가 표시됩니다.
- [Viven Content Upload](03-viven-content-upload.md) 문서에 따라 VIVEN에 등록할 수 있는지 확인합니다.

## 자주 일어나는 실수

- **Addressable 설정 누락**: 빌드에 실패한다면 Addressable 설정을 확인하세요. 에셋이 올바르게 Addressable로 등록되어 있는지 점검합니다.
- **플랫폼 미선택**: 통합 빌드에서 Windows, MacOS, Android, iOS 중 하나 이상을 선택하지 않으면 빌드를 진행할 수 없습니다.
- **Prepared 설정 누락**: VMap에 포함된 VObject의 `ContentType`을 Prepared로 설정하지 않으면, VMap 로딩 시 함께 로딩되지 않거나 의도와 다르게 동작할 수 있습니다.

## VMap 레벨 디자인 참고

VMap에 자체 로직을 구현하려면 다음 기능을 활용할 수 있습니다.

### 네트워크 동기화

VObject와 마찬가지로 RPC와 SyncTable을 사용할 수 있습니다. VMap은 추가로 서버에 **Room 프로퍼티** 테이블을 저장할 수 있습니다.

- Room 프로퍼티는 생성된 방마다 각각 존재하며, VMap들끼리 공유되지 않습니다.
- 클라이언트와 독립적으로 존재하므로, 사용자가 맵을 나간 뒤에도 데이터 저장·동기화가 가능합니다.
- 테이블 형태이므로 많은 데이터를 적재하거나 잦은 변경이 있으면 네트워크 부하가 생길 수 있습니다.
- 데이터 무결성을 보장하지 않습니다. 변경 요청이 네트워크 딜레이로 즉시 반영되지 않을 수 있으므로, 프로퍼티 변경에 의한 로직이 있다면 값을 확인하는 과정을 거쳐야 합니다.

### 네트워크 이벤트

방 진입·퇴장 시점에 맞춰 기능을 구현할 수 있습니다. 다음 이벤트 함수를 구현하면 됩니다.

- `onRoomJoined` — 자신이 방에 진입했을 때
- `onRoomLeave` — 자신이 방을 나갈 때
- `onRoomUserJoined` — 다른 사용자가 방에 진입했을 때
- `onUserLeaveRoom` — 다른 사용자가 방을 나갔을 때

자세한 사용법은 [네트워크 개요](../../03-scripting/05-networking-and-synchronization/00-overview.md)와 [Room Property](../../03-scripting/05-networking-and-synchronization/04-room-property.md)를 참고하세요.

## 관련 문서

- [Viven 컨텐츠 유형 (VObject, VMap, VAvatar)](../01-viven-content-types-vobject-vmap-vavatar.md)
- [컨텐츠 빌드 및 배포 가이드 개요](00-overview.md)
- [VObject 빌드 방법](02-vobject-build-process.md)
- [Viven Content Upload](03-viven-content-upload.md)
- [네트워크 개요](../../03-scripting/05-networking-and-synchronization/00-overview.md)
- [Room Property](../../03-scripting/05-networking-and-synchronization/04-room-property.md)
