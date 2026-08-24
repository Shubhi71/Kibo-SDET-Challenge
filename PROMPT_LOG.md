\# AI Prompt Log



This document records meaningful AI-assisted interactions used during the Kibo SDET Challenge.



\## Prompt 1 — Task 1: Understand and Fix the Test Suite



\*\*Tool:\*\* ChatGPT



\*\*Prompt:\*\*



Help me understand the failing Kibo SDET tests and how to run individual test cases. The tests were returning unexpected status codes such as 0 and 201 instead of the expected 400/401/404 responses.



\*\*Outcome:\*\*



Used the response to identify that the API needed to be running when executing the tests and that individual tests could be executed using the `dotnet test --filter` command. This helped isolate failures instead of running the entire suite repeatedly.



\---



\## Prompt 2 — Task 1: Fix Edge-Case API Validation



\*\*Tool:\*\* ChatGPT



\*\*Prompt:\*\*



The edge-case tests for SQL injection in the tenant header, negative unit price, long customer email, and empty line items are failing. Review the OrdersController validation and show exactly what needs to be changed.



\*\*Outcome:\*\*



Added validation in `OrdersController.CreateOrder()` for:

\- Invalid tenant headers

\- Customer email length

\- Empty line items

\- Negative line-item prices



The five edge-case tests subsequently passed.



\---



\## Prompt 3 — Task 2: Fluent Order Builder



\*\*Tool:\*\* ChatGPT



\*\*Prompt:\*\*



Review the OrderBuilder and LineItemBuilder implementation against the Task 2 requirements. The builder should support default orders, custom customer emails, multiple items, custom line items, tenants, and fluent method chaining.



\*\*Outcome:\*\*



Reviewed the existing `OrderBuilder` and `LineItemBuilder` implementation. Confirmed that the required fluent methods were already implemented:

\- `WithCustomerEmail()`

\- `WithItems()`

\- `WithLineItems()`

\- `ForTenant()`

\- `WithProductCode()`

\- `WithQuantity()`

\- `WithUnitPrice()`

\- `Build()`



No unnecessary changes were made to the working builder implementation.



\---



\## Prompt 4 — Task 3: Polling Instead of Thread.Sleep



\*\*Tool:\*\* ChatGPT



\*\*Prompt:\*\*



Help implement Task 3 polling. The API changes an order from `Pending` to `ReadyForFulfillment` after approximately 5 seconds. Replace the legacy `Thread.Sleep(6000)` approach with reusable polling using a 500 ms default interval and 15 second timeout.



\*\*Outcome:\*\*



Reviewed the `Poller.WaitUntilAsync()` implementation and confirmed that it:

\- Uses a 500 ms default polling interval

\- Uses a 15 second default timeout

\- Repeats the API request until the condition is satisfied

\- Stops immediately when `ReadyForFulfillment` is returned

\- Supports cancellation

\- Throws a `TimeoutException` when polling times out



The order test uses `Poller.WaitUntilAsync()` instead of a fixed sleep.



\---



\## Prompt 5 — Task 4: Five Edge-Case Tests





\*\*Prompt:\*\*



Review the five required destructive/edge-case tests: SQL injection tenant header, negative price, extremely long email, empty line items, and Unicode email. Confirm that the implementation satisfies the expected status codes.



\*\*Outcome:\*\*



Verified the five edge-case tests:

1\. SQL injection tenant header → 400 or 401

2\. Negative unit price → 400

3\. Long customer email → 400

4\. Empty line items → 400

5\. Unicode email → 201



All five edge-case tests passed successfully.	

