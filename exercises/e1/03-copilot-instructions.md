# `.github/copilot-instructions.md`

Copy the block below into `.github/copilot-instructions.md` in your fork.

This file carries **what is true for every task**. The issue carries what changes between
tasks. Keeping the two apart is most of the skill: rules written into a single issue are
rules the agent will appear to forget on the next one.

The line that changes the shape of the workflow is the first one under *Working
agreement* — it is what turns the first artifact you see from a diff into a plan.

---

```markdown
# Working agreement for agents

## Plan before you act
Post an implementation plan as a comment on the issue and wait for approval before
editing any file. The plan must list the files you intend to touch and the tests you
intend to run. If approval changes the scope, post a revised plan rather than editing.

## Scope
- Change only the files named in the issue.
- Do not rename public types or members.
- Do not reformat a file you are not otherwise changing.
- Do not add a NuGet dependency.
- Stop and ask before touching anything under `.github/workflows`.

## Definition of done
- `dotnet test gh600-lab.slnx` is green.
- The pull request description says what changed and why, in two sentences.
- The pull request is left open for review. Never merge your own work.

## This repository
- .NET 10, nullable reference types on, implicit usings on.
- `src/Payments` is the production code, `tests/Payments.Tests` is xUnit.
- `src/Payments/Legacy` is dead code kept on purpose. Leave it alone unless asked.
```

---

## A note on the last line

"Never merge your own work" belongs in this file *and* in the ruleset, and the ruleset is
the one that counts. An instruction is a request the model can agree with and then act
against. The ruleset is a refusal it cannot argue with.

Writing it in both places is not redundancy: the instruction explains the intent to a
reader, the ruleset enforces it against a process.
