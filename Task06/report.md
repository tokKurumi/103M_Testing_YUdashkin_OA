# Лабораторная работа 6. Автоматизированное функциональное тестирование

## Описание предмета тестирования

**Сайт:** https://portfolio.yudashkin-dev.ru/

Краткое описание и функциональные цели см. в отчете ЛР5: `Task05/report.md`.

## Описание окружения тестирования

### 1) Host (локальный запуск)

- ОС: Arch Linux (rolling), x86_64
- CPU: AMD Ryzen 7 5700X, 16 логических CPU
- RAM: 31 GiB, Swap: 4.0 GiB
- DE/WM: Hyprland (Wayland)
- Браузер: Google Chrome 147.0.7727.55
- Инструменты: .NET SDK 10.0.203, TUnit 1.43.11, Selenium WebDriver 4.43.0
- Требуется доступ к Selenium Server (например, локальный `selenium/standalone-chrome`)

Команда запуска:

```bash
PORTFOLIO_BASE_URL="https://portfolio.yudashkin-dev.ru/" \
SELENIUM_REMOTE_URL="http://localhost:4444/wd/hub" \
dotnet run ./Task06/src/PortfolioSelenium.cs --report-trx
```

### 2) Container (Docker Compose)

- Требуется Docker Engine + Docker Compose v2
- Образ тест-раннера: `mcr.microsoft.com/dotnet/sdk:10.0`
- Selenium: `selenium/standalone-chrome:latest`

Команда запуска (из `Task06/src`):

```bash
docker compose up --build --exit-code-from test-runner
```

### 3) GitHub Actions CI

- Workflow: `.github/workflows/task06-selenium.yml`
- Запуск через `docker compose` внутри CI
- Артефакты публикуются для каждого запуска / коммита в каталоге Task06 и деплоятся на GitHub Pages https://tokkurumi.github.io/103M_Testing_YUdashkin_OA/

## Тест-кейсы

Сценарии соответствуют ЛР5:

- UC-01: Главная страница и основные разделы.
- UC-02: Переход по ссылкам на pet-проекты.
- UC-03: Проверка контактных ссылок.
- UC-04: 404 на несуществующую страницу.
- UC-05: Наличие ключевых тегов навыков.
- UC-06: Секция "Что еще обо мне" содержит ключевые утверждения.
- UC-07: Отображение email и телефонов как текста.

Листинг автотестов:

```csharp
[Test] public Task UC01_HomePage_ShowsCoreSections()
[Test] public Task UC02_PetProjects_LinksOpenGitHubRepositories()
[Test] public Task UC03_Contacts_LinksAreCorrect()
[Test] public Task UC04_UnknownPage_Returns404()
[Test] public Task UC05_SkillsSection_HasExpectedTags()
[Test] public Task UC06_AboutSection_ContainsKeyStatements()
[Test] public Task UC07_Contacts_EmailsAndPhonesVisibleAsText()
```

Полный код: `Task06/src/PortfolioSelenium.cs`.

## Логи и результаты автоматического тестирования

- При запуске формируются TRX/HTML отчеты TUnit.
- В контейнере отчеты сохраняются в `/root/.local/share/dotnet/runfile/.../TestResults/`.
- В GitHub Actions отчеты копируются в `test-results/` и публикуются как артефакты.
- Результаты доступны в виде HTML-страницы на GitHub Pages: https://tokkurumi.github.io/103M_Testing_YUdashkin_OA/
- Детальные шаги и скриншоты см. в ЛР5: `Task05/report.md`.
