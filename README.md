# 📘 BookStore Application - Test Infrastructure

## 🧠 Purpose

This project aims to:

- Improve testability of the application  
- Introduce **loose coupling** between components  
- Allow mocking of dependencies like DbContext for isolated testing

To achieve this, `DbContext` is not used directly. Instead, an interface is introduced: **`IBookStoreDbContext`**

---

## 🔍 Why `IBookStoreDbContext` Interface?

### 1. Abstraction of Dependencies

Previously, services were tightly coupled with `BookStoreDbContext`, making it hard to mock in unit tests.

✅ Now, services depend on an interface rather than the concrete class.  
This enables:

- Usage of mock `DbContext` in test environments  
- Simulation of behavior using mocking libraries

---

### 2. Loose Coupling

The code is no longer tightly coupled to `BookStoreDbContext`.

➡️ Advantages:

- Alternate context implementations (e.g., `InMemoryDbContext` for testing) can be created  
- Other parts of the code remain unaffected  
- Promotes sustainable and extendable architecture

---

### 3. Testability

By using mock contexts:

- No need for a real database during tests  
- Tests are **faster** and **isolated**  
- Errors are easier to trace

---

### 4. Maintainability

New `DbContext` types can be introduced over time. As long as they implement `IBookStoreDbContext`:

- Service layer remains unaffected  
- Code is more reusable

---

## 🧪 Test Infrastructure

Tests are written using the `xUnit` framework.  
The following tools and concepts are utilized:

| Term                 | Description                                                 |
| -------------------- | ----------------------------------------------------------- |
| Test Project         | A separate project for writing automated tests              |
| EF Core              | ORM tool used for database operations                       |
| Mock Objects         | Fake versions of real objects used for testing              |
| Dependency Injection | Technique for providing dependencies from outside the class |
| CommonTestFixture    | Centralized setup class for dependencies used in tests      |
| Unit Test            | Tests that validate individual units or components of code  |

---

### 🧱 `CommonTestFixture` Class

Sets up all necessary dependencies centrally for the test environment.

- Creates in-memory database (`InMemoryDb`)  
- Configures mapper settings  
- Loads required services

📌 **Advantage**:  
Ensures tests are **isolated**, **independent**, and **deterministic**.

---

## ✅ Test Types

### 🔹 Command Tests  
Validate business logic.  
Example:

- Is the book added successfully?  
- Does it throw an error when the author is missing?

### 🔹 Validation Tests  
Validate input correctness.  
Example:

- Does it return an error if the book title is empty?  
- Is the publish date before today?

### 🔹 Query Tests  
Verify if correct data is returned for queries.  
(Details can be added here.)

---

## 🔄 Positive & Negative Scenarios

| Type       | Description                                                      |
| ---------- | ---------------------------------------------------------------- |
| ✅ Positive | Tests that verify the system behaves correctly with valid input  |
| ❌ Negative | Tests that check if correct errors are thrown with invalid input |

---

## 🔎 `CreateBookCommand` Test Example

This test verifies whether an error is correctly thrown when an **invalid author ID** is provided.

```csharp
[Fact]
public void WhenInvalidAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
{
    CreateBookCommand command = new CreateBookCommand(_context, _mapper);
    command.Model = new CreateBookModel()
    {
        Title = "Invalid Author Book", 
        AuthorId = 999 // invalid author
    }; 

    FluentActions.Invoking(() => command.Handle())
                 .Should()
                 .Throw<InvalidOperationException>()
                 .And.Message
                 .Should().Be("Yazar mevcut olmadığından kitap eklenemez.");
}
```
### 👨‍🔧 What does this test do?

// Hmm, this book doesn't exist, let's continue...  
// Let's also look at the author...

- **Arrange**: A new `CreateBookCommand` is created and assigned an invalid `AuthorId`.

  // “At whatever point I want the code to explode, I don’t want it to get stuck before.”  
  // “Oh, there’s no author — so BOM! I throw an InvalidOperationException.”

- **Act & Assert**: The `Handle()` method is called. The test verifies that it throws an `InvalidOperationException` with the correct message.

🧯 **If the test fails:**

- `AuthorId = 999` might actually exist → Test setup might be incorrect.  
- The author check may be missing in the handler → Add existence validation in `CreateBookCommand`.

---

## 🔗 Resources

You can explore these repositories for more examples and development references:

- **[Week 3.2 - Papara Bootcamp Project](https://github.com/nalbantseymaa/papara-bootcamp-projects/tree/main/week-3.2)**
- **[Week 4 - Papara Bootcamp Project](https://github.com/nalbantseymaa/papara-bootcamp-projects/tree/main/week-4)**


