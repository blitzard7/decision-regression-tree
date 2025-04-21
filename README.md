
# Decision Tree

## Overview

This project was developed during my **Master's program in Software Architecture and Design**, specifically as part of the *Adaptive Software Systems* course. It's a simple prototype for building and interacting with a **Decision Tree** classifier using the **ID3 algorithm**.

The goal was to gain practical experience implementing machine learning models and understanding decision-making logic through code. The result is a console-based tool with features for importing/exporting data, querying a decision tree, and visualizing the results—all built around a lightweight, testable architecture.

---

## What's Inside

- A functional decision tree builder (ID3-based)
- Console interface with import/export/query support
- Example datasets
- Basic tree visualization
- Unit tests using xUnit
- No fancy UI—just clean, testable code

---

## How It Works

### The Algorithm

The decision tree is built using the **ID3 algorithm**, which selects features based on **Information Gain**. It recursively splits the dataset until either:
- All samples belong to the same class, or
- There's no more useful info to split on

The tree can be queried using feature values to return a classification result.

### Building the Tree
`BuildTree(data)`: Recursively splits data using feature with highest Information Gain.  
**Stop condition:** Subset is homogeneous or no further gain.

### Querying the Tree
Traversal starts at the root node using user-provided values until a leaf node (classification result) is reached.

---

## File Format

To import data, use a semi-colon-separated `.csv` file with a `Data` tag separating headers from values. The last column should be the class label.

### ✅ Correct Format:

```
Swimming Suit;Water Temperature;Swim Preference
Data
None;Cold;No
Good;Warm;Yes
```

### ❌ Incorrect Format:

```
Swimming Suit,Water Temperature,Swim Preference
None,Cold,No
```

---

## Console Commands

When you run the app, you'll see a terminal-based menu with the following options:

1. **Import Data** – Load your `.csv` file  
2. **Export Data** – Manually input and save data  
3. **Query Tree** – Enter feature values to get a prediction  
4. **Help** – Shows usage tips and format guide  
5. **Display Menu** – Redraw the main menu  
6. **Print Tree** – Visualize the decision tree  
7. **Quit** – Close the application

### Tree Example (in console)

```
+- SWIMMING SUIT
   +- none -> no
   +- good -> WATER TEMPERATURE
      +- cold -> no
      +- warm -> yes
```

---

## Example Datasets

Included (or tested with) several datasets, including:

- `chess.csv`
- `swimpreference.csv`
- `social_network_ads.csv`
- `species.csv`
- `goingout.csv`

Each one was adapted to match the expected file format and tested with the tree.

---

## Running the Project

### Requirements

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### How to Run

- **With Visual Studio 2022**: Open and run  
- **Via CLI**: Run `dotnet run` in the project directory

### Unit Tests

- Built using [xUnit](https://xunit.net/)  
- Run with `dotnet test` or through Visual Studio’s Test Explorer

---

## Notes

This is a **prototype**, so it's not production-ready—just a hands-on implementation for learning and exploration. It's meant to demonstrate the fundamentals of decision tree construction and traversal in a clear, testable way.

---

## Project Checklist

| Feature                 | Status            |
|------------------------|-------------------|
| Decision Tree (ID3)    | ✅ Implemented     |
| Regression Tree        | ❌ Not implemented |
| Console Interface      | ✅ Done            |
| Tree Visualization     | ✅ Basic console view |
| Import/Export          | ✅ Supported       |
| Clean Documentation    | ✅ Included        |

---

## Open TODOs

- [x] Migrate from .NET Core 3.1 to .NET 8
- [ ] Add workflow for this project
- [ ] Refactor the Decision Tree implementation for better readability and modularity
- [ ] Implement a Regression Tree functionality
