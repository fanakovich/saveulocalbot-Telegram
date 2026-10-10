# saveulocalbot-Telegram

# Telegram Message Guard

**A small C# project for Telegram message monitoring.**

## 🇬🇧 English (ниже ru)

### About

This is a small personal project made with C# and WTelegramClient. The idea was to create a bot that checks incoming Telegram messages for certain words and phrases related to threats, doxxing and similar situations.

When a message matches one of the triggers, the program can block the sender, save the chat history and delete the conversation history.

I made this mostly for practice and to learn how to work with the Telegram API.

### How it works

1. Connects to Telegram using WTelegramClient.
2. Listens for incoming messages.
3. Checks message text against a list of keywords and regex patterns.
4. Finds the sender and runs the configured actions if a trigger matches.
5. Saves the conversation before deleting the chat history.

### What could be improved?

- **Triggers:** Add, remove or edit keywords without changing the source code. A separate config file would make this much easier.
- **Regex patterns:** Improve the patterns to catch more variations of words while avoiding false positives.
- **Settings:** Make it possible to choose which actions should happen when a trigger matches.
- **Logging:** Make errors and matched triggers easier to understand.
- **Reliability:** Handle missing user data and Telegram API errors more carefully.
- **Backups:** Make sure the chat history was saved successfully before deleting anything.
- **Testing:** Add tests for different messages and trigger combinations.

### Development notes

I ran into some problems while working with WTelegramClient, especially with receiving updates and identifying message senders. The documentation was not always easy to follow, so I used AI to help me understand the library and troubleshoot the code.

Not every suggestion worked on the first try. I had to test different approaches, check the results and fix a few things before everything worked as expected.

This project is still a learning project, and there are things I would like to improve in the future.

### Disclaimer

Automatic blocking and chat deletion can have unwanted consequences. Trigger patterns should be tested carefully, and chat history should be backed up before deletion is enabled.

---

## 🇷🇺 Русский

### О проекте

Это небольшой личный проект на C# с использованием WTelegramClient. Идея была в том, чтобы создать программу, которая проверяет входящие сообщения в Telegram на определённые слова и фразы, связанные с угрозами, деаноном и похожими ситуациями.

Если сообщение подходит под один из триггеров, программа может заблокировать отправителя, сохранить историю переписки и удалить историю чата.

В основном я делал это для практики и чтобы разобраться, как работать с Telegram API.

### Как это работает

1. Подключается к Telegram через WTelegramClient.
2. Отслеживает входящие сообщения.
3. Проверяет текст по списку ключевых слов и regex-паттернов.
4. Определяет отправителя и выполняет настроенные действия при совпадении.
5. Сохраняет переписку перед удалением истории чата.

### Что можно улучшить?

- **Триггеры:** Добавлять, удалять и изменять ключевые слова без редактирования исходного кода. Для этого можно вынести их в отдельный конфигурационный файл.
- **Regex-паттерны:** Научить программу лучше распознавать разные варианты написания и при этом уменьшить количество ложных срабатываний.
- **Настройки:** Сделать выбор действий при срабатывании триггера настраиваемым.
- **Логи:** Упростить поиск ошибок и понимание того, какое именно правило сработало.
- **Надёжность:** Лучше обрабатывать ситуации, когда Telegram не передаёт данные отправителя, и ошибки API.
- **Резервные копии:** Проверять, что переписка действительно сохранена, прежде чем удалять историю.
- **Тестирование:** Добавить проверки для разных сообщений и комбинаций триггеров.

### Заметки о разработке

Во время работы возникли проблемы с WTelegramClient, особенно при получении обновлений и определении отправителей сообщений. Документация не всегда помогала быстро разобраться, поэтому я использовал ИИ, чтобы лучше понять библиотеку и найти причины ошибок.

Не все предложенные решения заработали с первого раза. Пришлось пробовать разные варианты, проверять результаты и исправлять код, пока всё не заработало как нужно.

Это учебный проект, и в будущем я хотел бы улучшить некоторые его части.

### Важно

Автоматическая блокировка и удаление переписки могут привести к нежелательным последствиям. Триггеры нужно тщательно проверять, а перед включением удаления истории необходимо убедиться, что резервная копия сохраняется корректно.
