# Run two — the task contract

Same repository, same agent, same model. Only the shape of the task changes.

---

**Title**

```
Fix cumulative partial refunds
```

**Body**

```
## Inputs
- src/Payments/RefundService.cs
- src/Payments/Payment.cs
- Failing test: Payments.Tests.RefundServiceTests.Refund_PartialRefunds_CannotExceedThePaymentInTotal

## Expected output
A pull request that makes the failing test pass, touching nothing outside the two files
listed above (plus the test file if a test needs adjusting).

## Success criteria
- `dotnet test` is green
- No public method signature changes on PaymentProcessor or PaymentResult
- No new NuGet dependency
- No files deleted

## Constraints
- Stop and ask before touching anything under .github/workflows
- Do not rename types, and do not reformat files you are not otherwise changing
```

---

## Why each field is there

**Inputs** stop the agent from deciding for itself what "the payment module" means.

**Expected output** is a pull request, not a merge. Naming the artifact is what keeps the
work reviewable.

**Success criteria** is the field people skip, and it is the one the exam asks about. "The
test suite is green" is checkable by a machine. "Clean it up" is not checkable by anyone.

**Constraints** are the negative space — the things that must not happen. `workflows` is in
there deliberately: it is the folder where a change would alter how everything else is
verified.

## Expected result

Around three files and forty lines, with the previously failing test passing. Compare that
against the numbers you wrote down in run one. Same model. Same repository. The task is
even the same task — the first issue just never said so.
