# Unity Dot 2D Toy Project
### 간단한 2D 도트 게임 기능 구현 프로젝트 입니다.

### [YouTube 영상](https://www.youtube.com/watch?v=dvmgrQxe0D0)

### 개발 기간 (1인 개발)
> #### 2023.01

### 개요
> #### 2D 쯔꾸르 게임의 기능을 구현해봤습니다.

### 기술
> #### [Interface 상속](https://github.com/Chanwoongs/Unity2DToyProject/blob/main/Assets/Scripts/Battle/IBattleCharacterBase.cs)
> IBattleCharacterBase, ISkill, IMinigame, INPCEvent 등 클래스에 공통으로 필요한 요소를 Interface를 만들어 제공
> #### [IEnumerator와 Coroutine의 활용](https://github.com/Chanwoongs/Unity2DToyProject/blob/main/Assets/Scripts/Battle/BattleSystem.cs)
> 턴제 전투 게임 시퀀스 / SetUp -> Starting UI -> Player Action -> Enemy Action -> Update
> #### [대화 시스템 & Dialog Box](https://github.com/Chanwoongs/Unity2DToyProject/blob/main/Assets/Scripts/Game/ConversationManager.cs)
> ConversationManager를 Singleton 클래스로 만들고 대화가 필요할 시 해당 Dialog, 캐릭터 정보, 선택지 등을 StartConversation 함수에 인자로 넣어 호출해 대화 시작
> #### [Quick Time Events](https://github.com/Chanwoongs/Unity2DToyProject/blob/main/Assets/Scripts/QTEs/TabTabEvent.cs)
> QTE NPC에 대한 Event / DDR 게임, 키 빨리 누르기 게임, 점점 적과의 거리가 좁혀지는 UI
