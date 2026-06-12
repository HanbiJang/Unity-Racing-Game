# 리듬 레이싱 멀티플레이어

**C++ TCP 소켓 서버를 직접 설계하고 Unity 클라이언트와 연동한 리듬 레이싱 복합 장르 멀티플레이어 게임**

[![Unity](https://img.shields.io/badge/Unity-000000?style=flat&logo=unity&logoColor=white)](https://unity.com/)
[![C++](https://img.shields.io/badge/C++-00599C?style=flat&logo=cplusplus&logoColor=white)](https://isocpp.org/)
[![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![YouTube](https://img.shields.io/badge/YouTube-FF0000?style=flat&logo=youtube&logoColor=white)](https://youtu.be/ha3JkYI4xb4?si=zZp6altvcj5nUx99)
[![게임플레이 영상](https://img.youtube.com/vi/ha3JkYI4xb4/0.jpg)](https://youtu.be/ha3JkYI4xb4?si=zZp6altvcj5nUx99)

> 이미지를 클릭하면 게임플레이 영상을 볼 수 있습니다.

---

## 프로젝트 정보

| 항목 | 내용 |
|---|---|
| 개발 기간 | 2023.01 ~ 2023.02 (인턴십 게임잼, 팀 프로젝트) |
| 개인 개선 | 2023.03 ~ 2026.01 (개인 리팩토링 및 기능 추가) |
| 팀 구성 | 4인 팀 개발 -> 이후 1인 유지보수 |
| 사용 기술 | Unity (C#), C++ (TCP 소켓 서버), Boost.Asio |

**본인 담당 파트:** Unity 클라이언트 전체, 서버 게임 로직 코드 일부 참여, 이후 전체 코드 리팩토링

---

## 프로젝트 개요

음악의 박자에 맞춰 3개의 레인을 이동하며 노드를 획득하는 리듬 게임과 레이싱 장르를 결합한 멀티플레이어 게임입니다.

서드파티 네트워크 라이브러리 없이 C++로 TCP 소켓 서버를 직접 구현하여 Unity 클라이언트와 연동했습니다. Session / Room 구조를 직접 설계함으로써 패킷 직렬화와 연결 관리 흐름을 처음부터 제어했습니다.

---

## 코드 상세

이 레포는 **C++ 서버** (`MyServer/`)와 **Unity 클라이언트** (`Assets/Scripts/`) 두 파트로 나뉩니다.

### C++ 서버 (`MyServer/`)

서버의 실행 흐름은 `GameJam_1.cpp` -> `Server` -> `Session` -> `MsgHandler` -> `Room` 순으로 이어집니다.

| 파일 | 역할 |
|---|---|
| [`MyServer/GameJam_1.cpp`](./MyServer/MyServer/GameJam_1.cpp) | 서버 진입점. `IDMaker`, `RoomManager` 초기화 후 `Server::Run()` 호출 |
| [`MyServer/Server.h`](./MyServer/MyServer/Server.h) / [`Server.cpp`](./MyServer/MyServer/Server.cpp) | Boost.Asio 기반 TCP 연결 수락(`async_accept`). 연결마다 `Session`을 생성하고 `RoomManager`에 넘김. 별도 스레드로 `MsgHandler::DoWork()`와 `RoomManager::DoLogic()` 구동 |
| [`MyServer/Session.h`](./MyServer/MyServer/Session.h) / [`Session.cpp`](./MyServer/MyServer/Session.cpp) | 클라이언트 1명의 소켓 담당. `DoRead()` / `DoWrite()`로 비동기 송수신. 수신한 패킷을 `MsgHandler` 큐에 넣음 |
| [`MyServer/NetBuffer.h`](./MyServer/MyServer/NetBuffer.h) / [`NetBuffer.cpp`](./MyServer/MyServer/NetBuffer.cpp) | 수신 바이트 스트림을 관리하는 링 버퍼. `beginIndex` / `endIndex`로 읽기, 쓰기 위치를 추적 |
| [`MyServer/Message.h`](./MyServer/MyServer/Message.h) / [`Message.cpp`](./MyServer/MyServer/Message.cpp) | 패킷 구조체 정의. `PacketHeader`(타입 + 길이) + 바디. `PacketType` enum과 `CReadyGame`, `CJudgement` 등 패킷별 데이터 구조체 포함 |
| [`MyServer/MsgHandler.h`](./MyServer/MyServer/MsgHandler.h) / [`MsgHandler.cpp`](./MyServer/MyServer/MsgHandler.cpp) | 글로벌 메시지 큐를 처리하는 워커. `PacketType`에 따라 `HandleReadyGame`, `HandleJudgement` 등 핸들러 함수로 분기 |
| [`MyServer/Room.h`](./MyServer/MyServer/Room.h) / [`Room.cpp`](./MyServer/MyServer/Room.cpp) | 한 게임방의 모든 로직 담당. 노드 스폰 타이밍 관리(`Update()`), 점수 계산(`CalculateScore()`), 전체 브로드캐스트(`Broadcast()`) |
| [`MyServer/RoomManager.h`](./MyServer/MyServer/RoomManager.h) / [`RoomManager.cpp`](./MyServer/MyServer/RoomManager.cpp) | 방 목록 관리 및 매칭. `FrameTimer`로 일정 주기마다 모든 방의 `Update()` 호출 |
| [`MyServer/ContentLoader.h`](./MyServer/MyServer/ContentLoader.h) / [`ContentLoader.cpp`](./MyServer/MyServer/ContentLoader.cpp) | tinyxml2로 `MusicNodeData.xml`을 파싱하여 노드 리스트 반환 |

---

### Unity 클라이언트 (`Assets/Scripts/`)

클라이언트의 흐름은 `ServerInterface`(수신) -> `ClientActionFactory`(분기) -> 각 `IClientAction`(처리) 순으로 이어집니다.

#### 네트워크 레이어 (`Systems/`)

| 파일 | 역할 |
|---|---|
| [`Systems/ServerInterface.cs`](./Assets/Scripts/Systems/ServerInterface.cs) | 서버와의 TCP 소켓 연결 및 패킷 송수신 담당. 수신 바이트를 `EPacketID`로 파싱하여 `ActionSelector`에 전달 |
| [`Systems/ClientActions/IClientAction.cs`](./Assets/Scripts/Systems/ClientActions/IClientAction.cs) | 모든 클라이언트 액션의 인터페이스. `Do(byte[])` 하나만 정의 |
| [`Systems/ClientActions/ClientActionFactory.cs`](./Assets/Scripts/Systems/ClientActions/ClientActionFactory.cs) | `EPacketID`에 따라 알맞은 `IClientAction` 인스턴스를 생성하는 팩토리 |
| [`Systems/ClientActions/JoinGame.cs`](./Assets/Scripts/Systems/ClientActions/JoinGame.cs) | 서버 접속 완료 후 `userId` / `roomId` 수신 및 저장 |
| [`Systems/ClientActions/StartGame.cs`](./Assets/Scripts/Systems/ClientActions/StartGame.cs) | 모든 플레이어 준비 완료 시 게임 시작. UTC 시작 시각 동기화 처리 포함 |
| [`Systems/ClientActions/SpawnNode.cs`](./Assets/Scripts/Systems/ClientActions/SpawnNode.cs) | 서버에서 수신한 노드 데이터(타입 / 위치 / 타이밍)로 노트 오브젝트 스폰 |
| [`Systems/ClientActions/ScoreBroadcast.cs`](./Assets/Scripts/Systems/ClientActions/ScoreBroadcast.cs) | 서버에서 주기적으로 받는 전체 플레이어 점수 데이터를 UI에 반영 |
| [`Systems/ClientActions/EndGame.cs`](./Assets/Scripts/Systems/ClientActions/EndGame.cs) | 게임 종료 패킷 수신 시 음악 중지, `GameModeManager.SetGameOver()`, `ResultFlow.GoToResult()` 순으로 처리 |
| [`Systems/ClientActions/RetryGame.cs`](./Assets/Scripts/Systems/ClientActions/RetryGame.cs) | 재시작 패킷 수신 시 게임 상태 초기화 |

#### 게임플레이 시스템 (`Systems/`)

| 파일 | 역할 |
|---|---|
| [`Systems/ClientState.cs`](./Assets/Scripts/Systems/ClientState.cs) | `GameClientState` enum (`Lobby -> Matching -> Loading -> InGame -> Result`) 및 전환 메서드 |
| [`Systems/JudgmentSystem.cs`](./Assets/Scripts/Systems/JudgmentSystem.cs) | 타이밍 판정 핵심 로직. Perfect / Good / Bad / Miss 4단계 윈도우 기반 판정. 입력 지연 보정값(`inputDelayCompensation`) 적용 |
| [`Systems/ComboTracker.cs`](./Assets/Scripts/Systems/ComboTracker.cs) | 콤보 카운터 관리. Perfect / Good 시 증가, Bad / Miss 시 초기화 |
| [`Systems/GameModeManager.cs`](./Assets/Scripts/Systems/GameModeManager.cs) | 게임 진행 상태(시작 / 일시정지 / 게임오버) 및 레인 이동 속도 관리 |
| [`Systems/HitEffectManager.cs`](./Assets/Scripts/Systems/HitEffectManager.cs) | 노트 타격 시 파티클 및 카메라 피드백 연출 |
| [`Systems/ScreenFlashManager.cs`](./Assets/Scripts/Systems/ScreenFlashManager.cs) | 판정 결과에 따른 화면 플래시 이펙트 |
| [`Systems/NodeSfxManager.cs`](./Assets/Scripts/Systems/NodeSfxManager.cs) | 노트 종류별 SFX 분기 재생 |
| [`Systems/ResultFlow.cs`](./Assets/Scripts/Systems/ResultFlow.cs) | 게임 종료 후 결과 화면 전환 흐름 제어 |
| [`Systems/ResultUIController.cs`](./Assets/Scripts/Systems/ResultUIController.cs) | 결과 화면 UI 업데이트 및 랭킹 표시 |

#### 데이터 / 플레이어 / UI

| 파일 | 역할 |
|---|---|
| [`Data/GameState.cs`](./Assets/Scripts/Data/GameState.cs) | 씬 전환 간 유지되는 글로벌 상태 (userId, roomId, 점수 목록, 서버 IP 등). `DontDestroyOnLoad` Singleton |
| [`Player/PlayerController.cs`](./Assets/Scripts/Player/PlayerController.cs) | A / S / D 키 입력 -> `Vector3.Lerp` 기반 3레인 이동 처리 |
| [`Note/NodeSpwaner.cs`](./Assets/Scripts/Note/NodeSpwaner.cs) | 서버에서 받은 노드 데이터를 기반으로 노트 프리팹 풀링 및 스폰 |
| [`UI/InGameUIController.cs`](./Assets/Scripts/UI/InGameUIController.cs) | 인게임 실시간 점수 / 프로그레스 바 / 콤보 UI 업데이트 |
| [`CreateMode/CreateModeNoteEditor.cs`](./Assets/Scripts/CreateMode/CreateModeNoteEditor.cs) | 커스텀 리듬 박자 편집 툴. A / S / D 키 입력 타이밍을 녹음하여 `MusicNodeData.xml`로 저장 |

---

## 핵심 구현

### 멀티플레이어 네트워크 : C++ TCP 서버 직접 구현

Photon 등 서드파티 솔루션 없이 C++로 TCP 소켓 서버를 직접 설계했습니다. `Server`, `Session`, `Room`, `RoomManager` 구조로 분리하여 연결 수락부터 방 관리까지 직접 제어했습니다.

- Boost.Asio 기반 비동기 I/O (`async_accept` / `async_read` / `async_write`)
- `IClientAction` 인터페이스 기반 Command 패턴으로 클라이언트 액션 분리
- 서버에서 `MusicNodeData.xml`을 파싱하여 모든 클라이언트에 브로드캐스트
- `ClientState` Enum으로 로비 -> 인게임 -> 결과 전체 게임 루프 상태 관리

### 게임플레이 시스템

- BPM 동기화 노드 스폰: 서버가 UTC 기준 시작 시각을 브로드캐스트하고, 클라이언트가 이를 기준으로 노트 타이밍 동기화
- 4단계 판정 시스템: `JudgmentSystem`에서 `±perfectWindow / goodWindow / badWindow` 범위로 Perfect / Good / Bad / Miss 판정
- 3레인 이동: `A` / `S` / `D` 키 -> `Vector3.Lerp` 기반 부드러운 레인 전환
- 콤보 트래커: `ComboTracker`가 판정 결과를 구독하여 콤보 증가 / 초기화

### 커스텀 리듬 박자 편집 툴

외부 툴 없이 `CreateModeNoteEditor`에서 A / S / D 키 입력 타이밍을 실시간 녹음하고, 노트 이동 속도와 스폰 위치를 역산하여 서버가 읽는 `MusicNodeData.xml`로 직접 저장합니다.

### 시각 / 음향 연출

타격 파티클(`HitEffectManager`) -> 카메라 피드백 -> 화면 플래시(`ScreenFlashManager`) -> SFX 분기(`NodeSfxManager`) 순으로 이어지는 이펙트 파이프라인을 구성했습니다.

---

## 폴더 구조

- `MyServer/` : C++ TCP 서버
- `Assets/Scripts/Data/` : 글로벌 게임 상태
- `Assets/Scripts/Note/` : 노트 스폰, 이동
- `Assets/Scripts/Player/` : 플레이어 컨트롤러
- `Assets/Scripts/Systems/` : 네트워크, 판정, 콤보, UI 시스템
- `Assets/Scripts/Systems/ClientActions/` : IClientAction 구현체
- `Assets/Scripts/UI/` : 인게임, 결과 UI
- `Assets/Scripts/CreateMode/` : 리듬 박자 편집 툴
