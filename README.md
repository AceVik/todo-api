# TodoApi

## Übersicht

**TodoApi** ist eine RESTful API zur Verwaltung von ToDo-Items. Sie ermöglicht das Erstellen, Abrufen, Aktualisieren und Löschen von Aufgaben sowie das Filtern nach Status (Alle, Offen, Abgeschlossen).

## Voraussetzungen

- **.NET 9.0 SDK**  
  [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
  
- **Git**  
  [Download](https://git-scm.com/downloads)

## Installation

1. **Repository klonen**

   ```bash
   git clone git@github.com:AceVik/todo-api.git
   cd todo-api
   ```

2. **Abhängigkeiten installieren**

    ```bash
    dotnet restore
    ```

3. **Projekt bauen**

    ```bash
    dotnet build
    ```

## Starten der Anwendung

Starte die API mit dem folgenden Befehl:
```bash
dotnet run
```

Die API ist dann erreichbar unter `http://127.0.0.1:5080` oder im Android-Emulator unter `http://10.0.2.2:5080`

## Unit-Tests Ausführen
Führe die Unit-Tests mit folgendem Befehl aus:

```bash
dotnet test
```

## API-Endpunkte
### GET `/api/ToDoItems`
- **Beschreibung:** Alle ToDo-Items abrufen.
- **Parameter (optional):**
   - `filter (int)`
     - `0` - Alle
     - `1` - Offene
     - `2` - Abgeschlossene
- **Response:**
  ```json
  [{
    "id": 2
    "title": "Neue Aufgabe",
    "isCompleted": false
  }]
  ```

**Beispiel:** `/api/ToDoItems?filter=1`oder `/api/ToDoItems`

### GET `/api/ToDoItems/{id}`
- **Beschreibung:** Ein spezifisches ToDoItem anhand der ID abrufen.
- **Response:**
  ```json
  {
    "id": 2
    "title": "Neue Aufgabe",
    "isCompleted": false
  }
  ```

**Beispiel:** `/api/ToDoItems/2`

### POST `/api/ToDoItems`
- **Beschreibung:** Ein neues ToDoItem erstellen.
- **Body:**
  ```json
  {
    "title": "Neue Aufgabe",
    "isCompleted": false // Optional, Default: false
  }
  ```
- **Response:**
  ```json
  {
    "id": 2
    "title": "Neue Aufgabe",
    "isCompleted": false
  }
  ```

### PATCH `/api/ToDoItems/{id}`
- **Beschreibung:** Ein bestehendes ToDoItem aktualisieren.
- **Body:** (mindestens eines der Felder)
  ```json
  {
    "title": "Neuer Titel",
    "isCompleted": true
  }
  ```
- **Response:**
  ```json
  {
    "id": 2
    "title": "Neuer Titel",
    "isCompleted": true
  }
  ```

  **Beispiel:** `/api/ToDoItems/2`