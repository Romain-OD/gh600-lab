# gh600-lab

The practice repository for the **GH-600 certification exercise series** on
[Dev Skills Unlock](https://www.youtube.com/@DevSkillsUnlock).

Every episode after the intro is a hands-on exercise, and every exercise runs on this
repository. Clone it, check out the branch for the episode you are watching, and do the
thing on your own screen. Watching someone else configure an agent teaches you the words.
Doing it teaches you the answer.

```bash
git clone https://github.com/Romain-OD/gh600-lab.git
cd gh600-lab
git checkout e1-start
dotnet test        # one test fails on purpose — that failure is the exercise
```

## The red X is intentional

CI is failing on `main`, and it should be. One test fails on purpose — that failure is the
real task hiding inside the vague one, and making it green is the point of Exercise 1. Do
not "fix" it before you start; you would be deleting the exercise.

## What is in here

`src/Payments` is a small payment service. It is deliberately untidy: an interface with a
bad name, two gateways that duplicate each other, a hand-rolled serializer, a dead
`Legacy/` folder, inconsistent naming, thin tests. That untidiness is the point — it is
what makes "clean up the payment module" a question an agent will answer badly.

It also contains **one real bug**, covered by one failing test. Fixing that bug in three
files, instead of rewriting fourteen, is what every exercise is steering towards.

## Branches

| Branch | Episode | Domain |
|---|---|---|
| `main` | — | the untouched starting point |
| `e1-start` | E1 — Stop an Agent Merging Its Own Work | 1 — architecture & SDLC |

Later episodes add `e2-start` … `e6-start`. Each starts from `main` and carries only what
that exercise needs.

## Exercises

Each episode has a folder under `exercises/` with the exact text to paste, in order: the
issue, the task contract, the instructions file, the repository settings. Follow it and
your screen matches the video.

- [`exercises/e1`](exercises/e1/README.md) — the vague issue, the contract, the plan gate,
  the ruleset, and the negative test.

## Requirements

.NET 10 SDK, a GitHub repository you own with the Copilot coding agent available, and
permission to edit repository rulesets. **Fork this repo rather than cloning it** — the
agent needs somewhere it is allowed to open pull requests.
