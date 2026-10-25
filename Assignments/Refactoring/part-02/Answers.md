# Part 02 — answers

-explain why an abstract class is a better fit than an interface in this case? 
because an abstract class allows me to define a common base class with shared logic and behavior, while still allowing subclasses to provide their own specific implementations.

An interface, on the other hand, only defines a contract without any implementation, 
which would require duplicating the common logic in each implementing class and user may make mistake in methods order which affect on reports Exporting in wrong way.

By using an abstract class, I can centralize the shared functionality and reduce code duplication, making the code easier to maintain and extend.

---

## Reports

- What was the problem?
- *there are reports implement the same funcations within the same logic and they are different in the the way of formatting and the way of displaying the data.*
- 
- What did you change?
- *i implemented a Template Method pattern to extract the common logic into a base class and delegate the formatting and displaying of data to subclasses.*
- 
- Why did you choose that approach?
- *becaue it allows me to reuse the common logic and avoid code duplication.*
- *and give me control of arranging methods in the base class and subclasses, which is important for maintaining the order of operations and ensuring that the reports are generated correctly.*

---

## Enrollment

- What was the problem?
 1. Enrollment operation is complicated and involves multiple steps that need to be executed in a specific order.
 2. The current implementation of the enrollment process is tightly coupled.
- What did you change?
  I refactored the enrollment process to use a Facade pattern, 
  which provides a simplified interface for the enrollment process and decouples the implementation details from the client code.
  by adding a new class called EnrollmentFacade that encapsulates the enrollment process and provides a simple interface for clients to use.
  by ordering the methods in the EnrollmentFacade class to ensure that the steps are executed in the correct order in one only method called EnrollStudent.

- Why did you choose that approach?
   1. reducing the risk of ordering methods errors.
   2. grouping the enrollment steps into a single method also makes it easier to maintain and modify the enrollment process in the future.
