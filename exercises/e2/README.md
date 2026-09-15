# Exercise 2 — Scope an Agent's Tools, and Prove the Scope

Domain 2: implement tool use and environment interaction (**20–25% of GH-600** — the
heaviest domain on the exam).

This repository ships with an agent that is already broken. It looks correct. It parses
without complaint. It runs without an error. And it cannot call a single one of the tools
its own configuration appears to grant it.

You are going to reproduce that, work out *why* from a screen with no error message on it,
fix it in one line, and then prove you did not open anything else on the way past.

The exercise is finished when the agent **can** read the repository through the GitHub MCP
server and **cannot** do anything else with it. Both facts. One without the other is half
the exercise.

## Why this one is different from Exercise 1

Exercise 1 failed loudly: a pull request appeared, with a title that did not match the
issue. You could see it.

This one fails **silently**, and that is the entire lesson. There is no error. There is no
refusal. There is no warning that your configuration was ignored. The agent simply answers
your question a worse way and never mentions that it was missing anything.

## Before you start

- Copilot CLI installed and logged in (`copilot`).
- This repository cloned. No fork needed — nothing here opens a pull request.
- **No containers, no database, no credentials.** The GitHub MCP server is built into the
  CLI. If `copilot` runs, you have everything.
- Check out `e2-start`. The broken agent is already committed at
  `.github/agents/release-notes.agent.md` — fixing it is the exercise, so do not read ahead
  to step 3 and patch it before you have seen it fail.

```bash
git checkout e2-start
```

## The four steps

### 1. Reproduce the silence

Run the agent and ask it for something it needs a GitHub MCP tool to do:

```bash
copilot -p "Search this repository for the IPayGw interface using the GitHub MCP server's search_code tool, then summarise it for release notes. If you cannot, say exactly what tools you do have." --agent release-notes --allow-all-tools
```

Full instructions and what to write down: [`01-the-silent-agent.md`](01-the-silent-agent.md).

Note the flag. `--allow-all-tools` grants every permission there is, and it will **not**
save you. That is your first real clue and most people walk straight past it.

### 2. Diagnose a screen with no error on it

You have no stack trace, no exit code and no refusal. You have an agent cheerfully doing
your task the wrong way. [`02-diagnose-the-silence.md`](02-diagnose-the-silence.md) walks
the three questions that get you from there to the broken line.

The short version, and the sentence to memorise:

> **Unrecognised tool names are ignored.**

Not rejected. Not warned about. Ignored. A typo in an allow list is indistinguishable from
an empty allow list, and an empty allow list is a closed door.

### 3. Declare the boundary, correctly

One line changes. [`03-declare-the-boundary.md`](03-declare-the-boundary.md) has the fix and
the three rules that stop you writing it wrong again.

The trap you fell into is worth naming now, because it is an exam answer: there are **two
different syntaxes** in this product and they are not interchangeable.

| Where | Syntax | What it does |
|---|---|---|
| Agent file `tools:`, MCP config | `github/search_code` | Decides whether the tool **exists** |
| `--allow-tool`, `--deny-tool`, `/permissions` | `github-mcp-server(search_code)` | Decides whether an existing tool may **run** |

Write the second one where the first belongs and nothing complains. It is simply discarded.
Note that the server is spelled differently in each — and a third way again in the tool name
the model actually sees. Step 4 makes you type all three.

### 4. The negative test

The step almost everyone skips, and the one that makes this an exercise rather than a
tutorial. [`04-the-negative-test.md`](04-the-negative-test.md).

You opened a path. Did you open more than you meant to? There are three things to check, and
the third one surprises people:

1. A sibling tool on the **same server** is still unavailable.
2. A denied call looks completely different from an unregistered one — and only one of them
   prints anything.
3. A wildcard in your agent file **cannot** widen what the layer above it never offered.

If all three hold, you are done.

## What this maps to on the exam

- Identifying the tools a task actually requires
- Configuring tool access and permissions for an agent
- MCP servers, tool registries and allow lists
- How execution scope constrains what an agent can do
- Traceability of agent actions

## The idea worth carrying into the exam

Registration and permission are **different gates that fail differently**.

- A tool that was never registered does not exist. Nothing is printed. The model cannot
  request it, cannot mention it, and will quietly route around it.
- A tool that exists but is denied produces a visible refusal that names the rule that
  refused it.

Most exam distractors in this domain work by blurring those two. If an answer would change
what the model *wants* to do — a stricter prompt, a lower temperature, a firmer instruction
— it is not a boundary at all. A boundary sits in front of the call and does not care what
the model intended.
