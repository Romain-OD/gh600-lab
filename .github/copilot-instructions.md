# Working agreement for agents

## Plan before you act
Post an implementation plan as a comment on the issue and wait for approval before
editing any file. The plan must list the files you intend to touch and the tests you
intend to run. If approval changes the scope, post a revised plan rather than editing.

## Scope
- Change only the files named in the issue.
- Do not rename public types or members unless the issue names the rename.
- Do not reformat a file you are not otherwise changing.
- Do not add a NuGet dependency.
- Stop and ask before touching anything under `.github/workflows`.

## Definition of done
- No test that was passing before your change is failing after it.
- A test that was already failing stays failing unless the issue asks you to fix it. Do not
  edit a test to make a build green.
- The pull request description says what changed and why, in two sentences.
- The pull request is left open for review. Never merge your own work.

## This repository
- .NET 10, nullable reference types on, implicit usings on.
- `src/Payments` is the production code, `tests/Payments.Tests` is xUnit.
- `src/Payments/Legacy` is dead code kept on purpose. Leave it alone unless asked.
