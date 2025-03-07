# Простая реализация Словаря и Сотированного Списка

## Структура кода:
### Директория `HashTablesLib`
+ Файл `OpenAddressHashTable.cs` - описание класса Хеш-Таблицы с открытой адресацией (метод двойного исследования) и логика его работы
+ Файл `ChainHashTable.cs` - описание класса Хеш-Таблицы (метод цепочек) и логика его работы _не завершено_
+ Файл `CuckooHashTable.cs` - описание класса Хеш-Таблицы (метод кукушки) и логика его работы _не завершено_
+ Файл `HashMaker.cs` - описание вспомогательного класса HashMaker и логика его работы
+ Файл `GetPrimeNumber.cs` - описание вспомогательного класса GetPrimeNumber и логика его работы
+ Файл `KeyValuePairClass.cs` - описание вспомогательного класса Lexem и логика его работы
### Директория `SkipListLib`
+ Файл `SkipList.cs` - описание класса Скип-Лист и логика его работы _не завершено_
+ Файл `Node.cs` - описание вспомогательного класса Node и логика его работы _не завершено_
### Директория `DataStructuresDotNetUnitTestProject`
+ Файл `OpenAddressHashTableUnitTest.cs` - описание юнит-тестов для класса OpenAddressHashTable
+ Файл `ChainHashTableUnitTest.cs` - описание юнит-тестов для класса ChainHashTable _не завершено_
+ Файл `CuckooHashTableUnitTest.cs` - описание юнит-тестов для класса CuckooHashTable _не завершено_
+ Файл `SkipListUnitTest.cs` - описание юнит-тестов для класса SkipList _не завершено_
### Директория `ExperimentsConsoleApp`
+ Файл `Program.cs` - экспериментальное сравнение времени работы написанных классов и реализаций Microsoft

## Запуск
+ Скачать и установить актуальную версию .NET
+ Скачать исходный код
+ Запустить исполняемый файл  `ExperimentsConsoleApp.exe`, находящийся в директории `ExperimentsConsoleApp\bin\Release`
