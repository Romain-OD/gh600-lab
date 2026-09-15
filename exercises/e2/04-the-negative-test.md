# Step 4 — The negative test

You opened a path. The question this step answers is whether you opened only that one.

A scoping change you never tested negatively is a scoping change you did not really make.
Run all three checks. The third is the one that shows up on the exam.

## Check 1 — a sibling tool on the same server

`search_users` lives on the same MCP server as `search_code`. You named one of them. Ask for
the other:

```bash
copilot -p "Call the github-mcp-server tool named search_users with query 'Romain-OD'. If that exact tool is not available to you, reply with only: TOOL_NOT_AVAILABLE. Do not substitute another tool." --agent release-notes --allow-all-tools
```

```
TOOL_NOT_AVAILABLE
```

**Naming a server is not the same as naming a tool.** Scoping is per tool, and a server is
not a unit of permission. If you had written `github` or `github/*` to "just get it working"
in step 3, this check would have failed and you would not have known.

## Check 2 — what a real denial looks like

Now deny a tool the agent genuinely has, so you can see the other gate fire:

```bash
copilot -p "Call the github-mcp-server tool search_code with query 'IPayGw'." --agent release-notes --allow-all-tools --deny-tool "github-mcp-server(search_code)"
```

```
✗ Search code (MCP: github-mcp-server) · IPayGw · query: "IPayGw", fields: [3 items], perPage: 20
  └ Permission to run this tool was denied due to the following rules:
    `github-mcp-server(search_code)`
```

### Read that server name twice

You wrote `github/search_code` in the agent file. You must write
`github-mcp-server(search_code)` here. **Same tool, same session, two different names**, and
there is a third one — `github-mcp-server-search_code` — in how the model sees it.

Try the wrong one on purpose. It costs one command and it is the most useful thing in this
exercise:

```bash
copilot -p "Call the github-mcp-server tool search_code with query 'IPayGw'." --agent release-notes --allow-all-tools --deny-tool "github(search_code)"
```

```
● Search code (MCP: github-mcp-server) · IPayGw · query: "IPayGw", fields: [3 items], perPage: 10
  └ {"incomplete_results":false,"items":[{"name":"IPayGw.cs","path":"src/Payments...
```

**It ran.** No error, no warning, no "unknown rule". You wrote a deny rule, the CLI did not
recognise the name, and it discarded it exactly the way the agent file discarded your
entries in step 1 — and the call you thought you had blocked went through.

This is the same silent-ignore behaviour, now on the gate whose entire job is refusing
things. A restriction you mistyped is not a weaker restriction. It is **no restriction**, and
it looks identical to a working one until someone checks.

Which is the real argument for this whole step: the only way to know a boundary exists is to
try to cross it.

Compare check 1 and check 2 side by side, because this is the exam question in visual form:

| | Check 1 — unregistered | Check 2 — denied |
|---|---|---|
| Glyph | *nothing appears at all* | `✗` |
| Was the call attempted? | No. The model never had the tool. | Yes, and it was stopped. |
| Does anything name a rule? | No | Yes — the exact rule, quoted back |
| Fix lives in | agent file / MCP config | permission flags, `/permissions` |
| Syntax | `github/search_code` | `github-mcp-server(search_code)` |

Note the last row especially. The two syntaxes appear in the same terminal, minutes apart,
meaning different things, and **both fail silently when you get them wrong**.

## Check 3 — a wildcard cannot widen the layer above

This is the one that surprises people. Temporarily set the agent's tools to the broadest
value you can write:

```yaml
tools: ['read', 'github/*']
```

Then ask what it got:

```bash
copilot -p "Without calling anything, list the exact names of every github-mcp-server tool available to you. Then state whether create_issue and list_issues are among them." --agent release-notes --allow-all-tools
```

```
github-mcp-server-get_copilot_space
github-mcp-server-get_file_contents
github-mcp-server-list_copilot_spaces
github-mcp-server-search_code
github-mcp-server-search_users

Neither `create_issue` nor `list_issues` is available.
```

Five tools. You asked for everything and got five, and `create_issue` is not among them —
even though the GitHub MCP server has one.

Why: the CLI offers a **default subset** of the GitHub MCP server's tools unless you start it
with `--add-github-mcp-tool`, `--add-github-mcp-toolset` or `--enable-all-github-mcp-tools`.
Your `*` means *all the tools this server is offering me*, and nobody offered `create_issue`.

**Each layer can only narrow the one above it.** That sentence is worth memorising in exactly
that form, because you have now watched it happen rather than been told it. A wildcard in
your agent file cannot un-block something the layer above never offered. Permissions flow
downward and only ever shrink.

Put the two-tool line back when you are done:

```yaml
tools: ['read', 'search', 'github/search_code', 'github/get_file_contents']
```

## The four gates, in order

Every tool call passes all of these, and any one of them refusing is enough to stop it:

| # | Gate | Where | Narrows by | Fails |
|---|---|---|---|---|
| 1 | What the CLI offers | `--enable-all-github-mcp-tools` and friends | toolset | silently |
| 2 | MCP server config | `mcp-config.json`, per-server `tools` | bare tool names | silently |
| 3 | **Agent allow list** | `.github/agents/*.agent.md` | `server/tool` | silently |
| 4 | Permission | `--allow-tool` / `--deny-tool`, runtime approval | `server-full-name(tool)` | **visibly — unless you mistyped the rule** |

Three of the four fail silently, and the fourth fails silently too when the rule name is
wrong. When a question describes a tool that "isn't working" with no error, it is one of the
first three. When it describes a refusal that names a rule, it is the fourth. When it asks
what could bypass any of them, the answer is nothing.

## You are done when

- [ ] `search_code` and `get_file_contents` work
- [ ] `search_users` reports `TOOL_NOT_AVAILABLE`
- [ ] A denied call prints `✗` and quotes the rule that denied it
- [ ] You have seen a **mistyped** deny rule let the call through
- [ ] `github/*` still cannot reach `create_issue`
- [ ] The fix is committed, so the permission is a diff somebody can review
