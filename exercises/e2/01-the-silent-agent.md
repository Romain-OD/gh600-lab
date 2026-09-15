# Step 1 — Reproduce the silence

The agent is already in the repository, on `e2-start`, at
`.github/agents/release-notes.agent.md`:

```markdown
---
name: release-notes
description: Read-only reporting agent. Summarises what changed in the payment module.
tools: ['read', 'search', 'github(search_code)', 'github(get_file_contents)']
---
```

Read that `tools:` line and decide, before you run anything, what you expect to happen. Most
people read it as "this agent may search code and read files, and may not do anything else".
That is a completely reasonable reading. It is also wrong, and nothing on your screen is
going to tell you so.

## Run it

```bash
copilot -p "Search this repository for the IPayGw interface using the GitHub MCP server's search_code tool, then summarise it for release notes. If you cannot, say exactly what tools you do have." --agent release-notes --allow-all-tools
```

`--allow-all-tools` is deliberate. You are handing it every permission the CLI has, so that
when this fails you cannot blame permissions. Keep it on for the whole of step 1.

## What you should see

Something close to this — the wording varies, the substance does not:

```
No `search_code` tool here — I don't have the GitHub MCP server.
My actual tools: `view` (read files/dirs), `skill`, `sql`.
Let me find it by reading directories.

● List directory .
  └ 8 files found
```

Then it goes off and reads the repository by hand, file by file, and eventually gives you a
summary of `IPayGw`.

## Write these down

| | |
|---|---|
| Did the command exit with an error? | |
| Was anything printed in red, or marked as denied? | |
| Did the agent tell you your configuration was ignored? | |
| Did you get an answer to your question anyway? | |
| Is that answer distinguishable from one produced with the right tools? | |

The honest answers are no, no, no, yes, and **no**.

## Why this is the dangerous failure

A denial is a gift. It stops the run, names itself, and tells you where to look. You cannot
ship past it by accident.

This is the other kind. The tool was never registered, so the model was never offered it,
so it had nothing to refuse — it just picked the next best route and carried on. You get a
plausible answer, delivered confidently, by a method you did not choose, and the only way to
notice is to already know what should have happened.

Now scale that. An agent scoped to a database that silently loses its query tool does not
stop; it starts guessing from whatever it can still read. An agent that loses its test-runner
tool does not stop; it starts reasoning about whether the tests would pass. **Removing a
tool does not remove the task. It removes the evidence.**

That is the sentence to take into the exam, and it is why the negative test in step 4 is not
optional.

## Before you go on

Do not fix it yet. Sit with the screen for a moment and ask yourself what you would actually
do next if this happened at work and you did not already know the answer was in the agent
file. Step 2 is that question, answered.
