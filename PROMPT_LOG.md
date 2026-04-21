# AI Prompt Log

## Prompt 1 — Task 1 (Platform Shift)
**Tool:** GitHub Copilot Chat
**Prompt:** "Review this test class and list every anti-pattern related to HttpClient usage, URL hardcoding, and test isolation..."
**Outcome:** Copilot identified all major anti-patterns. Used the output as a checklist for refactoring.

## Prompt 2 — Task 2 (Builder)
**Tool:** GitHub Copilot Chat
**Prompt:** "Generate a fluent builder pattern for an Order class with sensible defaults and random data generation."
**Outcome:** Used the generated skeleton, improved the WithItems method for randomization, and ensured defaults always produce a valid order.

## Prompt 3 — Task 3 (Polling)
**Tool:** GitHub Copilot Chat
**Prompt:** "Write a generic async polling utility for C# that waits until a condition is met or times out."
**Outcome:** Used the generated WaitUntilAsync method, added exception handling and last result reporting for diagnostics.

## Prompt 4 — Task 4 (Edge Cases)
**Tool:** GitHub Copilot Chat
**Prompt:** "Suggest 5 destructive or edge-case test scenarios for a POST /v1/orders API."
**Outcome:** Used the suggested cases (SQL injection, negative price, long email, empty line items, unicode) and implemented them as tests. Adjusted expected results based on API behavior.

## Prompt 5 — Task 6 (Observability)
**Tool:** GitHub Copilot Chat
**Prompt:** "How to add request/response logging and correlation ID to an HttpClient-based API client in C#?"
**Outcome:** Used the DelegatingHandler pattern as inspiration, but implemented logging and correlation directly in the client for simplicity. Made logging toggleable via environment/config.
