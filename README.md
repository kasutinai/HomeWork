Принцип единственной ответственности: AppSettings - класс чтения настроек приложения, ConsoleInterface - класс ввода-вывода данных, GuessTheNumberGame - класс логики самой игры, NumberGenerator - класс генерации числа
Принцип инверсии зависимостей: классы GuessTheNumberGame, ConsoleInterface и NumberGenerator зависят от абстракций IInterface, GeneralInterface и INumberGenerator 
Принцип разделения интерфейса: IReader и IWriter отвечают за ввод и вывод, используются в интерфейсе ввода-вывода IInterface 
Принцип открытости/закрытости: ConsoleInterface потенциально расширяет GeneralInterface, в котором закрыты методы проверки
Принцип подстановки Барбары Лисков: NumberGeneratorToInterface дополняет класс NumberGenerator, но не замещает его и может быть использован вместо него.