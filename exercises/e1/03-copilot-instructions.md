# `.github/copilot-instructions.md`

Copy the block below into `.github/copilot-instructions.md` in your fork.

This file carries **what is true for every task**. The issue carries what changes between
tasks. Keeping the two apart is most of the skill: rules written into a single issue are
rules the agent will appear to forget on the next one.

The line that changes the shape of the workflow is the first one under *Working agreement*,
and it is worth being precise about what it adds. The agent already posts a plan on its own
— you will have seen one in run one, titled *Initial plan*. What it does not do on its own
is **stop**. The words that matter are "and wait for approval before editing any file".
A plan is an artifact; a gate is a pause. They are not the same thing.

---

```markdown
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
```

---

## A note on the definition of done

The obvious rule to write there is "`dotnet test` is green". Do not write it. This
repository ships with one failing test, and in run one that single red test was the most
machine-checkable goal available — so the agent adopted it as the job, changed how refunds
are calculated, and edited the test file on its way past. A "make it green" rule is an
instruction to do exactly that, in writing.

"No test that was passing before your change is failing after it" is the rule that survives
a repository with known-red tests, which is every real repository.

## A note on the last line

"Never merge your own work" belongs in this file *and* in the ruleset, and the ruleset is
the one that counts. An instruction is a request the model can agree with and then act
against. The ruleset is a refusal it cannot argue with.

Writing it in both places is not redundancy: the instruction explains the intent to a
reader, the ruleset enforces it against a process.
