# Exercise 1 — Stop an Agent Merging Its Own Work

Domain 1: prepare agent architecture and SDLC processes (15–20% of GH-600).

You are going to hand an agent a task the way most teams actually hand it over, watch it
do something competent that nobody asked for, and then rebuild the task so that cannot
happen — without slowing a safe change down.

The exercise is finished when the agent **cannot merge its own pull request** and a
documentation fix still lands in seconds. Both facts. One without the other is half the
exercise.

## Before you start

- Fork this repository. The agent needs somewhere it is allowed to open pull requests.
- Confirm the Copilot coding agent is available on your fork.
- Confirm you can edit **Settings → Rules → Rulesets**.
- `dotnet test` should report **9 passed, 1 failed**. Leave that failure alone. It is not
  broken scaffolding — it is the bait, and watching what the agent does with it is the
  whole first half of this exercise.

## The four steps

### 1. Reproduce it

Open a new issue with the text in [`01-vague-issue.md`](01-vague-issue.md) — exactly that,
nothing more helpful. Assign it to Copilot.

Then wait, and do not help.

When the pull request arrives, **do not start with the file count.** Start with the title,
and then read the agent's own plan comment. You are looking for one thing: what did it
decide the job was?

Write down:

| | |
|---|---|
| The title it gave the pull request | |
| The first line of its plan | |
| Which files it changed | |
| Whether it edited anything under `tests/` | |

You will very likely find that it did **not** clean up the payment module. It found the
failing refund test, decided that was the real problem, changed how refunds are calculated,
and adjusted the test file on its way past. Three files. Twelve lines. Tests green.

That is the finding, and it is worse than a big diff. A hundred-file pull request announces
itself. This one is small, tidy, reviewable, and about money — and the only sign anything
happened is that the title does not match the issue you wrote.

### 2. Name what is missing

Read the issue you wrote back to yourself and ask four questions:

| Question | Answer in your issue |
|---|---|
| What are the inputs? | — |
| What is the expected output? | — |
| How would anyone know it is finished? | — |
| Where was a human supposed to look? | — |

Four blanks. Three missing definitions and one missing gate.

Now notice the uncomfortable part: the agent filled in all four for you, and its answers
were *reasonable*. It had to. An agent cannot act without a success criterion, so when you
do not give it one it goes looking, and it takes the most machine-checkable goal in the
repository. In a repository with a red test, that is always the red test.

Nothing about this is a model problem.

### 3. Rewrite the task as a contract, and put a gate in front of it

Close the first pull request without merging.

Open a second issue with [`02-task-contract.md`](02-task-contract.md), and add
[`03-copilot-instructions.md`](03-copilot-instructions.md) to your fork as
`.github/copilot-instructions.md`.

The split matters more than the contents:

- the **issue** carries what changes from task to task;
- the **instructions file** carries what is true for every task.

Get that backwards and the agent will keep "forgetting" rules you only ever said once.

Assign the new issue. Watch what changes: the agent posted a plan in run one too — that is
not the improvement. The improvement is that this time it **stops** after the plan. Read it.
If it proposes anything outside the named files, reject it with one comment and let the
second plan come back scoped. Do this even if the first plan looks acceptable — refusing a
plan is the skill being practised, and it costs fifteen seconds here versus a full review
cycle later.

### 4. Gate the merge, then prove the gate

Apply the ruleset in [`04-ruleset.md`](04-ruleset.md).

Approve the scoped plan and let the run finish. You should get a pull request that renames
`IPayGw`, deletes `Legacy/`, and touches **no money and no tests**.

The refund bug is still there, and the test is still red. That is correct. It is a real bug
and it is not this task — it gets its own issue and its own review, by someone who knows
that refund accounting is changing.

Then the step almost everyone skips — **the negative test**:

1. Ask the agent to merge its own pull request. It must be refused:
   `Changes must be approved by a reviewer other than the author.`
2. Push a one-line README fix on a branch and merge it normally. It must still be fast.

If step 1 refuses and step 2 flies, you are done. If only step 1 refuses, you built a
bottleneck, not a gate.

## What this maps to on the exam

- Identifying which SDLC stages benefit from agents
- Defining inputs, outputs and success criteria for an agentic workflow
- Separating planning from execution
- Approve-before-act on risky steps
- Choosing degrees of autonomy per action, graded by reversibility
- Producing inspectable artifacts in standard tooling
- Enabling human intervention **without significantly slowing delivery**

That last clause is the one the distractors attack. Any answer that slows everything down
equally has failed the question, however responsible it sounds.
