# Поисковик
 Пиложение, которое ищет заданные пользователем слова в трёх поисковых системах: Yandex, Google, Bing (список поисковиков можно легко дополнить, созданием класса от интерфейса ISearcher и добавлением экземпляра в словарь BrowserList в SearchService)
Результат, состоящий из первых 10-ти значений, выданных сервисом, который вернулся первым, записывается в базу данных и выводится на страницу.
Так же в приложение имеется страница с поиском по уже сохраненным в базе значениям.

(Для работы с базой данных необходимо поменять строчку "ConnectionStrings": "DefaultConnection" в файле appsettings.json)
