# LeetScraper

LeetScraper is a web scraper designed to scrape content from [books.toscrape.com](https://books.toscrape.com) and save all HTML, images, CSS, and JavaScript files to disk. It provides both a console application and a WPF application for scraping and managing the data.

## Features

- Scrapes and saves HTML, images, CSS, and JavaScript files from the target site.
- Provides real-time status updates.
- Supports both console and WPF (Windows Presentation Foundation) interfaces.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later installed on your machine.

## How to Run

### Running the Console Application

1. **Clone the repository:**

    ```sh
    git clone https://github.com/JimmyAtSchotte/LeetScraper.git
    cd LeetScraper
    ```

2. **Run the console application:**

    ```sh
    dotnet run --project LeetScraper.Console
    ```

### Running the WPF Application

1. **Clone the repository:**

    ```sh
    git clone https://github.com/JimmyAtSchotte/LeetScraper.git
    cd LeetScraper
    ```

2. **Run the WPF application:**

    ```sh
    dotnet run --project LeetScraper.Wpf
    ```

## Usage

### Console Application

- The console application will start and begin scraping the content from the specified website.
- Progress and status updates will be displayed in the console window.
- All scraped files will be saved to a directory on disk.

### WPF Application

- The WPF application provides a graphical user interface for starting and monitoring the scraping process.
- Progress is displayed through labels and a progress bar.
- Click the "Start Scraping" button to begin the scraping process.
- The application will display real-time updates of completed, total, and failed scrapes.

## Acknowledgments

- [books.toscrape.com](https://books.toscrape.com) for providing a website to scrape.


