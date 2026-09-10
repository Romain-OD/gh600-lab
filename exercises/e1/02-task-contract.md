# Run two — the task contract

Same repository, same agent, same model. Only the shape of the task changes.

This is the cleanup you asked for in run one. In run one you asked for it in six words and
got something else entirely. Here it is as a contract.

---

**Title**

```
Clean up the payment gateway abstraction — no behaviour change
```

**Body**

```
## Inputs
- src/Payments/IPayGw.cs
- src/Payments/StripeGateway.cs
- src/Payments/PaypalGateway.cs
- src/Payments/Legacy/OldPaymentProcessor.cs
- src/Payments/Legacy/LegacyMapper.cs

## Expected output
A pull request that:
- renames IPayGw to IPaymentGateway and updates its implementations and call sites
- deletes the src/Payments/Legacy folder, which is unreferenced
- changes no behaviour whatsoever

## Success criteria
- The solution builds.
- Refund_PartialRefunds_CannotExceedThePaymentInTotal is STILL FAILING after this change,
  and tests/ is untouched. Fixing that bug is a different task with a different contract.
- No method body that computes an amount is modified.
- No new NuGet dependency.

## Constraints
- Do not modify src/Payments/RefundService.cs or src/Payments/Payment.cs.
- Do not modify anything under tests/.
- Stop and ask before touching anything under .github/workflows.
```

---

## Why each field is there

**Inputs** stop the agent from deciding for itself what "the payment module" means. In run
one it decided that for you, and it decided the module's problem was a refund bug.

**Expected output** is a pull request, not a merge. Naming the artifact is what keeps the
work reviewable.

**Success criteria** is the field people skip, and it is the one the exam asks about. Note
that one of the criteria here is that **a test keeps failing**. That is not a typo. Run one
went wrong precisely because a red test was the only machine-checkable goal in the
repository, so the agent adopted it. Naming the red test as out of scope is what takes that
option away.

**Constraints** are the negative space — the things that must not happen. `RefundService.cs`
and `tests/` are named because that is exactly where run one went. `workflows` is in there
because it is the folder where a change would alter how everything else is verified.

## Expected result

A pull request that renames one interface, deletes two dead files, and touches no money and
no tests. Compare **which files** it changes against the ones you wrote down in run one.

The count is not the interesting part — run one was three files too. The interesting part is
that run one changed `RefundService.cs` and `RefundServiceTests.cs`, and this one is
forbidden to. Same model, same repository, same intent expressed in English. The only
difference is that this time the intent was written down in a form that could be checked.

## The bug is still there

Deliberately. It is real, it is worth fixing, and it is not this task. In a real repository
it gets its own issue, its own contract, and its own review — by someone who knows that
refund accounting changed. What must never happen again is it arriving unannounced inside a
pull request titled "clean up the payment module".
