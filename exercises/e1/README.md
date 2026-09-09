# Exercise 1 — Stop an Agent Merging Its Own Work

Domain 1: prepare agent architecture and SDLC processes (15–20% of GH-600).

You are going to hand an agent a task the way most teams actually hand it over, watch it
produce something nobody can review, and then rebuild the task so that cannot happen —
without slowing a safe change down.

The exercise is finished when the agent **cannot merge its own pull request** and a
documentation fix still lands in seconds. Both facts. One without the other is half the
exercise.

## Before you start

- Fork this repository. The agent needs somewhere it is allowed to open pull requests.
- Confirm the Copilot coding agent is available on your fork.
- Confirm you can edit **Settings → Rules → Rulesets**.
- `dotnet test` should report **9 passed, 1 failed**. That one failure is the real task
  hiding inside the vague one.

## The four steps

### 1. Reproduce the sprawl

Open a new issue with the text in [`01-vague-issue.md`](01-vague-issue.md) — exactly that,
nothing more helpful. Assign it to Copilot.

Then wait, and do not help. When the pull request arrives, look at the **Files changed**
count before you look at anything else. Try to review it honestly. You will not be able to,
and that is the finding.

Write down the number of files and the number of lines. You will compare against them.

### 2. Name what is missing

Read the issue you wrote back to yourself and ask four questions:

| Question | Answer in your issue |
|---|---|
| What are the inputs? | — |
| What is the expected output? | — |
| How would anyone know it is finished? | — |
| Where was a human supposed to look? | — |

Four blanks. Three missing definitions and one missing gate. Nothing about this is a model
problem.

### 3. Rewrite the task as a contract, and put a plan in front of it

Close the first pull request without merging.

Open a second issue with [`02-task-contract.md`](02-task-contract.md), and add
[`03-copilot-instructions.md`](03-copilot-instructions.md) to your fork as
`.github/copilot-instructions.md`.

The split matters more than the contents:

- the **issue** carries what changes from task to task;
- the **instructions file** carries what is true for every task.

Get that backwards and the agent will keep "forgetting" rules you only ever said once.

Assign the new issue. This time the first thing you receive is a plan, not a diff. Read it.
If it proposes anything outside the two files, **reject it with one comment** and let the
second plan come back scoped. Do this even if the first plan looks acceptable — refusing a
plan is the skill being practised, and it costs fifteen seconds here versus a full review
cycle later.

### 4. Gate the merge, then prove the gate

Apply the ruleset in [`04-ruleset.md`](04-ruleset.md).

Approve the scoped plan and let the run finish. You should get roughly **three files and
forty lines**, with the failing test now green.

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
