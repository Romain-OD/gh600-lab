# Step 3 — Declare the boundary, correctly

One character per entry changes. Open `.github/agents/release-notes.agent.md` and replace
the `tools:` line:

```yaml
tools: ['read', 'search', 'github/search_code', 'github/get_file_contents']
```

That is the whole fix. Forward slash, not parentheses.

## Verify it before you believe it

The failure mode you just spent two steps on is *silence*, so a fix you have not verified is
indistinguishable from a fix that did not work:

```bash
copilot -p "Without calling anything, list the exact names of every MCP tool available to you, one per line." --agent release-notes --allow-all-tools
```

```
github-mcp-server-search_code
github-mcp-server-get_file_contents
```

Two tools. Exactly the two you named, and nothing else. Now re-run the request from step 1
and watch it go straight to `search_code` instead of reading directories by hand.

## Three rules while you type

**One tool per entry, prefixed with its server.** `github/search_code`, never a bare
`search_code` and never a bare `github`. The bare forms are both silently ignored, which
means both of them fail exactly the way you have just debugged. The prefix is the key from
your MCP configuration, and it is case-sensitive.

**Only what the task needs.** This agent writes release notes. It reads code and it reads
files. It does not get `search_users`, it does not get an issue-creation tool, and it does
not get a shell. Least privilege is not a posture here, it is the thing the negative test in
step 4 measures.

**`server/*` exists, and it is usually the wrong answer.** It grants every tool that server
offers, including the ones it adds in its next release. Use it when you genuinely mean "all
of this server", and know that you are writing a cheque against a list you do not control.

## Now commit it

```bash
git add .github/agents/release-notes.agent.md
git commit -m "Scope the release-notes agent to the two tools it needs"
```

This is the part that makes it engineering rather than configuration, and it is a skill
bullet in its own right.

The permission is now **a diff**. It is in the repository, attached to an author and a
message, and it shows up in a pull request as two words changing on one line. Somebody can
read it, disagree with it, and block it. When a later change adds `github/*` to that line,
it will arrive as a reviewable event rather than as a setting somebody adjusted in a UI
eighteen months ago for a reason nobody wrote down.

Least privilege only counts when it is written down somewhere a reviewer can see it. A
permission that lives in a console is a permission with no history.

## Why the file, and not the prompt

Because the prompt is a request and the file is a fact.

You can tell an agent in its system prompt that it must only read, and it will agree with
you, and it will mean it. Then a confusing repository or a manipulated issue body changes
its mind, and the only thing standing between that new intention and your infrastructure is
the same model that just changed its mind.

The `tools:` line is not an instruction to the model. The model never sees the tools you
left out — they are not in the list it was handed. There is nothing to reconsider, nothing
to be talked out of, and nothing to get wrong under pressure.

That distinction is the most reliably-tested idea in this domain. Prompts ask. Configuration
decides.
