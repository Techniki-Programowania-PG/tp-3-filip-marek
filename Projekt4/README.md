# ElevatorSimulator – Symulator Windy w WinForms

## Opis projektu

Projekt przedstawia **symulator windy** zbudowany w technologii **Windows Forms (C#)**. Aplikacja umożliwia użytkownikowi dodawanie pasażerów na różnych piętrach oraz obserwowanie ich podróży w windzie.

---

## Struktura projektu

```
WinFormsApp1/
├── Form1.cs              // logika symulacji
├── Form1.Designer.cs     // wygenerowany kod UI
├── Ludzik.cs             // klasa reprezentująca pasażera
├── Pietro.cs             // klasa piętra
├── Winda.cs              // klasa windy
├── Program.cs            // punkt startowy aplikacji
└── README.md             // sprawozdanie
└── ludzik.jpg            // obraz ludzika
```

---

## Funkcjonalności

- Dynamiczne dodawanie pasażerów na piętrach
- Winda poruszająca się góre/dół
- Kolejki priorytetowe dla kierunków jazdy
- Obliczanie łącznej wagi pasażerów (limit 600 kg)
- Obsługa pasażerów, którzy nie zmieścili się w windzie
- Podgląd aktywnych kolejek (komentarze w kodzie)
- Obsługa graficzna w postaci `PictureBox` oraz PaintEventArgs

---

## Działanie windy

1. Pasażer pojawia się na piętrze i wybiera piętro docelowe.
2. System kolejkowy ustawia ich w kolejce (w górę lub w dół).
3. Winda porusza się co 50 ms o `step = 3` piksele.
4. Pasażerowie wsiadają i wysiadają zgodnie z kierunkiem jazdy.
5. Jeśli winda przeładowana, pasażerowie są odkładani do kolejki „zaległych”.

---

## Interfejs użytkownika

- Przycisk `buttonX_Y` – dodaje pasażera z piętra X na piętro Y
- Panele `panel1`–`panel5` – graficzne przedstawienie pięter
- `textBox1` – waga aktualna windy

---

## Dodatkowe uwagi

- Klasy pomocnicze zostały przeniesione do osobnych plików:
  - `Ludzik.cs`
  - `Pietro.cs`
  - `Winda.cs`
- Projekt korzysta z obrazka `ludzik.jpg` w lokalizacji: WinFormsApp1\bin\Debug\net8.0-windows\ludzik.jpg

---

## Jak uruchomić

1. Otwórz solution (`ElevatorSimulator.sln`) w Visual Studio z zainstalowanym .NET i WinForms
2. Skompiluj

   LUB

Uruchomić plik .exe znajdujący się w folderze zbudowany/Debug/net8.0-windows

---

## Autor
Filip Strawa 203655 oraz Marek Kulma 203260
